using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour NouveauPrêts.xaml
    /// </summary>
    public partial class NouveauPrêts : UserControl
    {
        //Class principale de l'interface des types de prets

        public NouveauPrêts()
        {
            InitializeComponent();

            List<PRET> prets = new List<PRET>();
            Types_Prets.ItemsSource = null;
            prets.Clear();
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                PRET pret = new PRET();
                pret.Description = liste.Value.Description;
                if (liste.Value.Disponibilité == 1)
                {
                    pret.Disponible = "oui";
                }
                else
                {
                    pret.Disponible = "non";
                }
                if (liste.Value.Remboursable == 1)
                {
                    pret.Remboursable = "oui";
                }
                else
                {
                    pret.Remboursable = "non";
                }
                prets.Add(pret);
            }

            Types_Prets.ItemsSource = prets;
        }


        //class interne pour permettre l'affectation des données

        public class PRET
        {
            public string Description { get; set; }
            public string Remboursable { get; set; }
            public string Disponible { get; set; }
        }


        //methodes de manupulation de l'interface

        private void Annuler_formulaire_Click(object sender, RoutedEventArgs e)
        {
            Ajout.Visibility = Visibility.Collapsed;
            Remboursable.IsChecked=false;
            Disponible.IsChecked=false;
            Description.Text=null;
        }

        private void Confirmer_formulaire_Click(object sender, RoutedEventArgs e)
        {
            if (Description.Text != "")
            {
                bool a = (bool)Remboursable.IsChecked;
                bool b = (bool)Disponible.IsChecked;
                string c = Description.Text;

                //ajout dans la datagrid

                List<PRET> prets = new List<PRET>();
                Types_Prets.ItemsSource = null;
                prets.Clear();
                foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
                {
                    PRET pret = new PRET();
                    pret.Description = liste.Value.Description;
                    if (liste.Value.Disponibilité == 1)
                    {
                        pret.Disponible = "oui";
                    }
                    else
                    {
                        pret.Disponible = "non";
                    }
                    if (liste.Value.Remboursable == 1)
                    {
                        pret.Remboursable = "oui";
                    }
                    else
                    {
                        pret.Remboursable = "non";
                    }
                    prets.Add(pret);
                }

                PRET nouveauPret = new PRET();
                nouveauPret.Description = c;
                if (b)
                {
                    nouveauPret.Disponible = "oui";
                }
                else
                {
                    nouveauPret.Disponible = "non";
                }
                if (a)
                {
                    nouveauPret.Remboursable = "oui";
                }
                else
                {
                    nouveauPret.Remboursable = "non";
                }
                prets.Add(nouveauPret);
                Types_Prets.ItemsSource = prets;

                int cpt = 1;
                foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
                {
                    if (liste.Value.Type_de_pret >= cpt)
                        cpt = liste.Value.Type_de_pret + 1;
                }
                int dispo = 1;
                if (b == false) dispo = 0;
                int remb = 1;
                if (a == false) remb = 0;
                responsable.Creer_Type_pret(cpt, dispo, c, remb);

                Erreur_formulaire.Visibility = Visibility.Hidden;
                Ajouter_type_label.Visibility = Visibility.Visible;
                DoubleAnimation d = new DoubleAnimation();
                d.From = 1.0; d.To = 0.0;
                d.Duration = new Duration(TimeSpan.FromSeconds(4));
                Ajouter_type_label.BeginAnimation(OpacityProperty, d);

                d.Completed += new EventHandler(d_completed);

            }
            else
            {
                Erreur_formulaire.Visibility = Visibility.Visible;
                Ajouter_type_label.Visibility = Visibility.Hidden;
                DoubleAnimation k = new DoubleAnimation();
                k.From = 1.0; k.To = 0.0;
                k.Duration = new Duration(TimeSpan.FromSeconds(4));
                Erreur_formulaire.BeginAnimation(OpacityProperty, k);
            }
            actualiser();
        }

        private void d_completed(object sender, EventArgs e)
        {
            Ajout.Visibility = Visibility.Collapsed;
            Remboursable.IsChecked = false;
            Disponible.IsChecked = false;
            Description.Text = null;
        }

        private void Display_Note_Click(object sender, RoutedEventArgs e)
        {
            if(Note.Visibility==Visibility.Visible)
            {
                Note.Visibility = Visibility.Hidden;
                Icon_Display_Note.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowDownBoldCircleOutline;
                Display_Note.ToolTip = "Afficher la Note";
            }
            else
            {
                Note.Visibility = Visibility.Visible;
                Icon_Display_Note.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowUpBoldCircleOutline;
                Display_Note.ToolTip = "Cacher la Note";
            }
        }

        private void Confirmer_modification_Click(object sender, RoutedEventArgs e)
        {
            bool b = (bool)Disponible_existant.IsChecked;
            PRET pret = (PRET)Types_Prets.SelectedItem;
            string c = pret.Description;
            int dispo = 1;
            if (b == false) dispo = 0;
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                if (liste.Value.Description.Equals(c))
                {
                    liste.Value.Disponibilité = dispo;
                    responsable.pile_modifications.Add(new Modification(2, liste.Key));
                }
            }

            Mise_a_jour_type.Visibility = Visibility.Visible;
            DoubleAnimation k = new DoubleAnimation();
            k.From = 1.0; k.To = 0.0;
            k.Duration = new Duration(TimeSpan.FromSeconds(4));
            Mise_a_jour_type.BeginAnimation(OpacityProperty, k);
            k.Completed += new EventHandler(k_completed);
            actualiser();
        }

        private void k_completed(object sender, EventArgs e)
        {
            Modification.Visibility = Visibility.Collapsed;
            Disponible_existant.IsChecked = false;
        }

        private void Annuler_modification_Click(object sender, RoutedEventArgs e)
        {
            Modification.Visibility = Visibility.Collapsed;
            Disponible_existant.IsChecked = false;
        }

        private void Modifier_type_Click(object sender, RoutedEventArgs e)
        {
            if (Types_Prets.SelectedItems.Count == 1)
            {
                Modification.Visibility = Visibility.Visible;
            }
            else
            {
                Erreur_Modification.Visibility = Visibility.Visible;
                DoubleAnimation n = new DoubleAnimation();
                n.From = 1.0; n.To = 0.0;
                n.Duration = new Duration(TimeSpan.FromSeconds(4));
                Erreur_Modification.BeginAnimation(OpacityProperty, n);
            }

        }

        private void Ajouter_type_Click(object sender, RoutedEventArgs e)
        {
            Ajout.Visibility = Visibility.Visible;
        }

        private void actualiser()
        {
            List<PRET> prets = new List<PRET>();
            Types_Prets.ItemsSource = null;
            prets.Clear();
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                PRET pret = new PRET();
                pret.Description = liste.Value.Description;
                if (liste.Value.Disponibilité == 1)
                {
                    pret.Disponible = "oui";
                }
                else
                {
                    pret.Disponible = "non";
                }
                if (liste.Value.Remboursable == 1)
                {
                    pret.Remboursable = "oui";
                }
                else
                {
                    pret.Remboursable = "non";
                }
                prets.Add(pret);
            }
            Types_Prets.ItemsSource = prets;
        }
    }
}
