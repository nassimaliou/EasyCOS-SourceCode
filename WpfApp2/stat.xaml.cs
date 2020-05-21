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

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour stat.xaml
    /// </summary>
    
    //fenetre intermediaire pour choix du mode de statistiques

    public partial class stat : UserControl
    {
        public stat()
        {
            InitializeComponent();
        }
        private void On_graph_Click(object sender, RoutedEventArgs e)
        {
            Grid_Principale.Children.Clear();
            Grid_Principale.Children.Add(new Statistiques());
        }

        private void On_list_Click(object sender, RoutedEventArgs e)
        {
            Grid_Principale.Children.Clear();
            Grid_Principale.Children.Add(new Window3());
        }
    }
}
