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
    public partial class Stagiaires : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        OleDbDataAdapter adapt;
        DataSet dts;
        DataTable dt;
        //DataRow dtr;
        OleDbCommandBuilder cb;
        int index;
        public Stagiaires()
        {
            InitializeComponent();
        }

        private void btnRetour_Click(object sender, EventArgs e)
        {
            Menu_Principal menu = new Menu_Principal();
            menu.Show();
            this.Close();
        }

        private void Stagiaires_Load(object sender, EventArgs e)
        {
            try
            {
                cnx.Open();
                OleDbCommand cmd = new OleDbCommand("select CIN,Nom,Prenom,[Date De Naissance],[Adresse Email],Institut,Specialite from Stagiaires", cnx);
                dts = new DataSet();
                adapt = new OleDbDataAdapter(cmd);
                adapt.Fill(dts, "Stagiaires");
                dt = dts.Tables["Stagiaires"];
                dgvStagiares.DataSource = dt;
                dgvStagiares.Columns[0].Width = 60;
                dgvStagiares.Columns[1].Width = 80;
                dgvStagiares.Columns[2].Width = 79;
                dgvStagiares.Columns[3].Width = 70;
                dgvStagiares.Columns[4].Width = 150;
                dgvStagiares.Columns[5].Width = 48;
                dgvStagiares.Columns[6].Width = 55;


            }
            catch(OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = ("prenom like '" + textBox1.Text + "%'");
            dgvStagiares.DataSource = dv;
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Voulez-Vous Vraiment Supprimer Ce Stagiaire ?" , "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    index = dgvStagiares.CurrentRow.Index;
                    String ch = dt.Rows[index][0].ToString();
                    OleDbCommand cmd = new OleDbCommand("delete from Stagiaires where cin=\'" + ch + "\'", cnx);
                    int nb = cmd.ExecuteNonQuery();
                    dts.Clear();
                    adapt.Fill(dts, "Stagiaires");
                    dt = dts.Tables["Stagiaires"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void dgvStagiares_Click(object sender, EventArgs e)
        {
            index = dgvStagiares.CurrentRow.Index;
            if(index >= 0)
            {
                btnModifier.Enabled = true;
                btnSupprimer.Enabled = true;
            }
            else
            {
                btnModifier.Enabled = false;
                btnSupprimer.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                cb = new OleDbCommandBuilder(adapt);
                adapt.Update(dts,"Stagiaires");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            index = dgvStagiares.CurrentRow.Index;
            String cin = dt.Rows[index][0].ToString();
            String nom = dt.Rows[index][1].ToString();
            String prenom = dt.Rows[index][2].ToString();
            String date = dt.Rows[index][3].ToString();
            String email = dt.Rows[index][4].ToString();
            String institut = dt.Rows[index][5].ToString();
            String spec = dt.Rows[index][6].ToString();
            ModificationStagiaires modif = new ModificationStagiaires(cin,nom,prenom,date,email,institut,spec);
            modif.Show();
            this.Close();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterStagiaires ajout = new AjouterStagiaires();
            ajout.Show();
            this.Close();
        }

        private void Stagiaires_FormClosing(object sender, FormClosingEventArgs e)
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
