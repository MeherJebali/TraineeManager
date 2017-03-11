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
    public partial class ModificationStagiaires : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public ModificationStagiaires(String cin,String nom,String prenom,String date,String email,String institut,String spec)
        {
            InitializeComponent();
            txtCin.Text = cin;
            txtNom.Text = nom;
            txtPrenom.Text = prenom;
            txtDateN.Text = date;
            txtDateN.Text = date;
            txtEmail.Text = email;
            txtInstitut.Text = institut;
            txtSpecialite.Text = spec;

        }

        private void ModificationStagiaires_Load(object sender, EventArgs e)
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

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Stagiaires menu = new Stagiaires();
            menu.Show();
            this.Close();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (txtCin.Text != "" && txtNom.Text != "" && txtPrenom.Text != "" && txtDateN.Text!="" && txtEmail.Text!="" && txtInstitut.Text!="" && txtSpecialite.Text!="")
            {
                OleDbCommand cmd = new OleDbCommand("Update Stagiaires set Nom='" + txtNom.Text + "',Prenom='" + txtPrenom.Text + "', [Date De Naissance]='" + txtDateN.Text + "',[Adresse Email]='" + txtEmail.Text + "',Institut='" + txtInstitut.Text + "',Specialite='" + txtSpecialite.Text + "' WHERE cin='" + txtCin.Text + "'", cnx);
                try
                {
                    int nb = cmd.ExecuteNonQuery();
                    MessageBox.Show("Stagiaire Modifié ");
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vous Devez Remplir tous les champs");
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

        private void ModificationStagiaires_FormClosing(object sender, FormClosingEventArgs e)
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
