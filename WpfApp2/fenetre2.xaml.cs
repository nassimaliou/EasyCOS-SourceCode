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

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour fenetre2.xaml
    /// </summary>
    public partial class fenetre2 : UserControl
    {
        public fenetre2()
        {
            InitializeComponent();
            actualiser();
        }
        public class employee
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

            public String Duree_de_paiment { get; set; }
            public String Observation { get; set; }
            public String prem_paiement { get; set; }
            public String fin_paiement { get; set; }

            public string sum_rembours { get; set; }

        }
        private void actualiser()
        {

            Resultats_recherche.ItemsSource = null;
            List<employee> source = new List<employee>();
            source.Clear();
            foreach (KeyValuePair<int, Archive> liste in responsable.liste_filtres)
            {

                employee Employe = new employee();
                Employe.Nom = liste.Value.Pret.Employé.Nom;
                Employe.Prenom = liste.Value.Pret.Employé.Prenom;
                Employe.N_Pv = liste.Value.Pret.Num_pv.ToString();
                Employe.Type_Prêt = liste.Value.Pret.Type_Pret.Description;
                Employe.Date_de_Pv = liste.Value.Pret.Date_pv.ToString();
                Employe.Motif = liste.Value.Pret.Motif;
                Employe.Date_demande = liste.Value.Pret.Date_demande.ToString();
                Employe.Montant_Prét_lettre = liste.Value.Pret.Montant_lettre;
                Employe.Montant_Prét = liste.Value.Pret.Montant.ToString();
                Employe.prem_paiement = liste.Value.Pret.prem_paiment();
                Employe.fin_paiement = liste.Value.Pret.fin_paiement();
                Employe.sum_rembours = liste.Value.Pret.somme_rembours();
                Employe.Observation = liste.Value.Observations;
                if (liste.Value.Durée != -1)
                {
                    Employe.Duree_de_paiment = liste.Value.Durée.ToString();
                }
                else
                {
                    Employe.Duree_de_paiment = "0";
                }

                source.Add(Employe);

            }
            Resultats_recherche.ItemsSource = source;
            cpt.Text = responsable.liste_filtres.Count.ToString();
        }
        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            Main_Grid.Children.Clear();
            Main_Grid.Children.Add(new Archivage());


        }

        private void details(object sender, RoutedEventArgs e)
        {
            fenet.Visibility = Visibility.Visible; fenet.IsEnabled = true;
            employee st = Resultats_recherche.SelectedItem as employee;
            Archivage sv = Resultats_recherche.SelectedItem as Archivage;
            Archive pret = null;
            foreach (KeyValuePair<int, Archive> liste in responsable.liste_filtres)
            {

                if (DateTime.Parse(st.Date_demande).Equals(liste.Value.Pret.Date_demande) && (DateTime.Parse(st.Date_de_Pv).Equals(liste.Value.Pret.Date_pv)) && (Double.Parse(st.Montant_Prét) == liste.Value.Pret.Montant) && st.Nom.Equals(liste.Value.Pret.Employé.Nom) && st.Prenom.Equals(liste.Value.Pret.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Value.Pret.Num_pv && (st.Type_Prêt.Equals(liste.Value.Pret.Type_Pret.Description) && st.sum_rembours.Equals(liste.Value.Pret.somme_rembours())) && (st.fin_paiement.Equals(liste.Value.Pret.fin_paiement())) && (st.Observation.Equals(liste.Value.Observations) && (st.prem_paiement.Equals(liste.Value.Pret.prem_paiment()))))

                {

                    if ((st.Duree_de_paiment.Equals(liste.Value.Durée.ToString()) || (st.Duree_de_paiment.Equals((liste.Value.Durée + 1).ToString()))))
                    {
                        pret = liste.Value;
                    }
                }
            }

            nom_detail.Text = pret.Pret.Employé.Nom + " " + pret.Pret.Employé.Prenom;
            prenom_detail.Text = pret.Pret.Employé.Email;
            date_nais_info.Text = pret.Pret.Employé.Date_naissance.ToString();
            num_sec_info.Text = pret.Pret.Employé.sec_soc;
            matricule_info.Text = pret.Pret.Employé.Matricule;
            date_recru_info.Text = pret.Pret.Employé.Date_prem.ToString();
            etat_soc_info.Text = pret.Pret.Employé.etats;
            service_info.Text = pret.Pret.Employé.Service;
            num_tel_info.Text = pret.Pret.Employé.tel;
            ccp_info.Text = pret.Pret.Employé.compte_ccp;
            cle_ccp_info.Text = pret.Pret.Employé.Cle_ccp;
            grade_info.Text = pret.Pret.Employé.Grade;
            description_info.Text = pret.Pret.Type_Pret.Description;
            num_pv_info.Text = pret.Pret.Num_pv.ToString();
            date_pv_info.Text = pret.Pret.Date_pv.ToString();
            date_demande_info.Text = pret.Pret.Date_demande.ToString();
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
            Main_Grid.Visibility = Visibility.Visible; Main_Grid.IsEnabled = true;
            fenet.Visibility = Visibility.Hidden;
        }
    }
}