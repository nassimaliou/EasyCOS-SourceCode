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
using System.Windows.Navigation;
using System.Globalization;


namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour Window3.xaml
    /// </summary>
    /// 

    //Class principale de l'interface de recherche statiatique par liste

    public partial class Window3 : UserControl
    {

        public Window3()
        {
            responsable.liste_filtres_rem.Clear();
            responsable.liste_filtres.Clear();
            responsable.liste_filtres_non_rem.Clear();
            InitializeComponent();
            complete_type_pret();
            complete();
            complete_1();
            


        }



        //methodes de manupulation de l'interface

        private void complete_type_pret()
        {
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                type_p.Items.Add(liste.Value.Description);
            }
        }

        private void complete()
        {
            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {
                string nom = liste.Value.Nom + " " + liste.Value.Prenom;
                cmbp2.Items.Add(nom);
            }
        }
        private void complete_1()
        {
            cmb3.Items.Add("marié");
            cmb3.Items.Add("celibataire");
            cmb3.Items.Add("divorcé");
            cmb3.Items.Add("veuf(ve)");
        }


        private void recherche_Click(object sender, RoutedEventArgs e)
        {
            int compt = 0;

            responsable.liste_filtres.Clear();
            char[] whitespace = new char[] { ' ', '\t' };
            bool remboursable = false;
            int choix = 0;
            bool date1 = false;
            DateTime d_inf = new DateTime();
            bool date2 = false;
            DateTime d_max = new DateTime();
            bool date3 = false;
            DateTime pv_min = new DateTime();
            bool date4 = false;
            DateTime pv_max = new DateTime();
            bool durée1 = false;
            int durée_min = 0;
            bool durée2 = false;
            int durée_max = 0;
            bool somme1 = false;
            double somme_min = 0;
            bool somme2 = false;
            double somme_max = 0;
            bool employé = false;
            bool type = false;
            bool et= false;
            string etat=" ";
            bool drmb = false;
            DateTime drmd = new DateTime();
            bool drmaxb = false;
            DateTime drmaxd = new DateTime();

            if (!String.IsNullOrEmpty(min_pm.Text))
            {
                date1 = true;
                d_inf = DateTime.Parse(min_pm.Text);
            }

            //----------------------------------------------

            if (!String.IsNullOrEmpty(max_pm.Text))
            {
                date2 = true;
                d_inf = DateTime.Parse(max_pm.Text);
            }
            //----------------------------------------------

            if (!String.IsNullOrEmpty(min_pv.Text))
            {
                date3 = true;
                pv_min = DateTime.Parse(min_pv.Text);
            }
            //----------------------------------------------

            if (!String.IsNullOrEmpty(max_pv.Text))
            {
                date4 = true;
                pv_max = DateTime.Parse(max_pv.Text);
            }
            //----------------------------------------------

            if (!String.IsNullOrEmpty(dur_min.Text))
            {
                durée1 = true;
                try
                {
                    durée_min = int.Parse(dur_min.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("La durée minimale entrée est invalide" ,"erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    compt++;
                }
            }
            //----------------------------------------------
            if (!String.IsNullOrEmpty(dur_max.Text))
            {
                durée2 = true;
                try
                {
                    durée_max = int.Parse(dur_max.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("La durée maximale entrée est invalide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    compt++;
                }
            }
            //--------------------------------------------
            if (!String.IsNullOrEmpty(som_min.Text))
            {
                somme1 = true;
                try
                {
                    somme_min = double.Parse(som_min.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("La somme minimale entrée est invalide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    compt++;
                }
            }
            //--------------------------------------------
            if (!String.IsNullOrEmpty(som_max.Text))
            {
                somme2 = true;
                try
                {

                    somme_max = double.Parse(som_max.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("La somme maximale entrée est invalide", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    compt++;
                }
            }
            //--------------------------------------------
            if (!String.IsNullOrEmpty(cmb3.Text))
            {
                et = true;
                etat = cmb3.Text;
            }
            //--------------------------------------------
            if (!String.IsNullOrEmpty(drm.Text))
            {
                drmb = true;
                drmd = DateTime.Parse(drm.Text);
            }
            //--------------------------------------------
            if (!String.IsNullOrEmpty(drmax.Text))
            {
                drmaxb = true;
                drmaxd = DateTime.Parse(drmax.Text);
            }

            if (!String.IsNullOrEmpty(type_p.Text))
            {
                type = true;
                foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
                {
                    if (type_p.Text == liste.Value.Description)
                    {
                        responsable.clés_types.Add(liste.Value.Cle);
                        break;
                    }

                }

            }
            if (!String.IsNullOrEmpty(cmbp2.Text))
            {
                employé = true;
                string[] sizes = cmbp2.Text.Split(whitespace);
                foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
                {
                    if (sizes[0] == liste.Value.Nom && sizes[1] == liste.Value.Prenom)
                    {
                        responsable.clés_employés.Add(liste.Value.Cle);
                        break;
                    }

                }
            }
            if (DateTime.Compare(d_inf, d_max) > 0 && !String.IsNullOrEmpty(min_pm.Text) && !String.IsNullOrEmpty(max_pm.Text))
            {
                compt++;
                MessageBox.Show("La date minimale de demande doit etre inférieure à la date maximale de demande","erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (DateTime.Compare(pv_min, pv_max) > 0 && !String.IsNullOrEmpty(max_pv.Text) && !String.IsNullOrEmpty(min_pv.Text))
            {
                compt++;
                MessageBox.Show("La date minimale de PV doit etre inférieure à la date maximale de PV", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (durée_min > durée_max && !String.IsNullOrEmpty(dur_max.Text) && !String.IsNullOrEmpty(dur_min.Text))
            {
                compt++;
                MessageBox.Show("La durée minimale doit etre inférieure à la durée maximale", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (somme_min > somme_max && !String.IsNullOrEmpty(som_max.Text) && !String.IsNullOrEmpty(som_min.Text))
            {
                compt++;
                MessageBox.Show("La somme minimale doit etre inférieure à la somme maximale", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (somme_min < 0)
            {
                compt++;
                MessageBox.Show("La somme minimale doit etre positive", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            if (somme_max < 0)
            {
                compt++;
                MessageBox.Show("La somme maximale doit etre positive", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            if (durée_min < 0)
            {
                compt++;
                MessageBox.Show("La durée minimale doit etre positive","erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            if (durée_max < 0)
            {
                compt++;
                MessageBox.Show("La durée maximale doit etre positive","erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            if (DateTime.Compare(drmd, drmaxd) > 0 && !String.IsNullOrEmpty(drm.Text) && !String.IsNullOrEmpty(drmax.Text))
            {
                compt++;
                MessageBox.Show("La date minimale de recrutement doit etre inférieure à la date maximale", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (oui.IsChecked == true)
            {
                remboursable = true;
                choix = 1;
            }
            else if (non.IsChecked == true)
            {
                remboursable = true;
                choix = 2;
            }
            if (compt == 0)
            {
                if (clos.Text == "Clos")
                {
                    responsable.recherche_par_criteres(remboursable, choix, date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                }
                else if (clos.Text == "Non clos")
                {

                    if (oui.IsChecked == true)
                    {
                        remboursable = true;
                        choix = 1;

                        responsable.recherche_par_criteres_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                    }
                    else if (non.IsChecked == true)
                    {
                        remboursable = true;
                        choix = 2;

                        responsable.recherche_par_criteres_non_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);

                    }
                    else
                    {
                        responsable.recherche_par_criteres_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                        responsable.recherche_par_criteres_non_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                    }
                }
                else
                {
                    if (oui.IsChecked == true)
                    {
                        remboursable = true;
                        choix = 1;

                        responsable.recherche_par_criteres(remboursable, choix, date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                        responsable.recherche_par_criteres_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);

                    }
                    else if (non.IsChecked == true)
                    {
                        remboursable = true;
                        choix = 2;


                        responsable.recherche_par_criteres(remboursable, choix, date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                        responsable.recherche_par_criteres_non_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);

                    }
                    else
                    {
                        responsable.recherche_par_criteres(remboursable, choix, date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                        responsable.recherche_par_criteres_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, durée1, durée_min, durée2, durée_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                        responsable.recherche_par_criteres_non_rem(date1, d_inf, date2, d_max, date3, pv_min, date4, pv_max, somme1, somme_min, somme2, somme_max, employé, type, et, etat, drmb, drmd, drmaxb, drmaxd);
                    }
                    


                }

                //----------------------------------------------


                responsable.clés_employés.Clear();
                responsable.clés_types.Clear();
                Grid_Principale.Children.Clear();
                Grid_Principale.Children.Add(new Window4());
            }

            else
            {
                MessageBox.Show("Pas de recherche Tant que les données entrées sont érronnées");
                compt = 0;
            }
        }

        private void oui_Checked(object sender, RoutedEventArgs e)
        { 

            dur_min.IsEnabled = true;
            dur_max.IsEnabled = true;
            min_pm.IsEnabled = true;
            max_pm.IsEnabled = true;

        }

        private void non_Checked(object sender, RoutedEventArgs e)
        {

            dur_min.IsEnabled = false;
            dur_max.IsEnabled = false;
            min_pm.IsEnabled = false;
            max_pm.IsEnabled = false;
        }
    }
}
