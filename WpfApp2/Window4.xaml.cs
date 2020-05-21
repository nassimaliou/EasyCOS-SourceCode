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
    /// Logique d'interaction pour Window4.xaml
    /// </summary>

    //Class principale de l'interface de statistiques par graph

    public partial class Window4 : UserControl
    {
        public Window4()
        {
            InitializeComponent();
            actualiser();
        }

        //class interne pour permettre l'affectation des données

        public class employee
        {
            public String Nom { get; set; }
            public String Prenom { get; set; }
            public String etat_social { get; set; }
            public String N_Pv { get; set; }
            public String Type_Prêt { get; set; }
            public String Date_de_Pv { get; set; }
            public String Motif { get; set; }
            public String Date_demande { get; set; }
            public String Date_de_recrutement { get; set; }
            public String Montant_Prét_lettre { get; set; }
            public String Montant_Prét { get; set; }
            public String Date_dernier_paiment { get; set; }
            public String Duree_de_paiment { get; set; }
        }

        //methodes de manupulation de l'interface

        private void actualiser()
        {
            double somme = 0;

            Resultats_recherche.ItemsSource = null;
            List<employee> source = new List<employee>();
            source.Clear();
            foreach (KeyValuePair<int, Archive> liste in responsable.liste_filtres)
            {
                employee Employe = new employee();
                Employe.Nom = liste.Value.Pret.Employé.Nom;
                Employe.Prenom = liste.Value.Pret.Employé.Prenom;
                Employe.etat_social = liste.Value.Pret.Employé.etats;
                Employe.N_Pv = liste.Value.Pret.Num_pv.ToString();
                Employe.Type_Prêt = liste.Value.Pret.Type_Pret.Description;
                Employe.Date_de_Pv = liste.Value.Pret.Date_pv.ToShortDateString();
                Employe.Motif = liste.Value.Pret.Motif;
                Employe.Date_de_recrutement = liste.Value.Pret.Employé.Date_prem.ToShortDateString();
                Employe.Montant_Prét_lettre = liste.Value.Pret.Montant_lettre;
                Employe.Montant_Prét = liste.Value.Pret.Montant.ToString();
                Employe.Date_dernier_paiment = liste.Value.Date_fin_remboursement.ToShortDateString();
                if (liste.Value.Durée < 0) { Employe.Duree_de_paiment = "/"; Employe.Date_demande = "/"; }
                else { Employe.Duree_de_paiment = liste.Value.Durée.ToString(); Employe.Date_demande = liste.Value.Pret.Date_demande.ToShortDateString(); }
                source.Add(Employe);
            }
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_filtres_rem)
            {
                employee Employe = new employee();
                Employe.Nom = liste.Value.Employé.Nom;
                Employe.Prenom = liste.Value.Employé.Prenom;
                Employe.etat_social = liste.Value.Employé.etats;
                Employe.N_Pv = liste.Value.Num_pv.ToString();
                Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                Employe.Date_de_Pv = liste.Value.Date_pv.ToShortDateString();
                Employe.Motif = liste.Value.Motif;
                Employe.Date_de_recrutement = liste.Value.Employé.Date_prem.ToShortDateString();
                Employe.Montant_Prét_lettre = liste.Value.Montant_lettre;
                Employe.Montant_Prét = liste.Value.Montant.ToString();
                Employe.Date_dernier_paiment = "En cours";
                Employe.Duree_de_paiment = "En cours";
                Employe.Date_demande = liste.Value.Date_demande.ToShortDateString();
                source.Add(Employe);
            }
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_filtres_non_rem)
            {
                employee Employe = new employee();
                Employe.Nom = liste.Value.Employé.Nom;
                Employe.Prenom = liste.Value.Employé.Prenom;
                Employe.etat_social = liste.Value.Employé.etats;
                Employe.N_Pv = liste.Value.Num_pv.ToString();
                Employe.Type_Prêt = liste.Value.Type_Pret.Description;
                Employe.Date_de_Pv = liste.Value.Date_pv.ToShortDateString();
                Employe.Motif = liste.Value.Motif;
                Employe.Date_de_recrutement = liste.Value.Employé.Date_prem.ToShortDateString();
                Employe.Montant_Prét_lettre = liste.Value.Montant_lettre;
                Employe.Montant_Prét = liste.Value.Montant.ToString();
                Employe.Date_dernier_paiment = "/";
                Employe.Duree_de_paiment = "/";
                Employe.Date_demande ="/" ;
                source.Add(Employe);
            }
            Resultats_recherche.ItemsSource = source;
            int a = responsable.liste_filtres.Count + responsable.liste_filtres_rem.Count + responsable.liste_filtres_non_rem.Count;
            cpt.Text = a.ToString();
            foreach (KeyValuePair<int, Archive> liste in responsable.liste_filtres)
            {
                somme += liste.Value.Pret.Montant;
            }
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_filtres_rem)
            {
                somme += liste.Value.Montant;
            }
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_filtres_non_rem)
            {
                somme += liste.Value.Montant;
            }
            somme_total.Text = somme.ToString();
            }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            Main_Grid.Children.Clear();
            Main_Grid.Children.Add(new Window3());

        }
    }
}
