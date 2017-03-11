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
    public partial class ModificationEncadreurs : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public ModificationEncadreurs(String id, String nom, String prenom, String email, String poste, String dep)
        {
            InitializeComponent();
            txtId.Text = id;
            txtNom.Text = nom;
            txtPrenom.Text = prenom;
            txtEmail.Text = email;
            txtPoste.Text = poste;
            cboDep.SelectedValue = dep;
            cboDep.SelectedItem = dep;
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Encadreurs menu = new Encadreurs();
            menu.Show();
            this.Close();
        }

        private void ModificationEncadreurs_Load(object sender, EventArgs e)
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

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (txtId.Text != "" && txtNom.Text != "" && txtPrenom.Text != "" && txtPoste.Text != "" && txtEmail.Text != "" && cboDep.Text != "")
            {
                OleDbCommand cmd = new OleDbCommand("Update Encadreurs set Nom='" + txtNom.Text + "',Prenom='" + txtPrenom.Text + "', [Adresse Email]='" + txtEmail.Text + "',Poste='" + txtPoste.Text + "',[Département]='" + cboDep.Text + "' WHERE Identifiant=" + txtId.Text + "", cnx);
                try
                {
                    int nb = cmd.ExecuteNonQuery();
                    MessageBox.Show("Les Données de l'Encadreur ont été Modifié ");
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void cboDep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPrenom_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNom_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Click(object sender, EventArgs e)
        {

        }

        private void txtPoste_TextChanged(object sender, EventArgs e)
        {

        }

        private void ModificationEncadreurs_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
