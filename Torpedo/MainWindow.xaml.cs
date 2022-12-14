using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vector = Torpedo.Models.Vector;
using Ship = Torpedo.Models.Ship;
using Player = Torpedo.Models.Player;
using JSONScores = Torpedo.Models.JSONScores;

namespace Torpedo
{

    public partial class MainWindow : Window
    {
        private const int _gameWidth = 10;
        private const int _gameHeight = 10;

        Brush _greenLight = new SolidColorBrush(Color.FromRgb(161, 239, 140));
        Brush _green = new SolidColorBrush(Color.FromRgb(63, 172, 149));
        Brush _blue = new SolidColorBrush(Color.FromRgb(68, 97, 118));
        Brush _blueDark = new SolidColorBrush(Color.FromRgb(44, 33, 55));
        Brush _missColor = new SolidColorBrush(Color.FromRgb(68, 97, 118)); //blue
        Brush _hitColor = new SolidColorBrush(Color.FromRgb(161, 239, 140)); //greenLight

        int turnCounter;

        //Ideiglenes
        Player player1;// = new Player("", null);
        Player player2;// = new Player("", null);

        public bool isVsCom_Global = false;
        public bool isPrevAiShotHit = false;

        public MainWindow(Player player1, Player player2, bool isVsCom)
        {
            this.player1 = player1;
            this.player2 = player2;
            isVsCom_Global = isVsCom;
            turnCounter = 0;
            InitializeComponent();
            RandomStarter();
            UpdateScoreboard(GetCurrentPlayer());

            GenerateShips(player1, _greenLight);
            GenerateShips(player2, _greenLight); 



            DrawShip(GetCurrentPlayer().Canvas, GetCurrentPlayer().ShipList.ToArray());
            //DrawShip(player2.Canvas, player2.ShipList.ToArray());
            DrawRemainingShips(GetCurrentPlayer(), RemainingCanvas);

            AiShotButton.Visibility = Visibility.Hidden;
            if (isVsCom)
            {
                AiShotButton.Visibility = Visibility.Visible;
            }
        }

        private List<Ship> GenerateShips(Player player, Brush color)
        {
            List<Ship> resultList = new List<Ship>();

            RemoveShipsFromCanvasAndPlayer(player);
            Random r = new Random();

            //i<=5 -> 5 ships
            for (int i = 5; i >= 2; i--)
            {
                if (i == 2)
                {
                    MakeShip(i, new Vector(r.Next(0, _gameWidth), r.Next(0, _gameHeight)), color, player);
                }
                while (MakeShip(i, new Vector(r.Next(0, _gameWidth), r.Next(0, _gameHeight)), color, player) == null)
                {
                    //MessageBox.Show("Starting position duplicate OR no free space");
                }
            }
            //DrawShip(player.Canvas, player.ShipList.ToArray());
            return resultList;
        }

        private Ship MakeShip(int shipSize, Vector startPosition, Brush color, Player owner)
        {
            //null ha startposition foglalt
            if (owner.AllShipSegments.Contains(startPosition))
            {
                return null;
            }

            int randomFreeDirection = FreeDirectionChooser(shipSize, startPosition, owner);

            //null ha nincs szabad hely
            if (randomFreeDirection == -1)
            {
                //MessageBox.Show("There were no free spaces!");
                return null;
            }

            var segments = new List<Vector>();
            segments.Add(startPosition);

            var directionVector = SetDirectionVector(randomFreeDirection);
            for (int i = 0; i < shipSize - 1; i++)
            {
                startPosition += directionVector;
                segments.Add(startPosition);
            }

            Ship ship = new Ship(startPosition, segments, color, owner);
            owner.AddShip(ship);
            return ship;
        }

