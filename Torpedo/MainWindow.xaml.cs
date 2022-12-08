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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GameWidth = 10;
        private const int GameHeight = 10;

        List<Vector> OccupiedSpaces = new List<Vector>();

        Brush _neonGreen = new SolidColorBrush(Color.FromRgb(161, 239, 140));

        //Lehet nem szukseges
        IDictionary<string, int> ShipType = new Dictionary<string, int>() {
            {"CARRIER_5", 5 },
            {"BATTLESHIP_4", 4 },
            {"SUBMARINE_3", 3 },
            {"DESTROYER_2", 2 },
        };

        public MainWindow()
        {
            InitializeComponent();

            var player1 = new Player("Toldi Miklos");

            //Ship ship1 = new Ship(new Vector(4, 4), )
            Ship ship1 = MakeShip(3, (new Vector(0, 0)), _neonGreen, player1);
            Ship ship2 = MakeShip(2, (new Vector(2, 4)), _neonGreen, player1);
            Ship ship3 = MakeShip(5, (new Vector(6, 8)), _neonGreen, player1);
            Ship ship4 = MakeShip(4, (new Vector(9, 2)), _neonGreen, player1);

            player1.AddShip(ship1, ship2, ship3, ship4);

            DrawShip(ship1, ship2, ship3, ship4);
        }

        private Ship MakeShip(int shipSize, Vector startPosition, Brush color, Player owner)
        {
            var segments = new List<Vector>();
            segments.Add(startPosition);
            //TODO: Ellenorozni, hogy szabad-e a hely!!

            int randomFreeDirection = FreeDirectionChooser(shipSize, startPosition);
            //TODO: Ha -1 a return, akkor nem szabalyos a pozicio, mert nem fer el
            var directionVector = SetDirectionVector(randomFreeDirection);
            for (int i = 0; i < shipSize--; i++)
            {
                startPosition += directionVector;
                segments.Add(startPosition);
            }

            Ship ship = new Ship(startPosition, segments, color, owner);
            return ship;
        }

        private void DrawShip(params Ship[] ships)
        {
            foreach (var s in ships)
            {
                foreach (var segment in s.Segments)
                {
                    DrawSingleSegment(segment, s.Color);
                }
            }
            
            //1. Szegmens
            //DrawSingleSegment(ship.StartSegment, /*ship.Color*/ Brushes.Black);
        }

        ////0: Bal, 1: Fel, 2: Jobb, 3: Le, -1: Nincs szabad
        private int FreeDirectionChooser(int shipSize, Vector position)
        {
            Random rnd = new Random();
            //0: Bal, 1: Fel, 2: Jobb, 3: Le
            int randDir = rnd.Next(0, 4);
            int iteration = 1;
            while (!IsDirectionClear(shipSize, position, randDir) && iteration != 4)
            {
                randDir++;
                if (randDir > 3)
                {
                    randDir = 0;
                }
                iteration++;
            }

            if (IsDirectionClear(shipSize, position, randDir))
            {
                //Draw in that direction
                return randDir;
            }
            //Ha semmerre sem szabad az irány:
            return -1;
        }

        private bool IsDirectionClear(int shipSize, Vector position, int direction)
        {
            var directionVector = SetDirectionVector(direction);
            for (int i = 0; i < shipSize--; i++)
            {
                position += directionVector;
                //Ha foglalt a position, vagy kiesne a pályáról
                if (OccupiedSpaces.Contains(position) || (position.X < 0) || (position.X > GameWidth - 1) || (position.Y < 0) || (position.Y > GameHeight - 1))
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

        private void DrawSingleSegment(Vector position, Brush brush)
        {
            //Egy szegmens merete
            var shape = new Rectangle();
            shape.Fill = brush;
            var unitX = OwnCanvas.Width / GameWidth;
            var unitY = OwnCanvas.Height / GameHeight;
            shape.Width = unitX;
            shape.Height = unitY;
            //Szegmens poz.
            Canvas.SetTop(shape, unitY * position.Y);
            Canvas.SetLeft(shape, unitX * position.X);
            //szegmens hozzaadasa a foglalt helyekhez
            OccupiedSpaces.Add(position);
            //Szegmens hozzaadas/kirajzolas
            OwnCanvas.Children.Add(shape);
        }

        private void ClearCanvasFromShips()
        {

        }
    }
}
