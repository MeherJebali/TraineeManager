using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace GestionStagiaires
{
    public partial class Authentification : Form
    {
        public Authentification()
        {
            InitializeComponent();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            txtLogin.Clear();
            txtPass.Clear();
            errorProvider.Clear();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool user = VerifLogin();
            bool pass = VerifPass();
            if (user & pass)
            {
                Menu_Principal menu = new Menu_Principal();
                menu.Show();
                this.Hide();
            }
        }
        private bool VerifLogin()
        {
            bool login = true;
            if(txtLogin.Text.Equals(""))
            {
                this.errorProvider.SetError(txtLogin, "Veuillez Saisir Votre Identifiant");
                login = false;
            }
            else if (!(txtLogin.Text.Equals("admin")))
            {
                this.errorProvider.SetError(txtLogin, "Veuillez Vérifier L'identifiant SVP");
                login = false;
            }
            else
            {
                this.errorProvider.Clear();
                login = true;
            }
            return login;
        }
        private bool VerifPass()
        {
            bool pass = true;
            if (txtPass.Text.Equals(""))
            {
                this.errorProvider.SetError(txtPass, "Veuillez Saisir Votre Mot de passe");
                pass = false;
            }
            else if (!(txtPass.Text.Equals("admin")))
            {
                this.errorProvider.SetError(txtPass, "Veuillez Vérifier Votre Mot De Passe");
                pass = false;
            }
            else
            {
                this.errorProvider.Clear();
                pass = true;
            }
            return pass;
        }

        private void txtLogin_Validating(object sender, CancelEventArgs e)
        {
            VerifLogin();
        }

        private void txtPass_Validating(object sender, CancelEventArgs e)
        {
            VerifPass();
        }
    }
}
