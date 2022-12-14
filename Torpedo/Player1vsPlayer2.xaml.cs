using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using Torpedo.Models;

namespace Torpedo
{
    public partial class Player1vsPlayer2 : Page
    {
        Brush _greenLight = new SolidColorBrush(Color.FromRgb(161, 239, 140));
        Brush _green = new SolidColorBrush(Color.FromRgb(63, 172, 149));
        Brush _blue = new SolidColorBrush(Color.FromRgb(68, 97, 118));
        Brush _blueDark = new SolidColorBrush(Color.FromRgb(44, 33, 55));

        public Player1vsPlayer2()
        {
            InitializeComponent();
            NameWarning.Visibility = Visibility.Hidden;
        }
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Beginning());
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckName(Player1Name.Text);
                CheckName(Player2Name.Text);
                Player player1 = new Player(Player1Name.Text, null);
                Player player2 = new Player(Player2Name.Text, null);
                var newWindow = new MainWindow(player1, player2, false);
                newWindow.Show(); //Lehet nem sima Show() kell ez okozhat kesobb problemakat
                App.Current.MainWindow.Close();
            }catch(ArgumentException ex)
            {
                //warning
            }
        }

        void CheckName(string name)
        {
            if (!Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
            {
                NameWarning.Visibility = Visibility.Visible;
                //NameWarning.Foreground = _greenLight;
                NameWarning.Foreground = _green;
                throw new ArgumentException();
            }

            /*if (!string.IsNullOrWhiteSpace(name))
            {
                char[] ch = new char[name.Length];

                for (int i = 0; i < name.Length; i++)
                {
                    ch[i] = name[i];
                }

                int count = 0;

                for (int i = 0; i < ch.Length; i++)
                {
                    switch (ch[i])
                    {
                        //NUMBERS
                        case '0':
                            count++;
                            break;
                        case '1':
                            count++;
                            break;
                        case '2':
                            count++;
                            break;
                        case '3':
                            count++;
                            break;
                        case '4':
                            count++;
                            break;
                        case '5':
                            count++;
                            break;
                        case '6':
                            count++;
                            break;
                        case '7':
                            count++;
                            break;
                        case '8':
                            count++;
                            break;
                        case '9':
                            count++;
                            break;

                        //LETTERS UPPER (HUNGARIAN)
                        case 'Ö':
                            count++;
                            break;
                        case 'Ü':
                            count++;
                            break;
                        case 'Ó':
                            count++;
                            break;
                        case 'Q':
                            count++;
                            break;
                        case 'W':
                            count++;
                            break;
                        case 'E':
                            count++;
                            break;
                        case 'R':
                            count++;
                            break;
                        case 'T':
                            count++;
                            break;
                        case 'Z':
                            count++;
                            break;
                        case 'U':
                            count++;
                            break;
                        case 'I':
                            count++;
                            break;
                        case 'O':
                            count++;
                            break;
                        case 'P':
                            count++;
                            break;
                        case 'Ő':
                            count++;
                            break;
                        case 'Ú':
                            count++;
                            break;
                        case 'A':
                            count++;
                            break;
                        case 'S':
                            count++;
                            break;
                        case 'D':
                            count++;
                            break;
                        case 'F':
                            count++;
                            break;
                        case 'G':
                            count++;
                            break;
                        case 'H':
                            count++;
                            break;
                        case 'J':
                            count++;
                            break;
                        case 'K':
                            count++;
                            break;
                        case 'L':
                            count++;
                            break;
                        case 'É':
                            count++;
                            break;
                        case 'Á':
                            count++;
                            break;
                        case 'Ű':
                            count++;
                            break;
                        case 'Í':
                            count++;
                            break;
                        case 'Y':
                            count++;
                            break;
                        case 'X':
                            count++;
                            break;
                        case 'C':
                            count++;
                            break;
                        case 'V':
                            count++;
                            break;
                        case 'B':
                            count++;
                            break;
                        case 'N':
                            count++;
                            break;
                        case 'M':
                            count++;
                            break;

                        //LETTERS LOWER (HUNGARIAN)
                        case 'ö':
                            count++;
                            break;
                        case 'ü':
                            count++;
                            break;
                        case 'ó':
                            count++;
                            break;
                        case 'q':
                            count++;
                            break;
                        case 'w':
                            count++;
                            break;
                        case 'e':
                            count++;
                            break;
                        case 'r':
                            count++;
                            break;
                        case 't':
                            count++;
                            break;
                        case 'z':
                            count++;
                            break;
                        case 'u':
                            count++;
                            break;
                        case 'i':
                            count++;
                            break;
                        case 'o':
                            count++;
                            break;
                        case 'p':
                            count++;
                            break;
                        case 'ő':
                            count++;
                            break;
                        case 'ú':
                            count++;
                            break;
                        case 'a':
                            count++;
                            break;
                        case 's':
                            count++;
                            break;
                        case 'd':
                            count++;
                            break;
                        case 'f':
                            count++;
                            break;
                        case 'g':
                            count++;
                            break;
                        case 'h':
                            count++;
                            break;
                        case 'j':
                            count++;
                            break;
                        case 'k':
                            count++;
                            break;
                        case 'l':
                            count++;
                            break;
                        case 'é':
                            count++;
                            break;
                        case 'á':
                            count++;
                            break;
                        case 'ű':
                            count++;
                            break;
                        case 'í':
                            count++;
                            break;
                        case 'y':
                            count++;
                            break;
                        case 'x':
                            count++;
                            break;
                        case 'c':
                            count++;
                            break;
                        case 'v':
                            count++;
                            break;
                        case 'b':
                            count++;
                            break;
                        case 'n':
                            count++;
                            break;
                        case 'm':
                            count++;
                            break;


                        case ' ':
                            count++;
                            break;

                        default:
                            // nothing happens
                            break;
                    }
                }

                if ((count == ch.Length) && (ch[0] != ' ') && (ch[ch.Length - 1] != ' '))
                {
                    return;
                }
            }*/
            
        }
        
    }
}
