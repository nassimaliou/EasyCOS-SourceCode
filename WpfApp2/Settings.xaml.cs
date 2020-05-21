using Microsoft.Win32;
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
using System.IO;

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour Window2.xaml
    /// </summary>
    public partial class Window2 : UserControl
    {
        public ImageSource user_photo;
        public static string user_name;
        public static string user_Password;

        public static bool mode_archivage ;
        public static string durée_avant_archivage_d ;
        public static string durée_avant_archivage_m ;
        public static bool envoi_notif ;
        public static bool mode_envoi ;

        //sauvgarde et chargement des parametres
        public static void save_option()
        {
            try
            {
                string path = @".\\option.sESI";
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(mode_archivage.ToString() + '|' + durée_avant_archivage_m + '|' + durée_avant_archivage_d + '|' + envoi_notif.ToString() + '|' + mode_envoi.ToString() + '|' + user_name + '|' + user_Password + '|' + responsable.User_mail + '|' + responsable.User_pwd);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pu être lu1.");
                Console.WriteLine(e.Message);
            }
        }

        public static void charger_option()
        {
            string[] op = new string[6];
            try
            {
                string path = @".\\option.sESI";
                using (StreamReader sr = new StreamReader(path))
                {
                    sr.BaseStream.Position = 0;
                    string line = sr.ReadLine();
                    op = line.Split('|');
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pu être lu.");
                Console.WriteLine(e.Message);
            }
            try
            {
                mode_archivage = bool.Parse(op[0]);
            }
            catch
            {
                Console.WriteLine(op[0]);
            }
            durée_avant_archivage_m = op[1];
            durée_avant_archivage_d = op[2];
            envoi_notif = bool.Parse(op[3]);
            mode_envoi = bool.Parse(op[4]);
            user_name = op[5];
            user_Password = op[6];
            responsable.User_mail = op[7];
            responsable.User_pwd = op[8];
        }        

        //Class principale de l'interface de parametres

        public Window2(TextBox psuedo_show, PasswordBox pass, Image img)
        {
            InitializeComponent();
            charger_option();
            if (mode_archivage)
            { jours_archv.Visibility = Visibility.Visible; mois_archv.Visibility = Visibility.Visible; label_d.Visibility = Visibility.Visible; }
            else { jours_archv.Visibility = Visibility.Hidden; mois_archv.Visibility = Visibility.Hidden; label_d.Visibility = Visibility.Hidden; }

            if (envoi_notif)
            { toogle_mode_envoi.Visibility = Visibility.Visible; auto.Visibility = Visibility.Visible; man.Visibility = Visibility.Visible; label_m.Visibility = Visibility.Visible; }
            else { toogle_mode_envoi.Visibility = Visibility.Hidden; auto.Visibility = Visibility.Hidden; man.Visibility = Visibility.Hidden; label_m.Visibility = Visibility.Hidden; }

            user_name = psuedo_show.Text;
            user_Password = pass.Password;
            user_photo = img.Source;

            toogle_mode_archive.IsChecked = mode_archivage;
            jours_archv.Text = durée_avant_archivage_d;
            mois_archv.Text = durée_avant_archivage_m;
            tooggle_envoi_notif.IsChecked = envoi_notif;
            toogle_mode_envoi.IsChecked = mode_envoi;
            Label_aide.Foreground = Brushes.Blue;
        }



        //methodes de manupulation de l'interface

        private void Confirmer_changement_Click(object sender, RoutedEventArgs e)
        {
            if ((mot_de_passe_nouveau_confirmation_Valide.Visibility == Visibility.Visible) && (mot_de_passe_actuel_Valide.Visibility == Visibility.Visible))
            {
                user_Password = mot_de_passe_nouveau_confirmation.Password;
                mot_de_passe_actuel.Password = null;
                mot_de_passe_nouveau.Password = null;
                mot_de_passe_nouveau_confirmation = null;
            }
            else if (mot_de_passe_actuel_Invalide.Visibility == Visibility.Visible)
            {
                DoubleAnimation c = new DoubleAnimation();
                c.From = 1.0; c.To = 0.0;
                c.Duration = new Duration(TimeSpan.FromSeconds(4));
                mot_de_passe_actuel_Invalide.BeginAnimation(OpacityProperty, c);

                if (mot_de_passe_nouveau_confirmation_Invalide.Visibility == Visibility.Visible)
                {
                    DoubleAnimation k = new DoubleAnimation();
                    k.From = 1.0; k.To = 0.0;
                    k.Duration = new Duration(TimeSpan.FromSeconds(4));
                    mot_de_passe_nouveau_confirmation_Invalide.BeginAnimation(OpacityProperty, k);
                }
            }
            else if (mot_de_passe_nouveau_confirmation_Invalide.Visibility == Visibility.Visible)
            {
                DoubleAnimation k = new DoubleAnimation();
                k.From = 1.0; k.To = 0.0;
                k.Duration = new Duration(TimeSpan.FromSeconds(4));
                mot_de_passe_nouveau_confirmation_Invalide.BeginAnimation(OpacityProperty, k);
                if (mot_de_passe_actuel_Invalide.Visibility == Visibility.Visible)
                {
                    DoubleAnimation c = new DoubleAnimation();
                    c.From = 1.0; c.To = 0.0;
                    c.Duration = new Duration(TimeSpan.FromSeconds(4));
                    mot_de_passe_actuel_Invalide.BeginAnimation(OpacityProperty, c);
                }
            }
        }

        private void Annuler_changement_Click(object sender, RoutedEventArgs e)
        {
            Label_nom_utilisateur.Visibility = Visibility.Visible;
            Label_photo.Visibility = Visibility.Visible;
            Label_mail.Visibility = Visibility.Visible;

            mot_de_passe_actuel_Invalide.Visibility = Visibility.Hidden;
            mot_de_passe_actuel_Valide.Visibility = Visibility.Hidden;
            mot_de_passe_nouveau_confirmation_Invalide.Visibility = Visibility.Hidden;
            mot_de_passe_nouveau_confirmation_Valide.Visibility = Visibility.Hidden;
            mot_de_passe_modification.Visibility = Visibility.Collapsed;

        }

        private void mot_de_passe_nouveau_confirmation_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (mot_de_passe_nouveau.Password.Equals(mot_de_passe_nouveau_confirmation.Password) && (mot_de_passe_nouveau.Password.Length != 0))
            {
                mot_de_passe_nouveau_confirmation_Valide.Visibility = Visibility.Visible;
                mot_de_passe_nouveau_confirmation_Invalide.Visibility = Visibility.Hidden;
            }
            else
            {
                mot_de_passe_nouveau_confirmation_Valide.Visibility = Visibility.Hidden;
                mot_de_passe_nouveau_confirmation_Invalide.Visibility = Visibility.Visible;
            }
        }

        private void back_Menu_Click(object sender, RoutedEventArgs e)
        {
            if(mois_archv.Text.Equals("") || jours_archv.Text.Equals(""))
            {
                MessageBox.Show("Durée avant archivage non asignée.\nUne valeur par defaut sera afffectée.", "Options durée", MessageBoxButton.OK, MessageBoxImage.Information);

                if(mois_archv.Text.Equals(""))
                {
                    mois_archv.Text = "0";
                }
                if (jours_archv.Text.Equals(""))
                {
                    jours_archv.Text = "30";
                }
            }

            if(!(int.TryParse(mois_archv.Text,out int j)) || !(int.TryParse(jours_archv.Text,out int i)))
            {
                MessageBox.Show("Introduisez une durée valide.", "Options durée", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            { 
                mode_archivage = toogle_mode_archive.IsChecked.Value;
                durée_avant_archivage_d = jours_archv.Text;
                durée_avant_archivage_m = mois_archv.Text;
                envoi_notif = tooggle_envoi_notif.IsChecked.Value;
                mode_envoi = toogle_mode_envoi.IsChecked.Value;

                grid_settings.Visibility = Visibility.Hidden;
                grid_settings.IsEnabled = false;
                save_option();
            }
        }

        private void Confirmer_changement_Nom_utilisateur_Click(object sender, RoutedEventArgs e)
        {
            if ((mot_de_passe_Valide.Visibility == Visibility.Visible) && (Nom_utilisateur_nouveau.Text != null))
            {
                user_name = Nom_utilisateur_nouveau.Text;
                mot_de_passe.Password = null;
                Nom_utilisateur_nouveau.Text = null;
            }
            else
            {
                DoubleAnimation anim = new DoubleAnimation();
                anim.From = 1.0; anim.To = 0.0;
                anim.Duration = new Duration(TimeSpan.FromSeconds(4));
                mot_de_passe_Invalide.BeginAnimation(OpacityProperty, anim);
            }

        }

        private void Annuler_changement_Nom_utilisateur_Click(object sender, RoutedEventArgs e)
        {
            Label_pwd.Visibility = Visibility.Visible;
            Label_photo.Visibility = Visibility.Visible;
            Label_mail.Visibility = Visibility.Visible;

            mot_de_passe.Password = null;
            Nom_utilisateur_nouveau.Text = null;
            mot_de_passe_Valide.Visibility = Visibility.Hidden;
            mot_de_passe_Invalide.Visibility = Visibility.Hidden;
            Pseudo_modification.Visibility = Visibility.Collapsed;
        }

        private void mot_de_passe_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (mot_de_passe.Password.Equals(user_Password))
            {
                mot_de_passe_Invalide.Visibility = Visibility.Hidden;
                mot_de_passe_Valide.Visibility = Visibility.Visible;
            }
            else
            {
                mot_de_passe_Invalide.Visibility = Visibility.Visible;
                mot_de_passe_Valide.Visibility = Visibility.Hidden;
            }
        }

        private void mot_de_passe_actuel_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (mot_de_passe_actuel.Password.Equals(user_Password))
            {
                mot_de_passe_actuel_Valide.Visibility = Visibility.Visible;
                mot_de_passe_actuel_Invalide.Visibility = Visibility.Hidden;
            }
            else
            {
                mot_de_passe_actuel_Valide.Visibility = Visibility.Hidden;
                mot_de_passe_actuel_Invalide.Visibility = Visibility.Visible;
            }
        }

        private void Label_aide_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label_aide.Foreground = Brushes.BlueViolet;
            System.Diagnostics.Process.Start("https://easycoshelp2020.000webhostapp.com/");
        }

        private void upload_image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                profil.Source = new BitmapImage(fileUri);

                confirmer_upload_image.Visibility = Visibility.Visible;
                upload_image.Visibility = Visibility.Hidden;
            }

        }

        private void annuler_upload_image_Click(object sender, RoutedEventArgs e)
        {
            Label_pwd.Visibility = Visibility.Visible;
            Label_nom_utilisateur.Visibility = Visibility.Visible;
            Label_mail.Visibility = Visibility.Visible;

            profil.Source = null;
            default_picture.Visibility = Visibility.Hidden;
            Image_modification.Visibility = Visibility.Collapsed;
        }

        private void confirmer_upload_image_Click(object sender, RoutedEventArgs e)
        {
            user_photo = profil.Source;
            profil_confirmation.Visibility = Visibility.Visible;
            DoubleAnimation k = new DoubleAnimation();
            k.From = 1.0;
            k.To = 0.0;
            k.Duration = new Duration(TimeSpan.FromSeconds(5));
            k.Completed += new EventHandler(fin_label);
            profil_confirmation.BeginAnimation(OpacityProperty, k);

        }
        private void fin_label(object sender, EventArgs e)
        {
            default_picture.Visibility = Visibility.Hidden;
            Image_modification.Visibility = Visibility.Collapsed;
            profil.Source = null;
        }

        private void toogle_mode_archive_Checked(object sender, RoutedEventArgs e)
        {
            mode_archivage = toogle_mode_archive.IsChecked.Value;

            if (mode_archivage)
            { jours_archv.Visibility = Visibility.Visible; mois_archv.Visibility = Visibility.Visible; label_d.Visibility = Visibility.Visible; }
            else { jours_archv.Visibility = Visibility.Hidden; mois_archv.Visibility = Visibility.Hidden; label_d.Visibility = Visibility.Hidden; }
        }

        private void tooggle_envoi_notif_Checked(object sender, RoutedEventArgs e)
        {
            envoi_notif = tooggle_envoi_notif.IsChecked.Value;

            if (envoi_notif)
            { toogle_mode_envoi.Visibility = Visibility.Visible; auto.Visibility = Visibility.Visible; man.Visibility = Visibility.Visible; label_m.Visibility = Visibility.Visible; }
            else { toogle_mode_envoi.Visibility = Visibility.Hidden; auto.Visibility = Visibility.Hidden; man.Visibility = Visibility.Hidden; label_m.Visibility = Visibility.Hidden; }

        }

        private void tooggle_envoi_notif_Unchecked(object sender, RoutedEventArgs e)
        {
            envoi_notif = tooggle_envoi_notif.IsChecked.Value;

            if (envoi_notif)
            { toogle_mode_envoi.Visibility = Visibility.Visible; auto.Visibility = Visibility.Visible; man.Visibility = Visibility.Visible; label_m.Visibility = Visibility.Visible; }
            else { toogle_mode_envoi.Visibility = Visibility.Hidden; auto.Visibility = Visibility.Hidden; man.Visibility = Visibility.Hidden; label_m.Visibility = Visibility.Hidden; }

        }

        private void toogle_mode_archive_Unchecked(object sender, RoutedEventArgs e)
        {
            mode_archivage = toogle_mode_archive.IsChecked.Value;

            if (mode_archivage)
            { jours_archv.Visibility = Visibility.Visible; mois_archv.Visibility = Visibility.Visible; label_d.Visibility = Visibility.Visible; }
            else { jours_archv.Visibility = Visibility.Hidden; mois_archv.Visibility = Visibility.Hidden; label_d.Visibility = Visibility.Hidden; }
        }

        private void remise_new_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult r = MessageBox.Show("Voulez vous vraiment reinitaliser les données ?\nCette action est irreversible.", "Attention", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            switch(r)
            {
                case MessageBoxResult.OK:
                    WpfTutorialSamples.Dialogs.InputDialogSample2 fenetre = new WpfTutorialSamples.Dialogs.InputDialogSample2();
                    fenetre.ShowActivated = true;
                    fenetre.Show();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void Label_nom_utilisateur_Click(object sender, RoutedEventArgs e)
        {
            Label_pwd.Visibility = Visibility.Collapsed;
            Label_photo.Visibility = Visibility.Collapsed;

            Pseudo_modification.Visibility = Visibility.Visible;
            mot_de_passe_modification.Visibility = Visibility.Collapsed;
            Image_modification.Visibility = Visibility.Collapsed;
            default_picture.Visibility = Visibility.Collapsed;

            mdp_modification.Visibility = Visibility.Collapsed;
            Label_mail.Visibility = Visibility.Collapsed;
        }

        private void Label_photo_Click(object sender, RoutedEventArgs e)
        {
            Label_pwd.Visibility = Visibility.Collapsed;
            Label_nom_utilisateur.Visibility = Visibility.Collapsed;

            Pseudo_modification.Visibility = Visibility.Collapsed;
            mot_de_passe_modification.Visibility = Visibility.Collapsed;
            Image_modification.Visibility = Visibility.Visible;
            default_picture.Visibility = Visibility.Visible;

            mdp_modification.Visibility = Visibility.Collapsed;
            Label_mail.Visibility = Visibility.Collapsed;
        }

        private void Label_pwd_Click(object sender, RoutedEventArgs e)
        {
            Label_photo.Visibility = Visibility.Collapsed;
            Label_nom_utilisateur.Visibility = Visibility.Collapsed;

            mot_de_passe_modification.Visibility = Visibility.Visible;
            Pseudo_modification.Visibility = Visibility.Collapsed;
            Image_modification.Visibility = Visibility.Collapsed;
            default_picture.Visibility = Visibility.Collapsed;

            mdp_modification.Visibility = Visibility.Collapsed;
            Label_mail.Visibility = Visibility.Collapsed;
        }

        private void Label_mail_Click(object sender, RoutedEventArgs e)
        {
            Label_pwd.Visibility = Visibility.Collapsed;
            Label_photo.Visibility = Visibility.Collapsed;
            Label_nom_utilisateur.Visibility = Visibility.Collapsed;

            Pseudo_modification.Visibility = Visibility.Collapsed;
            mot_de_passe_modification.Visibility = Visibility.Collapsed;
            Image_modification.Visibility = Visibility.Collapsed;
            default_picture.Visibility = Visibility.Collapsed;

            mdp_modification.Visibility = Visibility.Visible;
        }

        private void Confirmer_changement_mail_Click(object sender, RoutedEventArgs e)
        {
            if (!Mail.Text.Equals("") && !mdp_mail.Password.Equals(""))
            { 
                responsable.User_mail = Mail.Text;
                responsable.User_pwd = mdp_mail.Password;
            }
            else 
            {
                MessageBox.Show("Introduisez un mail ou mot de passe valide.", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Annuler_changement_mail_Click(object sender, RoutedEventArgs e)
        {
            Label_pwd.Visibility = Visibility.Visible;
            Label_photo.Visibility = Visibility.Visible;
            Label_mail.Visibility = Visibility.Visible;
            Label_nom_utilisateur.Visibility = Visibility.Visible;

            mdp_modification.Visibility = Visibility.Collapsed;
        }
    }
}