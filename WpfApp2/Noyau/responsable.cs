using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Net.Mail;
using System.Windows;
using System.Security.RightsManagement;
using System.Reflection;

namespace WpfApp2
{
    public class responsable
    {
        public static double tresor;//Le compte virtuel du COS
        public static int cle_liste_types = 1;              // des attributs statiques permettant de donner des
        public static int cle_liste_pret_remboursable = 1;           // cles uniques aux differents dictionnaires 
        public static int cle_liste_non_remboursable = 1;                 // utilisés en s'incrémentant à chaque ajout
        public static int cle_liste_employe = 1;                             // les clés se sont gérées par nous et pas selon l'introduction
        public static int cle_liste_archive = 1;                                     // de l'utilisateur.
        public static Dictionary<int, Employé> liste_employes = new Dictionary<int, Employé>();//liste des employés de l'école
        public static Dictionary<int, Type_pret> liste_types = new Dictionary<int, Type_pret>();//liste comportant tous les types prets existants
        public static Dictionary<int, Archive> liste_archives = new Dictionary<int, Archive>();//liste des prets archivés
        public static Dictionary<int, pret_remboursable> liste_pret_remboursable = new Dictionary<int, pret_remboursable>();//liste des prets remboursables accordés
        public static Dictionary<int, pret_non_remboursable> liste_pret_non_remboursables = new Dictionary<int, pret_non_remboursable>();//liste des prets non remboursables décérnés
        public static Dictionary<int, pret_remboursable> liste_pret_remboursable_provisoire = new Dictionary<int, pret_remboursable>();
        public static Dictionary<int, Archive> liste_archives_provisoire = new Dictionary<int, Archive>();

        public static List<Modification> pile_modifications = new List<Modification>();

        public static List<Prets> bilan = new List<Prets>();//liste comportant tous les prets accordées dans une année donnée
        public static Dictionary<int, Archive> liste_filtres = new Dictionary<int, Archive>(); //liste des archives apres filtrage
        public static Dictionary<int, pret_remboursable> liste_filtres_rem = new Dictionary<int, pret_remboursable>();//liste des prets remboursables apres filtrage
        public static Dictionary<int, pret_non_remboursable> liste_filtres_non_rem = new Dictionary<int, pret_non_remboursable>();//liste des prets non remboursables apres filtrage
        public static List<int> clés_employés = new List<int>();
        public static List<int> clés_types = new List<int>();
        //------------------------------------------------------------------------------
        //Liste pour faire des statistiques
        public static Dictionary<int, int> liste_stat_1 = new Dictionary<int, int>();
        public static Dictionary<int, double> list_sup = new Dictionary<int, double>();
        public static Dictionary<int, double> list_inf = new Dictionary<int, double>();
        public static Dictionary<int, double> liste_tresor = new Dictionary<int, double>();

        public static List<String> services = new List<String>();
        public static List<String> choix_service = new List<string>();
        //----------------------------------------------------------------------------------
        public static string User_mail = "im_aliousalah@esi.dz";
        public static string User_pwd = "";
        //lecture-----------------------------------------------------------
        public static void initialiser_dictionnaire_employes() //Initialisation de la liste des employés depuis la base de donnée
        {
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            string commande = "SELECT COUNT(*) FROM employes;";
            int longueur_table = 0;
            cmd.CommandText = commande;
            cmd.ExecuteNonQuery();
            SqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            longueur_table = (int)rdr1.GetValue(0);
            rdr1.Close();
            for (int i = 1; i <= longueur_table; i++)
            {
                commande = "SELECT * FROM employes WHERE cle = " + i.ToString() + " ;";
                cmd.CommandText = commande;
                cmd.ExecuteNonQuery();
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                DateTime date_naiss = DateTime.Parse(rdr.GetValue(4).ToString());
                DateTime date_prem = DateTime.Parse(rdr.GetValue(6).ToString());

                Employé emp = new Employé((int)rdr.GetValue(0), rdr.GetValue(11).ToString(), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), date_naiss, rdr.GetValue(5).ToString(), date_prem, rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString());
                rdr.Close();
                responsable.liste_employes.Add(emp.Cle, emp);
            }
            cnx.Close();
        }

        public static void initialiser_dictionnaire_archive()//Initialisation de la liste des archives depuis la base de donnée
        {
            int longueur_table = 0;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM archive;";
            cmd.ExecuteNonQuery();
            SqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            longueur_table = (int)rdr1.GetValue(0);
            rdr1.Close();
            cmd.Cancel();
            for (int i = 1; i <= longueur_table; i++)
            {
                SqlCommand commande = cnx.CreateCommand();
                commande.CommandText = "SELECT * FROM archive WHERE cle = " + i.ToString() + " ;";
                commande.ExecuteNonQuery();
                SqlDataReader rdr = commande.ExecuteReader();
                rdr.Read();
                float var = (float)rdr.GetDouble(9);
                if (var != -1.0)
                {
                    int id_employe = (int)rdr.GetValue(1);
                    Employé emp = responsable.liste_employes[id_employe];
                    Dictionary<int, double> mois = new Dictionary<int, double>();
                    mois.Add(0, (double)rdr.GetDouble(9));
                    mois.Add(1, (double)rdr.GetDouble(10));
                    mois.Add(2, (double)rdr.GetDouble(11));
                    mois.Add(3, (double)rdr.GetDouble(12));
                    mois.Add(4, (double)rdr.GetDouble(13));
                    mois.Add(5, (double)rdr.GetDouble(14));
                    mois.Add(6, (double)rdr.GetDouble(15));
                    mois.Add(7, (double)rdr.GetDouble(16));
                    mois.Add(8, (double)rdr.GetDouble(17));
                    mois.Add(9, (double)rdr.GetDouble(18));
                    //
                    int cle_type_pret = (int)rdr.GetValue(2);
                    SqlConnection cnx2 = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
                    cnx2.Open();
                    SqlCommand cmd2 = cnx2.CreateCommand();
                    cmd2.CommandText = "SELECT * FROM type_prets WHERE cle = " + cle_type_pret.ToString() + " ;";
                    cmd2.ExecuteNonQuery();
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    rdr2.Read();
                    Type_pret type = new Type_pret(cle_type_pret, (int)rdr2.GetValue(1), (int)rdr2.GetValue(3), rdr2.GetValue(2).ToString(), (int)rdr2.GetValue(4));
                    rdr2.Close();
                    //
                    DateTime date_pv = DateTime.Parse(rdr.GetValue(22).ToString());
                    DateTime date_demande = DateTime.Parse(rdr.GetValue(3).ToString());
                    DateTime date_premier_paiment = DateTime.Parse(rdr.GetValue(4).ToString());
                    pret_remboursable pret = new pret_remboursable((int)rdr.GetValue(0), emp, type, rdr.GetValue(8).ToString(), (int)rdr.GetValue(20), date_pv, (double)rdr.GetDouble(5), date_demande, rdr.GetValue(6).ToString(), date_premier_paiment, (int)rdr.GetValue(23), 0, mois, (int)rdr.GetValue(21));
                    DateTime date_fin_rembourssement = DateTime.Parse(rdr.GetValue(7).ToString());
                    Archive archive = new Archive((int)rdr.GetValue(0), pret, rdr.GetValue(19).ToString(), date_fin_rembourssement, (int)rdr.GetValue(23));
                    responsable.liste_archives.Add((int)rdr.GetValue(0), archive);
                }
                else if (var == -1.0)
                {
                    int id_employe = (int)rdr.GetValue(1);
                    Employé emp = responsable.liste_employes[id_employe];
                    int cle_type_pret = (int)rdr.GetValue(2);
                    Dictionary<int, double> mois = new Dictionary<int, double>();
                    for (int k = 0; k < 10; k++)
                        mois.Add(k, -1);
                    SqlConnection cnx2 = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
                    cnx2.Open();
                    SqlCommand cmd2 = cnx2.CreateCommand();
                    cmd2.CommandText = "SELECT * FROM type_prets WHERE cle = " + cle_type_pret.ToString() + " ;";
                    cmd2.ExecuteNonQuery();
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    rdr2.Read();
                    Type_pret type = new Type_pret(cle_type_pret, (int)rdr2.GetValue(1), (int)rdr2.GetValue(3), rdr2.GetValue(2).ToString(), (int)rdr2.GetValue(4));
                    rdr2.Close();
                    DateTime date_pv = DateTime.Parse(rdr.GetValue(22).ToString());
                    DateTime date_demande = DateTime.Parse(rdr.GetValue(3).ToString());
                    pret_non_remboursable pret = new pret_non_remboursable((int)rdr.GetValue(0), emp, type, rdr.GetValue(8).ToString(), (int)rdr.GetValue(20), date_pv, (double)rdr.GetValue(5), date_demande, rdr.GetValue(6).ToString());
                    Archive archive = new Archive((int)rdr.GetValue(0), pret, rdr.GetValue(19).ToString(), date_demande, (int)rdr.GetValue(23));
                    responsable.liste_archives.Add((int)rdr.GetValue(0), archive);
                }
                rdr.Close();
            }
            cnx.Close();
        }

        public static void initialiser_dictionnaire_types_prets()//Initialisation de la liste des types prets depuis la base de donnée
        {
            int longueur_table = 0;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM type_prets;";
            cmd.ExecuteNonQuery();
            SqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            longueur_table = (int)rdr1.GetValue(0);
            rdr1.Close();
            SqlCommand commande = cnx.CreateCommand();
            for (int i = 1; i <= longueur_table; i++)
            {
                commande.CommandText = "SELECT * FROM type_prets WHERE cle = " + i.ToString() + " ;";
                commande.ExecuteNonQuery();
                SqlDataReader rdr = commande.ExecuteReader();
                rdr.Read();
                Type_pret type = new Type_pret((int)rdr.GetValue(0), (int)rdr.GetValue(1), (int)rdr.GetValue(3), rdr.GetValue(2).ToString(), (int)rdr.GetValue(4));
                responsable.liste_types.Add((int)rdr.GetValue(0), type);
                rdr.Close();
            }
            cnx.Close();
        }

        public static void initialiser_dictionnaire_pret_remboursable()//Initialisation de la liste des prets remboursables depuis la base de donnée
        {
            int longueur_table = 0;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM prets_remboursable;";
            cmd.ExecuteNonQuery();
            SqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            longueur_table = (int)rdr1.GetValue(0);
            rdr1.Close();
            SqlCommand commande = cnx.CreateCommand();
            for (int i = 1; i <= longueur_table; i++)
            {
                commande.CommandText = "SELECT * FROM prets_remboursable WHERE cle = " + i.ToString() + " ;";
                commande.ExecuteNonQuery();
                SqlDataReader rdr = commande.ExecuteReader();
                rdr.Read();
                try
                {
                    Employé emp = responsable.liste_employes[(int)rdr.GetValue(1)];
                    Type_pret type = responsable.liste_types[(int)rdr.GetValue(2)];
                    DateTime date_pv = DateTime.Parse(rdr.GetValue(21).ToString());
                    DateTime date_demande = DateTime.Parse(rdr.GetValue(3).ToString());
                    DateTime date_prem_paiment = DateTime.Parse(rdr.GetValue(5).ToString());
                    Dictionary<int, double> mois = new Dictionary<int, double>();
                    mois.Add(0, (double)rdr.GetDouble(10));
                    mois.Add(1, (double)rdr.GetDouble(11));
                    mois.Add(2, (double)rdr.GetDouble(12));
                    mois.Add(3, (double)rdr.GetDouble(13));
                    mois.Add(4, (double)rdr.GetDouble(14));
                    mois.Add(5, (double)rdr.GetDouble(15));
                    mois.Add(6, (double)rdr.GetDouble(16));
                    mois.Add(7, (double)rdr.GetDouble(17));
                    mois.Add(8, (double)rdr.GetDouble(18));
                    mois.Add(9, (double)rdr.GetDouble(19));
                    pret_remboursable pret = new pret_remboursable((int)rdr.GetInt32(0), emp, type, rdr.GetValue(8).ToString(), (int)rdr.GetInt32(4), date_pv, (double)rdr.GetDouble(6), date_demande, rdr.GetValue(7).ToString(), date_prem_paiment, (int)rdr.GetInt32(22), (int)rdr.GetValue(9), mois, (int)rdr.GetInt32(20));
                    liste_pret_remboursable.Add(pret.Cle, pret);

                }
                catch
                {
                    longueur_table++;
                }

                rdr.Close();

            }
            cnx.Close();
        }

