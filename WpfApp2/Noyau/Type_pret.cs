using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Type_pret
    {
        // private static int cle_liste_types =1;// attribut permettant l'unicite des cles concernant les types de prets.
        private int cle;
        private int type_du_pret;
        private int disponibilité;
        private string description;
        private int remboursable;

        public Type_pret(int cle_,int type_pret, int dispo, string descri, int remboursable)
        {
            this.cle = cle_;
            this.type_du_pret = type_pret;
            this.disponibilité = dispo;
            this.description = descri;
            this.remboursable = remboursable;
            // responsable.ajouter_type_pret(this);//ajout automatique du type pret a la liste des types_prets.
            // Type_pret.cle_liste_types++;
        }

        public void affiche_attribus()
        {
            Console.WriteLine(this.type_du_pret + " | " + this.disponibilité + " | " + this.Description + " | " + this.remboursable);
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

        public int Type_de_pret
        {
            get
            {
                return this.type_du_pret;
            }
            set
            {
                this.type_du_pret = value;
            }
        }

        public int Disponibilité
        {
            get
            {
                return this.disponibilité;
            }
            set
            {
                this.disponibilité = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int Remboursable
        {
            get
            {
                return this.remboursable;
            }
            set
            {
                this.remboursable = value;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Type_pret p = obj as Type_pret;
            return ((this.type_du_pret == p.Type_de_pret)&&(this.remboursable==p.Remboursable)&&(this.disponibilité==p.Disponibilité)&&(this.description==this.Description));
        }
    }
}
