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
        Brush _tmpColor = new SolidColorBrush(Color.FromRgb(255, 255, 0));

        int turnCounter;

        //Ideiglenes
        Player player1 = new Player("", null);
        Player player2 = new Player("", null);

        public MainWindow()
        {
            InitializeComponent();
            RandomStarter();
            //---------Player 1 próba--------
            

            UpdateScoreboard(GetCurrentPlayer());

            //Ship ship1 = MakeShip(3, (new Vector(0, 0)), _neonGreen, player1);
            //Ship ship2 = MakeShip(2, (new Vector(2, 4)), _neonGreen, player1);
            //Ship ship3 = MakeShip(5, (new Vector(6, 8)), _neonGreen, player1);
            //Ship ship4 = MakeShip(4, (new Vector(9, 2)), _neonGreen, player1);

            //player1.AddShip(ship1, ship2, ship3, ship4);
            //-------------------------------

            //---------Player 2 próba--------

            /*
            Ship ship11 = MakeShip(2, (new Vector(0, 0)), _lightGreen, player2);
            Ship ship22 = MakeShip(3, (new Vector(2, 2)), _lightGreen, player2);
            Ship ship33 = MakeShip(5, (new Vector(4, 4)), _lightGreen, player2);
            Ship ship44 = MakeShip(5, (new Vector(6, 6)), _lightGreen, player2);
            Ship ship55 = MakeShip(4, (new Vector(8, 8)), _lightGreen, player2);
            */
            //player2.AddShip(ship11, ship22, ship33, ship44, ship55);
            //-------------------------------

            turnCounter = 0;
            

            GenerateShips(player1, _greenLight);
            GenerateShips(player2, _greenLight); 

            DrawShip(player1.Canvas, player1.ShipList.ToArray());
            DrawShip(player2.Canvas, player2.ShipList.ToArray());
            DrawRemainingShips(GetCurrentPlayer(), RemainingCanvas);
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
        private void DrawSingleMiss(Vector position, Brush brush, Canvas canvas)
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
                DrawSingleMiss(pos, _tmpColor, EnemyCanvas);
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
                        DrawSingleSegment(current, _tmpColor, OwnCanvas, canvas);
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
            RemainingCanvas.Children.Clear();
            EnemyCanvas.Children.Clear();
            OwnCanvas.Children.Clear();
            DrawShip(enemy.Canvas, enemy.ShipList.ToArray());
            DrawShip(current.Canvas, current.ShipList.ToArray());
            DrawRemainingShips(enemy, RemainingCanvas);
            DrawMisses(current);

        }
        private void RandomStarter()
        {
            Random r = new Random();
            int rInt = r.Next(0, 2);
            
            if (rInt == 0)
            {
                player1 = new Player("Player 1", OwnCanvas);
                player2 = new Player("Player 2", EnemyCanvas);
                MessageBox.Show("Player 1 set to OWNCANVAS");
            }
            else if (rInt == 1)
            {
                player1 = new Player("Player 1", EnemyCanvas);
                player2 = new Player("Player 2", OwnCanvas);
                MessageBox.Show("Player 1 set to ENEMY");
            }
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

        private bool TakeAction()
        {
            Point p = Mouse.GetPosition(EnemyCanvas);
            Vector targetVector = new Vector(Convert.ToInt32(Math.Floor((p.X / _gameWidth / 3))), Convert.ToInt32(Math.Floor((p.Y / _gameHeight / 3))));
            string posString = "X: " + targetVector.X.ToString() + " Y: " + targetVector.Y.ToString();
            switch (Action(targetVector))
            {
                case 0:
                    //Misses
                    posString += " MISS!";
                    MessageBox.Show(posString);
                    PassTurn();
                    return false;
                case 1:
                    //Hits
                    posString += " HIT!";
                    MessageBox.Show(posString);
                    GetCurrentPlayer().GetsScore();
                    PassTurn();
                    return true;
                case 2:
                    //Hits already hit target
                    MessageBox.Show("Target already hit!");
                    break;
                default:
                    break;
            }
            return false;
        }

        private Player PassTurn()
        {
            OwnCanvas.Visibility = Visibility.Hidden;
            EnemyCanvas.Visibility = Visibility.Hidden;
            RemainingCanvas.Visibility = Visibility.Hidden;
            MessageBox.Show("Are you ready?", "End of turn", MessageBoxButton.OK, MessageBoxImage.None);
            OwnCanvas.Visibility = Visibility.Visible;
            EnemyCanvas.Visibility = Visibility.Visible;
            RemainingCanvas.Visibility = Visibility.Visible;

            SwitchPlayerCanvas();
            RedrawCanvases(GetCurrentPlayer(), GetEnemyPlayer());
            turnCounter++;

            UpdateScoreboard(GetCurrentPlayer());
            return GetCurrentPlayer();
        }

        private void ChangeShipSegmentToHit(Vector shotSegment)
        {
            //Change ships segment to be HIT
            GetEnemyPlayer().GetShipBySegment(shotSegment).HitSegments.Add(shotSegment);
            RedrawCanvases(GetCurrentPlayer(), GetEnemyPlayer());
            //TODO if delete this if not needed
        }

        private void GenerateP1_Click(object sender, RoutedEventArgs e)
        {
            GenerateShips(player1, _greenLight);
            DrawShip(player1.Canvas, player1.ShipList.ToArray());
        }

        private void GenerateP2_Click(object sender, RoutedEventArgs e)
        {
            GenerateShips(player2, _green);
            DrawShip(player2.Canvas, player2.ShipList.ToArray());
        }

        private void SwitchP_Click(object sender, RoutedEventArgs e)
        {
            SwitchPlayerCanvas();
            RedrawCanvases(GetCurrentPlayer(), GetEnemyPlayer());
        }
        private void PassTurn_Click(object sender, RoutedEventArgs e)
        {
            PassTurn();
        }

        private void EnemyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TakeAction();
        }
    }
}