        public static void initialiser_dictionnaire_pret_non_remboursable()//Initialisation de la liste des prets non remboursables depuis la base de donnée
        {
            int longueur_table = 0;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM prets_non_remboursable;";
            cmd.ExecuteNonQuery();
            SqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            longueur_table = (int)rdr1.GetValue(0);
            rdr1.Close();
            SqlCommand commande = cnx.CreateCommand();
            for (int i = 1; i <= longueur_table; i++)
            {
                commande.CommandText = "SELECT * FROM prets_non_remboursable WHERE cle = " + i.ToString() + " ;";
                commande.ExecuteNonQuery();
                SqlDataReader rdr = commande.ExecuteReader();
                rdr.Read();
                try
                {
                    Employé emp = responsable.liste_employes[(int)rdr.GetValue(1)];
                    Type_pret type = responsable.liste_types[(int)rdr.GetValue(8)];
                    DateTime date_pv = DateTime.Parse(rdr.GetValue(7).ToString());
                    DateTime date_demande = DateTime.Parse(rdr.GetValue(2).ToString());
                    pret_non_remboursable pret = new pret_non_remboursable((int)rdr.GetInt32(0), emp, type, rdr.GetValue(6).ToString(), (int)rdr.GetInt32(3), date_pv, (double)rdr.GetDouble(4), date_demande, rdr.GetValue(5).ToString());
                    responsable.liste_pret_non_remboursables.Add((int)rdr.GetInt32(0), pret);
                    pret.Employé.ajouter_pret_non_remboursable_employe(pret);
                }
                catch
                {
                    longueur_table++;
                }
                rdr.Close();
            }
            cnx.Close();
        }

        //affichge-----------------------------------------------------------
        public static void affiche_liste_employes()
        {
            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {
                Console.Write("Clé = " + liste.Key + " ||  ");
                liste.Value.affiche_attribus();
            }
        }

