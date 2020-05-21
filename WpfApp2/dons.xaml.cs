using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour Window4.xaml
    /// </summary>

    //Class principale de l'interface de Dons

    public partial class dons : UserControl
    {

        private static string nom_emp_;
        private static string prenom_emp_;
        private static string matricule_emp_;
        private static string num_sec_seoc_emp_;
        private static string grade_emp_;
        private static string etat_emp_;
        private static string ccp_emp_;
        private static string cle_ccp_emp_;
        private static string tel_emp_;
        private static string date_naiss_emp_;
        private static string date_recru_emp_;
        private static string service_emp_;
        private static string email_;
        private static string etat_service_;

        public dons()
        {

            //affectation des données dans la table de données des Dons de l'application 

            InitializeComponent();
            List<don> source = new List<don>();
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
            {
                don d = new don();
                d.cle = liste.Key;
                d.cle_emp = liste.Value.Employé.Cle;
                d.Nom = liste.Value.Employé.Nom;
                d.Prenom = liste.Value.Employé.Prenom;
                d.N_Pv = liste.Value.Num_pv.ToString();
                d.Motif = liste.Value.Motif;
                d.Date_demande = liste.Value.Date_demande.ToShortDateString();
                d.Montant_Prét = liste.Value.Montant.ToString();
                d.Montant_Prét_lettre = liste.Value.Montant_lettre;
                d.type = liste.Value.Type_Pret.Cle.ToString();
                source.Add(d);
            }
            Donnée_dons.ItemsSource = source;
            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {
                liste_employes.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
                liste_employe_rech.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
            }
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                if (liste.Value.Remboursable == 0 && liste.Value.Disponibilité == 1)
                {
                    liste_types.Items.Add(liste.Value.Description);
                }
            }
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                if (liste.Value.Remboursable == 0 && liste.Value.Disponibilité == 1)
                {
                    Type.Items.Add(liste.Value.Description);
                }
            }
        }

        //class interne pour permettre l'affectation des données

        public class don
        {
            public int cle { get; set; }

            public int cle_emp { get; set; }
            public String Nom { get; set; }
            public String Prenom { get; set; }
            public String N_Pv { get; set; }
            public String Date_de_Pv { get; set; }
            public String Motif { get; set; }
            public String Date_demande { get; set; }
            public String Montant_Prét_lettre { get; set; }
            public String Montant_Prét { get; set; }
            public String type { get; set; }
        }


        //methodes de manupulation de l'interface

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            data_grid.Visibility = Visibility.Hidden; data_grid.IsEnabled = false;
            Grid_Ajout.Visibility = Visibility.Visible; Grid_Ajout.IsEnabled = true;
        }

        private void introduire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                liste_employes.Visibility = Visibility.Visible;
            }
        }

        private void Confirmer_Ajout_Click(object sender, RoutedEventArgs e)
        {
            int cpt = 0;
            if (num_pv.Text.Equals("") || date_pv.SelectedDate.Equals(null) || date_dem.SelectedDate.Equals(null) || motif.Text.Equals("") || montant.Text.Equals("") || montant_lettre.Text.Equals(""))
            {
                Remarquee.Visibility = Visibility.Visible;
                DoubleAnimation a = new DoubleAnimation();
                a.From = 1.0; a.To = 0.0;
                a.Duration = new Duration(TimeSpan.FromSeconds(5));
                Remarquee.BeginAnimation(OpacityProperty, a);
            }
            else
            {
                bool verif = true;
                try
                {
                    int k = 0;
                    double d1 = 0;

                    if (!liste_employes.Items.Contains(liste_employes.Text))
                    { MessageBox.Show("entrez un employé valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
                    if (!double.TryParse(montant.Text, out d1))
                    {
                        MessageBox.Show("entrez un montant valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        cpt++;
                    }
                    if (!int.TryParse(num_pv.Text, out k))
                    {
                        MessageBox.Show("entrez un numero de pv valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        cpt++;
                    }
                    if (DateTime.Compare(date_pv.SelectedDate.Value.Date, date_dem.SelectedDate.Value.Date) > 0)
                    {
                        cpt++;
                        MessageBox.Show("La date de demande doit etre inférieure à la date de PV", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (cpt == 0)
                    {
                        if (d1 <= 0)
                        {
                            cpt++;
                            MessageBox.Show("Le montant doit etre positif", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (k <= 0)
                        {
                            cpt++;
                            MessageBox.Show("Le numéro de PV doit etre positif", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    int i = 0;
                    int j = 0;

                    foreach (KeyValuePair<int, Type_pret> value in responsable.liste_types)
                    {
                        if (value.Value.Description.Equals(Type.Text))
                            i = value.Key;
                    }

                    foreach (KeyValuePair<int, Employé> value in responsable.liste_employes)
                    {
                        if ((liste_employes.Text.Split(')'))[0].Equals(value.Key.ToString()))
                        {
                            j = value.Key;
                        }

                    }
                    if (cpt == 0)
                    {
                        responsable.Creer_pret_non_remboursable(j, i, motif.Text, int.Parse(num_pv.Text), date_pv.SelectedDate.Value.Date, Double.Parse(montant.Text), date_dem.SelectedDate.Value.Date, montant_lettre.Text);
                        responsable.tresor = responsable.tresor - Double.Parse(montant.Text);
                    }
                    List<don> source = new List<don>();
                    foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
                    {
                        don d = new don();
                        d.cle = liste.Key;
                        d.Nom = liste.Value.Employé.Nom;
                        d.Prenom = liste.Value.Employé.Prenom;
                        d.N_Pv = liste.Value.Num_pv.ToString();
                        d.Motif = liste.Value.Motif;
                        d.Date_demande = liste.Value.Date_demande.ToShortDateString();
                        d.Montant_Prét = liste.Value.Montant.ToString();
                        d.Montant_Prét_lettre = liste.Value.Montant_lettre;
                        source.Add(d);
                    }
                    Donnée_dons.ItemsSource = source;
                    liste_employes.ItemsSource = null;
                    foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
                    {
                        liste_employes.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
                    }

                }
                catch (Exception r)
                {                    
                    verif = false;
                }
                if (verif)
                {
                    if (cpt == 0)
                    {
                        Grid_Ajout.Visibility = Visibility.Hidden; Grid_Ajout.IsEnabled = false;
                        data_grid.Visibility = Visibility.Visible; data_grid.IsEnabled = true;
                    }
                }
            }

        }

        private void Annuler_Ajout_Click(object sender, RoutedEventArgs e)
        {
            Grid_Ajout.Visibility = Visibility.Hidden; Grid_Ajout.IsEnabled = false;
            data_grid.Visibility = Visibility.Visible; data_grid.IsEnabled = true;
        }

        private void Détails_Click(object sender, RoutedEventArgs e)
        {
            data_grid.Visibility = Visibility.Hidden; data_grid.IsEnabled = false;
            suivi.Visibility = Visibility.Visible;

            pret_non_remboursable pret = null;
            don st = Donnée_dons.SelectedItem as don;
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
            {
                if (st.cle == liste.Key || ((st.Nom.Equals(liste.Value.Employé.Nom) && (st.Prenom.Equals(liste.Value.Employé.Prenom)) && (Int32.Parse(st.Montant_Prét) == liste.Value.Montant))))
                {
                    pret = liste.Value;
                }
            }

            nom_info.Text = pret.Employé.Nom;
            prenom_info.Text = pret.Employé.Prenom;
            nom_detail.Text = pret.Employé.Nom;
            prenom_detail.Text = pret.Employé.Prenom;
            date_nais_info.Text = pret.Employé.Date_naissance.ToShortDateString();
            num_sec_info.Text = pret.Employé.sec_soc;
            matricule_info.Text = pret.Employé.Matricule;
            date_recru_info.Text = pret.Employé.Date_prem.ToShortDateString();
            etat_soc_info.Text = pret.Employé.etats;
            service_info.Text = pret.Employé.Service;
            num_tel_info.Text = pret.Employé.tel;
            ccp_info.Text = pret.Employé.compte_ccp;
            cle_ccp_info.Text = pret.Employé.Cle_ccp;
            grade_info.Text = pret.Employé.Grade;
            description_info.Text = pret.Type_Pret.Description;
            num_pv_info.Text = pret.Num_pv.ToString();
            date_pv_info.Text = pret.Date_pv.ToShortDateString();
            date_demande_info.Text = pret.Date_demande.ToShortDateString();
            montant_info.Text = pret.Montant.ToString();
            montant_lettre_info.Text = pret.Montant_lettre;
            motif_info.Text = pret.Motif;

        }
        private void retourner_detail_emp_Click(object sender, RoutedEventArgs e)
        {
            data_grid.Visibility = Visibility.Visible; data_grid.IsEnabled = true;
            suivi.Visibility = Visibility.Hidden;
        }

        private void liste_employes_TextChanged(object sender, TextChangedEventArgs e)
        {
            liste_employes.Items.Clear();
            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {
                liste_employes.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
            }

            string[] table_combo = new string[liste_employes.Items.Count];
            liste_employes.Items.CopyTo(table_combo, 0);

            string to_search = liste_employes.Text.ToLower();

            if ((to_search != null) && (to_search != ""))
            {
                foreach (string value in table_combo)
                {
                    if (!(value.ToLower().Contains(liste_employes.Text.ToLower())))
                        liste_employes.Items.RemoveAt(liste_employes.Items.IndexOf(value));
                }
            }
            else
            {
                liste_employes.Items.Clear();
                foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
                {
                    liste_employes.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
                }
            }
        }

        //methodes recherche
        private void Recherche_Click(object sender, RoutedEventArgs e)
        {
            data_grid.Visibility = Visibility.Hidden;
            data_grid.IsEnabled = false;
            grid_rech.Visibility = Visibility.Visible;
            grid_rech.IsEnabled = true;
        }

        private void liste_types_TextChanged(object sender, TextChangedEventArgs e)
        {
            liste_types.Items.Clear();
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                if (liste.Value.Remboursable == 0 && liste.Value.Disponibilité == 1)
                {
                    liste_types.Items.Add(liste.Value.Description);
                }
            }

            string[] table_combo = new string[liste_types.Items.Count];
            liste_types.Items.CopyTo(table_combo, 0);

            string to_search = liste_types.Text.ToLower();

            if ((to_search != null) && (to_search != ""))
            {
                foreach (string value in table_combo)
                {
                    if (!(value.ToLower().Contains(liste_types.Text.ToLower())))
                        liste_types.Items.RemoveAt(liste_types.Items.IndexOf(value));
                }
            }
            else
            {
                liste_types.Items.Clear();
                foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
                {
                    if (liste.Value.Remboursable == 0 && liste.Value.Disponibilité == 1)
                    {
                        liste_types.Items.Add(liste.Value.Description);
                    }
                }
            }
        }

        private void liste_employe_rech_TextChanged(object sender, TextChangedEventArgs e)
        {
            liste_employe_rech.Items.Clear();
            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {
                liste_employe_rech.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
            }

            string[] table_combo = new string[liste_employe_rech.Items.Count];
            liste_employe_rech.Items.CopyTo(table_combo, 0);

            string to_search = liste_employe_rech.Text.ToLower();

            if ((to_search != null) && (to_search != ""))
            {
                foreach (string value in table_combo)
                {
                    if (!(value.ToLower().Contains(liste_employe_rech.Text.ToLower())))
                        liste_employe_rech.Items.RemoveAt(liste_employe_rech.Items.IndexOf(value));
                }
            }
            else
            {
                liste_employe_rech.Items.Clear();
                foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
                {
                    liste_employe_rech.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
                }
            }
        }

        private void Confirmer_rech_Click(object sender, RoutedEventArgs e)
        {
            if (!liste_types.Text.Equals(""))
            {
                foreach (KeyValuePair<int, Type_pret> value in responsable.liste_types)
                {
                    if (value.Value.Description.Equals(liste_types.Text))
                        responsable.clés_types.Add(value.Key);
                }
            }
            if (!liste_employe_rech.Text.Equals(""))
            {
                foreach (KeyValuePair<int, Employé> value in responsable.liste_employes)
                {
                    if ((liste_employe_rech.Text.Split(')'))[0].Equals(value.Key.ToString()))
                    {
                        responsable.clés_employés.Add(value.Key);
                    }

                }
            }
          

            responsable.remplissage_liste_filtres_non_rem();

            responsable.filtrer_par_employés_non_rem(!liste_employe_rech.Text.Equals(""));
            responsable.filtrer_par_types_non_rem(!liste_types.Text.Equals(""));

            int cpt = 0;
            double c1 = 0;
            double c2 = 0;

            if (!(date_dem_inf.SelectedDate.Equals(null)))
                responsable.filtrer_par_date_demande_inf_non_rem(!date_dem_inf.Equals(null), date_dem_inf.SelectedDate.Value.Date);
            if (!date_de_sup.SelectedDate.Equals(null))
                responsable.filtrer_par_date_demande_max_non_rem(!date_de_sup.Equals(null), date_de_sup.SelectedDate.Value.Date);
            if (!date_pv_inf.SelectedDate.Equals(null))
                responsable.filtrer_par_date_pv_inf_non_rem(!date_pv_inf.Equals(null), date_pv_inf.SelectedDate.Value.Date);
            if (!date_pv_sup.SelectedDate.Equals(null))
                responsable.filtrer_par_date_pv_max_non_rem(!date_pv_sup.Equals(null), date_pv_sup.SelectedDate.Value.Date);
            if (!double.TryParse(somme_min.Text, out c1))
            {
                cpt++;
                MessageBox.Show("entrez une somme minimale valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                responsable.filtrer_par_somme_min_non_rem(!somme_min.Equals(null), double.Parse(somme_min.Text.ToString()));
            }
            if (!double.TryParse(somme_max.Text, out c2))
            {
                cpt++;
                MessageBox.Show("entrez une somme maximale valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                responsable.filtrer_par_somme_max_non_rem(!somme_max.Equals(null), double.Parse(somme_max.Text.ToString()));
            }

            List<don> source = new List<don>();
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_filtres_non_rem)
            {
                don d = new don();
                d.Nom = liste.Value.Employé.Nom;
                d.Prenom = liste.Value.Employé.Prenom;
                d.N_Pv = liste.Value.Num_pv.ToString();
                d.Motif = liste.Value.Motif;
                d.Date_demande = liste.Value.Date_demande.ToShortDateString();
                d.Montant_Prét = liste.Value.Montant.ToString();
                d.Montant_Prét_lettre = liste.Value.Montant_lettre;
                source.Add(d);
            }
            Donnée_dons.ItemsSource = source;
            grid_rech.Visibility = Visibility.Hidden;
            grid_rech.IsEnabled = false;
            data_grid.Visibility = Visibility.Visible;
            data_grid.IsEnabled = true;
        }

        private void annuler_rech_Click(object sender, RoutedEventArgs e)
        {
            grid_rech.Visibility = Visibility.Hidden;
            grid_rech.IsEnabled = false;
            data_grid.Visibility = Visibility.Visible;
            data_grid.IsEnabled = true;
        }

        //metrhodes archive
        private void Archiver_Click(object sender, RoutedEventArgs e)
        {
            check_box_Archiver.Visibility = Visibility.Visible;
            Options_Principale.Visibility = Visibility.Hidden;
            Options_Principale.IsEnabled = false;

            Options_archiver.Visibility = Visibility.Visible;
            Options_archiver.IsEnabled = true;
        }
        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            var firstCol = Donnée_dons.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            foreach (var item in Donnée_dons.Items)
            {
                i++;
                var chBx = firstCol.GetCellContent(item) as CheckBox;
                DataGridRow row = firstCol.GetCellContent(item) as DataGridRow;
                if (chBx == null)
                {
                    continue;
                }
                if (chBx.IsChecked == true)
                {
                    chBx.Visibility = Visibility.Hidden;
                    int cpt = 0;
                    foreach (don elem in Donnée_dons.ItemsSource)
                    {
                        cpt++;
                        if (i == cpt)
                        {
                            responsable.archiver_manuel_pret_non_remboursable(elem.cle);
                        }
                    }
                }
            }

            List<don> source = new List<don>();
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
            {
                don d = new don();
                d.cle = liste.Key;
                d.Nom = liste.Value.Employé.Nom;
                d.Prenom = liste.Value.Employé.Prenom;
                d.N_Pv = liste.Value.Num_pv.ToString();
                d.Motif = liste.Value.Motif;
                d.Date_demande = liste.Value.Date_demande.ToShortDateString();
                d.Montant_Prét = liste.Value.Montant.ToString();
                d.Montant_Prét_lettre = liste.Value.Montant_lettre;
                source.Add(d);
            }
            Donnée_dons.ItemsSource = source;
        }

        private void Selectionner_Tout_Click(object sender, RoutedEventArgs e)
        {
            var firstCol = Donnée_dons.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            foreach (var item in Donnée_dons.Items)
            {
                var chBx = firstCol.GetCellContent(item) as CheckBox;
                if (chBx == null)
                {
                    continue;
                }
                chBx.IsChecked = true;
            }
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            var firstCol = Donnée_dons.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            foreach (var item in Donnée_dons.Items)
            {
                var chBx = firstCol.GetCellContent(item) as CheckBox;
                if (chBx == null)
                {
                    continue;
                }
                chBx.IsChecked = false;
            }

            check_box_Archiver.Visibility = Visibility.Hidden;
            Options_Principale.Visibility = Visibility.Visible;
            Options_Principale.IsEnabled = true;

            Options_archiver.Visibility = Visibility.Hidden;
            Options_archiver.IsEnabled = false;
        }

        private void Donnée_dons_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (check_box_Archiver.Visibility == Visibility.Visible)
            {
                var firstCol = Donnée_dons.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                foreach (var item in Donnée_dons.Items)
                {
                    var chBx = firstCol.GetCellContent(item) as CheckBox;
                    DataGridRow row = firstCol.GetCellContent(item) as DataGridRow;
                    if ((chBx != null) && (row != null))
                    {
                        if (row.IsSelected)
                        {
                            chBx.IsChecked = true;
                        }
                    }

                }
            }
        }

        private void Sortie_excel_Click(object sender, RoutedEventArgs e)
        {
            responsable.export_prêts_non_remboursable();
        }

        private void Import_excel_Click(object sender, RoutedEventArgs e)
        {
            responsable.import_prêts_non_remboursable();
            Donnée_dons.ItemsSource = null;
            List <don> source = new List<don>();
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
            {
                don d = new don();
                d.cle = liste.Key;
                d.cle_emp = liste.Value.Employé.Cle;
                d.Nom = liste.Value.Employé.Nom;
                d.Prenom = liste.Value.Employé.Prenom;
                d.N_Pv = liste.Value.Num_pv.ToString();
                d.Motif = liste.Value.Motif;
                d.Date_demande = liste.Value.Date_demande.ToShortDateString();
                d.Montant_Prét = liste.Value.Montant.ToString();
                d.Montant_Prét_lettre = liste.Value.Montant_lettre;
                d.type = liste.Value.Type_Pret.Cle.ToString();
                source.Add(d);
            }
            Donnée_dons.ItemsSource = source;
        }
    }
}