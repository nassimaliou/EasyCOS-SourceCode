using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class pret_non_remboursable : Prets
    {
        public pret_non_remboursable(int cle_, Employé employé, Type_pret type, string motif, int num_pv, DateTime date_pv, double montant, DateTime date_demande, string montant_lettre) : base(cle_, employé, type, motif, num_pv, date_pv, montant, date_demande, montant_lettre)
        {
        }
        public override void affiche_attributs_complets()
        {
            this.affiche_attribus();
        }

        public override int Debor()
        {
            return -1;
        }

        public void archiver(string observation = "Aucune observation indroduite par l'utilisateur.")
        {
            int difference = DateTime.Compare(DateTime.Now, this.Date_pv.AddDays(int.Parse(Window2.durée_avant_archivage_d) + (int.Parse(Window2.durée_avant_archivage_m) * 30)));
            if ( (difference > 0) && (Window2.mode_archivage))
            {
                int cle = responsable.cle_a_affecter_archive();
                Archive a = new Archive(cle, this, observation, this.Date_pv, -1);
                responsable.liste_archives_provisoire.Add(responsable.cle_a_affecter_archive(), a);
                responsable.liste_archives.Add(cle, a);
            }
        }
        public void archiver_manuel(string observation = "Aucune observation indroduite par l'utilisateur.")
        {
            int cle = responsable.cle_a_affecter_archive();
            Archive a = new Archive(cle, this, observation, this.Date_pv, -1);
            responsable.liste_archives_provisoire.Add(responsable.cle_a_affecter_archive(), a);
            responsable.liste_archives.Add(cle, a);
        }

        public override string prem_paiment()
        {
            return ("                         / ");
        }

        public override string fin_paiement()
        {
            return ("                         / ");
        }

        public override string somme_rembours()
        {
            return ("   0    ");
        }
    }
}