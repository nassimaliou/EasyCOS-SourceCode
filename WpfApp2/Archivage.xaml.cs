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
    /// Interaction logic for Archivage.xaml
    /// </summary>
    public partial class Archivage : UserControl
    {
        //Class principale de l'interface d'archive
        public Archivage()
        {
            InitializeComponent();

            //affectation des données dans la table de données des archives de l'application 

            List<Archives> source = new List<Archives>();
            foreach (KeyValuePair<int, Archive> liste in responsable.liste_archives)
            {
                if (liste.Value.nino() == true)
                {

                    Archives arch = new Archives();
                    arch.Nom = liste.Value.Pret.Employé.Nom;
                    arch.Prenom = liste.Value.Pret.Employé.Prenom;
                    arch.N_Pv = liste.Value.Pret.Num_pv.ToString();
                    arch.Motif = liste.Value.Pret.Motif;
                    arch.Date_demande = liste.Value.Pret.Date_demande.ToShortDateString();
                    arch.Montant_Prét = liste.Value.Pret.Montant.ToString();
                    arch.Montant_Prét_lettre = liste.Value.Pret.Montant_lettre;
                    arch.Observation = liste.Value.Observations;
                    arch.Type_Prêt = liste.Value.Pret.Type_Pret.Description.ToString();
                    arch.Date_de_Pv = liste.Value.Pret.Date_pv.ToShortDateString();
                    arch.prem_paiement = liste.Value.Pret.prem_paiment();
                    arch.fin_paiement = liste.Value.Pret.fin_paiement();
                    arch.sum_rembours = liste.Value.Pret.somme_rembours();
                    if (liste.Value.Durée != -1)
                    {
                        arch.Durée = liste.Value.Durée.ToString();
                    }
                    else
                    {
                        arch.Durée = "0";
                    }
                    source.Add(arch);
                }
            }
            Donnée_Archivage.ItemsSource = source;
        }



        //class interne pour permettre l'affectation des données
        public class Archives
        {
            public String Nom { get; set; }
            public String Prenom { get; set; }
            public String N_Pv { get; set; }
            public String Type_Prêt { get; set; }
            public String Date_de_Pv { get; set; }
            public String Motif { get; set; }
            public String Date_demande { get; set; }
            public String Montant_Prét_lettre { get; set; }
            public String Montant_Prét { get; set; }
            public String Observation { get; set; }
            public String Durée { get; set; }

            public String prem_paiement { get; set; }
            public String fin_paiement { get; set; }

            public string sum_rembours { get; set; }
        }



        //methodes de manupulation de l'interface

        private void Filtrer_Click(object sender, RoutedEventArgs e)
        {
            archivees.Children.Clear();
            archivees.Children.Add(new UserControl1());
        }

        private void Enregistrer_excel_click(object sender, RoutedEventArgs e)
        {
            responsable.export_Archive();
        }

        private void Recherche_Click(object sender, RoutedEventArgs e)
        {
            data_grid.Visibility = Visibility.Hidden;
            data_grid.IsEnabled = false;
        }
        private void details(object sender, RoutedEventArgs e)
        {
            archi.Visibility = Visibility.Visible; archi.IsEnabled = true;
            Archives st = Donnée_Archivage.SelectedItem as Archives;
            Archive pret = null;
            foreach (KeyValuePair<int, Archive> liste in responsable.liste_archives)
            {
                if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Pret.Date_demande) && (DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Pret.Date_pv)) && (Double.Parse(st.Montant_Prét) == liste.Value.Pret.Montant) && st.Nom.Equals(liste.Value.Pret.Employé.Nom) && st.Prenom.Equals(liste.Value.Pret.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Pret.Num_pv && (st.Type_Prêt.Equals(liste.Value.Pret.Type_Pret.Description) && st.sum_rembours.Equals(liste.Value.Pret.somme_rembours())) && (st.fin_paiement.Equals(liste.Value.Pret.fin_paiement())) && (st.Observation.Equals(liste.Value.Observations) && (st.prem_paiement.Equals(liste.Value.Pret.prem_paiment()))))

                {
                    if ((st.Durée.Equals(liste.Value.Durée.ToString()) || (st.Durée.Equals((liste.Value.Durée + 1).ToString()))))
                    {
                        pret = liste.Value;
                    }
                }
            }
            nom_detail.Text = pret.Pret.Employé.Nom + " " + pret.Pret.Employé.Prenom;
            prenom_detail.Text = pret.Pret.Employé.Email;
            date_nais_info.Text = pret.Pret.Employé.Date_naissance.ToShortDateString();
            num_sec_info.Text = pret.Pret.Employé.sec_soc;
            matricule_info.Text = pret.Pret.Employé.Matricule;
            date_recru_info.Text = pret.Pret.Employé.Date_prem.ToShortDateString();
            etat_soc_info.Text = pret.Pret.Employé.etats;
            service_info.Text = pret.Pret.Employé.Service;
            num_tel_info.Text = pret.Pret.Employé.tel;
            ccp_info.Text = pret.Pret.Employé.compte_ccp;
            cle_ccp_info.Text = pret.Pret.Employé.Cle_ccp;
            grade_info.Text = pret.Pret.Employé.Grade;
            description_info.Text = pret.Pret.Type_Pret.Description;
            num_pv_info.Text = pret.Pret.Num_pv.ToString();
            date_pv_info.Text = pret.Pret.Date_pv.ToShortDateString();
            date_demande_info.Text = pret.Pret.Date_demande.ToShortDateString();
            montant_info.Text = pret.Pret.Montant.ToString();
            montant_lettre_info.Text = pret.Pret.Montant_lettre;
            motif_info.Text = pret.Pret.Motif;
            if (pret.Durée != -1)
            {
                Duréee.Text = pret.Durée.ToString();
            }
            else
            {
                Duréee.Text = "0";
            }
            Some.Text = pret.Pret.somme_rembours();
            primo.Text = pret.Pret.prem_paiment();
            fino.Text = pret.Pret.fin_paiement();
        }

        private void retour_details(object sender, RoutedEventArgs e)
        {
            archivees.Visibility = Visibility.Visible; archivees.IsEnabled = true;
            archi.Visibility = Visibility.Hidden;
        }
    }
}