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
    /// Logique d'interaction pour Suivi_Prét.xaml
    /// </summary>

    public partial class Suivi_Prét : UserControl
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
        private static string service_;
        private static string montant;
        private static int nb_de_mois;
        private static int year_pret;
        private static string email_;
        private static string etat_service_;


        //Class principale de l'interface de suivi des prets

        public Suivi_Prét()
        {
            InitializeComponent();
            actualiser();
            methode_prelevement.Items.Add("Paiement Standard.");
            methode_prelevement.Items.Add("Paiement sur plusieurs mois.");
            methode_prelevement.Items.Add("Paiement Anticipé (Total).");
            methode_prelevement.Items.Add("Retardement.");
            methode_prelevement.Items.Add("Paiement Libre");
        }

        //class interne pour permettre l'affectation des données

        public class employee
        {
            public String Nom { get; set; }
            public String Prenom { get; set; }
            public String N_Pv { get; set; }
            public String Type_Prêt { get; set; }
            public String Date_de_Pv { get; set; }
            public String Date_demande { get; set; }
            public String Montant_Prét { get; set; }

        }


        //methodes de manupulation de l'interface

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            data_grid.Visibility = Visibility.Hidden; data_grid.IsEnabled = false;
            Grid_Ajout.Visibility = Visibility.Visible; Grid_Ajout.IsEnabled = true;
        }


        private void Selectionner_Tout_Click(object sender, RoutedEventArgs e)
        {
            var firstCol = Donnée_Suivi_Prêt.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            foreach (var item in Donnée_Suivi_Prêt.Items)
            {
                var chBx = firstCol.GetCellContent(item) as CheckBox;
                if (chBx == null)
                {
                    continue;
                }
                chBx.IsChecked = true;
            }

        }

        private void Selection_Tout_Click(object sender, RoutedEventArgs e)
        {
            var firstCol = Donnée_Suivi_Prêt.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 1);
            foreach (var item in Donnée_Suivi_Prêt.Items)
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
            var firstCol = Donnée_Suivi_Prêt.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);

            foreach (var item in Donnée_Suivi_Prêt.Items)
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

        private void annule_Click(object sender, RoutedEventArgs e)
        {
            var firstCol = Donnée_Suivi_Prêt.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 1);

            foreach (var item in Donnée_Suivi_Prêt.Items)
            {
                var chBx = firstCol.GetCellContent(item) as CheckBox;
                if (chBx == null)
                {
                    continue;
                }
                chBx.IsChecked = false;
            }
            check_box_effacer.Visibility = Visibility.Hidden;
            Options_Principale.Visibility = Visibility.Visible;
            Options_Principale.IsEnabled = true;

            Options_effacer.Visibility = Visibility.Hidden;
            Options_effacer.IsEnabled = false;
        }

        private void Confirmer_Ajout_Click(object sender, RoutedEventArgs e)
        {
            Remarque.Visibility = Visibility.Visible;
            DoubleAnimation a = new DoubleAnimation();
            a.From = 1.0; a.To = 0.0;
            a.Duration = new Duration(TimeSpan.FromSeconds(5));
            Remarque.BeginAnimation(OpacityProperty, a);
            int k = 0;
            int cpt = 0;
            int i = 0;
            int j = 0;

            foreach (KeyValuePair<int, Type_pret> value in responsable.liste_types)
            {
                if (value.Value.Description.Equals(Prêt_Type_ajout.Text))
                    i = value.Key;
            }

            foreach (KeyValuePair<int, Employé> value in responsable.liste_employes)
            {
                if (liste_employes.Text.Split(')')[0].Equals(value.Key.ToString()))
                {
                    j = value.Key;
                }
            }

            if (Prêt_Type_ajout.SelectedIndex == -1)
            {
                MessageBox.Show("Choisissez un type de prêt", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                cpt++;
            }
            double d = 0;
            if (!double.TryParse(Montant_Prêt_ajout.Text, out d))
            {
                MessageBox.Show("Entrez un montant valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                cpt++;
            }
            if (!int.TryParse(Numero_Pv_ajout.Text, out k))
            {
                MessageBox.Show("Entrez un numero de pv valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                cpt++;
            }
            if (!int.TryParse(durée.Text, out k))
            {
                MessageBox.Show("Entrez une durée de paiement valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                cpt++;
            }    
            if(Date_Demande_ajout.SelectedDate == null)
            {
                MessageBox.Show("Choisissez une date de demande", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                cpt++;
            }
            if (Date_prem_paiement.SelectedDate == null)
            {
                MessageBox.Show("Choisissez une date de premier paiement", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                cpt++;
            }    
            if (Date_pv_ajout.SelectedDate == null)
            {
                MessageBox.Show("Entrez la date du PV", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                cpt++;
            }
            if (cpt == 0)
            {
                if (DateTime.Compare(Date_pv_ajout.SelectedDate.Value.Date, Date_Demande_ajout.SelectedDate.Value.Date) < 0)
                {
                    cpt++;
                    MessageBox.Show("La date de demande doit etre inférieure à la date de PV", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (DateTime.Compare(Date_prem_paiement.SelectedDate.Value.Date, Date_Demande_ajout.SelectedDate.Value.Date) < 0)
                {
                    cpt++;
                    MessageBox.Show("La date de demande doit etre inférieure à la date de premier paiement", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (DateTime.Compare(Date_prem_paiement.SelectedDate.Value.Date, Date_pv_ajout.SelectedDate.Value.Date) < 0)
                {
                    cpt++;
                    MessageBox.Show("La date de PV doit etre inférieure à la date de premier paiement", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (d <= 0)
                {
                    cpt++;
                    MessageBox.Show("Le montant doit etre positif", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (k <= 0)
                {
                    cpt++;
                    MessageBox.Show("La durée de remboursement doit etre positive", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                int pvv = int.Parse(Numero_Pv_ajout.Text);
                if (pvv <= 0)
                {
                    cpt++;
                    MessageBox.Show("Le numéro de PV doit etre positif", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (cpt == 0)
            {
                responsable.Creer_pret_remboursable(j, i, Motif_ajout.Text, Int32.Parse(Numero_Pv_ajout.Text), DateTime.Parse(Date_pv_ajout.SelectedDate.ToString()), Double.Parse(Montant_Prêt_ajout.Text), DateTime.Parse(Date_Demande_ajout.SelectedDate.ToString()), Montant_Prêt_lettre_ajout.Text, DateTime.Parse(Date_prem_paiement.SelectedDate.ToString()), Int32.Parse(durée.Text));
                responsable.affiche_liste_pret_remboursable();
                actualiser();
                data_grid.Visibility = Visibility.Visible; data_grid.IsEnabled = true;
                Grid_Ajout.Visibility = Visibility.Hidden; Grid_Ajout.IsEnabled = false;
                Prêt_Type_ajout.SelectedIndex = -1;
            }            
        }

        private void Annuler_Ajout_Click(object sender, RoutedEventArgs e)
        {
            Grid_Ajout.Visibility = Visibility.Hidden; Grid_Ajout.IsEnabled = false;
            data_grid.Visibility = Visibility.Visible; data_grid.IsEnabled = true;
        }

        private void Détails_Click(object sender, RoutedEventArgs e)
        {
            janvier.Text = "";
            fevrier.Text = "";
            mars.Text = "";
            avril.Text = "";
            mai.Text = "";
            juin.Text = "";
            juillet.Text = "";
            aout.Text = "";
            septembre.Text = "";
            octobre.Text = "";
            novembre.Text = "";
            decembre.Text = "";
            Annee_suivi.Text = "";
            data_grid.Visibility = Visibility.Hidden; data_grid.IsEnabled = false;
            suivi.Visibility = Visibility.Visible; suivi.IsEnabled = true;
            employee st = Donnée_Suivi_Prêt.SelectedItem as employee;
            pret_remboursable pret = null;
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Date_demande) && DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Date_pv) && Double.Parse(st.Montant_Prét) == liste.Value.Montant && st.Nom.Equals(liste.Value.Employé.Nom) && st.Prenom.Equals(liste.Value.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Num_pv && st.Type_Prêt.Equals(liste.Value.Type_Pret.Description) && liste.Value.isPere())
                {
                    pret = liste.Value;
                }
            }
            int mois = pret.Date_premier_paiment.Month;
            int year = pret.Date_premier_paiment.Year;
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
            date_prem_paiement_info.Text = pret.Date_premier_paiment.ToShortDateString();
            date_demande_info.Text = pret.Date_demande.ToShortDateString();
            montant_info.Text = pret.Montant.ToString();
            montant_lettre_info.Text = pret.Montant_lettre;
            motif_info.Text = pret.Motif;
            if (pret.Debordement == -1)
            {
                montant_remboursé.Text = pret.Somme_remboursée.ToString();
                monatant_restant.Text = (pret.Montant - pret.Somme_remboursée).ToString();
                if(pret.Montant != pret.Somme_remboursée)
                    date_prochain_info.Text = pret.Date_actuelle.ToShortDateString();
                else
                    date_prochain_info.Text = " / ";
            }
            else
            {
                pret_remboursable p = pret;
                while (p.Debordement != -1)
                {
                    p = p.getFils();
                }
                montant_remboursé.Text = p.Somme_remboursée.ToString();
                monatant_restant.Text = (p.Montant - p.Somme_remboursée).ToString();
                if (p.Montant != p.Somme_remboursée)
                    date_prochain_info.Text = p.Date_actuelle.ToShortDateString();
                else
                    date_prochain_info.Text = " / ";
            }
            Annee_suivi.Text = "          " + pret.Date_demande.Year.ToString();
            switch (pret.Date_demande.Month)
            {
                case 1: { janvier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 2: { fevrier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 3: { mars.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 4: { avril.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 5: { mai.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 6: { juin.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 7: { juillet.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 8: { aout.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 9: { septembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 10: { octobre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 11: { novembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                case 12: { decembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
            }
            if (pret.Date_demande.Year.Equals(pret.Date_pv.Year))
            {
                switch (pret.Date_pv.Month)
                {
                    case 1: { janvier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 2: { fevrier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 3: { mars.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 4: { avril.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 5: { mai.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 6: { juin.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 7: { juillet.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 8: { aout.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 9: { septembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 10: { octobre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 11: { novembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                    case 12: { decembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                }
            }
            if (pret.Debordement == -1)
            {
                year_pret = pret.Date_demande.Year;
                Dictionary<DateTime, double> suivi = new Dictionary<DateTime, double>();
                DateTime paiement = pret.Date_premier_paiment;
                double sum = 0;
                foreach (KeyValuePair<int, double> kvp in pret.Etat)
                {
                    if (kvp.Value != -1 && sum < pret.Montant)
                    {
                        sum = sum + kvp.Value;
                        suivi.Add(paiement, kvp.Value);
                        paiement = paiement.AddMonths(1);
                    }
                }
                foreach (KeyValuePair<DateTime, double> liste in suivi)
                {
                    if (liste.Key.Year == pret.Date_demande.Year)
                    {
                        if (liste.Value == pret.Montant / pret.Durée)
                        {
                            switch (liste.Key.Month)
                            {
                                case 1: { janvier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 2: { fevrier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 3: { mars.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 4: { avril.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 5: { mai.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 6: { juin.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 7: { juillet.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 8: { aout.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 9: { septembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 10: { octobre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 11: { novembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 12: { decembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                            }
                        }
                        else
                        {
                            if ((liste.Value == 0))
                            {
                                switch (liste.Key.Month)
                                {
                                    case 1: { janvier.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 2: { fevrier.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 3: { mars.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 4: { avril.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 5: { mai.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 6: { juin.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 7: { juillet.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 8: { aout.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 9: { septembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 10: { octobre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 11: { novembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 12: { decembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                }
                            }
                            else
                            {
                                if (liste.Value == (pret.Montant - pret.Somme_remboursée))
                                {
                                    switch (liste.Key.Month)
                                    {
                                        case 1: { janvier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 2: { fevrier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 3: { mars.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 4: { avril.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 5: { mai.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 6: { juin.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 7: { juillet.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 8: { aout.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 9: { septembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 10: { octobre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 11: { novembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 12: { decembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                    }
                                }
                                else
                                {
                                    if (liste.Value != -1)
                                    {
                                        switch (liste.Key.Month)
                                        {
                                            case 1: { janvier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 2: { fevrier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 3: { mars.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 4: { avril.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 5: { mai.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 6: { juin.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 7: { juillet.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 8: { aout.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 9: { septembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 10: { octobre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 11: { novembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 12: { decembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                double somme = 0;
                foreach (pret_remboursable p in responsable.liste_pret_remboursable.Values)
                {
                    if (p.Debordement == pret.Cle)
                    {
                        somme = p.Somme_remboursée;
                    }
                }
                year_pret = pret.Date_demande.Year;
                List<double> mois_debor = new List<double>();
                while (pret.Debordement != -1)
                {
                    foreach (double d in pret.Etat.Values)
                    {
                        if (somme != pret.Montant)
                        {
                            somme = somme + d;
                            mois_debor.Add(d);
                        }
                    }
                    pret = pret.getFils();
                }
                foreach (double d in pret.Etat.Values)
                {
                    //if (somme != pret.Montant)
                    //{
                    somme = somme + d;
                    mois_debor.Add(d);
                    // }
                }

                Dictionary<DateTime, double> suivi = new Dictionary<DateTime, double>();
                DateTime paiement = pret.Date_premier_paiment;
                foreach (double d in mois_debor)
                {
                    if (d != -1)
                    {
                        suivi.Add(paiement, d);
                        paiement = paiement.AddMonths(1);
                    }
                }

                foreach (KeyValuePair<DateTime, double> liste in suivi)
                {
                    if (liste.Key.Year == pret.Date_demande.Year)
                    {
                        if (liste.Value == pret.Montant / pret.Durée)
                        {
                            switch (liste.Key.Month)
                            {
                                case 1: { janvier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 2: { fevrier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 3: { mars.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 4: { avril.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 5: { mai.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 6: { juin.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 7: { juillet.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 8: { aout.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 9: { septembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 10: { octobre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 11: { novembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                case 12: { decembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                            }
                        }
                        else
                        {
                            if ((liste.Value == 0))
                            {
                                switch (liste.Key.Month)
                                {
                                    case 1: { janvier.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 2: { fevrier.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 3: { mars.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 4: { avril.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 5: { mai.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 6: { juin.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 7: { juillet.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 8: { aout.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 9: { septembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 10: { octobre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 11: { novembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    case 12: { decembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                }
                            }
                            else
                            {
                                if (liste.Value == (pret.Montant - pret.Somme_remboursée))
                                {
                                    switch (liste.Key.Month)
                                    {
                                        case 1: { janvier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 2: { fevrier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 3: { mars.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 4: { avril.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 5: { mai.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 6: { juin.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 7: { juillet.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 8: { aout.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 9: { septembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 10: { octobre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 11: { novembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        case 12: { decembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                    }
                                }
                                else
                                {
                                    if (liste.Value != -1)
                                    {
                                        switch (liste.Key.Month)
                                        {
                                            case 1: { janvier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 2: { fevrier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 3: { mars.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 4: { avril.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 5: { mai.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 6: { juin.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 7: { juillet.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 8: { aout.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 9: { septembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 10: { octobre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 11: { novembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            case 12: { decembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void annee_back(object sender, RoutedEventArgs e)
        {
            int i = year_pret - 1;
            bool marche = false;
            employee st = Donnée_Suivi_Prêt.SelectedItem as employee;
            pret_remboursable pret = null;
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Date_demande) && DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Date_pv) && Double.Parse(st.Montant_Prét) == liste.Value.Montant && st.Nom.Equals(liste.Value.Employé.Nom) && st.Prenom.Equals(liste.Value.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Num_pv && st.Type_Prêt.Equals(liste.Value.Type_Pret.Description) && liste.Value.isPere())
                {
                    pret = liste.Value;
                }
            }

            if (pret.Debordement == -1)
            {
                Dictionary<DateTime, double> suivi = new Dictionary<DateTime, double>();
                DateTime paiement = pret.Date_premier_paiment;
                foreach (KeyValuePair<int, double> kvp in pret.Etat)
                {
                    if (kvp.Value != -1)
                    {
                        suivi.Add(paiement, kvp.Value);
                        paiement = paiement.AddMonths(1);
                    }
                }
                foreach (DateTime d in suivi.Keys)
                {
                    if (d.Year == i)
                    {
                        marche = true;
                    }
                }
                if (marche == true)
                {
                    year_pret--;
                    janvier.Text = "";
                    fevrier.Text = "";
                    mars.Text = "";
                    avril.Text = "";
                    mai.Text = "";
                    juin.Text = "";
                    juillet.Text = "";
                    aout.Text = "";
                    septembre.Text = "";
                    octobre.Text = "";
                    novembre.Text = "";
                    decembre.Text = "";
                    Annee_suivi.Text = "";
                    Annee_suivi.Text = "          " + i.ToString();
                    if (i == pret.Date_demande.Year)
                    {
                        switch (pret.Date_demande.Month)
                        {
                            case 1: { janvier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 2: { fevrier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 3: { mars.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 4: { avril.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 5: { mai.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 6: { juin.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 7: { juillet.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 8: { aout.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 9: { septembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 10: { octobre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 11: { novembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 12: { decembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                        }
                    }
                    if (i == pret.Date_pv.Year)
                    {
                        switch (pret.Date_pv.Month)
                        {
                            case 1: { janvier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 2: { fevrier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 3: { mars.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 4: { avril.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 5: { mai.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 6: { juin.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 7: { juillet.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 8: { aout.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 9: { septembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 10: { octobre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 11: { novembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 12: { decembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                        }
                    }
                    foreach (KeyValuePair<DateTime, double> liste in suivi)
                    {
                        if (liste.Key.Year == i)
                        {
                            if (liste.Value == pret.Montant / pret.Durée)
                            {
                                switch (liste.Key.Month)
                                {
                                    case 1: { janvier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 2: { fevrier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 3: { mars.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 4: { avril.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 5: { mai.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 6: { juin.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 7: { juillet.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 8: { aout.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 9: { septembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 10: { octobre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 11: { novembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 12: { decembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                }
                            }
                            else
                            {
                                if ((liste.Value == 0))
                                {
                                    switch (liste.Key.Month)
                                    {
                                        case 1: { janvier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 2: { fevrier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 3: { mars.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 4: { avril.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 5: { mai.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 6: { juin.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 7: { juillet.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 8: { aout.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 9: { septembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 10: { octobre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 11: { novembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 12: { decembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    }
                                }
                                else
                                {
                                    if (liste.Value == (pret.Montant - pret.Somme_remboursée))
                                    {
                                        switch (liste.Key.Month)
                                        {
                                            case 1: { janvier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 2: { fevrier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 3: { mars.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 4: { avril.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 5: { mai.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 6: { juin.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 7: { juillet.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 8: { aout.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 9: { septembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 10: { octobre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 11: { novembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 12: { decembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        }
                                    }
                                    else
                                    {
                                        if (liste.Value != -1)
                                        {
                                            switch (liste.Key.Month)
                                            {
                                                case 1: { janvier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 2: { fevrier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 3: { mars.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 4: { avril.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 5: { mai.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 6: { juin.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 7: { juillet.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 8: { aout.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 9: { septembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 10: { octobre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 11: { novembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 12: { decembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                List<double> mois_debor = new List<double>();
                while (pret.Debordement != -1)
                {
                    foreach (double d in pret.Etat.Values)
                    {
                        mois_debor.Add(d);
                    }
                    pret = pret.getFils();
                }
                foreach (double d in pret.Etat.Values)
                {
                    mois_debor.Add(d);
                }

                Dictionary<DateTime, double> suivi = new Dictionary<DateTime, double>();
                DateTime paiement = pret.Date_premier_paiment;
                foreach (double d in mois_debor)
                {
                    if (d != -1)
                    {
                        suivi.Add(paiement, d);
                        paiement = paiement.AddMonths(1);
                    }
                }
                foreach (DateTime d in suivi.Keys)
                {
                    if (d.Year == i)
                    {
                        marche = true;
                    }
                }
                if (marche == true)
                {
                    year_pret--;
                    janvier.Text = "";
                    fevrier.Text = "";
                    mars.Text = "";
                    avril.Text = "";
                    mai.Text = "";
                    juin.Text = "";
                    juillet.Text = "";
                    aout.Text = "";
                    septembre.Text = "";
                    octobre.Text = "";
                    novembre.Text = "";
                    decembre.Text = "";
                    Annee_suivi.Text = "";
                    Annee_suivi.Text = "          " + i.ToString();
                    if (i == pret.Date_demande.Year)
                    {
                        switch (pret.Date_demande.Month)
                        {
                            case 1: { janvier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 2: { fevrier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 3: { mars.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 4: { avril.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 5: { mai.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 6: { juin.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 7: { juillet.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 8: { aout.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 9: { septembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 10: { octobre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 11: { novembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 12: { decembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                        }
                    }
                    if (i == pret.Date_pv.Year)
                    {
                        switch (pret.Date_pv.Month)
                        {
                            case 1: { janvier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 2: { fevrier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 3: { mars.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 4: { avril.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 5: { mai.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 6: { juin.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 7: { juillet.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 8: { aout.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 9: { septembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 10: { octobre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 11: { novembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 12: { decembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                        }
                    }
                    foreach (KeyValuePair<DateTime, double> liste in suivi)
                    {
                        if (liste.Key.Year == i)
                        {
                            if (liste.Value == pret.Montant / pret.Durée)
                            {
                                switch (liste.Key.Month)
                                {
                                    case 1: { janvier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 2: { fevrier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 3: { mars.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 4: { avril.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 5: { mai.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 6: { juin.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 7: { juillet.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 8: { aout.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 9: { septembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 10: { octobre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 11: { novembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 12: { decembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                }
                            }
                            else
                            {
                                if ((liste.Value == 0))
                                {
                                    switch (liste.Key.Month)
                                    {
                                        case 1: { janvier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 2: { fevrier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 3: { mars.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 4: { avril.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 5: { mai.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 6: { juin.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 7: { juillet.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 8: { aout.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 9: { septembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 10: { octobre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 11: { novembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 12: { decembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    }
                                }
                                else
                                {
                                    if (liste.Value == (pret.Montant - pret.Somme_remboursée))
                                    {
                                        switch (liste.Key.Month)
                                        {
                                            case 1: { janvier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 2: { fevrier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 3: { mars.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 4: { avril.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 5: { mai.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 6: { juin.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 7: { juillet.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 8: { aout.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 9: { septembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 10: { octobre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 11: { novembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 12: { decembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        }
                                    }
                                    else
                                    {
                                        if (liste.Value != -1)
                                        {
                                            switch (liste.Key.Month)
                                            {
                                                case 1: { janvier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 2: { fevrier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 3: { mars.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 4: { avril.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 5: { mai.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 6: { juin.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 7: { juillet.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 8: { aout.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 9: { septembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 10: { octobre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 11: { novembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 12: { decembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void annee_forward(object sender, RoutedEventArgs e)
        {
            int i = year_pret + 1;
            bool marche = false;
            employee st = Donnée_Suivi_Prêt.SelectedItem as employee;
            pret_remboursable pret = null;
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Date_demande) && DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Date_pv) && Double.Parse(st.Montant_Prét) == liste.Value.Montant && st.Nom.Equals(liste.Value.Employé.Nom) && st.Prenom.Equals(liste.Value.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Num_pv && st.Type_Prêt.Equals(liste.Value.Type_Pret.Description) && liste.Value.isPere())
                {
                    pret = liste.Value;
                }
            }

            if (pret.Debordement == -1)
            {
                Dictionary<DateTime, double> suivi = new Dictionary<DateTime, double>();
                DateTime paiement = pret.Date_premier_paiment;
                foreach (KeyValuePair<int, double> kvp in pret.Etat)
                {
                    if (kvp.Value != -1)
                    {
                        suivi.Add(paiement, kvp.Value);
                        paiement = paiement.AddMonths(1);
                    }
                }
                foreach (DateTime d in suivi.Keys)
                {
                    if (d.Year == i)
                    {
                        marche = true;
                    }
                }
                if (marche == true)
                {
                    year_pret++;
                    janvier.Text = "";
                    fevrier.Text = "";
                    mars.Text = "";
                    avril.Text = "";
                    mai.Text = "";
                    juin.Text = "";
                    juillet.Text = "";
                    aout.Text = "";
                    septembre.Text = "";
                    octobre.Text = "";
                    novembre.Text = "";
                    decembre.Text = "";
                    Annee_suivi.Text = "";
                    Annee_suivi.Text = "          " + i.ToString();
                    if (i == pret.Date_demande.Year)
                    {
                        switch (pret.Date_demande.Month)
                        {
                            case 1: { janvier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 2: { fevrier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 3: { mars.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 4: { avril.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 5: { mai.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 6: { juin.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 7: { juillet.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 8: { aout.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 9: { septembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 10: { octobre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 11: { novembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 12: { decembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                        }
                    }
                    if (i == pret.Date_pv.Year)
                    {
                        switch (pret.Date_pv.Month)
                        {
                            case 1: { janvier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 2: { fevrier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 3: { mars.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 4: { avril.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 5: { mai.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 6: { juin.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 7: { juillet.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 8: { aout.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 9: { septembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 10: { octobre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 11: { novembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 12: { decembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                        }
                    }
                    foreach (KeyValuePair<DateTime, double> liste in suivi)
                    {
                        if (liste.Key.Year == i)
                        {
                            if (liste.Value == pret.Montant / pret.Durée)
                            {
                                switch (liste.Key.Month)
                                {
                                    case 1: { janvier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 2: { fevrier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 3: { mars.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 4: { avril.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 5: { mai.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 6: { juin.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 7: { juillet.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 8: { aout.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 9: { septembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 10: { octobre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 11: { novembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 12: { decembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                }
                            }
                            else
                            {
                                if ((liste.Value == 0))
                                {
                                    switch (liste.Key.Month)
                                    {
                                        case 1: { janvier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 2: { fevrier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 3: { mars.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 4: { avril.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 5: { mai.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 6: { juin.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 7: { juillet.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 8: { aout.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 9: { septembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 10: { octobre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 11: { novembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 12: { decembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    }
                                }
                                else
                                {
                                    if (liste.Value == (pret.Montant - pret.Somme_remboursée))
                                    {
                                        switch (liste.Key.Month)
                                        {
                                            case 1: { janvier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 2: { fevrier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 3: { mars.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 4: { avril.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 5: { mai.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 6: { juin.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 7: { juillet.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 8: { aout.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 9: { septembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 10: { octobre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 11: { novembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 12: { decembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        }
                                    }
                                    else
                                    {
                                        if (liste.Value != -1)
                                        {
                                            switch (liste.Key.Month)
                                            {
                                                case 1: { janvier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 2: { fevrier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 3: { mars.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 4: { avril.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 5: { mai.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 6: { juin.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 7: { juillet.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 8: { aout.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 9: { septembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 10: { octobre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 11: { novembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 12: { decembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                List<double> mois_debor = new List<double>();
                while (pret.Debordement != -1)
                {
                    foreach (double d in pret.Etat.Values)
                    {
                        mois_debor.Add(d);
                    }
                    pret = pret.getFils();
                }
                foreach (double d in pret.Etat.Values)
                {
                    mois_debor.Add(d);
                }

                Dictionary<DateTime, double> suivi = new Dictionary<DateTime, double>();
                DateTime paiement = pret.Date_premier_paiment;
                foreach (double d in mois_debor)
                {
                    if (d != -1)
                    {
                        suivi.Add(paiement, d);
                        paiement = paiement.AddMonths(1);
                    }
                }
                foreach (DateTime d in suivi.Keys)
                {
                    if (d.Year == i)
                    {
                        marche = true;
                    }
                }
                if (marche == true)
                {
                    year_pret++;
                    janvier.Text = "";
                    fevrier.Text = "";
                    mars.Text = "";
                    avril.Text = "";
                    mai.Text = "";
                    juin.Text = "";
                    juillet.Text = "";
                    aout.Text = "";
                    septembre.Text = "";
                    octobre.Text = "";
                    novembre.Text = "";
                    decembre.Text = "";
                    Annee_suivi.Text = "";
                    Annee_suivi.Text = "          " + i.ToString();
                    if (i == pret.Date_demande.Year)
                    {
                        switch (pret.Date_demande.Month)
                        {
                            case 1: { janvier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 2: { fevrier.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 3: { mars.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 4: { avril.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 5: { mai.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 6: { juin.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 7: { juillet.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 8: { aout.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 9: { septembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 10: { octobre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 11: { novembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                            case 12: { decembre.Text += "- L'employé a fait la demande du prêt\n"; break; }
                        }
                    }
                    if (i == pret.Date_pv.Year)
                    {
                        switch (pret.Date_pv.Month)
                        {
                            case 1: { janvier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 2: { fevrier.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 3: { mars.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 4: { avril.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 5: { mai.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 6: { juin.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 7: { juillet.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 8: { aout.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 9: { septembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 10: { octobre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 11: { novembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                            case 12: { decembre.Text += "- Réunion du bureau du COS et sortie du PV \n"; break; }
                        }
                    }
                    foreach (KeyValuePair<DateTime, double> liste in suivi)
                    {
                        if (liste.Key.Year == i)
                        {
                            if (liste.Value == pret.Montant / pret.Durée)
                            {
                                switch (liste.Key.Month)
                                {
                                    case 1: { janvier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 2: { fevrier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 3: { mars.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 4: { avril.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 5: { mai.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 6: { juin.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 7: { juillet.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 8: { aout.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 9: { septembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 10: { octobre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 11: { novembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                    case 12: { decembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                                }
                            }
                            else
                            {
                                if ((liste.Value == 0))
                                {
                                    switch (liste.Key.Month)
                                    {
                                        case 1: { janvier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 2: { fevrier.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 3: { mars.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 4: { avril.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 5: { mai.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 6: { juin.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 7: { juillet.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 8: { aout.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 9: { septembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 10: { octobre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 11: { novembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                        case 12: { decembre.Text += "- Retardement : \n  0 (DA)."; break; }
                                    }
                                }
                                else
                                {
                                    if (liste.Value == (pret.Montant - pret.Somme_remboursée))
                                    {
                                        switch (liste.Key.Month)
                                        {
                                            case 1: { janvier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 2: { fevrier.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 3: { mars.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 4: { avril.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 5: { mai.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 6: { juin.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 7: { juillet.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 8: { aout.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 9: { septembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 10: { octobre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 11: { novembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                            case 12: { decembre.Text += "- Paiement anticipé (total) : \n  " + (pret.Montant - pret.Somme_remboursée).ToString() + " (DA).\n"; break; }
                                        }
                                    }
                                    else
                                    {
                                        if (liste.Value != -1)
                                        {
                                            switch (liste.Key.Month)
                                            {
                                                case 1: { janvier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 2: { fevrier.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 3: { mars.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 4: { avril.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 5: { mai.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 6: { juin.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 7: { juillet.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 8: { aout.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 9: { septembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 10: { octobre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 11: { novembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                                case 12: { decembre.Text += "- Paiement d'un montant correspondant a \n" + liste.Value.ToString() + " (DA).\n"; break; }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void introduire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
             liste_employes.Visibility = Visibility.Visible;               
        }        

        private void actualiser_click(object sender, RoutedEventArgs e)
        {
            Donnée_Suivi_Prêt.ItemsSource = null;
            liste_employes.Items.Clear();
            Prêt_Type_ajout.Items.Clear();
            liste_types.Items.Clear();

            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                if (liste.Value.Disponibilité == 1 && liste.Value.Remboursable == 1)
                {
                    Prêt_Type_ajout.Items.Add(liste.Value.Description);
                    liste_types.Items.Add(liste.Value.Description);
                }
            }

            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {
                liste_employes.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
            }

            List<employee> source = new List<employee>();
            source.Clear();
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (liste.Value.isPere())
                {
                    employee Employe = new employee();
                    Employe.Nom = liste.Value.Employé.Nom;
                    Employe.Prenom = liste.Value.Employé.Prenom;
                    Employe.N_Pv = liste.Value.Num_pv.ToString();
                    Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                    Employe.Date_de_Pv = liste.Value.Date_pv.ToShortDateString();
                    Employe.Date_demande = liste.Value.Date_demande.ToShortDateString();
                    Employe.Montant_Prét = liste.Value.Montant.ToString();
                    source.Add(Employe);
                }
            }
            Donnée_Suivi_Prêt.ItemsSource = source;
        }


        public void actualiser()
        {
            Donnée_Suivi_Prêt.ItemsSource = null;
            liste_employes.Items.Clear();
            Prêt_Type_ajout.Items.Clear();
            liste_types.Items.Clear();

            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                if (liste.Value.Disponibilité == 1 && liste.Value.Remboursable == 1)
                {
                    Prêt_Type_ajout.Items.Add(liste.Value.Description);
                    liste_types.Items.Add(liste.Value.Description);
                }
            }

            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {
                liste_employes.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
                liste_employe_rech.Items.Add(liste.Key + ") " + liste.Value.Nom + " " + liste.Value.Prenom);
            }

            List<employee> source = new List<employee>();
            source.Clear();
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (liste.Value.isPere())
                {
                    employee Employe = new employee();
                    Employe.Nom = liste.Value.Employé.Nom;
                    Employe.Prenom = liste.Value.Employé.Prenom;
                    Employe.N_Pv = liste.Value.Num_pv.ToString();
                    Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                    Employe.Date_de_Pv = liste.Value.Date_pv.ToShortDateString();
                    Employe.Date_demande = liste.Value.Date_demande.ToShortDateString();
                    Employe.Montant_Prét = liste.Value.Montant.ToString();
                    source.Add(Employe);
                }
            }
            Donnée_Suivi_Prêt.ItemsSource = source;
        }

        private void retourner_detail_click(object sender, RoutedEventArgs e)
        {
            suivi.Visibility = Visibility.Hidden; suivi.IsEnabled = false;
            data_grid.Visibility = Visibility.Visible; data_grid.IsEnabled = true;
            actualiser();
        }

        private void retourner_detail_emp_click(object sender, RoutedEventArgs e)
        {
            details_emp.Visibility = Visibility.Hidden; details_emp.IsEnabled = false;
            suivi.Visibility = Visibility.Visible; suivi.IsEnabled = true;
        }

        private void details_emp_click(object sender, RoutedEventArgs e)
        {
            details_emp.Visibility = Visibility.Visible; details_emp.IsEnabled = true;
            suivi.Visibility = Visibility.Hidden; suivi.IsEnabled = false;
        }

        private void Prélèvement_click(object sender, RoutedEventArgs e)
        {
            suivi_calendar.Visibility = Visibility.Hidden; suivi_calendar.IsEnabled = false;
            confirmer_prelevement.Visibility = Visibility.Visible; confirmer_prelevement.IsEnabled = true;
            Prélèvement.Visibility = Visibility.Hidden; Prélèvement.IsEnabled = false;
            retourner_detail_bouton.Visibility = Visibility.Hidden; retourner_detail_bouton.IsEnabled = false;
            retourner_suivi_bouton.Visibility = Visibility.Visible; retourner_suivi_bouton.IsEnabled = true;
            titre_prelevement.Visibility = Visibility.Visible;
            methode_prelevement.Visibility = Visibility.Visible;
            montant_titre.Visibility = Visibility.Visible;
            montant_prelevement.Visibility = Visibility.Visible;
            da_titre.Visibility = Visibility.Visible;
            affiche_montant.Visibility = Visibility.Visible;
        }

        private void retourner_suivi_click(object sender, RoutedEventArgs e)
        {
            suivi_calendar.Visibility = Visibility.Visible; suivi_calendar.IsEnabled = true;
            confirmer_prelevement.Visibility = Visibility.Hidden; confirmer_prelevement.IsEnabled = false;
            Prélèvement.Visibility = Visibility.Visible; Prélèvement.IsEnabled = true;
            retourner_detail_bouton.Visibility = Visibility.Visible; retourner_detail_bouton.IsEnabled = true;
            retourner_suivi_bouton.Visibility = Visibility.Hidden; retourner_suivi_bouton.IsEnabled = false;
            titre_prelevement.Visibility = Visibility.Hidden;
            methode_prelevement.Visibility = Visibility.Hidden;
            montant_titre.Visibility = Visibility.Hidden;
            montant_prelevement.Visibility = Visibility.Hidden;
            da_titre.Visibility = Visibility.Hidden;
            nb_mois.Visibility = Visibility.Hidden;
            affiche_montant.Visibility = Visibility.Hidden;
        }

        private void confirmer_Prélèvement_click(object sender, RoutedEventArgs e)
        {
            int cpt = 0;
            employee st = Donnée_Suivi_Prêt.SelectedItem as employee;
            pret_remboursable pret = null;
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Date_demande) && DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Date_pv) && Double.Parse(st.Montant_Prét) == liste.Value.Montant && st.Nom.Equals(liste.Value.Employé.Nom) && st.Prenom.Equals(liste.Value.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Num_pv && st.Type_Prêt.Equals(liste.Value.Type_Pret.Description))
                {
                    pret = liste.Value;
                }
            }
            double montant_prelevé = 0;
            if (methode_prelevement.Text.Equals("Paiement Standard."))
            {
                montant_prelevé = pret.Montant - pret.Somme_remboursée;
                
                responsable.paiement_standard(pret.Cle);
                int mois = pret.Date_actuelle.Month - 1;
                montant_remboursé.Text = pret.Somme_remboursée.ToString();
                monatant_restant.Text = (pret.Montant - pret.Somme_remboursée).ToString();
                date_prochain_info.Text = pret.Date_actuelle.ToShortDateString();
                switch (mois)
                {
                    case 1: { janvier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 2: { fevrier.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 3: { mars.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 4: { avril.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 5: { mai.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 6: { juin.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 7: { juillet.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 8: { aout.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 9: { septembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 10: { octobre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 11: { novembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                    case 12: { decembre.Text += "- Paiement standard : \n  " + pret.Montant / pret.Durée + " (DA).\n"; break; }
                }
                mois++;

                montant_prelevé = montant_prelevé - (pret.Montant - pret.Somme_remboursée);
                Détails_Click(sender, e);
            }
            else
            {
                if (methode_prelevement.Text.Equals("Paiement sur plusieurs mois."))
                {
                    int c = 0;
                    if (!int.TryParse(nb_mois_saisi.Text, out c) || c <= 0)
                    {
                        MessageBox.Show("entrez un nombre de mois valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        cpt++;
                    }
                    else
                    {
                        montant_prelevé = pret.Montant - pret.Somme_remboursée;
                        responsable.paiement_plusieurs_mois(pret.Cle, Int32.Parse(nb_mois_saisi.Text));
                        int mois = pret.Date_actuelle.Month;
                        responsable.paiement_plusieurs_mois(pret.Cle, Int32.Parse(nb_mois_saisi.Text));
                        double d = 0;
                        foreach (double a in pret.Etat.Values)
                        {
                            if (a != -1)
                            {
                                d = a;
                            }

                        }
                        montant_remboursé.Text = pret.Somme_remboursée.ToString();
                        monatant_restant.Text = (pret.Montant - pret.Somme_remboursée).ToString();
                        date_prochain_info.Text = pret.Date_actuelle.ToShortDateString();
                        switch (mois)
                        {
                            case 1: { janvier.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 2: { fevrier.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 3: { mars.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 4: { avril.Text += "- Paiement d'un montant correspondant :\n  " + d.ToString() + " (DA).\n"; break; }
                            case 5: { mai.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 6: { juin.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 7: { juillet.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 8: { aout.Text += "- Paiement d'un montant correspondant a mois :\n  " + d.ToString() + " (DA).\n"; break; }
                            case 9: { septembre.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 10: { octobre.Text += "- Paiement d'un montant correspondant : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 11: { novembre.Text += "- Paiement d'un montant correspondant : \n " + d.ToString() + " (DA).\n"; break; }
                            case 12: { decembre.Text += "- Paiement d'un montant correspondant : \n " + d.ToString() + " (DA).\n"; break; }
                        }
                        montant_prelevé = montant_prelevé - (pret.Montant - pret.Somme_remboursée);
                        Détails_Click(sender, e);
                    }
                }
                else
                {
                    if (methode_prelevement.Text.Equals("Paiement Anticipé (Total)."))
                    {
                        montant_prelevé = pret.Montant - pret.Somme_remboursée;
                        double d = pret.Montant - pret.Somme_remboursée;
                        
                        responsable.paiement_anticipé(pret.Cle);
                        int mois = pret.Date_actuelle.Month - 1;
                        montant_remboursé.Text = pret.Somme_remboursée.ToString();
                        monatant_restant.Text = (pret.Montant - pret.Somme_remboursée).ToString();
                        date_prochain_info.Text = pret.Date_actuelle.ToShortDateString();
                        switch (mois)
                        {
                            case 1: { janvier.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 2: { fevrier.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 3: { mars.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 4: { avril.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 5: { mai.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 6: { juin.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 7: { juillet.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 8: { aout.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 9: { septembre.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 10: { octobre.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 11: { novembre.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                            case 12: { decembre.Text += "- Paiement anticipé : \n  " + d.ToString() + " (DA).\n"; break; }
                        }
                        montant_prelevé = montant_prelevé - (pret.Montant - pret.Somme_remboursée);
                        Détails_Click(sender, e);
                    }
                    else
                    {
                        if (methode_prelevement.Text.Equals("Retardement."))
                        {
                            responsable.retardement_paiement(pret.Cle);
                            int mois = pret.Date_actuelle.Month - 1;
                            montant_remboursé.Text = pret.Somme_remboursée.ToString();
                            monatant_restant.Text = (pret.Montant - pret.Somme_remboursée).ToString();
                            date_prochain_info.Text = pret.Date_actuelle.ToShortDateString();

                            switch (mois)
                            {
                                case 1: { janvier.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 2: { fevrier.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 3: { mars.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 4: { avril.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 5: { mai.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 6: { juin.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 7: { juillet.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 8: { aout.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 9: { septembre.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 10: { octobre.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 11: { novembre.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                                case 12: { decembre.Text += "- Paiment différé : \n  0 (DA).\n"; break; }
                            }
                            Détails_Click(sender, e);
                        }
                        else
                        {
                            if (methode_prelevement.Text.Equals("Effacement des Dettes"))
                            {
                                responsable.effacement_dettes(pret.Cle);
                                int mois = pret.Date_actuelle.Month;
                                montant_remboursé.Text = pret.Somme_remboursée.ToString();
                                monatant_restant.Text = "0";
                                date_prochain_info.Text = "/";
                                switch (mois)
                                {
                                    case 1: { janvier.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 2: { fevrier.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 3: { mars.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 4: { avril.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 5: { mai.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 6: { juin.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 7: { juillet.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 8: { aout.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 9: { septembre.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 10: { octobre.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 11: { novembre.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                    case 12: { decembre.Text += "- Effacement des dettes :   \n 0 (DA)."; break; }
                                }
                                Détails_Click(sender, e);
                            }
                            else
                            {
                                if (methode_prelevement.Text.Equals("Paiement Libre"))
                                {
                                    double d=0;
                                    montant_prelevé = pret.Montant - pret.Somme_remboursée;
                                    if (!Double.TryParse(montant_prelevement.Text, out d) || d <= 0)
                                    {
                                        cpt++;
                                        MessageBox.Show("entrez un montant valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else 
                                    { 
                                        double montant = Double.Parse(montant_prelevement.Text);
                                        responsable.paiement_spécial(pret.Cle, montant);
                                        int mois = pret.Date_actuelle.Month - 1;
                                        montant_remboursé.Text = pret.Somme_remboursée.ToString();
                                        monatant_restant.Text = (pret.Montant - pret.Somme_remboursée).ToString();
                                        date_prochain_info.Text = pret.Date_actuelle.ToShortDateString();
                                        switch (mois)
                                        {
                                            case 1: { janvier.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 2: { fevrier.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 3: { mars.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 4: { avril.Text += "- Paiement d'un montant correspondant :\n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 5: { mai.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 6: { juin.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 7: { juillet.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 8: { aout.Text += "- Paiement d'un montant correspondant a mois :\n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 9: { septembre.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 10: { octobre.Text += "- Paiement d'un montant correspondant : \n  " + montant.ToString() + " (DA).\n"; break; }
                                            case 11: { novembre.Text += "- Paiement d'un montant correspondant : \n " + montant.ToString() + " (DA).\n"; break; }
                                            case 12: { decembre.Text += "- Paiement d'un montant correspondant : \n " + montant.ToString() + " (DA).\n"; break; }
                                        }
                                        montant_prelevé = montant_prelevé - (pret.Montant - pret.Somme_remboursée);
                                        Détails_Click(sender, e);
                                    }
                                }
                            }
                        }
                    }
                }
                // retourner_detail_click(sender, e);


                //Détails_Click(sender, e);
            }
            if(cpt == 0) 
            { 
                if (Window2.envoi_notif)
                {
                    if (Window2.mode_envoi)
                    {
                        if (!pret.Employé.Email.Equals(""))
                            responsable.Envoi_mail(pret, montant_prelevé);
                        else
                        {
                            WpfTutorialSamples.Dialogs.InputDialogSample input = new WpfTutorialSamples.Dialogs.InputDialogSample(pret, montant_prelevé, "Veuillez entrer le mail de l'employé :", "mail@esi.dz");
                            input.ShowActivated = true;
                            input.Show();
                        }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Voulez vous envoyer une notification E-mail ?", "Notification E-mail", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                if (!pret.Employé.Email.Equals(""))
                                    responsable.Envoi_mail(pret, montant_prelevé);
                                else
                                {
                                    WpfTutorialSamples.Dialogs.InputDialogSample input = new WpfTutorialSamples.Dialogs.InputDialogSample(pret, montant_prelevé, "Veuillez entrer le mail de l'employé :", "mail@esi.dz");
                                    input.ShowActivated = true;
                                    input.Show();
                                }
                                break;
                            case MessageBoxResult.No:
                                MessageBox.Show("La notification sera pas envoyé", "Notification E-mail", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                }

                suivi_calendar.Visibility = Visibility.Visible; suivi_calendar.IsEnabled = true;
                confirmer_prelevement.Visibility = Visibility.Hidden; confirmer_prelevement.IsEnabled = false;
                Prélèvement.Visibility = Visibility.Visible; Prélèvement.IsEnabled = true;
                retourner_detail_bouton.Visibility = Visibility.Visible; retourner_detail_bouton.IsEnabled = true;
                retourner_suivi_bouton.Visibility = Visibility.Hidden; retourner_suivi_bouton.IsEnabled = false;
                titre_prelevement.Visibility = Visibility.Hidden;
                methode_prelevement.Visibility = Visibility.Hidden;
                montant_titre.Visibility = Visibility.Hidden;
                montant_prelevement.Visibility = Visibility.Hidden;
                da_titre.Visibility = Visibility.Hidden;
                nb_mois.Visibility = Visibility.Hidden;
                affiche_montant.Visibility = Visibility.Hidden;
                m.Visibility = Visibility.Hidden;
                nb_mois_saisi.Visibility = Visibility.Hidden;
                methode_prelevement.Text = "";
                montant_prelevement.Text = "";
            }
        }

        private void montant_prelevement_selection_changed(object sender, SelectionChangedEventArgs e)
        {
            montant_prelevement.Text = "";
            try
            {
                employee st = Donnée_Suivi_Prêt.SelectedItem as employee;
                pret_remboursable pret = null;
                foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
                {
                    if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Date_demande) && DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Date_pv) && Double.Parse(st.Montant_Prét) == liste.Value.Montant && st.Nom.Equals(liste.Value.Employé.Nom) && st.Prenom.Equals(liste.Value.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Num_pv && st.Type_Prêt.Equals(liste.Value.Type_Pret.Description))
                    {
                        pret = liste.Value;
                    }
                }
                if (methode_prelevement.SelectedItem.ToString().Equals("Paiement Standard."))
                {
                    nb_mois.Visibility = Visibility.Hidden;
                    nb_mois_saisi.Visibility = Visibility.Hidden;
                    m.Visibility = Visibility.Hidden;
                    montant_prelevement.IsReadOnly = true;
                    Suivi_Prét.montant = "      " + (pret.Montant / pret.Durée).ToString();
                }
                else
                {
                    if (methode_prelevement.SelectedItem.ToString().Equals("Paiement sur plusieurs mois."))
                    {
                        nb_mois.Visibility = Visibility.Visible;
                        nb_mois_saisi.Visibility = Visibility.Visible;
                        m.Visibility = Visibility.Visible;
                        double nb_mois_ = Double.Parse(nb_mois_saisi.Text);
                        double montant_multip = (pret.Montant / (double)pret.Durée) * nb_mois_;
                        montant_prelevement.IsReadOnly = true;
                        Suivi_Prét.montant = "      " + montant_multip.ToString();
                    }
                    else
                    {
                        if (methode_prelevement.SelectedItem.ToString().Equals("Paiement Anticipé (Total)."))
                        {
                            nb_mois.Visibility = Visibility.Hidden;
                            nb_mois_saisi.Visibility = Visibility.Hidden;
                            m.Visibility = Visibility.Hidden;
                            montant_prelevement.IsReadOnly = true;
                            Suivi_Prét.montant = "      " + (pret.Montant - pret.Somme_remboursée).ToString();
                        }
                        else
                        {
                            if (methode_prelevement.SelectedItem.ToString().Equals("Retardement."))
                            {
                                nb_mois.Visibility = Visibility.Hidden;
                                nb_mois_saisi.Visibility = Visibility.Hidden;
                                m.Visibility = Visibility.Hidden;
                                montant_prelevement.IsReadOnly = true;
                                Suivi_Prét.montant = "      0";
                            }
                            else
                            {
                                if (methode_prelevement.SelectedItem.ToString().Equals("Effacement des Dettes"))
                                {
                                    nb_mois.Visibility = Visibility.Hidden;
                                    nb_mois_saisi.Visibility = Visibility.Hidden;
                                    m.Visibility = Visibility.Hidden;
                                    montant_prelevement.IsReadOnly = true;
                                    Suivi_Prét.montant = "      0";
                                }
                                else
                                {
                                    if (methode_prelevement.SelectedItem.ToString().Equals("Paiement Libre"))
                                    {
                                        nb_mois.Visibility = Visibility.Hidden;
                                        nb_mois_saisi.Visibility = Visibility.Hidden;
                                        m.Visibility = Visibility.Hidden;
                                        montant_prelevement.IsReadOnly = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception o)
            { }
        }

        private void affiche_montant_click(object sender, RoutedEventArgs e)
        {
            employee st = Donnée_Suivi_Prêt.SelectedItem as employee;
            pret_remboursable pret = null;
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Date_demande) && DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Date_pv) && Double.Parse(st.Montant_Prét) == liste.Value.Montant && st.Nom.Equals(liste.Value.Employé.Nom) && st.Prenom.Equals(liste.Value.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Num_pv && st.Type_Prêt.Equals(liste.Value.Type_Pret.Description))
                {
                    pret = liste.Value;
                }
            }
            if (methode_prelevement.Text.Equals("Paiement sur plusieurs mois."))
            {
                double nb_mois_ = Double.Parse(nb_mois_saisi.Text);
                double montant_multip = (pret.Montant / (double)pret.Durée) * nb_mois_;
                Suivi_Prét.montant = "      " + montant_multip.ToString();
            }
            montant_prelevement.Text = Suivi_Prét.montant;
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
                if (liste.Value.Remboursable == 1 && liste.Value.Disponibilité == 1)
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
                    if (liste.Value.Remboursable == 1 && liste.Value.Disponibilité == 1)
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
 
            responsable.remplissage_liste_filtres_rem();

            responsable.filtrer_par_employés_rem(!liste_employe_rech.Text.Equals(""));
            responsable.filtrer_par_types_rem(!liste_types.Text.Equals(""));

            double k = 0;
            int cpt = 0;
            double somme = 0;
            if (!(date_dem_inf.SelectedDate.Equals(null)))
                responsable.filtrer_par_date_demande_inf_rem(!date_dem_inf.Equals(null), date_dem_inf.SelectedDate.Value.Date);
            if (!date_de_sup.SelectedDate.Equals(null))
                responsable.filtrer_par_date_demande_max_rem(!date_de_sup.Equals(null), date_de_sup.SelectedDate.Value.Date);
            if (!date_pv_inf.SelectedDate.Equals(null))
                responsable.filtrer_par_date_pv_inf_rem(!date_pv_inf.Equals(null), date_pv_inf.SelectedDate.Value.Date);
            if (!date_pv_sup.SelectedDate.Equals(null))
                responsable.filtrer_par_date_pv_max_rem(!date_pv_sup.Equals(null), date_pv_sup.SelectedDate.Value.Date);
            if (!somme_min.Text.Equals(""))
            {
                if (!double.TryParse(somme_min.Text, out k))
                {
                    MessageBox.Show("entrez une somme minimale valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    cpt++;
                }
                else
                {
                    responsable.filtrer_par_somme_min_rem(!somme_min.Equals(null), double.Parse(somme_min.Text.ToString()));
                }

            }
            if (!somme_max.Text.Equals(""))
            {
                if (!double.TryParse(somme_max.Text, out somme))
                {
                    MessageBox.Show("entrez une somme maximale valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    cpt++;
                }
                else
                {
                    responsable.filtrer_par_somme_max_rem(!somme_max.Equals(null), double.Parse(somme_max.Text.ToString()));
                }
            }

            List<employee> source = new List<employee>();
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_filtres_rem)
            {
                employee Employe = new employee();
                Employe.Nom = liste.Value.Employé.Nom;
                Employe.Prenom = liste.Value.Employé.Prenom;
                Employe.N_Pv = liste.Value.Num_pv.ToString();
                Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                Employe.Date_de_Pv = liste.Value.Date_pv.ToShortDateString();
                //Employe.Motif = liste.Value.Motif;
                Employe.Date_demande = liste.Value.Date_demande.ToShortDateString();
                //Employe.Montant_Prét_lettre = liste.Value.Montant_lettre;
                Employe.Montant_Prét = liste.Value.Montant.ToString();
                source.Add(Employe);
            }
            Donnée_Suivi_Prêt.ItemsSource = source;
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
        private void effacer_Click(object sender, RoutedEventArgs e)
        {
            check_box_effacer.Visibility = Visibility.Visible;
            Options_Principale.Visibility = Visibility.Hidden;
            Options_Principale.IsEnabled = false;

            Options_effacer.Visibility = Visibility.Visible;
            Options_effacer.IsEnabled = true;
        }
        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            var firstCol = Donnée_Suivi_Prêt.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            foreach (employee item in Donnée_Suivi_Prêt.Items)
            {



                var chBx = firstCol.GetCellContent(item) as CheckBox;
                DataGridRow row = firstCol.GetCellContent(item) as DataGridRow;
                if (chBx == null)
                {
                    continue;
                }
                if (chBx.IsChecked == true)
                {
                    chBx.Visibility = Visibility.Hidden;
                    int a = 0;
                    foreach (pret_remboursable p in responsable.liste_pret_remboursable.Values)
                    {
                        if (DateTime.Parse(item.Date_demande).Equals(p.Date_demande) && DateTime.Parse(item.Date_de_Pv).Equals(p.Date_pv) && item.Nom.Equals(p.Employé.Nom) && item.Prenom.Equals(p.Employé.Prenom) && Double.Parse(item.Montant_Prét) == p.Montant && Int32.Parse(item.N_Pv) == p.Num_pv && item.Type_Prêt.Equals(p.Type_Pret.Description))
                        {
                            a = p.Cle;
                        }
                    }
                    responsable.archiver_manuel_pret_remboursable(a);
                }
            }

            List<employee> source = new List<employee>();

            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (liste.Value.isPere())
                {
                    employee Employe = new employee();
                    Employe.Nom = liste.Value.Employé.Nom;
                    Employe.Prenom = liste.Value.Employé.Prenom;
                    Employe.N_Pv = liste.Value.Num_pv.ToString();
                    Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                    Employe.Date_de_Pv = liste.Value.Date_pv.ToString();
                    Employe.Date_demande = liste.Value.Date_demande.ToShortDateString();
                    Employe.Montant_Prét = liste.Value.Montant.ToString();
                    source.Add(Employe);
                }
            }

            Donnée_Suivi_Prêt.ItemsSource = source;
            Options_Principale.Visibility = Visibility.Visible;
            Options_Principale.IsEnabled = true;
            check_box_Archiver.Visibility = Visibility.Hidden;

            Options_archiver.Visibility = Visibility.Hidden;
            Options_archiver.IsEnabled = false;
        }

        private void Confirmer_effacer(object sender, RoutedEventArgs e)
        {
            int i = 0;
            var firstCol = Donnée_Suivi_Prêt.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 1);            
            foreach (employee item in Donnée_Suivi_Prêt.Items)
            {



                var chBx = firstCol.GetCellContent(item) as CheckBox;
                DataGridRow row = firstCol.GetCellContent(item) as DataGridRow;
                if (chBx == null)
                {
                    continue;
                }
                if (chBx.IsChecked == true)
                {
                    chBx.Visibility = Visibility.Hidden;
                    int a = 0;
                    foreach (pret_remboursable p in responsable.liste_pret_remboursable.Values)
                    {
                        if (DateTime.Parse(item.Date_demande).Equals(p.Date_demande) && DateTime.Parse(item.Date_de_Pv).Equals(p.Date_pv) && item.Nom.Equals(p.Employé.Nom) && item.Prenom.Equals(p.Employé.Prenom) && Double.Parse(item.Montant_Prét) == p.Montant && Int32.Parse(item.N_Pv) == p.Num_pv && item.Type_Prêt.Equals(p.Type_Pret.Description))
                        {
                            a = p.Cle;
                        }
                    }
                    responsable.effacement_dettes(a);
                }
            }

            List<employee> source = new List<employee>();

            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (liste.Value.isPere())
                {
                    employee Employe = new employee();
                    Employe.Nom = liste.Value.Employé.Nom;
                    Employe.Prenom = liste.Value.Employé.Prenom;
                    Employe.N_Pv = liste.Value.Num_pv.ToString();
                    Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                    Employe.Date_de_Pv = liste.Value.Date_pv.ToShortDateString();
                    Employe.Date_demande = liste.Value.Date_demande.ToShortDateString();
                    Employe.Montant_Prét = liste.Value.Montant.ToString();
                    source.Add(Employe);
                }
            }

            Donnée_Suivi_Prêt.ItemsSource = source;
            Options_Principale.Visibility = Visibility.Visible;
            Options_Principale.IsEnabled = true;
            check_box_effacer.Visibility = Visibility.Hidden;

            Options_effacer.Visibility = Visibility.Hidden;
            Options_effacer.IsEnabled = false;

        }

        public void auto()
        {
            List<employee> source = new List<employee>();

            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                if (liste.Value.isPere())
                {
                    employee Employe = new employee();
                    Employe.Nom = liste.Value.Employé.Nom;
                    Employe.Prenom = liste.Value.Employé.Prenom;
                    Employe.N_Pv = liste.Value.Num_pv.ToString();
                    Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                    Employe.Date_de_Pv = liste.Value.Date_pv.ToShortDateString();
                    Employe.Date_demande = liste.Value.Date_demande.ToShortDateString();
                    Employe.Montant_Prét = liste.Value.Montant.ToString();
                    source.Add(Employe);
                }
            }


            Donnée_Suivi_Prêt.ItemsSource = source;
        }
        private void Donnée_dons_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (check_box_Archiver.Visibility == Visibility.Visible)
            {
                var firstCol = Donnée_Suivi_Prêt.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                foreach (var item in Donnée_Suivi_Prêt.Items)
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
            responsable.export_prêts_remboursable();
        }

        private void Import_excel_Click(object sender, RoutedEventArgs e)
        {
            responsable.import_prêts_remboursable();
            actualiser();
        }

    }
}