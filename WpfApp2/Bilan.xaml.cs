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
    /// Logique d'interaction pour Bilan.xaml
    /// </summary>
    public partial class Bilan : UserControl
    {

        //Class principale de l'interface de bilan annuel
        public Bilan()
        {
            InitializeComponent();
            Grid_année.Visibility = Visibility.Visible;

        }


        //class interne pour permettre l'affectation des données
        public class bilann
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
            public String Etat { get; set; }
            public String Durée { get; set; }

            public String prem_paiement { get; set; }
            public String fin_paiement { get; set; }

            public string sum_rembours { get; set; }
        }


        //methodes de manupulation de l'interface

        private void Clik(object sender, RoutedEventArgs e)
        {
            int ann = 0;
            int cpt = 0;

            if (!String.IsNullOrEmpty(an.Text))
            {

                try
                {
                    ann = int.Parse(an.Text);

                }
                catch (FormatException)
                {
                    MessageBox.Show("L'année entrée est invalide");
                    data_grid.Visibility = Visibility.Hidden;
                    Grid_année.Visibility = Visibility.Visible;
                    cpt++;
                }
                if (cpt == 0)
                {
                    Grid_année.Visibility = Visibility.Hidden;
                    data_grid.Visibility = Visibility.Visible;
                    responsable.remplissage_bilan(ann);
                    List<bilann> source = new List<bilann>();

                    foreach (Prets liste in responsable.bilan)
                    {
                        bilann arch = new bilann();
                        if (liste.GetType() == typeof(pret_remboursable))
                        {
                            if (responsable.liste_pret_remboursable.ContainsValue((pret_remboursable)liste))
                            {
                                arch.Etat = "en cours";
                            }
                            else
                            {
                                arch.Etat = "cloturé";
                            }
                        }
                        else
                        {
                            if (liste.GetType() == typeof(pret_non_remboursable))
                            {
                                if (responsable.liste_pret_non_remboursables.ContainsValue((pret_non_remboursable)liste))
                                {
                                    arch.Etat = "en cours";
                                }
                                else
                                {
                                    arch.Etat = "cloturé";
                                }

                            }

                        }



                        arch.Nom = liste.Employé.Nom;
                        arch.Prenom = liste.Employé.Prenom;
                        arch.N_Pv = liste.Num_pv.ToString();
                        arch.Motif = liste.Motif;
                        arch.Date_demande = liste.Date_demande.ToShortDateString();
                        arch.Montant_Prét = liste.Montant.ToString();
                        arch.Montant_Prét_lettre = liste.Montant_lettre;
                        arch.Observation = "";
                        arch.Type_Prêt = liste.Type_Pret.Description.ToString();
                        arch.Date_de_Pv = liste.Date_pv.ToShortDateString();
                        arch.prem_paiement = liste.prem_paiment();
                        arch.fin_paiement = liste.fin_paiement();
                        arch.sum_rembours = liste.somme_rembours();
                        if (liste.GetType() == typeof(pret_remboursable))
                        {
                            pret_remboursable p = (pret_remboursable)liste;
                            arch.Durée = p.Durée.ToString();
                        }
                        else
                        {
                            arch.Durée = "0";
                        }

                        source.Add(arch);

                    }
                    Donnée_bilan.ItemsSource = source;
                }

            }
        }
        private void Export(object sender, RoutedEventArgs e)
        {
            responsable.export_bilan();
        }

        private void Détails(object sender, RoutedEventArgs e)
        {
            Donnée_bilan.Visibility = Visibility.Hidden;
            archi.Visibility = Visibility.Visible; archi.IsEnabled = true;

            bilann st = Donnée_bilan.SelectedItem as bilann;
            Prets pret = null;
            foreach (Prets liste in responsable.bilan)
            {
                if (DateTime.Parse(st.Date_demande).Equals(liste.Date_demande) && (DateTime.Parse(st.Date_de_Pv).Equals(liste.Date_pv)) && (Double.Parse(st.Montant_Prét) == liste.Montant) && st.Nom.Equals(liste.Employé.Nom) && st.Prenom.Equals(liste.Employé.Prenom) && Int32.Parse(st.N_Pv) == liste.Num_pv && (st.Type_Prêt.Equals(liste.Type_Pret.Description) && st.sum_rembours.Equals(liste.somme_rembours())) && (st.fin_paiement.Equals(liste.fin_paiement())) && (st.prem_paiement.Equals(liste.prem_paiment())))

                {

                    pret = liste;

                }
            }
            nom_detail.Text = pret.Employé.Nom + " " + pret.Employé.Prenom;
            prenom_detail.Text = pret.Employé.Email;
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
            Some.Text = pret.somme_rembours();
            primo.Text = pret.prem_paiment();
            fino.Text = pret.fin_paiement();
            if (pret.GetType() == typeof(pret_non_remboursable))
            {
                Duréee.Text = "0";
            }
            if (pret.GetType() == typeof(pret_remboursable))
            {
                pret_remboursable p1 = (pret_remboursable)pret;
                Duréee.Text = p1.Durée.ToString();
            }
            archi.Visibility = Visibility.Visible;
        }
        private void retour_details(object sender, RoutedEventArgs e)
        {
            Donnée_bilan.Visibility = Visibility.Visible;
            archi.Visibility = Visibility.Hidden;

        }
    }

}