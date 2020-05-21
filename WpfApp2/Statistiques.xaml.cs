using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour Statistiques.xaml
    /// </summary>

    public partial class Statistiques : UserControl
    {
        public static int year = 2020;
        public static double montant = 0;
        public static bool s_f = true;

        //Class principale de l'interface de statistiques par graphe

        public Statistiques()
        {
            InitializeComponent();

        }

        //methodes de manupulation de l'interface

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 20;
        }
        public SeriesCollection SeriesCollection3 { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> LFormatter { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        private void first_page_Click(object sender, RoutedEventArgs e)
        {
            Année.Visibility = Visibility.Hidden;
            submit_year.Visibility = Visibility.Hidden;

            first_page_statistiques.Visibility = Visibility.Hidden;
            second_page_grid.Visibility = Visibility.Visible;
        }

        private void second_page_Click(object sender, RoutedEventArgs e)
        {
            Année.Visibility = Visibility.Hidden;
            submit_year.Visibility = Visibility.Hidden;

            first_page_statistiques.Visibility = Visibility.Visible;
            second_page_grid.Visibility = Visibility.Hidden;
        }

        private void confirmation_année(object sender, RoutedEventArgs e)
        {
            int parsedtvalue;
            if (!int.TryParse(Année.Text, out parsedtvalue))
            {
                year = parsedtvalue;
                MessageBox.Show("entrez une année valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                year = int.Parse(Année.Text);
                if (year >= 2000 && year <= 2050)
                {
                    grid_stat_principale.Children.Clear();
                    PointLabel = chartPoint =>
                    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

                    chargement_Piechart();
                    chargement_tresor();
                    chargement_nombre_prêt();

                    //SeriesCollection[3].Values.Add(5d);
                    DataContext = this;

                    grid_stat_principale.Children.Add(second_page_grid);
                    grid_stat_principale.Children.Add(first_page_statistiques);
                    grid_stat_principale.Children.Add(Année);
                    grid_stat_principale.Children.Add(submit_year);

                    Année.Visibility = Visibility.Hidden;
                    submit_year.Visibility = Visibility.Hidden;
                    first_page_statistiques.Visibility = Visibility.Hidden;
                    second_page_grid.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("entrez une année récente", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void chargement_tresor()
        {
            responsable.stat_tresor(year);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Tresor ESI",
                    Values = new ChartValues<double> ( responsable.liste_tresor.Values.AsEnumerable() ),
                    PointGeometry=DefaultGeometries.Circle
                },
            };

            Labels = new[] { "Jan", "", "", "", "Feb", "", "", "", "Mar", "", "", "", "Apr", "", "", "", "", "May", "", "", "", "Jun", "", "", "", "Jul", "", "", "", "Aôu", "", "", "", "", "Sep", "", "", "", "Oct", "", "", "", "Nov", "", "", "", "", "Dec" };

            YFormatter = value => value.ToString("N");
        }
        private void chargement_nombre_prêt()
        {

            responsable.stat_pret_durrée(year);

            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Fill=Brushes.Red,
                    Title="Nombre de prêts",
                    Values=new ChartValues<int>(responsable.liste_stat_1.Values.AsEnumerable())

                },
            };

            string[] s = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aôu", "Sep", "Oct", "Nov", "Dec" };

            //LFormatter = value => value.ToString("N");

        }
        private void chargement_Piechart()
        {
            responsable.stat_type_pret(montant, year);
            if (s_f)
            {
                SeriesCollection3 = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title="Prét social",
                        Values = new ChartValues<double> {responsable.list_sup[1]},
                        PushOut = 0,
                        DataLabels = true,
                        LabelPoint=PointLabel
                    },
                     new PieSeries
                    {
                        Title="Prét Electronmenager",
                        Values = new ChartValues<double> {responsable.list_sup[2]},
                        PushOut = 0,
                        DataLabels = true,
                        LabelPoint=PointLabel
                    },
                      new PieSeries
                    {
                        Title="Dons",
                        Values = new ChartValues<double> {responsable.list_sup[3]},
                        PushOut = 0,
                        DataLabels = true,
                        LabelPoint=PointLabel
                    }

                };
            }
            else
            {
                SeriesCollection3 = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title="Prét social",
                        Values = new ChartValues<double> {responsable.list_inf[1]},
                        PushOut = 0,
                        DataLabels = true,
                        LabelPoint=PointLabel
                    },
                     new PieSeries
                    {
                        Title="Prét Electronmenager",
                        Values = new ChartValues<double> {responsable.list_inf[2]},
                        PushOut = 0,
                        DataLabels = true,
                        LabelPoint=PointLabel
                    },
                      new PieSeries
                    {
                        Title="Dons",
                        Values = new ChartValues<double> {responsable.list_inf[3]},
                        PushOut = 0,
                        DataLabels = true,
                        LabelPoint=PointLabel
                    }

                };
            }
        }

        private void Paramétres_avancées_Click(object sender, RoutedEventArgs e)
        {
            if (Afficher_Paramétres_avancées.Visibility == Visibility.Hidden)
            {
                Afficher_Paramétres_avancées.Visibility = Visibility.Visible;
            }
            else
            {
                montant = 0;
                s_f = true;

                Afficher_Paramétres_avancées.Visibility = Visibility.Hidden;
                second_page_grid.Visibility = Visibility.Hidden;
                grid_stat_principale.Children.Clear();
                PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

                chargement_Piechart();
                chargement_tresor();
                chargement_nombre_prêt();

                //SeriesCollection[3].Values.Add(5d);
                DataContext = this;

                grid_stat_principale.Children.Add(second_page_grid);
                grid_stat_principale.Children.Add(first_page_statistiques);
                grid_stat_principale.Children.Add(Année);
                grid_stat_principale.Children.Add(submit_year);

                Année.Visibility = Visibility.Hidden;
                submit_year.Visibility = Visibility.Hidden;
                first_page_statistiques.Visibility = Visibility.Hidden;
                second_page_grid.Visibility = Visibility.Visible;
            }
        }

        private void Inf_Click(object sender, RoutedEventArgs e)
        {
            s_f = false;
            double parsedtvalue;
            if (double.TryParse(Valeur.Text, out parsedtvalue))
            {

                montant = parsedtvalue;

                second_page_grid.Visibility = Visibility.Hidden;
                grid_stat_principale.Children.Clear();
                PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

                chargement_Piechart();
                chargement_tresor();
                chargement_nombre_prêt();

                //SeriesCollection[3].Values.Add(5d);
                DataContext = this;

                grid_stat_principale.Children.Add(second_page_grid);
                grid_stat_principale.Children.Add(first_page_statistiques);
                grid_stat_principale.Children.Add(Année);
                grid_stat_principale.Children.Add(submit_year);

                Année.Visibility = Visibility.Hidden;
                submit_year.Visibility = Visibility.Hidden;
                first_page_statistiques.Visibility = Visibility.Hidden;
                second_page_grid.Visibility = Visibility.Visible;
                /*
                montant = 7000;
                pie.IsManipulationEnabled = true;
                
                responsable.stat_type_pret(montant, year);
                 this.pie.Series[0].Values[0] = responsable.list_inf[1] ;
                 this.pie.Series[1].Values[0] = responsable.list_inf[2];
                 this.pie.Series[2].Values[0] = responsable.list_inf[3];

                this.pie.Update(true, true);
                 
                 */



            }
            else
            {
                Erreur.Visibility = Visibility.Visible;
                DoubleAnimation a = new DoubleAnimation();
                a.From = 1.0; a.To = 0.0;
                a.Duration = new Duration(TimeSpan.FromSeconds(5));
                Erreur.BeginAnimation(OpacityProperty, a);
            }


        }

        private void Sup_Click(object sender, RoutedEventArgs e)
        {
            s_f = true;
            double parsedtvalue;
            if (double.TryParse(Valeur.Text, out parsedtvalue))
            {
                montant = parsedtvalue;

                second_page_grid.Visibility = Visibility.Hidden;
                grid_stat_principale.Children.Clear();
                PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

                chargement_Piechart();
                chargement_tresor();
                chargement_nombre_prêt();

                //SeriesCollection[3].Values.Add(5d);
                DataContext = this;

                grid_stat_principale.Children.Add(second_page_grid);
                grid_stat_principale.Children.Add(first_page_statistiques);
                grid_stat_principale.Children.Add(Année);
                grid_stat_principale.Children.Add(submit_year);

                Année.Visibility = Visibility.Hidden;
                submit_year.Visibility = Visibility.Hidden;
                first_page_statistiques.Visibility = Visibility.Hidden;
                second_page_grid.Visibility = Visibility.Visible;
                /*
                pie.IsManipulationEnabled = true;

                responsable.stat_type_pret(montant, year);
                SeriesCollection3[0].Values[0] = responsable.list_sup[1];
                SeriesCollection3[1].Values[0] = responsable.list_sup[2];
                SeriesCollection3[2].Values[0] = responsable.list_sup[3];
                */
            }
            else
            {
                Erreur.Visibility = Visibility.Visible;
                DoubleAnimation a = new DoubleAnimation();
                a.From = 1.0; a.To = 0.0;
                a.Duration = new Duration(TimeSpan.FromSeconds(5));
                Erreur.BeginAnimation(OpacityProperty, a);
            }
        }

        private void submit_year_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                year = int.Parse(Année.Text);
            }
            catch (Exception l)
            { }
        }

        private void Inf_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                montant = int.Parse(Valeur.Text);
            }
            catch (Exception s)
            { }
        }

        private void Sup_MouseEnter(object sender, MouseEventArgs e)
        {

            try
            {
                montant = int.Parse(Valeur.Text);
            }
            catch (Exception m)
            { }
        }
       

        private void screenshot1_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)second_page_grid.Width, (int)second_page_grid.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(second_page_grid);
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            string path = dialog.SelectedPath + "\\Stat_image_" + DateTime.Now.Day.ToString()+ "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Second.ToString() + ".jpeg";
            Console.WriteLine(path);
            FileStream stream = File.Create(path);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 90;
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
        }

        private void screenshot2_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(1000, 650, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(first_page_statistiques);
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            string path = dialog.SelectedPath + "\\Stat_image_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Second.ToString() + ".jpeg";
            Console.WriteLine(path);
            FileStream stream = File.Create(path);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 90;
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
        }

        private void retour_1_Click(object sender, RoutedEventArgs e)
        {
            first_page_statistiques.Visibility = Visibility.Hidden;
            second_page_grid.Visibility = Visibility.Hidden;
            Année.Visibility = Visibility.Visible;
            submit_year.Visibility = Visibility.Visible;
        }
    }

}