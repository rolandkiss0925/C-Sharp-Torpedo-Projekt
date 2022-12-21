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

namespace kezdokepernyo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal void ShowActivated()
        {
            throw new NotImplementedException();
        }

        private void OnPlayer1vsPlayer2(object sender, RoutedEventArgs e)
        {
            var newWindow = new Player1vsPlayer2();
            newWindow.Show();
            Close();
        }
    }
}