        public static void affiche_liste_type_pret()
        {
            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("Clé = " + liste.Key + " || ");
                liste.Value.affiche_attribus();
            }
        }
        public static void affiche_liste_pret_remboursable()
        {
            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("Clé = " + liste.Key + " || ");
                liste.Value.affiche_attributs_complets();
            }
        }
        public static void affiche_liste_pret_non_remboursable()
        {
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("Clé = " + liste.Key + " || ");
                liste.Value.affiche_attribus();
            }
        }

        public static void affiche_liste_archive()
        {
            foreach (KeyValuePair<int, Archive> liste in responsable.liste_archives)
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("Clé = " + liste.Key + " || ");
                liste.Value.affiche_attribue();
            }
        }


        //ajout-----------------------------------------------------------
        public static void ajouter_employe(Employé b) //ajout d'employés
        {

            if (!(liste_employes.ContainsValue(b)))
            {
                liste_employes.Add(b.Cle, b);
            }
            else
            {
                Console.WriteLine("pas d'ajout d'employe");
            }
        }
        public static void ajouter_type_pret(Type_pret b)//ajout de types prets
        {
            int cpt = 0;
            foreach (KeyValuePair<int, Type_pret> kvp in liste_types)
            {
                if (b.Equals(kvp.Value))
                {
                    Console.WriteLine(b.Cle + " " + b.Description);
                    cpt++;
                }
            }
            if (cpt == 0)
            {
                liste_types.Add(b.Cle, b);
            }
            else
            {
                Console.WriteLine("pas d'ajout de type");
            }
        }
        public static void ajouter_pret_remboursable(pret_remboursable b)//ajout de prets remboursables
        {

            if (!(liste_pret_remboursable.ContainsValue(b)))
            {
                liste_pret_remboursable.Add(b.Cle, b);
            }
            else
            {
                Console.WriteLine("pas d'ajout de pret remboursable");
            }
        }
        public static void ajouter_pret_non_remboursable(pret_non_remboursable b)//ajout de prets non remboursables
        {

            if (!(liste_pret_non_remboursables.ContainsValue(b)))
            {
                liste_pret_non_remboursables.Add(b.Cle, b);
                responsable.tresor = responsable.tresor - b.Montant;
            }
            else
            {
                Console.WriteLine("pas d'ajout de pret non remboursable");
            }
        }

        public static void ajouter_archive(Archive b)//ajout d'archives
        {

            if (!(liste_archives.ContainsValue(b)))
            {
                liste_archives.Add(b.Cle, b);
            }
            else
            {
                Console.WriteLine("pas d'ajout d'archive");
            }
        }

        public static void ajout_service()//ajout de services
        {
            foreach (Employé emp in responsable.liste_employes.Values)
            {
                if (!services.Contains(emp.Service))
                {
                    services.Add(emp.Service);
                }

            }
        }

        //manipulations des prets-----------------------------------------------------------
        public static void suivi()//Suivi mensuel des prets
        {
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                element.Value.paiement();
            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
            {
                responsable.liste_pret_remboursable.Add(element.Key, element.Value);
            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                responsable.liste_pret_remboursable_provisoire.Remove(element.Key);
            }
        }

        public static void retardement_paiement(int cle)//Retardement du paiement pendant un mois
        {
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                if (cle == element.Key)
                {
                    element.Value.retardement();
                    element.Value.paiement();
                }
            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
            {
                responsable.liste_pret_remboursable.Add(element.Key, element.Value);

            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                responsable.liste_pret_remboursable_provisoire.Remove(element.Key);
            }
        }

        //clés a affecter-----------------------------------------------------------

        public static int cle_a_affecter_employe()//Clé unique pour les employés
        {
            int cpt = 1;
            foreach (KeyValuePair<int, Employé> kvp in liste_employes)
            {
                if (kvp.Key >= cpt)
                {
                    cpt = kvp.Key + 1;
                }
            }
            return cpt;
        }
        public static int cle_a_affecter_archive()//Clé unique pour les archives
        {
            int cpt = 1;
            foreach (KeyValuePair<int, Archive> kvp in liste_archives)
            {
                if (kvp.Key >= cpt)
                {
                    cpt = kvp.Key + 1;
                }
            }
            return cpt;
        }

        public static int cle_a_affecter_pret_remboursable()//Clé unique pour les prets remboursables
        {
            int cpt = 1;
            int cpt2 = 1;
            foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
            {
                if (kvp.Key >= cpt)
                {
                    cpt = kvp.Key + 1;
                }
            }
            foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable_provisoire)
            {
                if (kvp.Key >= cpt)
                {
                    cpt2 = kvp.Key + 1;
                }
            }
            if (cpt > cpt2)
            {
                return cpt;
            }
            return cpt2;
        }
        public static int cle_a_affecter_pret_non_remboursable()//Clé unique pour les prets non remboursables
        {
            int cpt = 1;
            foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
            {
                if (kvp.Key >= cpt)
                {
                    cpt = kvp.Key + 1;
                }
            }
            return cpt;
        }
        public static int cle_a_affecter_type_pret()//Clé unique pour les types prets
        {
            int cpt = 1;
            foreach (KeyValuePair<int, Type_pret> kvp in liste_types)
            {
                if (kvp.Key >= cpt)
                {
                    cpt = kvp.Key + 1;
                }
            }
            return cpt;
        }

        public static int type_a_affecter_type_pret()
        {
            int cpt = 1;
            foreach (KeyValuePair<int, Type_pret> kvp in liste_types)
            {
                if (kvp.Value.Type_de_pret >= cpt)
                {
                    cpt = kvp.Value.Type_de_pret + 1;
                }
            }
            return cpt;
        }

        //methodes de creation-----------------------------------------------------------
        public static void Creer_employe(string matricule, string nom, string prenom, string num_sec_social, DateTime date_naissance, string grade, DateTime date_prem, string etat, string ccp, string cle_ccp, string tel, string service, string mail, string etat_service)//Ajout  d'un nouvel employé
        {
            int cpt = 0;
            int cle = cle_a_affecter_employe();
            Employé p = new Employé(cle, matricule, nom, prenom, num_sec_social, date_naissance, grade, date_prem, etat, ccp, cle_ccp, tel, service, mail, etat_service);
            foreach (KeyValuePair<int, Employé> kvp in liste_employes)
            {

                if (p.Equals(kvp.Value))
                {
                    cpt++;
                }
            }
            if (cpt == 0)
            {
                liste_employes.Add(p.Cle, p);
            }
            else
            {
                Console.WriteLine("Veuillez insérer un employé valide , Le numéro de sécurité sociale inséré est déja existant ! ");
            }
        }



        public static void Creer_Type_pret(int typepret, int dispo, string descri, int remboursable)//Création d'un nouveau type pret
        {
            int cpt = 0;
            int cle = cle_a_affecter_type_pret();
            Type_pret p = new Type_pret(cle, typepret, dispo, descri, remboursable);
            foreach (KeyValuePair<int, Type_pret> kvp in liste_types)
            {
                if (p.Equals(kvp.Value))
                {
                    cpt++;

                }
            }
            if (cpt == 0)
            {
                liste_types.Add(p.Cle, p);
            }
            else
            {
                Console.WriteLine("Ce type existe déja , veuillez insérer un nouveau!");
            }
        }



        public static void Creer_pret_non_remboursable(int employé, int type, string motif, int num_pv, DateTime date_pv, double montant, DateTime date_demande, string montant_lettre)//Ajout  d'un nouveau pret non remboursable
        {
            int cle = cle_a_affecter_pret_non_remboursable();
            Employé emp = null;
            Type_pret typ = null;
            foreach (KeyValuePair<int, Employé> kvp in liste_employes)
            {
                if (employé == kvp.Key)
                {
                    emp = kvp.Value;
                }
            }
            foreach (KeyValuePair<int, Type_pret> kvp in liste_types)
            {
                if (type == kvp.Key)
                {
                    typ = kvp.Value;
                }
            }
            if ((typ == null) || (emp == null))
            {
                if (typ == null)
                {
                    Console.WriteLine("Veillez choisir un type existant ou créer un nouveau ! ");
                }
                if (emp == null)
                {
                    Console.WriteLine("Veillez choisir un employé existant ou créer un nouveau ! ");
                }

            }
            else
            {
                pret_non_remboursable p = new pret_non_remboursable(cle, emp, typ, motif, num_pv, date_pv, montant, date_demande, montant_lettre);
                responsable.tresor = tresor - p.Montant;
                liste_pret_non_remboursables.Add(p.Cle, p);
                p.Employé.ajouter_pret_non_remboursable_employe(p);
            }
        }




        public static void Creer_pret_remboursable(int employé, int type, string motif, int num_pv, DateTime date_pv, double montant, DateTime date_demande, string montant_lettre, DateTime date_premier_paiment, int durée)//Ajout  d'un nouveau pret remboursable
        {
            int cle = cle_a_affecter_pret_remboursable();
            Employé emp = null;
            Type_pret typ = null;
            foreach (KeyValuePair<int, Employé> kvp in liste_employes)
            {
                if (employé == kvp.Key)
                {
                    emp = kvp.Value;
                }
            }
            foreach (KeyValuePair<int, Type_pret> kvp in liste_types)
            {
                if (type == kvp.Key)
                {
                    typ = kvp.Value;
                }
            }
            if ((typ == null) || (emp == null))
            {
                if (typ == null)
                {
                    Console.WriteLine("Veillez choisir un type existant ou créer un nouveau ! ");
                }
                if (emp == null)
                {
                    Console.WriteLine("Veillez choisir un employé existant ou créer un nouveau ! ");
                }

            }
            else
            {
                Dictionary<int, double> dico1 = new Dictionary<int, double>();
                for (int i = 0; i < 10; i++)
                {
                    dico1.Add(i, -1);
                }

                pret_remboursable p = new pret_remboursable(cle, emp, typ, motif, num_pv, date_pv, montant, date_demande, montant_lettre, date_premier_paiment, durée, 1, dico1, -1);
                responsable.tresor = responsable.tresor - p.Montant;
                p.Employé.ajouter_pret_remboursable_employe(p);
                liste_pret_remboursable.Add(p.Cle, p);
            }
        }

        //methodes sauvgarde-----------------------------------------------------------

        //methode verification si un element de clé (clé) existe dans le dictionnaire employés : pour savoir si c'est un nouvel element ou un elemnt dej existant
        public static bool Clé_Existante_Employé(int clé)
        {
            bool resultat = false;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd_cle = cnx.CreateCommand();
            cmd_cle.CommandText = "SELECT cle FROM employes ;";
            SqlDataReader rdr = cmd_cle.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr.GetInt32(0) == clé)
                {
                    resultat = true;
                    break;
                }
            }
            rdr.Close();

            return resultat;
        }

        //methode verification si un element de clé (clé) existe dans le dictionnaire type_pret : pour savoir si c'est un nouvel element ou un elemnt dej existant
        public static bool Clé_Existante_Type_Pret(int clé)
        {
            bool resultat = false;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd_cle = cnx.CreateCommand();
            cmd_cle.CommandText = "SELECT cle FROM type_prets ;";
            SqlDataReader rdr = cmd_cle.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr.GetInt32(0) == clé)
                {
                    resultat = true;
                    break;
                }
            }
            rdr.Close();

            return resultat;
        }

        //methode verification si un element de clé (clé) existe dans le dictionnaire archive : pour savoir si c'est un nouvel element ou un elemnt dej existant
        public static bool Clé_Existante_archive(int clé)
        {
            bool resultat = false;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd_cle = cnx.CreateCommand();
            cmd_cle.CommandText = "SELECT cle FROM archive ;";
            SqlDataReader rdr = cmd_cle.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr.GetInt32(0) == clé)
                {
                    resultat = true;
                    break;
                }
            }
            rdr.Close();

            return resultat;
        }

        //methode verification si un element de clé (clé) existe dans le dictionnaire pret remboursable : pour savoir si c'est un nouvel element ou un elemnt dej existant
        public static bool Clé_Existante_pret_remboursable(int clé)
        {
            bool resultat = false;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd_cle = cnx.CreateCommand();
            cmd_cle.CommandText = "SELECT cle FROM prets_remboursable ;";
            SqlDataReader rdr = cmd_cle.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr.GetInt32(0) == clé)
                {
                    resultat = true;
                    break;
                }
            }
            rdr.Close();

            return resultat;
        }

        //methode verification si un element de clé (clé) existe dans le dictionnaire pret_non_remboursable : pour savoir si c'est un nouvel element ou un elemnt dej existant
        public static bool Clé_Existante_pret_non_remboursable(int clé)
        {
            bool resultat = false;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd_cle = cnx.CreateCommand();
            cmd_cle.CommandText = "SELECT cle FROM prets_non_remboursable ;";
            SqlDataReader rdr = cmd_cle.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr.GetInt32(0) == clé)
                {
                    resultat = true;
                    break;
                }
            }
            rdr.Close();

            return resultat;
        }

        // methode qui verifie si un element a été supprimé dans le dictionnaire pret_remboursable et si un element est supprimé elle retourne sa clé dans la bdd ,retourne 0 sinon (retourne le premier element si ona plusieurs a supprimmer)
        public static int verif_sup_remboursable()
        {
            int clé_a_sup = 0;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
            cnx.Open();
            SqlCommand cmd_cle = cnx.CreateCommand();
            cmd_cle.CommandText = "SELECT cle FROM prets_remboursable ;";
            SqlDataReader rdr = cmd_cle.ExecuteReader();

            while (rdr.Read())
            {
                if (!responsable.liste_pret_remboursable.ContainsKey(rdr.GetInt32(0)))
                {
                    clé_a_sup = rdr.GetInt32(0);
                    rdr.Close();
                    break;
                }
            }
            rdr.Close();

            return clé_a_sup;
        }

        // methode qui verifie si un element a été supprimé dans le dictionnaire pret_non_remboursable et si un element est supprimé elle retourne sa clé dans la bdd, retourne 0 sinon (retourne le premier element si ona plusieurs a supprimmer)
        public static int verif_sup_non_remboursable()
        {
            int clé_a_sup = 0;
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");
            cnx.Open();
            SqlCommand cmd_cle = cnx.CreateCommand();
            cmd_cle.CommandText = "SELECT cle FROM prets_non_remboursable ;";
            SqlDataReader rdr = cmd_cle.ExecuteReader();

            while (rdr.Read())
            {
                if (!(responsable.liste_pret_non_remboursables.ContainsKey(rdr.GetInt32(0))))
                {

                    clé_a_sup = rdr.GetInt32(0);
                    Console.WriteLine(clé_a_sup);
                    rdr.Close();
                    break;
                }
            }
            rdr.Close();

            return clé_a_sup;
        }


        //methode sauvgarde de changements dans le dictionnaire employes (ajout et modification)
        public static void sauvgarde_Employe()
        {
            foreach (Employé emp in liste_employes.Values)
            {
                foreach (PropertyInfo prop in emp.GetType().GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (type == typeof(string))
                    {
                        if (((string)prop.GetValue(emp)).Contains("'"))
                        {
                            prop.SetValue(emp, ((string)prop.GetValue(emp)).Replace("'", "''"));
                        }
                    }
                }
            }

            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();


            foreach (KeyValuePair<int, Employé> liste in responsable.liste_employes)
            {

                if (!Clé_Existante_Employé(liste.Key))//ajout
                {
                    cmd.CommandText = "SET IDENTITY_INSERT employes ON;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT employes (cle,nom, prenom, num_securite_sociale, date_naissance, grade, date_prem, etat_sociale, ccp, cle_ccp, tel, matricule,service_travail,email,etat_fonction) VALUES(" + liste.Key + ",N'" + liste.Value.Nom + "',N'" + liste.Value.Prenom + "','" + liste.Value.sec_soc + "','" + liste.Value.Date_naissance.ToShortDateString() + "',N'" + liste.Value.Grade + "','" + liste.Value.Date_prem.ToShortDateString() + "',N'" + liste.Value.etats + "','" + liste.Value.compte_ccp + "','" + liste.Value.Cle_ccp + "','" + liste.Value.tel + "','" + liste.Value.Matricule + "',N'" + liste.Value.Service + "','" + liste.Value.Email + "',N'" + liste.Value.Etat_service + "');";
                    cmd.ExecuteNonQuery();
                }
                else//modification
                {
                    foreach (Modification element in responsable.pile_modifications)
                    {
                        if (element.Dic_modifié == 1)
                        {
                            if (element.Clé_element_modifié == liste.Key)
                            {
                                cmd.CommandText = " UPDATE employes SET email='" + liste.Value.Email + "' WHERE cle=" + element.Clé_element_modifié + ";";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        //methode sauvgarde de changements dans le dictionnaire type_pret (ajout et modification)
        public static void sauvgarde_Type_pret()
        {
            foreach (Type_pret typ in liste_types.Values)
            {
                foreach (PropertyInfo prop in typ.GetType().GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (type == typeof(string))
                    {
                        if (((string)prop.GetValue(typ)).Contains("'"))
                        {
                            prop.SetValue(typ, ((string)prop.GetValue(typ)).Replace("'", "''"));
                        }
                    }
                }
            }

            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();


            foreach (KeyValuePair<int, Type_pret> liste in responsable.liste_types)
            {

                if (!Clé_Existante_Type_Pret(liste.Key))//ajout
                {
                    cmd.CommandText = "SET IDENTITY_INSERT type_prets ON;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT type_prets (cle,type_du_pret, description_pret, disponibilite, remboursable) VALUES(" + liste.Key + "," + liste.Value.Type_de_pret + ",N'" + liste.Value.Description + "'," + liste.Value.Disponibilité + "," + liste.Value.Remboursable + "); ";
                    cmd.ExecuteNonQuery();
                }
                else//modification
                {
                    foreach (Modification element in responsable.pile_modifications)
                    {
                        if (element.Dic_modifié == 2)
                        {
                            if (element.Clé_element_modifié == liste.Key)
                            {
                                cmd.CommandText = "UPDATE type_prets SET disponibilite =" + liste.Value.Disponibilité + " WHERE cle=" + element.Clé_element_modifié + ";";
                                cmd.ExecuteNonQuery();

                            }
                        }
                    }
                }
            }
        }

        //methode sauvgarde de changements dans le dictionnaire archive (ajout seulement)
        public static void sauvgarde_archive()
        {
            foreach (Archive archv in liste_archives.Values)
            {
                foreach (PropertyInfo prop in archv.GetType().GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (type == typeof(string))
                    {
                        if (((string)prop.GetValue(archv)).Contains("'"))
                        {                            
                            prop.SetValue(archv, ((string)prop.GetValue(archv)).Replace("'", "''"));
                        }
                    }
                }
            }

            foreach (Archive archv in liste_archives.Values)
            {
                foreach (PropertyInfo prop in archv.Pret.GetType().GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (type == typeof(string))
                    {
                        if (((string)prop.GetValue(archv.Pret)).Contains("'"))
                        {
                            prop.SetValue(archv.Pret, ((string)prop.GetValue(archv.Pret)).Replace("'", "''"));
                        }
                    }
                }
            }

            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();


            foreach (KeyValuePair<int, Archive> liste in responsable.liste_archives)
            {
                if (!Clé_Existante_archive(liste.Key))
                {
                    if (liste.Value.Pret.Type_Pret.Remboursable == 1)//ajout de pret remboursable
                    {
                        cmd.CommandText = "SET IDENTITY_INSERT archive ON;";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT archive(cle,idetifiant_employe, cle_type_pret, date_demande, date_premier_paiment, montant_pret, montant_pret_lettre, date_fin_remboursement, motif, mois_1, mois_2, mois_3, mois_4, mois_5, mois_6, mois_7, mois_8, mois_9, mois_10, observation, num_pv, debordement,date_pv,long_remboursement) VALUES(" + liste.Key + "," + liste.Value.Pret.Employé.Cle + "," + liste.Value.Pret.Type_Pret.Cle + ",'" + liste.Value.Pret.Date_demande.ToShortDateString() + "','" + ((pret_remboursable)liste.Value.Pret).Date_premier_paiment.ToShortDateString() + "'," + liste.Value.Pret.Montant + ",'" + liste.Value.Pret.Montant_lettre + "','" + liste.Value.Date_fin_remboursement.ToShortDateString() + "',N'" + liste.Value.Pret.Motif + "'," + ((pret_remboursable)liste.Value.Pret).Etat[0] + "," + ((pret_remboursable)liste.Value.Pret).Etat[1] + "," + ((pret_remboursable)liste.Value.Pret).Etat[2] + "," + ((pret_remboursable)liste.Value.Pret).Etat[3] + "," + ((pret_remboursable)liste.Value.Pret).Etat[4] + "," + ((pret_remboursable)liste.Value.Pret).Etat[5] + "," + ((pret_remboursable)liste.Value.Pret).Etat[6] + "," + ((pret_remboursable)liste.Value.Pret).Etat[7] + "," + ((pret_remboursable)liste.Value.Pret).Etat[8] + "," + ((pret_remboursable)liste.Value.Pret).Etat[9] + ",N'" + liste.Value.Observations + "'," + liste.Value.Pret.Num_pv + "," + ((pret_remboursable)liste.Value.Pret).Debordement + ",'" + liste.Value.Pret.Date_pv.ToShortDateString() + "'," + liste.Value.Durée + ");";
                        cmd.ExecuteNonQuery();
                    }
                    else//ajout de pret non remboursable
                    {
                        cmd.CommandText = "SET IDENTITY_INSERT archive ON;";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT archive(cle,idetifiant_employe, cle_type_pret, date_demande, date_premier_paiment, montant_pret, montant_pret_lettre, date_fin_remboursement, motif, mois_1, mois_2, mois_3, mois_4, mois_5, mois_6, mois_7, mois_8, mois_9, mois_10, observation, num_pv, debordement,date_pv,long_remboursement) VALUES(" + liste.Key + "," + liste.Value.Pret.Employé.Cle + "," + liste.Value.Pret.Type_Pret.Cle + ",'" + liste.Value.Pret.Date_demande.ToShortDateString() + "',NULL," + liste.Value.Pret.Montant + ",'" + liste.Value.Pret.Montant_lettre + "',NULL,N'" + liste.Value.Pret.Motif + "',-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,N'" + liste.Value.Observations + "'," + liste.Value.Pret.Num_pv + ",NULL,'" + liste.Value.Pret.Date_pv.ToShortDateString() + "',-1);";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        //methode sauvgarde de changements dans le dictionnaire pret remboursable (ajout, modification, suppression)
        public static void sauvgarde_pret_remboursable()
        {
            foreach (pret_remboursable prt in liste_pret_remboursable.Values)
            {
                foreach (PropertyInfo prop in prt.GetType().GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (type == typeof(string))
                    {
                        if (((string)prop.GetValue(prt)).Contains("'"))
                        {
                            prop.SetValue(prt, ((string)prop.GetValue(prt)).Replace("'", "''"));
                        }
                    }
                }
            }

            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            int sup = 0;

            foreach (KeyValuePair<int, pret_remboursable> liste in responsable.liste_pret_remboursable)
            {

                if (!Clé_Existante_pret_remboursable(liste.Key))//ajout
                {
                    cmd.CommandText = "SET IDENTITY_INSERT prets_remboursable ON;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT prets_remboursable(cle,idetifiant_employe, type_pret, date_demande, num_pv, date_premier_paiment, montant_pret, montant_pret_lettre, motif, en_cours, mois_1, mois_2, mois_3, mois_4, mois_5, mois_6, mois_7, mois_8, mois_9, mois_10, debordement,date_pv,long_remboursement) VALUES(" + liste.Key + "," + liste.Value.Employé.Cle + "," + liste.Value.Type_Pret.Cle + ",'" + liste.Value.Date_demande.ToShortDateString() + "'," + liste.Value.Num_pv + ",'" + liste.Value.Date_premier_paiment.ToShortDateString() + "'," + liste.Value.Montant + ",'" + liste.Value.Montant_lettre + "',N'" + liste.Value.Motif + "'," + liste.Value.En_cours + "," + liste.Value.Etat[0] + "," + liste.Value.Etat[1] + "," + liste.Value.Etat[2] + "," + liste.Value.Etat[3] + "," + liste.Value.Etat[4] + "," + liste.Value.Etat[5] + "," + liste.Value.Etat[6] + "," + liste.Value.Etat[7] + "," + liste.Value.Etat[8] + "," + liste.Value.Etat[9] + "," + liste.Value.Debordement + ",'" + liste.Value.Date_pv.ToShortDateString() + "'," + liste.Value.Durée + ");";
                    cmd.ExecuteNonQuery();
                }
                else//modification
                {
                    foreach (Modification element in responsable.pile_modifications)
                    {
                        if (element.Dic_modifié == 4)
                        {
                            if (element.Clé_element_modifié == liste.Key)
                            {
                                cmd.CommandText = " UPDATE prets_remboursable SET idetifiant_employe=" + liste.Value.Employé.Cle + " ,type_pret=" + liste.Value.Type_Pret.Cle + " ,date_demande='" + liste.Value.Date_demande.ToShortDateString() + "' ,num_pv=" + liste.Value.Num_pv + " ,date_premier_paiment='" + liste.Value.Date_premier_paiment.ToShortDateString() + "' ,montant_pret=" + liste.Value.Montant + " ,montant_pret_lettre='" + liste.Value.Montant_lettre + "' ,motif=N'" + liste.Value.Motif + "' ,en_cours=" + liste.Value.En_cours + " ,mois_1=" + liste.Value.Etat[0] + " ,mois_2=" + liste.Value.Etat[1] + " ,mois_3=" + liste.Value.Etat[2] + " ,mois_4=" + liste.Value.Etat[3] + " ,mois_5=" + liste.Value.Etat[4] + " ,mois_6=" + liste.Value.Etat[5] + " ,mois_7=" + liste.Value.Etat[6] + " ,mois_8=" + liste.Value.Etat[7] + " ,mois_9=" + liste.Value.Etat[8] + " ,mois_10=" + liste.Value.Etat[9] + " ,debordement=" + liste.Value.Debordement + " ,date_pv='" + liste.Value.Date_pv.ToShortDateString() + "' ,long_remboursement=" + liste.Value.Durée + " WHERE cle=" + element.Clé_element_modifié + ";";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            sup = verif_sup_remboursable();//supression
            while (sup != 0)//on supprimme un seul element par itteration
            {
                cmd.CommandText = "DELETE FROM prets_remboursable WHERE cle=" + sup + ";";
                cmd.ExecuteNonQuery();
                sup = verif_sup_remboursable();
            }
        }

        //methode sauvgarde de changements dans le dictionnaire pret non remboursable (ajout, modification, suppression)
        public static void sauvgarde_pret_non_remboursable()
        {
            foreach (pret_non_remboursable prtn in liste_pret_non_remboursables.Values)
            {
                foreach (PropertyInfo prop in prtn.GetType().GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (type == typeof(string))
                    {
                        if (((string)prop.GetValue(prtn)).Contains("'"))
                        {
                            prop.SetValue(prtn, ((string)prop.GetValue(prtn)).Replace("'", "''"));
                        }
                    }
                }
            }
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            int sup = 0;

            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
            {

                if (!Clé_Existante_pret_non_remboursable(liste.Key))//ajout
                {
                    cmd.CommandText = "SET IDENTITY_INSERT prets_non_remboursable ON;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT prets_non_remboursable(cle,idetifiant_employe, type_pret, date_demande, num_pv, montant_don, montant_don_lettre, motif, date_pv) VALUES(" + liste.Key + "," + liste.Value.Employé.Cle + "," + liste.Value.Type_Pret.Cle + ",'" + liste.Value.Date_demande.ToShortDateString() + "'," + liste.Value.Num_pv + "," + liste.Value.Montant + ",'" + liste.Value.Montant_lettre + "',N'" + liste.Value.Motif + "','" + liste.Value.Date_pv.ToShortDateString() + "');";
                    cmd.ExecuteNonQuery();
                }
                else//modification
                {
                    foreach (Modification element in responsable.pile_modifications)
                    {
                        if (element.Dic_modifié == 5)
                        {
                            if (element.Clé_element_modifié == liste.Key)
                            {
                                cmd.CommandText = " UPDATE prets_non_remboursable SET idetifiant_employe=" + liste.Value.Employé.Cle + " ,type_pret=" + liste.Value.Type_Pret.Cle + " ,date_demande='" + liste.Value.Date_demande.ToShortDateString() + "' ,num_pv=" + liste.Value.Num_pv + " ,montant_don=" + liste.Value.Montant + " ,montant_don_lettre='" + liste.Value.Montant_lettre + "' ,motif=N'" + liste.Value.Motif + "' ,date_pv='" + liste.Value.Date_pv.ToShortDateString() + "' WHERE cle=" + element.Clé_element_modifié + ";";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            sup = verif_sup_non_remboursable();//supression
            while (sup != 0)//on supprimme un seul element par itteration
            {
                cmd.CommandText = "DELETE FROM prets_non_remboursable WHERE cle=" + sup + ";";
                cmd.ExecuteNonQuery();
                sup = verif_sup_non_remboursable();
            }
        }        

        //methodes archivage-----------------------------------------------------------
        public static void archiver_pret_non_remboursable()//archivage auto des prets non remboursables apres une durée choisie par l'utilisateur
        {
            foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
            {
                kvp.Value.archiver();
            }
            foreach (KeyValuePair<int, Archive> kvp in responsable.liste_archives_provisoire)
            {
                responsable.liste_pret_non_remboursables.Remove(kvp.Value.Pret.Cle);
            }
            responsable.liste_archives_provisoire.Clear();
        }
        public static void archiver_manuel_pret_non_remboursable(int cle)//archivage manuel des prets non remboursables
        {
            foreach (KeyValuePair<int, pret_non_remboursable> element in responsable.liste_pret_non_remboursables)
            {
                if (cle == element.Key)
                {
                    element.Value.archiver_manuel();
                }
            }
            foreach (KeyValuePair<int, Archive> kvp in responsable.liste_archives_provisoire)
            {
                responsable.liste_pret_non_remboursables.Remove(kvp.Value.Pret.Cle);
            }
            responsable.liste_archives_provisoire.Clear();
        }

        public static void archiver_pret_remboursable()//archivage auto des prets non remboursables apres une durée choisie par l'utilisateur
        {
            foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
            {
                kvp.Value.archiver();
            }
            foreach (KeyValuePair<int, Archive> kvp in responsable.liste_archives_provisoire)
            {
                responsable.liste_pret_remboursable.Remove(kvp.Value.Pret.Cle);
            }
            responsable.liste_archives_provisoire.Clear();
        }
        public static void archiver_manuel_pret_remboursable(int cle) //archivage automatique des prets remboursables apres une durée choisie par l'utilisateur

        {
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {

                if (cle == element.Key)
                {

                    element.Value.children();

                }
            }
            foreach (KeyValuePair<int, Archive> kvp in responsable.liste_archives_provisoire)
            {
                responsable.liste_pret_remboursable.Remove(kvp.Value.Pret.Cle);
            }
            responsable.liste_archives_provisoire.Clear();
        }

        public static void paiement_standard(int cle)//paiement mensuel d'un pret remboursable(paiement habituel)
        {
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                if (cle == element.Key)
                {
                    element.Value.paiement();
                }
            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
            {
                responsable.liste_pret_remboursable.Add(element.Key, element.Value);
            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                responsable.liste_pret_remboursable_provisoire.Remove(element.Key);
            }
        }

        public static void paiement_spécial(int cle, double cout)//paiement libre d'un pret remboursable selon les capacités de l'employé
        {

            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                if (cle == element.Key)
                {
                    element.Value.paiement_spécial(cout);
                }
            }

            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
            {
                responsable.liste_pret_remboursable.Add(element.Key, element.Value);

            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                responsable.liste_pret_remboursable_provisoire.Remove(element.Key);
            }
        }

        public static void paiement_plusieurs_mois(int cle, int nb_mois)//paiement de plusieurs mensualités à la foi d'un pret remboursable
        {

            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                if (cle == element.Key)
                {
                    element.Value.paiement_plusieurs_mois(nb_mois);
                }
            }

            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
            {
                responsable.liste_pret_remboursable.Add(element.Key, element.Value);

            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                responsable.liste_pret_remboursable_provisoire.Remove(element.Key);
            }
        }

        public static void effacement_dettes(int cle)//Effacement de dettes d'un pret remboursable pour des raisons bien définies
        {
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {

                if (cle == element.Key)
                {

                    element.Value.children_effacement_dettes();

                }
            }
            foreach (KeyValuePair<int, Archive> kvp in responsable.liste_archives_provisoire)
            {
                responsable.liste_pret_remboursable.Remove(kvp.Value.Pret.Cle);
            }
            responsable.liste_archives_provisoire.Clear();

        }

        public static void paiement_anticipé(int cle)//paiement de la totalité de la somme restante à rembourser
        {

            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                if (cle == element.Key)
                {
                    element.Value.paiement_anticipé();
                }
            }

            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
            {
                responsable.liste_pret_remboursable.Add(element.Key, element.Value);

            }
            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                responsable.liste_pret_remboursable_provisoire.Remove(element.Key);
            }
        }

        public static void initialisation_archive_auto()//Archivage automatique de touts les prets en tenant compte de la durée saisie par l'utilisateur
        {
            if (Window2.mode_archivage)
            {
                archiver_pret_remboursable();
                archiver_pret_non_remboursable();
            }
        }        
        //----------------------------------------------------------------------------------------------------------------------
        //Recherche par critères 
        public static void remplissage_liste_filtres()
        {
            responsable.liste_filtres.Clear();
            foreach (KeyValuePair<int, Archive> kvp in liste_archives)
            {
                responsable.liste_filtres.Add(kvp.Key, kvp.Value);
            }
        }

        public static void filtrer_par_remboursable_ou_non(bool remboursable, int choix)
        {

            if (remboursable == true)
            {
                if (choix == 1)
                {
                    foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                    {
                        if (kvp.Value.Pret.Type_Pret.Remboursable == 0)
                        {
                            liste_filtres.Remove(kvp.Key);
                        }
                    }
                }
                else
                {
                    if (choix == 2)
                    {
                        foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                        {
                            if (kvp.Value.Pret.Type_Pret.Remboursable == 1)
                            {
                                liste_filtres.Remove(kvp.Key);
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Veillez entrer un choix valide!");
                    }

                }
            }
        }


        public static void filtrer_par_date_demande_inf(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (DateTime.Compare(kvp.Value.Pret.Date_demande, d_inf) < 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }


        public static void filtrer_par_date_demande_max(bool date, DateTime d_max)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (DateTime.Compare(kvp.Value.Pret.Date_demande, d_max) > 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }



        public static void filtrer_par_date_pv_inf(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (DateTime.Compare(kvp.Value.Pret.Date_pv, d_inf) < 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }


        public static void filtrer_par_date_pv_max(bool date, DateTime d_max)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (DateTime.Compare(kvp.Value.Pret.Date_pv, d_max) > 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_durée_min(bool durée, int durée_min)
        {

            if (durée == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (kvp.Value.Durée < durée_min)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }


        public static void filtrer_par_durée_max(bool durée, int durée_min)
        {

            if (durée == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (kvp.Value.Durée > durée_min)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }



        public static void filtrer_par_somme_min(bool somme, double somme_min)
        {

            if (somme == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (kvp.Value.Pret.Montant < somme_min)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }



        public static void filtrer_par_somme_max(bool somme, double somme_max)
        {

            if (somme == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (kvp.Value.Pret.Montant > somme_max)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_employés(bool employé)
        {

            int cpt = 0;
            if (employé == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {
                    foreach (int entier in clés_employés)
                    {
                        if (entier == kvp.Value.Pret.Employé.Cle)
                        {
                            cpt++;
                        }
                    }

                    if (cpt == 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                    cpt = 0;
                }
            }
            //responsable.clés_employés.Clear();
        }


        public static void filtrer_par_types(bool type)
        {

            int cpt = 0;
            if (type == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {
                    foreach (int entier in clés_types)
                    {
                        if (entier == kvp.Value.Pret.Type_Pret.Cle)
                        {
                            cpt++;
                        }
                    }

                    if (cpt == 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                    cpt = 0;
                }
            }
            //responsable.clés_types.Clear();
        }

        public static void filtrer_par_service(bool service)
        {

            int cpt = 0;
            if (service == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {
                    foreach (String s in choix_service)
                    {
                        if (s.Equals(kvp.Value.Pret.Employé.Service))
                        {
                            cpt++;

                        }
                    }

                    if (cpt == 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                    cpt = 0;
                }
            }
            responsable.choix_service.Clear();
        }

        public static void recherche_par_criteres_deux(bool remboursable, int choix, bool date1, DateTime d_inf, bool date2, DateTime d_max, bool date3, DateTime pv_min, bool date4, DateTime pv_max, bool durée1, int durée_min, bool durée2, int durée_max, bool somme1, double somme_min, bool somme2, double somme_max, bool employé, bool type, bool service)
        {
            responsable.remplissage_liste_filtres();
            filtrer_par_remboursable_ou_non(remboursable, choix);
            filtrer_par_date_demande_inf(date1, d_inf);
            filtrer_par_date_demande_max(date2, d_max);
            filtrer_par_date_pv_inf(date3, pv_min);
            filtrer_par_date_pv_max(date4, pv_max);
            filtrer_par_durée_min(durée1, durée_min);
            filtrer_par_durée_max(durée2, durée_max);
            filtrer_par_employés(employé);
            filtrer_par_types(type);
            filtrer_par_service(service);
            filtrer_par_somme_min(somme1, somme_min);
            filtrer_par_somme_max(somme2, somme_max);
        }

        //methodes statistiques-----------------------------------------------------------

        public static void stat_pret_durrée(int année)
        {
            liste_stat_1.Clear();

            for (int i = 1; i < 13; i++)
            {
                liste_stat_1.Add(i, 0);
            }

            if (DateTime.Now.Year >= année)
            {

                foreach (KeyValuePair<int, pret_non_remboursable> liste in liste_pret_non_remboursables)
                {
                    if (liste.Value.Date_demande.Year == année)
                    {
                        foreach (KeyValuePair<int, int> mois in liste_stat_1)
                        {
                            if (mois.Key == liste.Value.Date_demande.Month.GetHashCode())
                            {
                                liste_stat_1[mois.Key]++;
                                break;
                            }
                        }
                    }

                }


                foreach (KeyValuePair<int, pret_remboursable> liste in liste_pret_remboursable)
                {
                    if (liste.Value.Date_demande.Year == année)
                    {
                        foreach (KeyValuePair<int, int> mois in liste_stat_1)
                        {
                            if (mois.Key == liste.Value.Date_demande.Month.GetHashCode())
                            {
                                liste_stat_1[mois.Key]++;
                                break;
                            }
                        }
                    }
                }


                foreach (KeyValuePair<int, Archive> liste in liste_archives)
                {
                    if (liste.Value.Pret.Date_demande.Year == année)
                    {
                        foreach (KeyValuePair<int, int> mois in liste_stat_1)
                        {
                            if (mois.Key == liste.Value.Pret.Date_demande.Month.GetHashCode())
                            {
                                liste_stat_1[mois.Key]++;
                                break;
                            }
                        }
                    }
                }

            }
        }

        public static void stat_type_pret(double montant, int annee)
        {
            list_sup.Clear();
            list_inf.Clear();

            foreach (KeyValuePair<int, Type_pret> liste in liste_types)
            {
                //Console.WriteLine(liste.Value.Type_de_pret);
                if (!list_sup.ContainsKey(liste.Value.Type_de_pret))
                {
                    list_sup.Add(liste.Value.Type_de_pret, 0);
                    list_inf.Add(liste.Value.Type_de_pret, 0);
                }
            }
            foreach (KeyValuePair<int, pret_remboursable> liste in liste_pret_remboursable)
            {
                if (liste.Value.Date_demande.Year == annee)
                {
                    if (liste.Value.Montant <= montant) list_inf[liste.Value.Type_Pret.Type_de_pret] += 1;//si 3eme champ =0 alors le montant du pret est inferieur ou egala celui defini  par le responsable
                    else list_sup[liste.Value.Type_Pret.Type_de_pret] += 1;
                }
            }

            foreach (KeyValuePair<int, pret_non_remboursable> liste in liste_pret_non_remboursables)
            {
                if (liste.Value.Date_demande.Year == annee)
                {
                    if (liste.Value.Montant <= montant) list_inf[liste.Value.Type_Pret.Type_de_pret] += 1;//si 3eme champ =0 alors le montant du pret est inferieur ou egala celui defini  par le responsable
                    else list_sup[liste.Value.Type_Pret.Type_de_pret] += 1;
                }
            }
            foreach (KeyValuePair<int, Archive> liste in liste_archives)
            {
                if (liste.Value.Pret.Date_demande.Year == annee)
                {
                    if (liste.Value.Pret.Montant <= montant) list_inf[liste.Value.Pret.Type_Pret.Type_de_pret] += 1;//si 3eme champ =0 alors le montant du pret est inferieur ou egala celui defini  par le responsable
                    else list_sup[liste.Value.Pret.Type_Pret.Type_de_pret] += 1;
                }
            }

        }

        public static void nouveau_tresor(double montant_tresor)
        {
            sauvgarder_montant_tresor();

            try
            {
                string path = @".\\Montant_tresor.txt";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.Write("\n¦" + montant_tresor.ToString() + "|" + DateTime.Now.ToShortDateString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pu être lu.");
                Console.WriteLine(e.Message);
            }
        }

        public static void charger_montant_tresor()
        {

            try
            {
                string path = @".\\Montant_tresor.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    sr.BaseStream.Position = 0;
                    string line = sr.ReadLine();
                    tresor = double.Parse(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pu être lu.");
                Console.WriteLine(e.Message);
            }
        }

        public static void sauvgarder_montant_tresor()
        {
            string line;
            string[] year_split = new string[100];
            bool vide = true;

            try
            {
                string path = @".\\Montant_tresor.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    sr.BaseStream.Seek(0, SeekOrigin.End);
                    if (sr.BaseStream.Position != 0)
                    {
                        vide = false;
                        sr.BaseStream.Position = 0;
                        line = sr.ReadToEnd();
                        year_split = line.Split('¦');
                    }
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    if (vide)
                        sw.Write(tresor.ToString());
                    else
                    {
                        year_split[0] = tresor.ToString();
                        line = string.Join("\n¦", year_split);
                        sw.Write(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pu être lu.");
                Console.WriteLine(e.Message);
            }
        }

        public static void ecriture_modif_tresor()
        {
            DateTime date_affectation_tresor = new DateTime();
            int longeur = 0;
            string[] line_split;
            string[] year_split;

            try
            {
                string path = @".\\Montant_tresor.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = sr.ReadToEnd();
                    year_split = line.Split('¦');
                    line_split = year_split[year_split.Length - 1].Split('|');
                    date_affectation_tresor = DateTime.Parse(line_split[1]);
                    longeur = line_split.Length;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pu être lu.");
                Console.WriteLine(e.Message);
            }

            DateTime date_dernier_affectation;
            if (longeur > 2)
                date_dernier_affectation = date_affectation_tresor.AddDays(7 * (longeur - 1));
            else
                date_dernier_affectation = date_affectation_tresor.AddDays(7);

            while (DateTime.Now > date_dernier_affectation)
            {
                try
                {
                    string path = @".\\Montant_tresor.txt";
                    using (StreamWriter sw = new StreamWriter(path, true))
                    {
                        sw.Write("|" + tresor.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Le fichier n'a pas pu être lu.");
                    Console.WriteLine(e.Message);
                }

                date_dernier_affectation = date_dernier_affectation.AddDays(7);
            }
        }


        public static void stat_tresor(int année)
        {
            liste_tresor.Clear();

            for (int i = 1; i < 53; i++)
            {
                liste_tresor.Add(i, 0);
            }

            string[] year_split;
            string[] line_split = new string[54];
            bool année_exist = false;

            try
            {
                string path = @".\\Montant_tresor.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = sr.ReadToEnd();
                    year_split = line.Split('¦');
                    for (int i = 1; i < year_split.Length; i++)
                    {
                        line_split = year_split[i].Split('|');

                        if (DateTime.Parse(line_split[1]).Year == année)
                        {
                            année_exist = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pu être lu.");
                Console.WriteLine(e.Message);
            }

            if (année_exist)
            {
                foreach (KeyValuePair<int, double> liste in responsable.liste_tresor.ToList())
                {
                    if (liste.Key < line_split.Length)
                    {
                        if (liste.Key == 1)
                            liste_tresor[liste.Key] = double.Parse(line_split[0]);
                        else
                            liste_tresor[liste.Key] = double.Parse(line_split[liste.Key]);
                    }
                }
            }
        }


        //-----------------------------------------------------------------------------------------------
        //Recherche par critères prets non remboursables et remboursables
        public static void remplissage_liste_filtres_rem()
        {
            responsable.liste_filtres_rem.Clear();
            foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
            {
                responsable.liste_filtres_rem.Add(kvp.Key, kvp.Value);
            }
        }

        public static void remplissage_liste_filtres_non_rem()
        {
            responsable.liste_filtres_non_rem.Clear();
            foreach (KeyValuePair<int, pret_non_remboursable> liste in responsable.liste_pret_non_remboursables)
            {
                responsable.liste_filtres_non_rem.Add(liste.Key, liste.Value);
            }
        }

        public static void filtrer_par_date_demande_inf_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (DateTime.Compare(kvp.Value.Date_demande, d_inf) < 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_date_demande_inf_non_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (DateTime.Compare(kvp.Value.Date_demande, d_inf) < 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_date_demande_max_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (DateTime.Compare(kvp.Value.Date_demande, d_inf) > 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_date_demande_max_non_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (DateTime.Compare(kvp.Value.Date_demande, d_inf) > 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_date_pv_inf_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (DateTime.Compare(kvp.Value.Date_pv, d_inf) < 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_date_pv_inf_non_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (DateTime.Compare(kvp.Value.Date_pv, d_inf) < 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_date_pv_max_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (DateTime.Compare(kvp.Value.Date_pv, d_inf) > 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_date_pv_max_non_rem(bool date, DateTime d_inf)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (DateTime.Compare(kvp.Value.Date_pv, d_inf) > 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_durée_min_rem(bool durée, int durée_min)
        {

            if (durée == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (kvp.Value.Durée < durée_min)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_durée_max_rem(bool durée, int durée_min)
        {

            if (durée == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (kvp.Value.Durée > durée_min)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }


        public static void filtrer_par_somme_min_rem(bool somme, double somme_min)
        {

            if (somme == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (kvp.Value.Montant < somme_min)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_somme_min_non_rem(bool somme, double somme_min)
        {

            if (somme == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (kvp.Value.Montant < somme_min)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_somme_max_rem(bool somme, double somme_min)
        {

            if (somme == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (kvp.Value.Montant > somme_min)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        public static void filtrer_par_somme_max_non_rem(bool somme, double somme_min)
        {

            if (somme == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (kvp.Value.Montant > somme_min)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_employés_rem(bool employé)
        {

            int cpt = 0;
            if (employé == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {
                    foreach (int entier in clés_employés)
                    {
                        if (entier == kvp.Value.Employé.Cle)
                        {
                            cpt++;
                        }
                    }

                    if (cpt == 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                    cpt = 0;
                }
            }
            //responsable.clés_employés.Clear();
        }

        public static void filtrer_par_employés_non_rem(bool employé)
        {

            int cpt = 0;
            if (employé == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {
                    foreach (int entier in clés_employés)
                    {
                        if (entier == kvp.Value.Employé.Cle)
                        {
                            cpt++;
                        }
                    }

                    if (cpt == 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                    cpt = 0;
                }
            }
            //responsable.clés_employés.Clear();
        }
        public static void filtrer_par_types_rem(bool type)
        {

            int cpt = 0;
            if (type == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {
                    foreach (int entier in clés_types)
                    {
                        if (entier == kvp.Value.Type_Pret.Cle)
                        {
                            cpt++;
                        }
                    }

                    if (cpt == 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                    cpt = 0;
                }
            }
            //responsable.clés_types.Clear();
        }

        public static void filtrer_par_types_non_rem(bool type)
        {

            int cpt = 0;
            if (type == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {
                    foreach (int entier in clés_types)
                    {
                        if (entier == kvp.Value.Type_Pret.Cle)
                        {
                            cpt++;
                        }
                    }

                    if (cpt == 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                    cpt = 0;
                }
            }
            //responsable.clés_types.Clear();
        }

        public static void recherche_par_criteres_rem(bool date1, DateTime d_inf, bool date2, DateTime d_max, bool date3, DateTime pv_min, bool date4, DateTime pv_max, bool durée1, int durée_min, bool durée2, int durée_max, bool somme1, double somme_min, bool somme2, double somme_max, bool employé, bool type)
        {
            responsable.remplissage_liste_filtres_rem();
            filtrer_par_date_demande_inf_rem(date1, d_inf);
            filtrer_par_date_demande_max_rem(date2, d_max);
            filtrer_par_date_pv_inf_rem(date3, pv_min);
            filtrer_par_date_pv_max_rem(date4, pv_max);
            filtrer_par_durée_min_rem(durée1, durée_min);
            filtrer_par_durée_max_rem(durée2, durée_max);
            filtrer_par_employés_rem(employé);
            filtrer_par_types_rem(type);
            filtrer_par_somme_min_rem(somme1, somme_min);
            filtrer_par_somme_max_rem(somme2, somme_max);
        }
        public static void recherche_par_criteres_non_rem(bool date1, DateTime d_inf, bool date2, DateTime d_max, bool date3, DateTime pv_min, bool date4, DateTime pv_max, bool somme1, double somme_min, bool somme2, double somme_max, bool employé, bool type)
        {
            responsable.remplissage_liste_filtres_non_rem();
            filtrer_par_date_demande_inf_non_rem(date1, d_inf);
            filtrer_par_date_demande_max_non_rem(date2, d_max);
            filtrer_par_date_pv_inf_non_rem(date3, pv_min);
            filtrer_par_date_pv_max_non_rem(date4, pv_max);
            filtrer_par_employés_non_rem(employé);
            filtrer_par_types_non_rem(type);
            filtrer_par_somme_min_non_rem(somme1, somme_min);
            filtrer_par_somme_max_non_rem(somme2, somme_max);
        }

        //Import et export ves Microsoft Excel
        public static void export_prêts_remboursable()
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            sheet1.Cells[1, 1] = "Nom";
            sheet1.Cells[1, 2] = "Prenom";
            sheet1.Cells[1, 3] = "Numero social";
            sheet1.Cells[1, 4] = "CCP";
            sheet1.Cells[1, 5] = "Clé CCP";
            sheet1.Cells[1, 6] = "Nature du prêt";
            sheet1.Cells[1, 7] = "Motif";
            sheet1.Cells[1, 8] = "Numero du PV";
            sheet1.Cells[1, 9] = "Date du PV";
            sheet1.Cells[1, 10] = "Montant (DA)";
            sheet1.Cells[1, 11] = "Somme remboursée (DA)";
            sheet1.Cells[1, 12] = "Date de demande";
            sheet1.Cells[1, 13] = "Montant en lettre";
            sheet1.Cells[1, 14] = "Date du premier paiment";
            sheet1.Cells[1, 15] = "durée";
            sheet1.Cells[1, 16] = "mois 1";
            sheet1.Cells[1, 17] = "mois 2";
            sheet1.Cells[1, 18] = "mois 3";
            sheet1.Cells[1, 19] = "mois 4";
            sheet1.Cells[1, 20] = "mois 5";
            sheet1.Cells[1, 21] = "mois 6";
            sheet1.Cells[1, 22] = "mois 7";
            sheet1.Cells[1, 23] = "mois 8";
            sheet1.Cells[1, 24] = "mois 9";
            sheet1.Cells[1, 25] = "mois 10";

            int i = 2;

            foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
            {
                if (element.Value.isPere())
                {
                    sheet1.Cells[i, 1] = element.Value.Employé.Nom;
                    sheet1.Cells[i, 2] = element.Value.Employé.Prenom;
                    sheet1.Cells[i, 3] = element.Value.Employé.sec_soc.ToString();
                    sheet1.Cells[i, 4] = element.Value.Employé.compte_ccp;
                    sheet1.Cells[i, 5] = element.Value.Employé.Cle_ccp;
                    sheet1.Cells[i, 6] = element.Value.Type_Pret.Description;
                    sheet1.Cells[i, 7] = element.Value.Motif;
                    sheet1.Cells[i, 8] = element.Value.Num_pv.ToString();
                    sheet1.Cells[i, 9] = element.Value.Date_pv;
                    sheet1.Cells[i, 10] = element.Value.Montant.ToString();
                    sheet1.Cells[i, 11] = element.Value.Somme_remboursée.ToString();
                    sheet1.Cells[i, 12] = element.Value.Date_demande;
                    sheet1.Cells[i, 13] = element.Value.Montant_lettre;
                    sheet1.Cells[i, 14] = element.Value.Date_premier_paiment;
                    sheet1.Cells[i, 15] = element.Value.Durée;

                    int k = 1;

                    foreach (KeyValuePair<int, double> elemens in element.Value.Etat)
                    {
                        sheet1.Cells[i, k + 15] = elemens.Value;
                        k++;
                    }
                    if (element.Value.Debordement != -1)
                    {
                        List<double> montant_fils = new List<double>();
                        montant_fils.Clear();
                    repeat:;
                        pret_remboursable fils = element.Value.getFils();
                        foreach (double d in fils.Etat.Values)
                        {
                            montant_fils.Add(d);
                        }
                        if (fils.Debordement != -1)
                        {
                            goto repeat;
                        }
                        int cpt = 26;
                        int mois_debor = 11;
                        foreach (double d in montant_fils)
                        {
                            sheet1.Cells[1, cpt] = "mois " + mois_debor.ToString();
                            sheet1.Cells[i, cpt] = d;
                            cpt++;
                            mois_debor++;
                        }
                    }
                    i++;
                }
            }
        }
        public static void export_prêts_non_remboursable()
        {
            Excel.Application excel = new Excel.Application();

            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            sheet1.Cells[1, 1] = "numero social";
            sheet1.Cells[1, 2] = "Nom";
            sheet1.Cells[1, 3] = "Prenom";
            sheet1.Cells[1, 4] = "type";
            sheet1.Cells[1, 5] = "motif";
            sheet1.Cells[1, 6] = "num_pv";
            sheet1.Cells[1, 7] = "date_pv";
            sheet1.Cells[1, 8] = "montant";
            sheet1.Cells[1, 9] = "date_demande";
            sheet1.Cells[1, 10] = "montant_lettre";

            int i = 2;
            foreach (KeyValuePair<int, pret_non_remboursable> element in responsable.liste_pret_non_remboursables)
            {
                sheet1.Cells[element.Key + 1, 1] = element.Value.Employé.sec_soc;
                sheet1.Cells[element.Key + 1, 2] = element.Value.Employé.Nom;
                sheet1.Cells[element.Key + 1, 3] = element.Value.Employé.Prenom;
                sheet1.Cells[element.Key + 1, 4] = element.Value.Type_Pret.Description;
                sheet1.Cells[element.Key + 1, 5] = element.Value.Motif;
                sheet1.Cells[element.Key + 1, 6] = element.Value.Num_pv;
                sheet1.Cells[element.Key + 1, 7] = element.Value.Date_pv;
                sheet1.Cells[element.Key + 1, 8] = element.Value.Montant;
                sheet1.Cells[element.Key + 1, 9] = element.Value.Date_demande;
                sheet1.Cells[element.Key + 1, 10] = element.Value.Montant_lettre;
                i++;
            }
            excel.Visible = true;
        }
        public static void export_Archive()
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            sheet1.Cells[1, 1] = "Numero social";
            sheet1.Cells[1, 2] = "Nom";
            sheet1.Cells[1, 3] = "Prenom";
            sheet1.Cells[1, 4] = "Type du pret";
            sheet1.Cells[1, 5] = "Motif";
            sheet1.Cells[1, 6] = "Numéro PV";
            sheet1.Cells[1, 7] = "Date PV";
            sheet1.Cells[1, 9] = "montant accordé";
            sheet1.Cells[1, 8] = "Date de demande";
            sheet1.Cells[1, 10] = "durée";
            sheet1.Cells[1, 11] = "Observation";

            int i = 2;

            foreach (KeyValuePair<int, Archive> element in responsable.liste_archives)
            {
                sheet1.Cells[i, 1] = element.Value.Pret.Employé.sec_soc;
                sheet1.Cells[i, 2] = element.Value.Pret.Employé.Nom;
                sheet1.Cells[i, 3] = element.Value.Pret.Employé.Prenom;
                sheet1.Cells[i, 4] = element.Value.Pret.Type_Pret.Description;
                sheet1.Cells[i, 5] = element.Value.Pret.Motif;
                sheet1.Cells[i, 6] = element.Value.Pret.Num_pv;
                sheet1.Cells[i, 7] = element.Value.Pret.Date_pv;
                sheet1.Cells[i, 9] = element.Value.Pret.Montant;
                sheet1.Cells[i, 8] = element.Value.Pret.Date_demande;
                sheet1.Cells[i, 10] = element.Value.Durée;
                sheet1.Cells[i, 11] = element.Value.Observations;

                i++;
            }
        }              


        public static void Envoi_mail(pret_remboursable pret, double montant)//Envoi de mail et notification à l'employé
        {
            if (montant != 0)
            {
                try
                {
                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.gmail.com";
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;

                    client.Credentials = new System.Net.NetworkCredential(User_mail, User_pwd);

                    client.Send(User_mail, pret.Employé.Email, "[Prelevement COS]", "Cher employé,\nUn montant de " + montant + " DA a été prelevé de votre compte le :" + DateTime.Now.ToShortDateString() + ",\n Cordialement.");
                }

                catch (Exception k)
                {
                    MessageBox.Show("Un Problème de connection est survenue.\nVeuillez vérifier votre connecvtion.", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.gmail.com";
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;

                    client.Credentials = new System.Net.NetworkCredential(User_mail, User_pwd);

                    client.Send(User_mail, pret.Employé.Email, "[Prelevement COS]", "Cher employé,\nVotre prélevemnet a été diféré ou vos detes sont effacées le :" + DateTime.Now.ToShortDateString() + ",\n Cordialement.");
                }

                catch (Exception k)
                {
                    MessageBox.Show("Un Problème de connection est survenue.\nVeuillez vérifier votre connecvtion.", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public static void filtrer_par_date_recru_min(bool date, DateTime d_recru_min)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (DateTime.Compare(kvp.Value.Pret.Employé.Date_prem, d_recru_min) < 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_date_recru_max(bool date, DateTime d_recru_max)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (DateTime.Compare(kvp.Value.Pret.Employé.Date_prem, d_recru_max) > 0)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_etat(bool e, string etat)
        {

            if (e == true)
            {
                foreach (KeyValuePair<int, Archive> kvp in liste_archives)
                {

                    if (kvp.Value.Pret.Employé.etats != etat)
                    {
                        liste_filtres.Remove(kvp.Key);
                    }
                }
            }
        }

        //Cette methode existait deja je l'ai juste modifie.
        public static void recherche_par_criteres(bool remboursable, int choix, bool date1, DateTime d_inf, bool date2, DateTime d_max, bool date3, DateTime pv_min, bool date4, DateTime pv_max, bool durée1, int durée_min, bool durée2, int durée_max, bool somme1, double somme_min, bool somme2, double somme_max, bool employé, bool type, bool e, string etat, bool a, DateTime drmin, bool b, DateTime drmax)
        {
            responsable.remplissage_liste_filtres();
            filtrer_par_remboursable_ou_non(remboursable, choix);
            filtrer_par_date_demande_inf(date1, d_inf);
            filtrer_par_date_demande_max(date2, d_max);
            filtrer_par_date_pv_inf(date3, pv_min);
            filtrer_par_date_pv_max(date4, pv_max);
            filtrer_par_durée_min(durée1, durée_min);
            filtrer_par_durée_max(durée2, durée_max);
            filtrer_par_employés(employé);
            filtrer_par_types(type);
            filtrer_par_somme_min(somme1, somme_min);
            filtrer_par_somme_max(somme2, somme_max);
            filtrer_par_etat(e, etat);
            filtrer_par_date_recru_min(a, drmin);
            filtrer_par_date_recru_max(b, drmax);
        }
        // les modifications sont ici
        //-------------------------------------------------------------------------------------------------------------------
        public static void filtrer_par_date_recru_min_rem(bool date, DateTime d_recru_min)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (DateTime.Compare(kvp.Value.Employé.Date_prem, d_recru_min) < 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_date_recru_max_rem(bool date, DateTime d_recru_max)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (DateTime.Compare(kvp.Value.Employé.Date_prem, d_recru_max) > 0)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_etat_rem(bool e, string etat)
        {

            if (e == true)
            {
                foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
                {

                    if (kvp.Value.Employé.etats != etat)
                    {
                        liste_filtres_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------

        public static void recherche_par_criteres_rem(bool date1, DateTime d_inf, bool date2, DateTime d_max, bool date3, DateTime pv_min, bool date4, DateTime pv_max, bool durée1, int durée_min, bool durée2, int durée_max, bool somme1, double somme_min, bool somme2, double somme_max, bool employé, bool type, bool e, string etat, bool a, DateTime drmin, bool b, DateTime drmax)
        {
            responsable.remplissage_liste_filtres_rem();
            filtrer_par_date_demande_inf_rem(date1, d_inf);
            filtrer_par_date_demande_max_rem(date2, d_max);
            filtrer_par_date_pv_inf_rem(date3, pv_min);
            filtrer_par_date_pv_max_rem(date4, pv_max);
            filtrer_par_durée_min_rem(durée1, durée_min);
            filtrer_par_durée_max_rem(durée2, durée_max);
            filtrer_par_employés_rem(employé);
            filtrer_par_types_rem(type);
            filtrer_par_somme_min_rem(somme1, somme_min);
            filtrer_par_somme_max_rem(somme2, somme_max);
            filtrer_par_etat_rem(e, etat);
            filtrer_par_date_recru_min_rem(a, drmin);
            filtrer_par_date_recru_max_rem(b, drmax);
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static void filtrer_par_date_recru_min_non_rem(bool date, DateTime d_recru_min)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (DateTime.Compare(kvp.Value.Employé.Date_prem, d_recru_min) < 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_date_recru_max_rem_non_rem(bool date, DateTime d_recru_max)
        {

            if (date == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (DateTime.Compare(kvp.Value.Employé.Date_prem, d_recru_max) > 0)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }
        public static void filtrer_par_etat_non_rem(bool e, string etat)
        {

            if (e == true)
            {
                foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
                {

                    if (kvp.Value.Employé.etats != etat)
                    {
                        liste_filtres_non_rem.Remove(kvp.Key);
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------

        public static void recherche_par_criteres_non_rem(bool date1, DateTime d_inf, bool date2, DateTime d_max, bool date3, DateTime pv_min, bool date4, DateTime pv_max, bool somme1, double somme_min, bool somme2, double somme_max, bool employé, bool type, bool e, string etat, bool a, DateTime drmin, bool b, DateTime drmax)
        {
            responsable.remplissage_liste_filtres_non_rem();
            filtrer_par_date_demande_inf_non_rem(date1, d_inf);
            filtrer_par_date_demande_max_non_rem(date2, d_max);
            filtrer_par_date_pv_inf_non_rem(date3, pv_min);
            filtrer_par_date_pv_max_non_rem(date4, pv_max);
            filtrer_par_employés_non_rem(employé);
            filtrer_par_types_non_rem(type);
            filtrer_par_somme_min_non_rem(somme1, somme_min);
            filtrer_par_somme_max_non_rem(somme2, somme_max);
            filtrer_par_etat_non_rem(e, etat);
            filtrer_par_date_recru_min_non_rem(a, drmin);
            filtrer_par_date_recru_max_rem_non_rem(b, drmax);
        }

        //Effacement des données contenues des les différentes listes

        public static void effacer_données()
        {
            responsable.liste_archives.Clear();
            responsable.liste_employes.Clear();
            responsable.liste_pret_non_remboursables.Clear();
            responsable.liste_pret_remboursable.Clear();
            responsable.tresor = 0;
            responsable.liste_archives_provisoire.Clear();
            responsable.liste_pret_remboursable_provisoire.Clear();
        }

        public static void remise_a_zero(int choix)//Restauration de l'application
        {
            SqlConnection cnx = new SqlConnection("Data Source = .\\SQLEXPRESS; Initial Catalog = BDD_COS_finale_v2; Integrated Security = True");//"Data Source = (localdb)\\localdb2; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"
            cnx.Open();
            SqlCommand cmd = cnx.CreateCommand();
            if (choix == 1)
            {
                try
                {
                    responsable.liste_employes.Clear();
                    cmd.CommandText = "DELETE FROM employes ;";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Vous devez d'abord vider les tables avec employé comme clé etrangère", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (choix == 2)
            {
                responsable.liste_pret_remboursable.Clear();
                responsable.liste_pret_remboursable_provisoire.Clear();
                cmd.CommandText = "DELETE FROM prets_remboursable ;";
                cmd.ExecuteNonQuery();
            }
            if (choix == 3)
            {
                responsable.liste_pret_non_remboursables.Clear();
                cmd.CommandText = "DELETE FROM prets_non_remboursable ;";
                cmd.ExecuteNonQuery();
            }
            if (choix == 4)
            {
                try
                {
                    responsable.liste_types.Clear();
                    cmd.CommandText = "DELETE FROM type_prets ;";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Vous devez d'abord vider les tables avec Type_pret comme clé etrangère", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (choix == 5)
            {
                responsable.liste_archives_provisoire.Clear();
                responsable.liste_archives.Clear();
                cmd.CommandText = "DELETE FROM archive ;";
                cmd.ExecuteNonQuery();
            }
            if(choix == 6)
            {
                responsable.tresor = 0;
                responsable.sauvgarder_montant_tresor();
            }
            cnx.Close();
        }

        //Bilan Annuel
        public static void remplissage_bilan(int année)
        {
            responsable.bilan.Clear();
            foreach (KeyValuePair<int, Archive> kvp in liste_archives)
            {
                if (kvp.Value.nino() && kvp.Value.Pret.Date_pv.Year == année)
                {
                    responsable.bilan.Add(kvp.Value.Pret);
                }
            }
            foreach (KeyValuePair<int, pret_remboursable> kvp in liste_pret_remboursable)
            {
                if (kvp.Value.isPere() && kvp.Value.Date_pv.Year == année)
                {
                    responsable.bilan.Add(kvp.Value);
                }
            }
            foreach (KeyValuePair<int, pret_non_remboursable> kvp in liste_pret_non_remboursables)
            {
                if (kvp.Value.Date_pv.Year == année)
                {
                    responsable.bilan.Add(kvp.Value);
                }

            }
        }

        public static void export_bilan()
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            sheet1.Cells[1, 1] = "Nom";
            sheet1.Cells[1, 2] = "Prenom";
            sheet1.Cells[1, 3] = "Nature du prêt";
            sheet1.Cells[1, 4] = "Motif";
            sheet1.Cells[1, 5] = "Numero du PV";
            sheet1.Cells[1, 6] = "Date du PV";
            sheet1.Cells[1, 7] = "Date de demande";
            sheet1.Cells[1, 8] = "Etat";
            sheet1.Cells[1, 9] = "Somme accordée (DA)";
            sheet1.Cells[1, 10] = "Somme remboursée (DA)";
            int i = 2;

            foreach (Prets element in responsable.bilan)
            {

                sheet1.Cells[i, 1] = element.Employé.Nom;
                sheet1.Cells[i, 2] = element.Employé.Prenom;
                sheet1.Cells[i, 3] = element.Type_Pret.Description;
                sheet1.Cells[i, 4] = element.Motif;
                sheet1.Cells[i, 5] = element.Num_pv.ToString();
                sheet1.Cells[i, 6] = element.Date_pv;
                sheet1.Cells[i, 9] = element.Montant.ToString();
                sheet1.Cells[i, 7] = element.Date_demande;
                sheet1.Cells[i, 10] = element.somme_rembours().ToString();
                if (element.GetType() == typeof(pret_remboursable))
                {
                    if (responsable.liste_pret_remboursable.ContainsValue((pret_remboursable)element))
                    {
                        sheet1.Cells[i, 8] = "en cours";
                    }
                    else
                    {
                        sheet1.Cells[i, 8] = "cloturé";
                    }
                }
                else
                {
                    if (element.GetType() == typeof(pret_non_remboursable))
                    {
                        if (responsable.liste_pret_non_remboursables.ContainsValue((pret_non_remboursable)element))
                        {
                            sheet1.Cells[i, 8] = "en cours";
                        }
                        else
                        {
                            sheet1.Cells[i, 8] = "cloturé";
                        }

                    }

                }
                i++;
            }
        }

        public static bool Verif_fichier_import_remboursable(Worksheet sheet)
        {
            bool validité = false;
            if (sheet.Cells[1, 1].Value.ToString().Equals("Nom") && sheet.Cells[1, 2].Value.ToString().Equals("Prénom") && sheet.Cells[1, 3].Value.ToString().Equals("Numéro de sécurité sociale") && sheet.Cells[1, 4].Value.ToString().Equals("Nature du prêt") && sheet.Cells[1, 5].Value.ToString().Equals("Motif") && sheet.Cells[1, 6].Value.ToString().Equals("Numéro du PV") && sheet.Cells[1, 7].Value.ToString().Equals("Date du PV(année-mois-jour)") && sheet.Cells[1, 8].Value.ToString().Equals("Montant(DA)") && sheet.Cells[1, 9].Value.ToString().Equals("Date de demande") && sheet.Cells[1, 10].Value.ToString().Equals("Montant en lettre") && sheet.Cells[1, 11].Value.ToString().Equals("Date du Premier Paiement") && sheet.Cells[1, 12].Value.ToString().Equals("Durée du rembourssement (en mois)") && sheet.Cells[1, 13].Value.ToString().Equals("Etat(Paiement régulier / Paiement retardé)") && sheet.Cells[1, 14].Value.ToString().Equals("Mois 1") && sheet.Cells[1, 15].Value.ToString().Equals("Mois 2") && sheet.Cells[1, 16].Value.ToString().Equals("Mois 3") && sheet.Cells[1, 17].Value.ToString().Equals("Mois 4") && sheet.Cells[1, 18].Value.ToString().Equals("Mois 5") && sheet.Cells[1, 19].Value.ToString().Equals("Mois 6") && sheet.Cells[1, 20].Value.ToString().Equals("Mois 7") && sheet.Cells[1, 21].Value.ToString().Equals("Mois 8") && sheet.Cells[1, 22].Value.ToString().Equals("Mois 9") && sheet.Cells[1, 23].Value.ToString().Equals("Mois 10") && sheet.Cells[1, 24].Value.ToString().Equals("Mois 11") && sheet.Cells[1, 25].Value.ToString().Equals("Mois 12") && sheet.Cells[1, 26].Value.ToString().Equals("Mois 13") && sheet.Cells[1, 27].Value.ToString().Equals("Mois 14") && sheet.Cells[1, 28].Value.ToString().Equals("Mois 15") && sheet.Cells[1, 29].Value.ToString().Equals("Mois 16") && sheet.Cells[1, 30].Value.ToString().Equals("Mois 17") && sheet.Cells[1, 31].Value.ToString().Equals("Mois 18") && sheet.Cells[1, 32].Value.ToString().Equals("Mois 19") && sheet.Cells[1, 33].Value.ToString().Equals("Mois 20") && sheet.Cells[1, 34].Value.ToString().Equals("Suite des mois"))
            {
                validité = true;
            }
            return validité;
        }

        public static void import_prêts_remboursable()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xlsx";
            ofd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            var sel = ofd.ShowDialog();
            if (sel == true)
            {
                Dictionary<int, pret_remboursable> Liste_prets = new Dictionary<int, pret_remboursable>();
                String a = ofd.FileName;
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;
                Excel.Workbook workBook = excelApp.Workbooks.Open(a);
                Worksheet sheet = (Worksheet)workBook.Sheets[1];
                bool ok = Verif_fichier_import_remboursable(sheet);
                if (ok)
                {
                    try
                    {
                        int i = 2;
                        int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing).Row;
                        while (i <= lastRow)
                        {
                            Employé emp = null;
                            bool check_emp = false;
                            foreach (KeyValuePair<int, Employé> elemen in responsable.liste_employes)
                            {
                                if (elemen.Value.Nom.Equals(sheet.Cells[i, 1].Value.ToString()) && elemen.Value.Prenom.Equals(sheet.Cells[i, 2].Value.ToString()) && elemen.Value.sec_soc.Equals(sheet.Cells[i, 3].Value.ToString()))
                                {
                                    emp = elemen.Value;
                                    check_emp = true;
                                    Type_pret type = null;
                                    bool check_type = false;
                                    foreach (KeyValuePair<int, Type_pret> elem in responsable.liste_types)
                                    {
                                        if (elem.Value.Description.Equals(sheet.Cells[i, 4].Value.ToString()))
                                        {
                                            type = elem.Value;
                                            check_type = true;

                                            Dictionary<int, double> dicot = new Dictionary<int, double>();
                                            int z = 0;
                                            int nb_mois = 0;
                                            while (sheet.Cells[i, z + 14].Value != null)
                                            {
                                                if (sheet.Cells[i, z + 14].Value.ToString().Equals("/"))
                                                {
                                                    dicot.Add(z, -1);
                                                }
                                                else
                                                {
                                                    dicot.Add(z, Double.Parse(sheet.Cells[i, z + 14].Value.ToString()));
                                                    nb_mois++;
                                                }
                                                z++;
                                            }
                                            int cpt = 0;
                                            foreach (double d in dicot.Values)
                                            {
                                                cpt++;
                                            }
                                            int en_cours;
                                            if (sheet.Cells[i, 13].Value.ToString().Equals("Paiement régulier") || sheet.Cells[i, 13].Value.ToString().Equals("paiement régulier") || sheet.Cells[i, 13].Value.ToString().Equals("Régulier") || sheet.Cells[i, 13].Value.ToString().Equals("régulier"))
                                                en_cours = 1;
                                            else
                                            {
                                                if (sheet.Cells[i, 13].Value.ToString().Equals("Paiement retardé") || sheet.Cells[i, 13].Value.ToString().Equals("paiement retardé") || sheet.Cells[i, 13].Value.ToString().Equals("Retardé") || sheet.Cells[i, 13].Value.ToString().Equals("retardé"))
                                                {
                                                    en_cours = 0;
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Veuillez vérifier le champs ''Etat''.");
                                                    goto fin;
                                                }

                                            }


                                            if (cpt <= 10)
                                            {
                                                pret_remboursable pret = new pret_remboursable(cle_a_affecter_pret_remboursable(), emp, type, sheet.Cells[i, 5].Value.ToString(), Int32.Parse(sheet.Cells[i, 6].Value.ToString()), DateTime.Parse(sheet.Cells[i, 7].Value.ToString()), Double.Parse(sheet.Cells[i, 8].Value.ToString()), DateTime.Parse(sheet.Cells[i, 9].Value.ToString()), sheet.Cells[i, 10].Value.ToString(), DateTime.Parse(sheet.Cells[i, 11].Value.ToString()), Int32.Parse(sheet.Cells[i, 12].Value.ToString()), en_cours, dicot, -1);
                                                responsable.liste_pret_remboursable.Add(pret.Cle, pret);
                                                pret.Employé.ajouter_pret_remboursable_employe(pret);
                                                responsable.tresor -= pret.Montant;
                                            }
                                            else
                                            {
                                                int nb_dico = cpt / 10 + 1;
                                                int nb_pret = nb_dico;
                                                List<pret_remboursable> liste = new List<pret_remboursable>();
                                                liste.Clear();
                                                int key = cle_a_affecter_pret_remboursable();
                                                int uniq = 0;
                                                int x = 0;
                                                while (nb_dico != 0)
                                                {
                                                    Dictionary<int, double> dico = new Dictionary<int, double>() { { 0, -1 }, { 1, -1 }, { 2, -1 }, { 3, -1 }, { 4, -1 }, { 5, -1 }, { 6, -1 }, { 7, -1 }, { 8, -1 }, { 9, -1 } };
                                                    for (int k = 0; k < 10; k++)
                                                    {
                                                        if (dicot[x] != -1)
                                                            dico[k] = dicot[x];
                                                        x++;
                                                        if (x == cpt)
                                                            break;
                                                    }
                                                    pret_remboursable pret = new pret_remboursable(key + uniq, emp, type, sheet.Cells[i, 5].Value.ToString(), Int32.Parse(sheet.Cells[i, 6].Value.ToString()), DateTime.Parse(sheet.Cells[i, 7].Value.ToString()), Double.Parse(sheet.Cells[i, 8].Value.ToString()), DateTime.Parse(sheet.Cells[i, 9].Value.ToString()), sheet.Cells[i, 10].Value.ToString(), DateTime.Parse(sheet.Cells[i, 11].Value.ToString()), Int32.Parse(sheet.Cells[i, 12].Value.ToString()), en_cours, dico, -1);
                                                    uniq++;
                                                    liste.Add(pret);
                                                    nb_dico--;
                                                }
                                                for (int j = 0; j < nb_pret - 1; j++)
                                                {
                                                    liste[j].Debordement = liste[j + 1].Cle;
                                                }
                                                foreach (pret_remboursable p in liste)
                                                {
                                                    responsable.liste_pret_remboursable.Add(p.Cle, p);
                                                    p.Employé.ajouter_pret_remboursable_employe(p);
                                                }
                                                responsable.tresor -= liste[0].Montant;
                                            }
                                        }
                                    }
                                    if (!check_type)
                                    {
                                        MessageBox.Show("Un ou plusieurs types de prets dans votre fichier ne font pas partie \nde la liste des types de prets de l'application.\nVeuillez consulter la section '' Types de pret existants '' \ndans l'onglet''Nouveau type''.");
                                        goto fin;
                                    }
                                }
                            }
                            if (!check_emp)
                            {
                                MessageBox.Show("Un ou plusieurs employés dans votre fichier ne font pas partie \n de la liste des employés de l'application.\n Veuillez consulter l'onglet ''Liste des Employés''.");
                                goto fin;
                            }
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Votre fichier contient des valeurs qui posent des problèmes durant la lecture.\nVeuillez le réviser SVP.");
                        goto fin;
                    }
                }
                else
                {
                    MessageBox.Show("Le fichier que vous avez choisi peut etre non compatible avec \n le modèle que vous trouverez dans le manuel d'utilisation de l'application.\n Veuillez consulter ce dernier pour eviter cette erreur.");
                }

            fin:;
                excelApp.Quit();
            }
        }

        public static bool Verif_fichier_import_non_remboursable(Worksheet sheet)
        {
            bool validité = false;
            if (sheet.Cells[1, 1].Value.ToString().Equals("Nom") && sheet.Cells[1, 2].Value.ToString().Equals("Prénom") && sheet.Cells[1, 3].Value.ToString().Equals("Numéro de sécurité sociale") && sheet.Cells[1, 4].Value.ToString().Equals("Nature du don") && sheet.Cells[1, 5].Value.ToString().Equals("Motif") && sheet.Cells[1, 6].Value.ToString().Equals("Numéro du PV") && sheet.Cells[1, 7].Value.ToString().Equals("Date du PV(année-mois-jour)") && sheet.Cells[1, 8].Value.ToString().Equals("Montant(DA)") && sheet.Cells[1, 9].Value.ToString().Equals("Date de demande") && sheet.Cells[1, 10].Value.ToString().Equals("Montant en lettre"))
            {
                validité = true;
            }
            return validité;
        }
        public static void import_prêts_non_remboursable()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xlsx";
            ofd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            var sel = ofd.ShowDialog();
            if (sel == true)
            {
                String a = ofd.FileName;
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;
                Excel.Workbook workBook = excelApp.Workbooks.Open(a);
                Worksheet sheet = (Worksheet)workBook.Sheets[1];
                bool ok = Verif_fichier_import_non_remboursable(sheet);
                if (ok)
                {
                    try
                    {
                        int i = 2;

                        int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing).Row;
                        while (i <= lastRow) // Parcours par lignes du fichier excel
                        {
                            Employé emp = null;
                            bool check_emp = false;
                            foreach (KeyValuePair<int, Employé> elemen in responsable.liste_employes)
                            {
                                if (((elemen.Value.Nom) == sheet.Cells[i, 1].Value.ToString()) && elemen.Value.Prenom.Equals(sheet.Cells[i, 2].Value.ToString()) && elemen.Value.sec_soc.Equals(sheet.Cells[i, 3].Value.ToString()))
                                {
                                    emp = elemen.Value; //trouver l employé existant
                                    check_emp = true;
                                    Type_pret type = null;
                                    bool check_type = false;
                                    foreach (KeyValuePair<int, Type_pret> elem in responsable.liste_types)
                                    {
                                        if (elem.Value.Description.Equals(sheet.Cells[i, 4].Value.ToString()))
                                        {
                                            type = elem.Value;
                                            check_type = true;
                                            responsable.Creer_pret_non_remboursable(emp.Cle, type.Cle, sheet.Cells[i, 5].Value.ToString(), Int32.Parse(sheet.Cells[i, 6].Value.ToString()), DateTime.Parse(sheet.Cells[i, 7].Value.ToString()), Double.Parse(sheet.Cells[i, 8].Value.ToString()), DateTime.Parse(sheet.Cells[i, 9].Value.ToString()), sheet.Cells[i, 10].Value.ToString());
                                        }
                                    }
                                    if (!check_type)
                                    {
                                        MessageBox.Show("Un ou plusieurs types de prets dans votre fichier ne font pas partie \nde la liste des types de prets de l'application.\nVeuillez consulter la section '' Types de pret existants '' \ndans l'onglet''Nouveau type''.");
                                        goto fin;
                                    }
                                }
                            }
                            if (!check_emp)
                            {
                                MessageBox.Show("Un ou plusieurs employés dans votre fichier ne font pas partie \n de la liste des employés de l'application.\n Veuillez consulter l'onglet ''Liste des Employés''.");
                                goto fin;
                            }
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Votre fichier contient des valeurs qui posent des problèmes durant la lecture.\nVeuillez le réviser SVP.");
                        goto fin;
                    }
                }
                else
                {
                    MessageBox.Show("Le fichier que vous avez choisi peut etre non compatible avec \n le modèle que vous trouverez dans le manuel d'utilisation de l'application.\n Veuillez consulter ce dernier pour eviter cette erreur.");
                    goto fin;
                }
            fin:;
                excelApp.Quit();
            }
        }

        public static bool Verif_fichier_import_employe(Worksheet sheet)
        {
            bool validité = false;
            if (sheet.Cells[1, 1].Value.ToString().Equals("Nom") && sheet.Cells[1, 2].Value.ToString().Equals("Prénom") && sheet.Cells[1, 3].Value.ToString().Equals("Matricule") && sheet.Cells[1, 4].Value.ToString().Equals("Numéro de sécurité sociale") && sheet.Cells[1, 5].Value.ToString().Equals("Date de naissance(année-mois-jour)") && sheet.Cells[1, 6].Value.ToString().Equals("Date de recrutement") && sheet.Cells[1, 7].Value.ToString().Equals("Statut sociale") && sheet.Cells[1, 8].Value.ToString().Equals("Grade") && sheet.Cells[1, 9].Value.ToString().Equals("Service") && sheet.Cells[1, 10].Value.ToString().Equals("CCP") && sheet.Cells[1, 11].Value.ToString().Equals("Clé CCP") && sheet.Cells[1, 12].Value.ToString().Equals("Téléphone") && sheet.Cells[1, 13].Value.ToString().Equals("Email") && sheet.Cells[1, 14].Value.ToString().Equals("Statut professionnel"))
            {
                validité = true;
            }
            return validité;
        }

        public static void import_employe()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xlsx";
            ofd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            var sel = ofd.ShowDialog();
            if (sel == true)
            {

                String a = ofd.FileName;
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;
                Excel.Workbook workBook = excelApp.Workbooks.Open(a);
                Worksheet sheet = (Worksheet)workBook.Sheets[1];
                bool ok = Verif_fichier_import_employe(sheet);
                if (ok)
                {
                    try
                    {
                        int i = 2;

                        int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing).Row;
                        while (i <= lastRow) // Parcours par lignes du fichier excel
                        {
                            responsable.Creer_employe(sheet.Cells[i, 3].Value.ToString(), sheet.Cells[i, 1].Value.ToString(), sheet.Cells[i, 2].Value.ToString(), sheet.Cells[i, 4].Value.ToString(), DateTime.Parse(sheet.Cells[i, 5].Value.ToString()), sheet.Cells[i, 8].Value.ToString(), DateTime.Parse(sheet.Cells[i, 6].Value.ToString()), sheet.Cells[i, 7].Value.ToString(), sheet.Cells[i, 10].Value.ToString(), sheet.Cells[i, 11].Value.ToString(), sheet.Cells[i, 12].Value.ToString(), sheet.Cells[i, 9].Value.ToString(), sheet.Cells[i, 13].Value.ToString(), sheet.Cells[i, 14].Value.ToString());
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Votre fichier contient des valeurs qui posent des problèmes durant la lecture.\nVeuillez le réviser SVP.");
                        goto fin;
                    }
                }
                else
                {
                    MessageBox.Show("Le fichier que vous avez choisi peut etre non compatible avec \n le modèle que vous trouverez dans le manuel d'utilisation de l'application.\n Veuillez consulter ce dernier pour eviter cette erreur.");
                    goto fin;
                }
            fin:;
                excelApp.Quit();
            }
        }
    }
}


