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

namespace Bankrekening
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BankrekeningDataContext db = new BankrekeningDataContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddCanvas(UserControl UC)
        {
            canvas.Children.Clear();
            canvas.Children.Add(UC);
        }

        private void btnRekeningen_Click(object sender, RoutedEventArgs e)
        {
            UCRekening UCR = new UCRekening(db);
            AddCanvas(UCR);
        }

        private void btnKlanten_Click(object sender, RoutedEventArgs e)
        {
            UCKlanten UCK = new UCKlanten(db);
            AddCanvas(UCK);
        }

        private void btnTypes_Click(object sender, RoutedEventArgs e)
        {
            UCTypes UCT = new UCTypes(db);
            AddCanvas(UCT);
        }
    }
}