        private void DrawShip(Canvas canvas, params Ship[] ships)
        {
            foreach (var ship in ships)
            {
                //Hajo kezdopontjat mas szinnel kirajzol
                /*for (int i = 0; i < s.Segments.Count; i++)
                {
                    if (i == 0)
                    {
                        DrawSingleSegment(s.Segments[i], _green, canvas);
                    }
                    else
                    {
                        DrawSingleSegment(s.Segments[i], s.Color, canvas);
                    }
                }*/
                foreach (var segment in ship.Segments)
                {
                    if (ship.HitSegments.Contains(segment))
                    {
                        //If segment is already hit
                        DrawSingleSegment(segment, _green, canvas, canvas);
                    }
                    else
                    {
                        DrawSingleSegment(segment, ship.Color, canvas, canvas);
                    }  
                }
            }

            //1. Szegmens
            //DrawSingleSegment(ship.StartSegment, /*ship.Color*/ Brushes.Black);
        }

        private int FreeDirectionChooser(int shipSize, Vector position, Player owner)
        {
            ////0: Bal, 1: Fel, 2: Jobb, 3: Le, -1: Nincs szabad
            Random rnd = new Random();
            int randDir = rnd.Next(0, 4);
            int iteration = 1;
            while (!IsDirectionClear(shipSize, position, randDir, owner) && iteration != 4)
            {
                randDir++;
                if (randDir > 3)
                {
                    randDir = 0;
                }
                iteration++;
            }

            if (IsDirectionClear(shipSize, position, randDir, owner))
            {
                //Draw in that direction
                return randDir;
            }
            //Ha semmerre sem szabad az irány:
            return -1;
        }

        private bool IsDirectionClear(int shipSize, Vector position, int direction, Player owner)
        {
            var directionVector = SetDirectionVector(direction);
            for (int i = 0; i < shipSize - 1; i++)
            {
                position += directionVector;
                //Ha foglalt a position, vagy kiesne a pályáról
                if (owner.AllShipSegments.Contains(position) || (position.X < 0) || (position.X > _gameWidth - 1) || (position.Y < 0) || (position.Y > _gameHeight - 1))
                {
                    return false;
                }
            }
            return true;
        }

        private Vector SetDirectionVector(int direction)
        {
            var directionVector = new Vector();
            switch (direction)
            {
                case 0:
                    directionVector = new Vector(-1, 0);
                    break;
                case 1:
                    directionVector = new Vector(0, 1);
                    break;
                case 2:
                    directionVector = new Vector(1, 0);
                    break;
                case 3:
                    directionVector = new Vector(0, -1);
                    break;
                default:
                    break;
            }
            return directionVector;
        }

        private void DrawSingleSegment(Vector position, Brush brush, Canvas canvasForSize, Canvas canvasToDraw)
        {
            //Egy szegmens merete
            var shape = new Rectangle();
            shape.Fill = brush;
            var unitX = canvasForSize.Width / _gameWidth;
            var unitY = canvasForSize.Height / _gameHeight;
            shape.Width = unitX;
            shape.Height = unitY;
            //Szegmens poz.
            Canvas.SetTop(shape, unitY * position.Y);
            Canvas.SetLeft(shape, unitX * position.X);

            //Szegmens hozzaadas/kirajzolas
            canvasToDraw.Children.Add(shape);
        }
        private void DrawSingleShot(Vector position, Brush brush, Canvas canvas)
        {
            //Egy szegmens merete
            var shape = new Rectangle();
            //var rotate = new RotateTransform(45, 25, 50);
            shape.Fill = brush;
            var unitX = canvas.Width / _gameWidth ;
            var unitY = canvas.Height / _gameHeight ;
            //var span = Math.Sqrt((unitX * unitX) + (unitY * unitY));
            //shape.Width = span;
            //shape.Height = span;
            shape.Width = unitX;
            shape.Height = unitY;

            //shape.RenderTransform = rotate;
            //Szegmens poz.
            Canvas.SetTop(shape, unitY * position.Y);
            Canvas.SetLeft(shape, unitX * position.X);
            //Szegmens hozzaadas/kirajzolas
            canvas.Children.Add(shape);
        }
        private void DrawMisses(Player player)
        {
            foreach (var pos in player.MissedShotLocations)
            {
                DrawSingleShot(pos, _missColor, EnemyCanvas);
            }
        }
        private void DrawHits(Player current, Player enemy)
        {
            foreach (var pos in enemy.AllHitShipSegments)
            {
                DrawSingleShot(pos, _hitColor, EnemyCanvas);
            }
        }
        private void DrawRemainingShips(Player player, Canvas canvas)
        {
            Vector start = new Vector(0, 0);
            Vector current = new Vector(0, 0);
            Vector direction = new Vector(1, 0);
            Vector stackingDirection = new Vector(0, 2);

            foreach (var ship in player.ShipList)
            {
                for (int i = 0; i < ship.ShipLength; i++)
                {
                    if (ship.IsDestroyed)
                    {
                        DrawSingleSegment(current, _blue, OwnCanvas, canvas);
                        current += direction;
                    }
                    else
                    {
                        DrawSingleSegment(current, _greenLight, OwnCanvas, canvas);
                        current += direction;
                    }
                }
                start += stackingDirection;
                current = start;
            }

            /*
            for (int i = 5; i <= 1; i--)
            {
                for (int j = 1; j <= i; j++)
                {                   
                    if (i == 1)
                    {
                        DrawSingleSegment(current, _greenLight, OwnCanvas, RemainingCanvas);
                        current += direction;
                    }
                    DrawSingleSegment(current, _greenLight, OwnCanvas, RemainingCanvas);
                    current += direction;
                }
                start += stackingDirection;
                current = start; 
            }*/
            
        }
        private void RemoveShipsFromCanvasAndPlayer(Player owner)
        {
            owner.Canvas.Children.Clear();
            owner.RemoveAllShip();
        }

