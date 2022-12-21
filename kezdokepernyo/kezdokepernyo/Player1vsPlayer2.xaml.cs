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
using System.Windows.Shapes;

namespace kezdokepernyo
{
    /// <summary>
    /// Interaction logic for Player1vsPlayer2.xaml
    /// </summary>
    public partial class Player1vsPlayer2 : Window
    {
        public Player1vsPlayer2()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.ShowActivated();
            Close();
        }
    }
}
