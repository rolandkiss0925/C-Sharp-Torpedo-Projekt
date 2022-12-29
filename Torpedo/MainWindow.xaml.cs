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
            

            GenerateShips(player1, _blue);
            GenerateShips(player2, _greenLight); 

            DrawShip(player1.Canvas, player1.ShipList.ToArray());
            DrawShip(player2.Canvas, player2.ShipList.ToArray());
        }

        private List<Ship> GenerateShips(Player player, Brush color)
        {
            List<Ship> resultList = new List<Ship>();

            RemoveShipsFromCanvasAndPlayer(player);
            Random r = new Random();

            //i<=5 -> 5 ships
            for (int i = 2; i <= 5; i++)
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
                //MessageBox.Show("Start dupe");
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
            foreach (var s in ships)
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
                foreach (var segment in s.Segments)
                {
                    DrawSingleSegment(segment, s.Color, canvas);
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

        private void DrawSingleSegment(Vector position, Brush brush, Canvas canvas)
        {
            //Egy szegmens merete
            var shape = new Rectangle();
            shape.Fill = brush;
            var unitX = canvas.Width / _gameWidth;
            var unitY = canvas.Height / _gameHeight;
            shape.Width = unitX;
            shape.Height = unitY;
            //Szegmens poz.
            Canvas.SetTop(shape, unitY * position.Y);
            Canvas.SetLeft(shape, unitX * position.X);

            //Szegmens hozzaadas/kirajzolas
            canvas.Children.Add(shape);
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
                MessageBox.Show("Current: P1 \nplayer1.Canvas.Name = OwnCanvas.Name");
                return player1;
            }
            MessageBox.Show("!Current: \nplayer1.Canvas.Name != OwnCanvas.Name");
            return player2;
        }
        private Player GetEnemyPlayer()
        {
            if (player2.Canvas.Name == EnemyCanvas.Name)
            {
                MessageBox.Show("Enemy: P2 \nplayer2.Canvas.Name = EnemyCanvas.Name");
                return player2;
            }
            MessageBox.Show("Enemy: P1 \nplayer2.Canvas.Name != EnemyCanvas.Name");
            return player1;
        }

        private void RedrawPlayers(params Player[] player)
        {
            foreach (var p in player)
            {
                p.Canvas.Children.Clear();
            }
            foreach (var p in player)
            {
                DrawShip(p.Canvas, p.ShipList.ToArray());
            }

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
            RedrawPlayers(player1, player2);
            turnCounter++;

            UpdateScoreboard(GetCurrentPlayer());
            return GetCurrentPlayer();
        }
        private void UpdateScoreboard(Player player)
        {
            CurrentPlayerNameTextBlock.Text = player.Name;
            TurnTextBlock.Text = turnCounter.ToString();
            HitsTextBlock.Text = player.Hits.ToString();
        }

        private bool Action(Vector shotSegment)
        {
            if (GetEnemyPlayer().AllShipSegments.Contains(shotSegment) && !GetEnemyPlayer().AllHitShipSegments.Contains(shotSegment))
            {
                GetEnemyPlayer().AllHitShipSegments.Add(shotSegment);
                //TODO Valami null-t ad vissza
                GetEnemyPlayer().GetShipBySegment(shotSegment).HitSegments.Add(shotSegment);
                return true;
            }
            return false;
        }

        private bool TakeAction()
        {
            Point p = Mouse.GetPosition(EnemyCanvas);
            Vector targetVector = new Vector(Convert.ToInt32(Math.Floor((p.X / _gameWidth / 3))), Convert.ToInt32(Math.Floor((p.Y / _gameHeight / 3))));
            string posString = "X: " + targetVector.X.ToString() + " Y: " + targetVector.Y.ToString();
            if (Action(targetVector))
            {
                posString += " HIT!";
                MessageBox.Show(posString);
                return true;
            }
            else
            {
                posString += " MISS!";
                MessageBox.Show(posString);
                return false;
            }
            
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
            RedrawPlayers(player1, player2);
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
