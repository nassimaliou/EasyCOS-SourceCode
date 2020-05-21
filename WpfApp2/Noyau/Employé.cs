using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Employé
    {
        private int cle;
        private static int cle_unique_employés = 1; // attribut pour l'unicité des clés des employés gerés par nous.
        private string nom;
        private string prenom;
        private string num_sec_social;
        private DateTime date_naissance;
        private DateTime date_prem;
        private string grade;
        private string ccp;
        private string cle_ccp;
        private string etat;
        private string num_tel;
        private string matricule;
        private string service;
        private string email;
        private string etat_service;
        public static int cle_liste_prets_remboursable_employe = 1; // cles pour l'unicite des cles des 2 dictionnaires 
        public static int cle_liste_prets_Non_remboursable_employe = 1; // (pret_remboursable_employe et pret_non_remboursable_employe)

        private Dictionary<int, pret_remboursable> pret_remboursable_employe = new Dictionary<int, pret_remboursable>();
        private Dictionary<int, pret_non_remboursable> pret_non_remboursable_employe = new Dictionary<int, pret_non_remboursable>();


        public Employé(int cle_, string matricule, string nom, string prenom, string num_sec_social, DateTime date_naissance, string grade, DateTime date_prem, string etat, string ccp, string cle_ccp, string tel, string service, string email, string etat_service)
        {
            //this.cle = Employé.cle_unique_employés;//l'unicité de la cle d'un employé
            this.cle = cle_;
            this.matricule = matricule;
            this.nom = nom;
            this.prenom = prenom;
            this.num_sec_social = num_sec_social;
            this.date_naissance = date_naissance;
            this.date_prem = date_prem;
            this.grade = grade;
            this.ccp = ccp;
            this.cle_ccp = cle_ccp;
            this.etat = etat;
            this.ccp = ccp;
            this.tel = tel;
            this.service = service;
            this.email = email;            
            this.etat_service = etat_service;
            // Console.WriteLine(this.cle + " " + this.Nom);
            // responsable.ajouter_employe(this);// ajout automatique d'un employé à la liste des employés.
        }

        public void affiche_attribus()
        {
            Console.WriteLine(this.cle + " | " + this.matricule + " | " + this.nom + " | " + this.prenom + " | " + this.num_sec_social + " | " + this.date_naissance + " | " + this.grade + " | " + this.date_prem + " | " + this.etat + " | " + this.ccp + " | " + this.cle_ccp + " | " + this.tel + " | " + " | " + this.service);
        }

        public int Cle
        {
            get
            {
                return this.cle;
            }
            set
            {
                this.cle = value;
            }
        }

        public string Matricule
        {
            get
            {
                return this.matricule;
            }
            set
            {
                this.matricule = value;
            }
        }

        public string Nom
        {
            get
            {
                return this.nom;
            }
            set
            {
                this.nom = value;
            }
        }

        public string Prenom
        {
            get
            {
                return this.prenom;
            }
            set
            {
                this.prenom = value;
            }
        }
        public string sec_soc
        {
            get
            {
                return this.num_sec_social;
            }
            set
            {
                this.num_sec_social = value;
            }
        }
        public string compte_ccp
        {
            get
            {
                return this.ccp;
            }
            set
            {
                this.ccp = value;
            }

        }
        public string Cle_ccp
        {
            get
            {
                return this.cle_ccp;
            }
            set
            {
                this.cle_ccp = value;
            }
        }

        public string etats
        {
            get
            {
                return this.etat;
            }
            set
            {
                this.etat = value;
            }
        }
        public string tel
        {
            get
            {
                return this.num_tel;
            }
            set
            {
                this.num_tel = value;
            }
        }

        public DateTime Date_naissance
        {
            get
            {
                return this.date_naissance;
            }
            set
            {
                this.date_naissance = value;
            }
        }

        public DateTime Date_prem
        {
            get
            {
                return this.date_prem;
            }
            set
            {
                this.date_prem = value;
            }
        }

        public string Grade
        {
            get
            {
                return this.grade;
            }
            set
            {
                this.grade = value;
            }
        }

        public string Service
        {
            get
            {
                return this.service;
            }
            set
            {
                this.service = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        public string Etat_service
        {
            get
            {
                return this.etat_service;
            }
            set
            {
                this.etat_service = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Employé emp = obj as Employé;
            return (this.num_sec_social == emp.sec_soc);
        }
        public void ajouter_pret_remboursable_employe(pret_remboursable p)
        {
            if (!(pret_remboursable_employe.ContainsValue(p)))
            {
                pret_remboursable_employe.Add(p.Cle, p);
                Employé.cle_liste_prets_remboursable_employe++;
            }
            else
            {
                Console.WriteLine("pas d'ajout");
            }
        }
        public void ajouter_pret_non_remboursable_employe(pret_non_remboursable p)
        {
            if (!(pret_non_remboursable_employe.ContainsValue(p)))
            {
                pret_non_remboursable_employe.Add(p.Cle, p);
                Employé.cle_liste_prets_Non_remboursable_employe++;
            }
            else
            {
                Console.WriteLine("pas d'ajout");
            }
        }
        public void affiche_liste_pret_remboursable_employe()
        {
            foreach (KeyValuePair<int, pret_remboursable> liste in this.pret_remboursable_employe)
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("Clé = " + liste.Key + " || ");
                liste.Value.affiche_attributs_complets();
            }
        }
        public void affiche_liste_pret_non_remboursable_employe()
        {
            foreach (KeyValuePair<int, pret_non_remboursable> liste in this.pret_non_remboursable_employe)
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("Clé = " + liste.Key + " || ");
                liste.Value.affiche_attribus();
            }
        }
    }
}