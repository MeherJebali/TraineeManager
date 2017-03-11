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
    public partial class Encadreurs : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        OleDbDataAdapter adapt;
        DataSet dts;
        DataTable dt;
        //DataRow dtr;
        //OleDbCommandBuilder cb;
        int index;
        public Encadreurs()
        {
            InitializeComponent();
        }

        private void btnRetour_Click(object sender, EventArgs e)
        {
            Menu_Principal menu = new Menu_Principal();
            menu.Show();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = ("prenom like '" + textBox1.Text + "%'");
            dgvStagiares.DataSource = dv;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Voulez-Vous Vraiment Supprimer Cet Encadreur ?", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    index = dgvStagiares.CurrentRow.Index;
                    String ch = dt.Rows[index][0].ToString();
                    OleDbCommand cmd = new OleDbCommand("delete from Encadreurs where Identifiant=" + ch, cnx);
                    int nb = cmd.ExecuteNonQuery();
                    dts.Clear();
                    adapt.Fill(dts, "Encadreurs");
                    dt = dts.Tables["Encadreurs"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            index = dgvStagiares.CurrentRow.Index;
            String id = dt.Rows[index][0].ToString();
            String nom = dt.Rows[index][1].ToString();
            String prenom = dt.Rows[index][2].ToString();
            String email = dt.Rows[index][3].ToString();
            String poste = dt.Rows[index][4].ToString();
            String dep = dt.Rows[index][5].ToString();
            ModificationEncadreurs modif = new ModificationEncadreurs(id, nom, prenom, email, poste, dep);
            modif.Show();
            this.Close();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterEncadreur ajt = new AjouterEncadreur();
            ajt.Show();
            this.Close();
        }

        private void dgvStagiares_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Encadreurs_Load(object sender, EventArgs e)
        {
            try
            {
                cnx.Open();
                OleDbCommand cmd = new OleDbCommand("select * from Encadreurs", cnx);
                dts = new DataSet();
                adapt = new OleDbDataAdapter(cmd);
                adapt.Fill(dts, "Encadreurs");
                dt = dts.Tables["Encadreurs"];
                dgvStagiares.DataSource = dt;
                dgvStagiares.Columns[0].Visible = false;
                //dgvStagiares.Columns[0].Width = 0;
                dgvStagiares.Columns[1].Width = 59;
                dgvStagiares.Columns[2].Width = 69;
                dgvStagiares.Columns[3].Width = 180;
                dgvStagiares.Columns[4].Width = 180;
                dgvStagiares.Columns[5].Width = 80;


            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvStagiares_Click(object sender, EventArgs e)
        {
            try
            {
                index = dgvStagiares.CurrentRow.Index;
                if (index >= 0)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Encadreurs_FormClosing(object sender, FormClosingEventArgs e)
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
