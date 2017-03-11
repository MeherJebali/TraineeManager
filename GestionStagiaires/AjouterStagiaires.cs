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
    public partial class AjouterStagiaires : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public AjouterStagiaires()
        {
            InitializeComponent();
            //txtCin.Focused = true;
            txtCin.Focus();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Stagiaires menu = new Stagiaires();
            menu.Show();
            this.Close();
        }

        private Boolean NotExiste()
        {
            bool verif = false;
            OleDbCommand cmd = new OleDbCommand("select count(*) from Stagiaires where CIN='" + txtCin.Text + "'", cnx);
            try
            {
                int nb = (int)cmd.ExecuteScalar();
                if (nb == 0)
                {
                    verif = true;
                }
                else
                    verif = false;
            }
            catch (OleDbException exp)
            {
                MessageBox.Show("Problème d'éxécution " + exp.Message);
            }
            return verif;
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (txtCin.Text != "" && txtNom.Text != "" && txtPrenom.Text != "" && txtDateN.Text != "" && txtEmail.Text != "" && txtInstitut.Text != "" && txtSpecialite.Text != "")
            {
                if (NotExiste())
                {
                    OleDbCommand cmd = new OleDbCommand("insert into Stagiaires (CIN,Nom,Prenom,[Date De Naissance],[Adresse Email],Institut,Specialite) values ('" + txtCin.Text + "','" + txtNom.Text + "','" + txtPrenom.Text + "','" + txtDateN.Text + "','" + txtEmail.Text + "','" + txtInstitut.Text + "','" + txtSpecialite.Text + "')", cnx);
                    try
                    {

                        int nb = cmd.ExecuteNonQuery();
                        MessageBox.Show("Stagiaire Ajouté ");
                    }
                    catch (OleDbException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    this.errorProvider.SetError(txtCin, "L'étudiant Avec Le Cin: " + txtCin.Text + " Existe Déjà");
                }
            }
            else
            {
                MessageBox.Show("Vous Devez Sasir Tous Les Champs");
            }
        }

        private void AjouterStagiaires_Load(object sender, EventArgs e)
        {
            txtCin.Focus();
            try
            {
                cnx.Open();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex emailValid = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (txtEmail.Text.Length > 0)
            {
                if (!emailValid.IsMatch(txtEmail.Text))
                {
                    this.errorProvider.SetError(txtEmail, "Veuillez Vérifier le Format de L'Email");
                    txtEmail.SelectAll();
                    e.Cancel = true;
                }
                else
                {
                    this.errorProvider.Clear();
                }
            }
            else
            {
                this.errorProvider.SetError(txtEmail, "Veuillez Saisir L'Email SVP");
            }

        }

        private void txtCin_Validating(object sender, CancelEventArgs e)
        {
            int cin;
            if(txtCin.Text == "")
            {
                this.errorProvider.SetError(txtCin, "Ce Champ Est Obligatoire ");
            }
            else if (!int.TryParse(txtCin.Text, out cin))
            {
                e.Cancel = true;
                this.errorProvider.SetError(txtCin, "Le Numéro de La Carte d'Identité doit être de type Entier");
            }
            else
            {
                this.errorProvider.Clear();
            }
        }

        private void AjouterStagiaires_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cnx.Close();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
