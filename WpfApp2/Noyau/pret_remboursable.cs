using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class pret_remboursable : Prets
    {
        private DateTime date_premier_paiment;
        private int en_cours;              /* cet entier indique si le remboursement d'un pret se deroule normalement ou pas , ie : en_cours == 1 si  (1/10 du montant par mois)                                                                                                                       
                                                                                                                                en_cours == 0 s   (l'employé a choisi de mettre en pause le remboursement) */

        private Dictionary<int, double> etat = new Dictionary<int, double>();
        private int debordement;
        private int mois_actuel = 0;
        private DateTime date_actuelle;
        private int durée;
        private double somme_remboursée;
        


        public pret_remboursable(int cle_, Employé employé, Type_pret type, string motif, int num_pv, DateTime date_pv, double montant, DateTime date_demande, string montant_lettre, DateTime date_premier_paiment, int durée, int en_cours, Dictionary<int, double> dico, int debordement) : base(cle_, employé, type, motif, num_pv, date_pv, montant, date_demande, montant_lettre)
        {
            this.date_premier_paiment = date_premier_paiment;
            this.en_cours = en_cours;
            this.etat = dico;
            this.debordement = debordement;
            foreach (pret_remboursable p in responsable.liste_pret_remboursable.Values)
            {
                if (p.debordement == this.cle)
                {
                    this.somme_remboursée = p.Somme_remboursée;
                }
            }
            this.durée = durée;            
            this.Date_actuelle = this.Date_premier_paiment;
            foreach (pret_remboursable p in responsable.liste_pret_remboursable.Values)
            {
                pret_remboursable k = p;
                if (this.cle == k.Debordement)
                {
                    this.Date_actuelle = k.Date_actuelle;
                }
            }
            foreach (double d in this.etat.Values)
            {
                if (d != -1)
                {
                    this.somme_remboursée = this.somme_remboursée + d;
                    if (this.somme_remboursée < this.montant)
                    {
                        this.date_actuelle = this.date_actuelle.AddMonths(1);
                        this.mois_actuel++;
                    }
                    if (this.somme_remboursée == this.montant)
                    {
                        this.mois_actuel = 11;
                    }
                }

            }
        }

        public Dictionary<int, double> Etat
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

        public int En_cours
        {
            get
            {
                return this.en_cours;
            }
            set
            {
                this.en_cours = value;
            }
        }

        public DateTime Date_premier_paiment
        {
            get
            {
                return this.date_premier_paiment;
            }
            set
            {
                this.date_premier_paiment = value;
            }
        }
        public DateTime Date_actuelle
        {
            get
            {
                return this.date_actuelle;
            }
            set
            {
                this.date_actuelle = value;
            }
        }

        public int Debordement
        {
            get
            {
                return this.debordement;
            }
            set
            {
                this.debordement = value;
            }
        }
        public int Mois_actuel
        {
            get
            {
                return this.mois_actuel;
            }
            set
            {
                this.mois_actuel = value;
            }
        }
        public double Somme_remboursée
        {
            get
            {
                return this.somme_remboursée;
            }
            set
            {
                this.somme_remboursée = value;
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

        public override void affiche_attributs_complets()
        {

            this.affiche_attribus();
            Console.WriteLine("Date de premeir paiement" + this.Date_premier_paiment);


            if (this.En_cours == 1)
            {
                Console.WriteLine("Etat actuel  : paiement régulier");
            }
            else
            {
                Console.WriteLine("Etat actuel  : en retardement");
            }
            Console.WriteLine("Etat de remboursement");
            foreach (double d in this.etat.Values)
            {
                Console.Write(" " + d);

            }
            Console.WriteLine("Debordement : " + this.debordement);
            Console.WriteLine("          ");
            Console.WriteLine(this.mois_actuel);
            Console.WriteLine(this.somme_remboursée);
        }

        public override int Debor()
        {
            return this.Debordement;
        }

        public void paiement()
        {
            responsable.pile_modifications.Add(new Modification(4, this.Cle));
            int difference = DateTime.Compare(DateTime.Now, this.Date_premier_paiment);
            if (difference > 0)
            {
                if ((this.montant - this.somme_remboursée) >= (this.montant / this.durée))
                {
                    if ((this.en_cours == 1) && (this.mois_actuel < 10) && (this.somme_remboursée < this.montant))
                    {
                        this.etat.Remove(this.mois_actuel);
                        this.etat.Add(this.mois_actuel, this.montant / this.durée);
                        responsable.tresor = responsable.tresor + (this.montant / this.durée);
                        this.somme_remboursée = this.somme_remboursée + (this.montant / this.durée);

                        this.date_actuelle = this.date_actuelle.AddMonths(1);
                        this.mois_actuel++;

                    }

                    else
                    {
                        if ((this.mois_actuel == 10) && (this.debordement == -1) && (this.somme_remboursée < this.montant))
                        {
                            pret_remboursable p = (pret_remboursable)this.MemberwiseClone();
                            p.cle = responsable.cle_a_affecter_pret_remboursable();
                            Dictionary<int, double> dico2 = new Dictionary<int, double>();
                            for (int i = 0; i < 10; i++)
                            {
                                dico2.Add(i, -1);
                            }
                            p.Etat = dico2;
                            this.debordement = p.cle;
                            this.mois_actuel++;
                            p.mois_actuel = 0;
                            p.somme_remboursée = this.somme_remboursée;
                            //p.paiement();
                            responsable.liste_pret_remboursable_provisoire.Add(p.cle, p);
                        }
                        else
                        {
                            if ((this.mois_actuel == 10) && (this.debordement != -1))
                            {
                                this.mois_actuel++;
                            }
                        }


                    }
                    if ((this.En_cours == 0) && (this.mois_actuel < 10))
                    {
                        this.etat.Remove(this.mois_actuel);
                        this.etat.Add(this.mois_actuel, 0);
                        this.mois_actuel++;
                        this.date_actuelle = this.date_actuelle.AddMonths(1);
                        this.en_cours = 0;
                    }
                    if ((this.somme_remboursée == this.montant) || ((this.somme_remboursée + 1 >= this.montant)))
                    {
                        int cpt = this.mois_actuel;
                        if (cpt < 10)
                        {
                            for (cpt = this.mois_actuel; cpt < 10; cpt++)
                            {
                                this.etat.Remove(cpt);
                                this.etat.Add(cpt, -1);
                            }
                        }
                        this.mois_actuel = 11;
                        this.date_actuelle = this.date_actuelle.AddMonths(-1);
                    }
                }
                else
                {
                    this.paiement_anticipé();
                }

                foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
                {
                    if ((this.debordement == kvp.Key))
                    {
                        kvp.Value.paiement();
                    }
                }
                foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
                {
                    if ((this.debordement == element.Key))
                    {
                        element.Value.paiement();
                    }
                }

            }

            this.En_cours = 1;
        }

        public void paiement_spécial(double cout)
        {
            responsable.pile_modifications.Add(new Modification(4, this.Cle));
            int difference = DateTime.Compare(DateTime.Now, this.Date_premier_paiment);
            if (difference > 0)
            {
                if ((this.montant - this.somme_remboursée) > cout)
                {
                    if ((this.en_cours == 1) && (this.mois_actuel < 10) && (this.somme_remboursée < this.montant))
                    {
                        this.etat.Remove(this.mois_actuel);
                        this.etat.Add(this.mois_actuel, cout);
                        responsable.tresor = responsable.tresor + cout;
                        this.somme_remboursée = this.somme_remboursée + cout;
                        this.date_actuelle = this.date_actuelle.AddMonths(1);
                        this.mois_actuel++;
                    }

                    else
                    {
                        if ((this.mois_actuel == 10) && (this.debordement == -1) && (this.somme_remboursée < this.montant))
                        {
                            pret_remboursable p = (pret_remboursable)this.MemberwiseClone();
                            p.cle = responsable.cle_a_affecter_pret_remboursable();
                            Dictionary<int, double> dico2 = new Dictionary<int, double>();
                            for (int i = 0; i < 10; i++)
                            {
                                dico2.Add(i, -1);
                            }
                            p.Etat = dico2;
                            this.debordement = p.cle;
                            this.mois_actuel++;
                            p.mois_actuel = 0;
                            p.somme_remboursée = this.somme_remboursée;
                            //p.paiement();
                            responsable.liste_pret_remboursable_provisoire.Add(p.cle, p);
                        }
                        else
                        {
                            if ((this.mois_actuel == 10) && (this.debordement != -1))
                            {
                                this.mois_actuel++;
                            }
                        }


                    }
                    if ((this.En_cours == 0) && (this.mois_actuel < 10))
                    {
                        this.etat.Remove(this.mois_actuel);
                        this.etat.Add(this.mois_actuel, 0);
                        this.mois_actuel++;
                        this.date_actuelle = this.date_actuelle.AddMonths(1);
                        this.en_cours = 0;
                    }
                    if ((this.somme_remboursée == this.montant) || ((this.somme_remboursée + 1 >= this.montant)))
                    {
                        int cpt = this.mois_actuel;
                        if (cpt < 10)
                        {
                            for (cpt = this.mois_actuel; cpt < 10; cpt++)
                            {
                                this.etat.Remove(cpt);
                                this.etat.Add(cpt, -1);
                            }
                        }
                        this.mois_actuel = 11;
                        this.date_actuelle = this.date_actuelle.AddMonths(-1);
                    }
                }
                else
                {
                    this.paiement_anticipé();
                }

                foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
                {
                    if ((this.debordement == kvp.Key))
                    {
                        kvp.Value.paiement_spécial(cout);
                    }
                }
                foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
                {
                    if ((this.debordement == element.Key))
                    {
                        element.Value.paiement_spécial(cout);
                    }
                }
            }
        }

        public void retardement()
        {
            responsable.pile_modifications.Add(new Modification(4, this.Cle));
            int difference = DateTime.Compare(DateTime.Now, this.Date_premier_paiment);
            if (difference > 0)
            {
                this.En_cours = 0;
                foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
                {
                    if (this.debordement == kvp.Key)
                    {
                        kvp.Value.retardement();
                    }
                }

                foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
                {
                    if ((this.debordement == kvp.Key))
                    {
                        kvp.Value.retardement();
                    }
                }
                foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
                {
                    if ((this.debordement == element.Key))
                    {
                        element.Value.retardement();
                    }
                }
            }
        }

        public void paiement_anticipé()
        {
            responsable.pile_modifications.Add(new Modification(4, this.Cle));
            int difference = DateTime.Compare(DateTime.Now, this.Date_premier_paiment);
            if (difference > 0)
            {
                int trace = -1;
                if ((this.mois_actuel < 10) && (this.somme_remboursée < this.montant))
                {
                    this.etat.Remove(this.mois_actuel);
                    this.etat.Add(this.mois_actuel, (this.montant - this.somme_remboursée));
                    responsable.tresor = responsable.tresor + (this.montant - this.somme_remboursée);
                    this.somme_remboursée = this.montant;
                    int cpt = this.mois_actuel;
                    for (cpt = this.mois_actuel + 1; cpt < 10; cpt++)
                    {
                        this.etat.Remove(cpt);
                        this.etat.Add(cpt, -1);
                    }
                    this.mois_actuel = 11;
                }
                else
                {
                    if ((this.mois_actuel == 10) && (this.debordement == -1) && (this.somme_remboursée < this.montant))
                    {
                        pret_remboursable p = (pret_remboursable)this.MemberwiseClone();
                        p.cle = responsable.cle_a_affecter_pret_remboursable();
                        trace = p.cle;
                        Dictionary<int, double> dico2 = new Dictionary<int, double>();
                        for (int i = 0; i < 10; i++)
                        {
                            dico2.Add(i, -1);
                        }
                        p.Etat = dico2;
                        this.debordement = p.cle;
                        this.mois_actuel++;
                        p.mois_actuel = 0;
                        p.somme_remboursée = this.somme_remboursée;
                        p.paiement_anticipé();
                        responsable.liste_pret_remboursable_provisoire.Add(p.cle, p);
                    }
                    else
                    {
                        if ((this.mois_actuel == 10) && (this.debordement != -1))
                        {
                            this.mois_actuel++;
                        }
                    }
                }

                foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
                {
                    if ((this.debordement == kvp.Key))
                    {
                        kvp.Value.paiement_anticipé();
                    }
                }
                foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
                {
                    if ((this.debordement == element.Key))
                    {
                        element.Value.paiement_anticipé();
                    }
                }
            }
        }

        public void paiement_plusieurs_mois(int nb_mois)
        {
            responsable.pile_modifications.Add(new Modification(4, this.Cle));
            int difference = DateTime.Compare(DateTime.Now, this.Date_premier_paiment);
            if (difference > 0)
            {
                if ((this.montant - this.somme_remboursée) > (this.montant / this.durée) * nb_mois)
                {
                    if ((this.en_cours == 1) && (this.mois_actuel < 10) && (this.somme_remboursée < this.montant))
                    {
                        this.etat.Remove(this.mois_actuel);
                        this.etat.Add(this.mois_actuel, (this.montant / this.durée) * nb_mois);
                        responsable.tresor = responsable.tresor + ((this.montant / this.durée) * nb_mois);
                        this.somme_remboursée = this.somme_remboursée + ((this.montant / this.durée) * nb_mois);
                        this.date_actuelle = this.date_actuelle.AddMonths(1);
                        this.mois_actuel++;
                    }

                    else
                    {
                        if ((this.mois_actuel == 10) && (this.debordement == -1) && (this.somme_remboursée < this.montant))
                        {
                            pret_remboursable p = (pret_remboursable)this.MemberwiseClone();
                            p.cle = responsable.cle_a_affecter_pret_remboursable();
                            Dictionary<int, double> dico2 = new Dictionary<int, double>();
                            for (int i = 0; i < 10; i++)
                            {
                                dico2.Add(i, -1);
                            }
                            p.Etat = dico2;
                            this.debordement = p.cle;
                            this.mois_actuel++;
                            p.mois_actuel = 0;
                            p.somme_remboursée = this.somme_remboursée;
                            responsable.liste_pret_remboursable_provisoire.Add(p.cle, p);
                        }
                        else
                        {
                            if ((this.mois_actuel == 10) && (this.debordement != -1))
                            {
                                this.mois_actuel++;
                            }
                        }


                    }
                    if ((this.En_cours == 0) && (this.mois_actuel < 10))
                    {
                        this.etat.Remove(this.mois_actuel);
                        this.etat.Add(this.mois_actuel, 0);
                        this.mois_actuel++;
                        this.date_actuelle = this.date_actuelle.AddMonths(1);
                        this.en_cours = 0;
                    }
                    if ((this.somme_remboursée == this.montant) || ((this.somme_remboursée + 1 >= this.montant)))
                    {
                        int cpt = this.mois_actuel;
                        if (cpt < 10)
                        {
                            for (cpt = this.mois_actuel; cpt < 10; cpt++)
                            {
                                this.etat.Remove(cpt);
                                this.etat.Add(cpt, -1);
                            }
                        }
                        this.mois_actuel = 11;
                        this.date_actuelle = this.date_actuelle.AddMonths(-1);
                    }
                }
                else
                {
                    this.paiement_anticipé();
                }

                foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
                {
                    if ((this.debordement == kvp.Key))
                    {
                        kvp.Value.paiement_plusieurs_mois(nb_mois);
                    }
                }
                foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable_provisoire)
                {
                    if ((this.debordement == element.Key))
                    {
                        element.Value.paiement_plusieurs_mois(nb_mois);
                    }
                }
            }
        }



        public pret_remboursable pere()
        {
            foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
            {
                if (kvp.Value.Debordement == this.cle)
                {
                    return kvp.Value;
                }
            }
            return null;
        }
        public void archiver() //archivage automatique apres un mois
        {
            int difference = DateTime.Compare(DateTime.Now, this.date_actuelle.AddDays(Int32.Parse(Window2.durée_avant_archivage_d) + (Int32.Parse(Window2.durée_avant_archivage_m) * 30)));
            if ((this.debordement == -1) && (this.montant == this.somme_remboursée) && (difference == 1) && (Window2.mode_archivage))
            {
                string observ;
                int cle = responsable.cle_a_affecter_archive();

                if (this.anticipé())
                {
                    observ = "paiement anticipé";

                }
                else
                {
                    observ = "paiement mensuel";
                }
                Archive a = new Archive(cle, this, observ, this.Date_actuelle, this.durée);
                responsable.liste_archives_provisoire.Add(responsable.cle_a_affecter_archive(), a);
                responsable.liste_archives.Add(cle, a);
                if (this.pere() != null)
                {
                    this.pere().archiver_pere(observ);
                }
            }

        }

        public void archiver_manuel()//archivage selon le voeux de l'utilisateur
        {
            if ((this.debordement == -1) && (this.montant == this.somme_remboursée))

            {
                string observ;
                int cle = responsable.cle_a_affecter_archive();

                if (this.anticipé())
                {
                    observ = "paiement anticipé";
                }
                else
                {
                    observ = "paiement mensuel";
                }
                Archive a = new Archive(cle, this, observ, this.Date_actuelle, this.durée);
                responsable.liste_archives_provisoire.Add(responsable.cle_a_affecter_archive(), a);
                responsable.liste_archives.Add(cle, a);
                if (this.pere() != null)
                {
                    this.pere().archiver_pere(observ);
                }
            }

        }

        public void archiver_manuel_dettes_effaces()//archivage selon le voeux de l'utilisateur
        {
            if (this.debordement == -1)
            {
                int cle = responsable.cle_a_affecter_archive();
                Archive a = new Archive(cle, this, "effacement des dettes", this.Date_actuelle, this.durée);
                responsable.liste_archives_provisoire.Add(responsable.cle_a_affecter_archive(), a);
                responsable.liste_archives.Add(cle, a);
                if (this.pere() != null)
                {
                    this.pere().archiver_pere("effacement des dettes");
                }
            }

        }
        public void archiver_pere(string observ)//archiver touts les peres d'un pret donné
        {
            int cle = responsable.cle_a_affecter_archive();
            Archive a = new Archive(cle, this, observ, this.Date_actuelle, this.durée);
            responsable.liste_archives_provisoire.Add(responsable.cle_a_affecter_archive(), a);
            responsable.liste_archives.Add(cle, a);
            if (this.pere() != null)
            {
                this.pere().archiver_pere(observ);
            }
        }
        public Boolean anticipé() //elle retourne vrai si le pret est payé de façon anticipé rah tsa3dna f l'observation de archive
        {
            int cpt = 0;
            foreach (double d in this.etat.Values)
            {
                if ((d == this.montant / this.durée) || (d == 0))
                {

                    cpt = cpt + 1;
                }

            }
            if (cpt != 10)
            {
                return true;
            }
            return false;
        }
        public void children()//va jusqu'au fils pour remonter dans l'archivage
        {
            int debord = this.debordement;
            if (debord != -1)
            {
                foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
                {
                    if (debord == element.Key)
                    {

                        element.Value.children();
                    }
                }
            }
            else
            {
                this.archiver_manuel();
            }

        }
        public void children_effacement_dettes()
        {
            int debord = this.debordement;
            if (debord != -1)
            {
                foreach (KeyValuePair<int, pret_remboursable> element in responsable.liste_pret_remboursable)
                {
                    if (debord == element.Key)
                    {
                        element.Value.children_effacement_dettes();
                    }
                }
            }
            else
            {
                int cpt = this.mois_actuel;
                for (cpt = this.mois_actuel; cpt < 10; cpt++)
                {
                    this.etat.Remove(cpt);
                    this.etat.Add(cpt, 0);


                }
                this.mois_actuel = 11;
                this.archiver_manuel_dettes_effaces();
            }
        }
        public bool isPere()
        {
            bool b = true;
            foreach (KeyValuePair<int, pret_remboursable> kvp in responsable.liste_pret_remboursable)
            {
                if (this.cle == kvp.Value.Debordement)
                {
                    b = false;
                    break;
                }
            }
            return b;
        }

        public pret_remboursable getFils()
        {
            if (this.debordement == -1)
            {
                return this;
            }
            else
            {
                return responsable.liste_pret_remboursable[this.debordement];
            }
        }

        public override string prem_paiment()
        {
            return (this.Date_premier_paiment.ToString());
        }
        public override string fin_paiement()
        {
            return (this.Date_actuelle.ToString());
        }
        public override string somme_rembours()
        {
            return (this.somme_remboursée.ToString());
        }
    }
}