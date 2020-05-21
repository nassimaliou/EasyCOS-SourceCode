using System;
using System.Windows;

namespace WpfTutorialSamples.Dialogs
{
	public partial class InputDialogSample : Window
	{
		public bool sortie = false;
		public bool aff = false;
		public double mo = 0;
		WpfApp2.pret_remboursable pr;

		//Class principale de la fenetre d'ajout de mail de l'employé

		public InputDialogSample(WpfApp2.pret_remboursable p, double m, string question, string defaultAnswer = "")
		{
			InitializeComponent();			
			lblQuestion.Content = question;
			txtAnswer.Text = defaultAnswer;
			mo = m;
			pr = p;
		}


		//methodes de manupulation de l'interface
		private void btnDialogOk_Click(object sender, RoutedEventArgs e)
		{

			pr.Employé.Email = txtAnswer.Text;
			WpfApp2.responsable.pile_modifications.Add(new WpfApp2.Modification(1, pr.Employé.Cle));

			WpfApp2.responsable.Envoi_mail(pr, mo);
			aff = true;
			this.Close();
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			txtAnswer.SelectAll();
			txtAnswer.Focus();
		}

		public string Answer
		{
			get { return txtAnswer.Text; }
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.sortie = true;
			this.Close();
		}
	}
}
