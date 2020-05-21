using System;
using System.Collections.Generic;
using System.Text;


namespace WpfApp2
{
    public class Modification
        {
        private int num_dic;
        private int clé_element_modifié;

        public Modification(int num_d, int clé)
        {
            this.num_dic = num_d;
            this.clé_element_modifié = clé;
        }

        public int Dic_modifié
        {
            get { return this.num_dic; }
            set { this.num_dic = value; }
        }

        public int Clé_element_modifié
        {
            get { return this.clé_element_modifié; }
            set { this.clé_element_modifié = value; }
        }
    }
}