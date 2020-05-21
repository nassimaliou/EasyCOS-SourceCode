using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    abstract public class Prets
    {
        protected int cle;
        protected Employé employé;
        protected Type_pret type;
        protected string motif;
        protected int num_pv;
        protected DateTime date_pv;
        protected double montant;
        protected DateTime date_demande;
        protected string montant_lettre;
        public abstract string prem_paiment();
        public abstract string fin_paiement();
        public abstract string somme_rembours();
        public abstract int Debor();

        public Prets(int cle_,Employé employé, Type_pret type, string motif, int num_pv, DateTime date_pv, double montant, DateTime date_demande, string montant_lettre)
        {
            this.type = type;
            this.cle = cle_; 
            this.employé = employé;
            this.type = type;
            this.motif = motif;
            this.num_pv = num_pv;
            this.date_pv = date_pv;
            this.montant = montant;
            this.date_demande = date_demande;
            this.montant_lettre = montant_lettre;
        }
        public abstract void affiche_attributs_complets();
       
        public void affiche_attribus()
        {
            Console.Write(this.cle + " | ");
            this.employé.affiche_attribus();
            this.type.affiche_attribus();
            Console.WriteLine(this.motif + " | " + this.num_pv + " | " + this.date_pv + " | " + this.montant + " | " + this.date_demande + " | " + this.montant_lettre);
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
      
        public Employé Employé
        {

            get
            {
                return this.employé;
            }
            set
            {
                this.employé = value;
            }
        }

        public int Num_pv
        {
            get
            {
                return this.num_pv;
            }
            set
            {
                this.num_pv = value;
            }
        }

        public Double Montant
        {
            get
            {
                return this.montant;
            }
            set
            {
                this.montant = value;
            }
        }

        public string Motif
        {
            get
            {
                return this.motif;
            }
            set
            {
                this.motif = value;
            }
        }

        public DateTime Date_demande
        {
            get
            {
                return this.date_demande;
            }
            set
            {
                this.date_demande = value;
            }
        }

        public string Montant_lettre
        {
            get
            {
                return this.montant_lettre;
            }
            set
            {
                this.montant_lettre = value;
            }
        }

        public Type_pret Type_Pret
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        public DateTime Date_pv
        {
            get
            {
                return this.date_pv;
            }
            set
            {
                this.date_pv = value;
            }
        }
        public override bool Equals(object obj)
        {
            Prets p = obj as Prets;
            if (p == null)
            {
                return false;
            }
            return ((this.cle == p.Cle)&&(this.type==p.Type_Pret));
        }
    }
}    