        private void SwitchPlayerCanvas()
        {
            Canvas tmpCanvas = player1.Canvas;
            player1.Canvas = player2.Canvas;
            player2.Canvas = tmpCanvas;

        }

        private Player GetCurrentPlayer()
        {
            if (player1.Canvas.Name == OwnCanvas.Name)
            {
                //MessageBox.Show("Current: P1 \nplayer1.Canvas.Name = OwnCanvas.Name");
                return player1;
            }
            //MessageBox.Show("!Current: \nplayer1.Canvas.Name != OwnCanvas.Name");
            return player2;
        }
        private Player GetEnemyPlayer()
        {
            if (player2.Canvas.Name == EnemyCanvas.Name)
            {
                //MessageBox.Show("Enemy: P2 \nplayer2.Canvas.Name = EnemyCanvas.Name");
                return player2;
            }
            //MessageBox.Show("Enemy: P1 \nplayer2.Canvas.Name != EnemyCanvas.Name");
            return player1;
        }
       
        private void RedrawCanvases(Player current, Player enemy)
        {
            if (isVsCom_Global && enemy == player1)
            {
                RemainingCanvas.Children.Clear();
                EnemyCanvas.Children.Clear();
                OwnCanvas.Children.Clear();
                //DrawShip(current.Canvas, current.ShipList.ToArray());
                DrawRemainingShips(enemy, RemainingCanvas);
                DrawMisses(current);
                DrawHits(current, enemy);
            }
            else
            {
                RemainingCanvas.Children.Clear();
                EnemyCanvas.Children.Clear();
                OwnCanvas.Children.Clear();
                //DrawShip(enemy.Canvas, enemy.ShipList.ToArray());
                DrawShip(current.Canvas, current.ShipList.ToArray());
                DrawRemainingShips(enemy, RemainingCanvas);
                DrawMisses(current);
                DrawHits(current, enemy);
            }
            

        }
        private Player RandomStarter()
        {
            Random r = new Random();
            int rInt = r.Next(0, 2);
            
            if (rInt == 0)
            {
                player1.Canvas = OwnCanvas;
                player2.Canvas = EnemyCanvas;
                MessageBox.Show("Starter Player: " + player1.Name);
                return player1;
            }
            else if (rInt == 1)
            {
                player1.Canvas = EnemyCanvas;
                player2.Canvas = OwnCanvas;
                MessageBox.Show("Starter Player: " + player2.Name);
                return player2;
            }
            return player2;
        }
        private void UpdateScoreboard(Player player)
        {
            CurrentPlayerNameTextBlock.Text = player.Name;
            TurnTextBlock.Text = turnCounter.ToString();
            HitsTextBlock.Text = player.NumOfHits.ToString();
        }

