using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Torpedo.Models;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for PlayervsComputer.xaml
    /// </summary>
    public partial class PlayervsComputer : Page
    {
        public PlayervsComputer()
        {
            InitializeComponent();
        }
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Beginning());
        }
        private void OnStart(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckName(PlayerName.Text);

                Player player1 = new Player(PlayerName.Text, null);
                Player com = new Player("COM", null);
                var newWindow = new MainWindow(player1, com);
                newWindow.Show(); //Lehet nem sima Show() kell ez okozhat kesobb problemakat
                App.Current.MainWindow.Close();

                //MÁSHOVA KELL A NAVIGÁLÁS --> AI

                /*var newWindow = new MainWindow();
                newWindow.CurrentPlayerNameTextBlock.Text = PlayerName.Text;
                //newWindow.Name.ToString() = Player1Name.Text;
                newWindow.Show(); //Lehet nem sima Show() kell ez okozhat kesobb problemakat
                App.Current.MainWindow.Close();*/
            }
            catch (ArgumentException ex)
            {
                //warning
            }
        }

        void CheckName(string name)
        {
            if (!Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
            {
                throw new ArgumentException();
            }
        }
    }
}

