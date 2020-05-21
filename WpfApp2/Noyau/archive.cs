using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Archive
    {
        private int cle;
        private Prets pret;
        private string observations;
        private DateTime date_fin_remboursement;
        private int durée;

        public Archive(int cle, Prets Pret_, string observations_, DateTime date_fin_remboursement_, int durée)
        {
            this.cle = cle;
            this.pret = Pret_;
            this.date_fin_remboursement = date_fin_remboursement_;
            this.observations = observations_;
            this.durée = durée;
        }

        public void affiche_attribue()
        {
            Console.Write(this.cle + " | ");
            if (this.Pret.Type_Pret.Remboursable==1)
            {
                this.Pret.affiche_attributs_complets();
            }
            else
            {
                this.Pret.affiche_attribus();
            }
            Console.WriteLine(this.observations + " | " + this.date_fin_remboursement);
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

        public Prets Pret
        {
            get
            {
                return this.pret;
            }
            set
            {
                this.pret = value;
            }
        }

        public string Observations
        {
            get
            {
                return this.observations;
            }
            set
            {
                this.observations = value;
            }
        }

        public DateTime Date_fin_remboursement
        {
            get
            {
                return this.date_fin_remboursement;
            }
            set
            {
                this.date_fin_remboursement = value;
            }
        }

        public int Durée
        {
            get
            {
                return this.durée;
            }
            set
            {
                this.durée = value;
            }
        }

        public bool nino() // retourne vrai si le pret est le fils
        {
            if (this.Pret.Debor() == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}