        private int Action(Vector shotSegment)
        {
            if (GetEnemyPlayer().AllShipSegments.Contains(shotSegment) && !GetEnemyPlayer().AllHitShipSegments.Contains(shotSegment))
            {
                //If HITS and not already hit
                GetEnemyPlayer().AllHitShipSegments.Add(shotSegment);
                //Change ships segment to be HIT
                ChangeShipSegmentToHit(shotSegment);
                //Change ship to DESTROYED if applicable
                GetEnemyPlayer().GetShipBySegment(shotSegment).DestroyTest();
                return 1;
            }
            if (GetEnemyPlayer().AllHitShipSegments.Contains(shotSegment) || GetCurrentPlayer().MissedShotLocations.Contains(shotSegment))
            {
                //Already shot segment being hit
                return 2;
            }
            GetCurrentPlayer().MissedShotLocations.Add(shotSegment);
            return 0;
        }

        private bool TakeAction(Player currentPlayer, Player enemyPlayer)
        {
            Point p = Mouse.GetPosition(EnemyCanvas);
            Vector targetVector = new Vector(Convert.ToInt32(Math.Floor((p.X / _gameWidth / 3))), Convert.ToInt32(Math.Floor((p.Y / _gameHeight / 3))));
            //string posString = "X: " + targetVector.X.ToString() + " Y: " + targetVector.Y.ToString();
            switch (Action(targetVector))
            {
                case 0:
                    //Misses
                    MessageBox.Show("You Missed :(");
                    PassTurn();
                    return false;
                case 1:
                    //Hits
                    //posString += " HIT!";
                    //MessageBox.Show(posString);
                    currentPlayer.GetsScore();

                    MessageBox.Show("You hit a ship!");

                    if (enemyPlayer.AllHitShipSegments.Count == currentPlayer.AllShipSegments.Count)
                    {
                        GameEnd(currentPlayer, enemyPlayer);
                    }
                    PassTurn();
                    return true;
                case 2:
                    //Hits already hit target
                    MessageBox.Show("Target already hit.");
                    break;
                default:
                    break;
            }
            return false;
        }
        private bool Ai_TakeAction(Vector targetVector)
        {
            switch (Action(targetVector))
            {
                case 0:
                    //Misses
                    MessageBox.Show(player2.Name + " Missed.");
                    isPrevAiShotHit = false;
                    PassTurn();
                    return false;
                case 1:
                    //Hits
                    //posString += " HIT!";
                    //MessageBox.Show(posString);
                    player2.GetsScore();
                    isPrevAiShotHit = true;
                    MessageBox.Show(player2.Name + "Hit your ship!");

                    if (player1.AllHitShipSegments.Count == player2.AllShipSegments.Count)
                    {
                        GameEnd(player2, player1);
                    }
                    PassTurn();
                    return true;
                case 2:
                    //Hits already hit target
                    MessageBox.Show("Target already hit.");
                    break;
                default:
                    break;
            }
            return false;
        }

