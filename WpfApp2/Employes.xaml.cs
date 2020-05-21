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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour Window6.xaml
    /// </summary>
    public partial class Employes : UserControl
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
        private static string email_;
        private static string etat_service_;

        //Class principale de l'interface des employés

        public Employes()
        {
            InitializeComponent();
            actualiser();
        }


        //class interne pour permettre l'affectation des données

        public class employe
        {
            public string Id { get; set; }
            public String Matricule { get; set; }
            public String Nom { get; set; }
            public String Prenom { get; set; }
            public String Num_sec_soc { get; set; }
            public String Date_naissance { get; set; }
            public String Grade { get; set; }
            public String Date_recrutement { get; set; }
            public String Etat { get; set; }
            public String CCP { get; set; }
            public String Cle_ccp { get; set; }
            public String Tel { get; set; }            
            public String Service { get; set; }
            public String Email { get; set; }
            public String Etat_service { get; set; }
        }


        //methodes de manupulation de l'interface

        private void actualiser_click(object sender, RoutedEventArgs e)
        {
            liste_employés.ItemsSource = null;
            List<employe> source = new List<employe>();
            source.Clear();
            foreach (Employé liste in responsable.liste_employes.Values)
            {
                employe emp = new employe();
                emp.Id = liste.Cle.ToString();
                emp.Matricule = liste.Matricule;
                emp.Nom = liste.Nom;
                emp.Prenom = liste.Prenom;
                emp.Num_sec_soc = liste.sec_soc;
                emp.Date_naissance = liste.Date_naissance.ToShortDateString();
                emp.Date_recrutement = liste.Date_prem.ToShortDateString();
                emp.Grade = liste.Grade;
                emp.Etat = liste.etats;
                emp.CCP = liste.compte_ccp;
                emp.Cle_ccp = liste.Cle_ccp;
                emp.Tel = liste.tel;
                emp.Service = liste.Service;
                emp.Email = liste.Email;
                emp.Etat_service = liste.Etat_service;
                source.Add(emp);
            }
            liste_employés.ItemsSource = source;
        }
        private void actualiser()
        {
            liste_employés.ItemsSource = null;
            List<employe> source = new List<employe>();
            source.Clear();
            foreach (Employé liste in responsable.liste_employes.Values)
            {
                employe emp = new employe();
                emp.Id = liste.Cle.ToString();
                emp.Matricule = liste.Matricule;
                emp.Nom = liste.Nom;
                emp.Prenom = liste.Prenom;
                emp.Num_sec_soc = liste.sec_soc;
                emp.Date_naissance = liste.Date_naissance.ToShortDateString();
                emp.Date_recrutement = liste.Date_prem.ToShortDateString();
                emp.Grade = liste.Grade;
                emp.Etat = liste.etats;
                emp.CCP = liste.compte_ccp;
                emp.Cle_ccp = liste.Cle_ccp;
                emp.Tel = liste.tel;
                emp.Service = liste.Service;
                emp.Email = liste.Email;
                emp.Etat_service = liste.Etat_service;
                source.Add(emp);
            }
            liste_employés.ItemsSource = source;
        }

        private void Confirmer_Ajout_emp_Click(object sender, RoutedEventArgs e)
        {
            if (nom_ajout.Text.Equals("") || prenom_ajout.Text.Equals("") || matricule.Text.Equals("") || num_sec_social.Text.Equals("") || grade.Text.Equals("") || etat.Text.Equals("") || ccp.Text.Equals("") || cle_ccp.Text.Equals("") || telephone.Text.Equals("") || date_naiss.SelectedDate.Equals(null) || date_prem.SelectedDate.Equals(null))
            {
                Remarquee.Visibility = Visibility.Visible;
                DoubleAnimation a = new DoubleAnimation();
                a.From = 1.0; a.To = 0.0;
                a.Duration = new Duration(TimeSpan.FromSeconds(5));
                Remarquee.BeginAnimation(OpacityProperty, a);
            }
            else
            {
                int cpt = 0;
                int k = 0;
                long l = 0;

                nom_emp_ = nom_ajout.Text;
                prenom_emp_ = prenom_ajout.Text;
                matricule_emp_ = matricule.Text;
                num_sec_seoc_emp_ = num_sec_social.Text;
                grade_emp_ = grade.Text;
                etat_emp_ = etat.Text;
                ccp_emp_ = ccp.Text;
                cle_ccp_emp_ = cle_ccp.Text;
                tel_emp_ = telephone.Text;
                date_naiss_emp_ = date_naiss.SelectedDate.Value.ToShortDateString();
                date_recru_emp_ = date_prem.SelectedDate.Value.ToShortDateString();
                email_ = mail.Text;
                etat_service_ = etat2.Text;
                service_ = Service.Text;

                if (!int.TryParse(cle_ccp.Text, out k))
                {
                    MessageBox.Show("entrez une clé ccp valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    cpt++;
                }
                if (!int.TryParse(telephone.Text, out k))
                {
                    MessageBox.Show("entrez un numéro de téléphone valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    cpt++;
                }
                if (!int.TryParse(ccp.Text, out k))
                {
                    MessageBox.Show("entrez un compte ccp valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    cpt++;
                }
                long l1 = 0;
                if (!long.TryParse(matricule.Text, out l1))
                {
                    MessageBox.Show("entrez un matricule valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    cpt++;
                }
                if (!long.TryParse(num_sec_social.Text, out l))
                {
                    MessageBox.Show("entrez numéro de sécurité sociale valide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    cpt++;
                }
                int compt = 0;
                foreach (Employé emp in responsable.liste_employes.Values)
                {

                    if (compt == 0)
                    {
                        string ll = emp.sec_soc.Replace(" ", "");
                        long lll = long.Parse(ll);
                        if (l == lll)
                        {
                            MessageBox.Show("Le numéro de sécurité saisi appartient à un autre employé ", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            compt++;
                        }
                    }
                }
                long compt2 = 0;
                foreach (Employé emp in responsable.liste_employes.Values)
                {

                    if (compt2 == 0)
                    {
                        string ll = emp.Matricule.Replace(" ", "");
                        long lll = long.Parse(ll);
                        if (l1 == lll)
                        {
                            MessageBox.Show("Le matricule saisi appartient à un autre employé ", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            compt2++;
                        }
                    }
                }
                if (DateTime.Compare(date_naiss.SelectedDate.Value, date_prem.SelectedDate.Value) > 0)
                {


                    MessageBox.Show("La date naissace doit etre inférieure à la date de recrutement", "erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cpt++;

                }
                if (cpt == 0 && compt==0 && compt2==0)
                {
                    responsable.Creer_employe(matricule.Text, nom_ajout.Text, prenom_ajout.Text, num_sec_social.Text, DateTime.Parse(date_naiss.SelectedDate.ToString()), grade.Text, DateTime.Parse(date_prem.SelectedDate.ToString()), etat.Text, ccp.Text, cle_ccp.Text, telephone.Text, service_, email_, etat_service_);


                    Grid_Ajout_employe.Visibility = Visibility.Hidden; Grid_Ajout_employe.IsEnabled = false;
                    liste_employés.Visibility = Visibility.Visible; liste_employés.IsEnabled = true;
                    actualiser();
                }
            }
        }
        private void Annuler_Ajout_emp_Click(object sender, RoutedEventArgs e)
        {
            Grid_Ajout_employe.Visibility = Visibility.Hidden; Grid_Ajout_employe.IsEnabled = false;
            liste_employés.Visibility = Visibility.Visible; liste_employés.IsEnabled = true;
        }

        private void ajouter_employe(object sender, RoutedEventArgs e)
        {
            liste_employés.Visibility = Visibility.Hidden; liste_employés.IsEnabled = false;
            Grid_Ajout_employe.Visibility = Visibility.Visible; Grid_Ajout_employe.IsEnabled = true;
        }

        private void Import_excel_Click(object sender, RoutedEventArgs e)
        {
            responsable.import_employe();
        }
    }
}
