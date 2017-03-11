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
    public partial class AjouterEncadreur : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public AjouterEncadreur()
        {
            InitializeComponent();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Encadreurs menu = new Encadreurs();
            menu.Show();
            this.Close();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (txtNom.Text != "" && txtPrenom.Text != "" && txtPoste.Text != "" && txtEmail.Text != "" && cboDep.Text != "")
            {
                if (NotExiste())
                {
                    OleDbCommand cmd = new OleDbCommand("insert into Encadreurs (Nom,Prenom,[Adresse Email],Poste,[Département]) values ('" + txtNom.Text + "','" + txtPrenom.Text + "','" +  txtEmail.Text + "','" + txtPoste.Text + "','" + cboDep.Text + "')", cnx);
                    try
                    {

                        int nb = cmd.ExecuteNonQuery();
                        MessageBox.Show("Encadreur Ajouté ");
                    }
                    catch (OleDbException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    this.errorProvider.SetError(txtEmail, "L'Encadreur Avec L'Adresse Email: " + txtEmail.Text + " Existe Déjà");
                }
            }
            else
            {
                MessageBox.Show("Vous Devez Remplir tous les champs");
            }
        }

        private void AjouterEncadreur_Load(object sender, EventArgs e)
        {
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

        private Boolean NotExiste()
        {
            bool verif = false;
            OleDbCommand cmd = new OleDbCommand("select count(*) from Encadreurs where [Adresse Email]='" + txtEmail.Text + "'", cnx);
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

        private void AjouterEncadreur_FormClosing(object sender, FormClosingEventArgs e)
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