        private Player PassTurn()
        {
            if (isVsCom_Global && GetCurrentPlayer() == player2)
            {
                SwitchPlayerCanvas();
                
                RedrawCanvases(GetCurrentPlayer(), GetEnemyPlayer());
               
                turnCounter++;
                UpdateScoreboard(GetCurrentPlayer());
                return GetCurrentPlayer();
            }
            SwitchPlayerCanvas();
            
            OwnCanvas.Visibility = Visibility.Hidden;
            EnemyCanvas.Visibility = Visibility.Hidden;
            RemainingCanvas.Visibility = Visibility.Hidden;
            MessageBox.Show("Are you ready?", "End of turn", MessageBoxButton.OK, MessageBoxImage.None);
            OwnCanvas.Visibility = Visibility.Visible;
            EnemyCanvas.Visibility = Visibility.Visible;
            RemainingCanvas.Visibility = Visibility.Visible;

            RedrawCanvases(GetCurrentPlayer(), GetEnemyPlayer());
            turnCounter++;
            UpdateScoreboard(GetCurrentPlayer());
            return GetCurrentPlayer();
        }
        private void AiTurn()
        {
            if (GetCurrentPlayer() == player2)
            {
                List<Vector> allComputerShots = player2.MissedShotLocations;
                allComputerShots.AddRange(player1.AllHitShipSegments);
                if (allComputerShots.Count > 0)
                {
                    //If com already shot
                    isPrevAiShotHit = Ai_TakeAction(AiShotDecision(isPrevAiShotHit));
                }
                else
                {
                    //Com first shot
                    isPrevAiShotHit = Ai_TakeAction(AiShotDecision(false));
                }
            }
            else
            {
                MessageBox.Show("It's still your turn!");
            }
        }

        private void ChangeShipSegmentToHit(Vector shotSegment)
        {
            //Change ships segment to be HIT
            GetEnemyPlayer().GetShipBySegment(shotSegment).HitSegments.Add(shotSegment);
            RedrawCanvases(GetCurrentPlayer(), GetEnemyPlayer());
            //TODO if delete this if not needed
        }
        private Vector AiShotDecision(bool isPreviousHit)
        {
            Vector shotToTake = new Vector(0, 0);
            Random r = new Random();
            //int rInt = r.Next();
            shotToTake = new Vector(r.Next(0, 10), r.Next(0, 10));
            List<Vector> allComputerShots = player2.MissedShotLocations;
            allComputerShots.AddRange(player1.AllHitShipSegments);

            if (!isPreviousHit)
            {
                //If previous was miss then random shooting
                while (allComputerShots.Contains(shotToTake))
                {
                    shotToTake = new Vector(r.Next(0, 10), r.Next(0, 10));
                }               
            }
            else
            {
                //If previous was Hit
                Vector guessDirection = new Vector();
                do
                {
                    int rInt = r.Next(0, 5);
                    switch (rInt)
                    {
                        case 0:
                            guessDirection = new Vector(0, 1);
                            break;
                        case 1:
                            guessDirection = new Vector(1, 0);
                            break;
                        case 2:
                            guessDirection = new Vector(0, -1);
                            break;
                        case 3:
                            guessDirection = new Vector(-1, 0);
                            break;
                        default:
                            guessDirection = new Vector(0, 1);
                            break;
                    }
                    shotToTake = player1.AllHitShipSegments.Last() + guessDirection;
                } while (allComputerShots.Contains(shotToTake));
            }
            return shotToTake;
        }
        private void GameEnd(Player currentPlayer, Player enemyPlayer)
        {
            //int Score = 1000 - (turnCounter * 10) - currentPlayer.MissedShotLocations.Count;
            double Score = 1.0 / (double)turnCounter * 100000.0;
            Score = Convert.ToInt32(Score);
            string winStr = "Congratulations!\nThe winner is: " + currentPlayer.Name + "\nScore: " + Score;
            MessageBox.Show(winStr);

            JSONScores scores = new JSONScores((player1.Name, player1.NumOfHits), (player2.Name, player2.NumOfHits), turnCounter, currentPlayer.Name);
            scores.SaveScores(scores);

            //return (int) Score;
            Application.Current.Shutdown();
        }

        private void EnemyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isVsCom_Global)
            {
                TakeAction(GetCurrentPlayer(), GetEnemyPlayer());
            }
            else
            {
                if (GetCurrentPlayer() == player1)
                {
                    TakeAction(GetCurrentPlayer(), GetEnemyPlayer());
                }
            }
            
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                if (isVsCom_Global && GetCurrentPlayer() == player1)
                {
                    DrawShip(GetEnemyPlayer().Canvas, GetEnemyPlayer().ShipList.ToArray());
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AiTurn();
        }
        private void End_Click(object sender, RoutedEventArgs e)
        {
            GameEnd(GetCurrentPlayer(), GetEnemyPlayer());
        }
    }
}
