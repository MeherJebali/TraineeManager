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
    public partial class Sujets : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        OleDbDataAdapter adapt;
        DataSet dts;
        DataTable dt;
        int index;
        public Sujets()
        {
            InitializeComponent();
        }

        private void btnRetour_Click(object sender, EventArgs e)
        {
            Menu_Principal menu = new Menu_Principal();
            menu.Show();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Sujets_Load(object sender, EventArgs e)
        {
            try
            {
                cnx.Open();
                OleDbCommand cmd = new OleDbCommand("select * from Sujets", cnx);
                dts = new DataSet();
                adapt = new OleDbDataAdapter(cmd);
                adapt.Fill(dts, "Sujets");
                dt = dts.Tables["Sujets"];
                dgvSujets.DataSource = dt;
                dgvSujets.Columns[0].Visible = false;
                dgvSujets.Columns[1].Width = 120;
                dgvSujets.Columns[2].Width = 387;
                dgvSujets.Columns[3].Width = 53;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = ("[Titre Du Sujet] like '" + textBox1.Text + "%'");
            dgvSujets.DataSource = dv;
        }

        private void dgvSujets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            index = dgvSujets.CurrentRow.Index;
            String idSujet = dt.Rows[index][0].ToString();
            String titre = dt.Rows[index][1].ToString();
            String desc = dt.Rows[index][2].ToString();
            String dific = dt.Rows[index][3].ToString();
            DetailSujet dtl = new DetailSujet(idSujet,titre,desc,dific);
            dtl.Show();
            this.Close();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Voulez-Vous Vraiment Supprimer Ce Sujet ?", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    index = dgvSujets.CurrentRow.Index;
                    String ch = dt.Rows[index][0].ToString();
                    OleDbCommand cmd = new OleDbCommand("delete from Sujets where idSujet=" + ch + "", cnx);
                    OleDbCommand cmd2 = new OleDbCommand("delete from Taches where idSujet=" + ch + "", cnx);
                    int nb = cmd.ExecuteNonQuery();
                    int nb2 = cmd2.ExecuteNonQuery();
                    dts.Clear();
                    adapt.Fill(dts, "Sujets");
                    dt = dts.Tables["Sujets"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dgvSujets_Click(object sender, EventArgs e)
        {
            try
            {

                index = dgvSujets.CurrentRow.Index;
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

        private void Sujets_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnModifier_Click(object sender, EventArgs e)
        {
            index = dgvSujets.CurrentRow.Index;
            String idSujet = dt.Rows[index][0].ToString();
            String titre = dt.Rows[index][1].ToString();
            String desc = dt.Rows[index][2].ToString();
            ModificationSujets modifsujet = new ModificationSujets(idSujet, titre, desc);
            modifsujet.Show();
            this.Close();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterSujet ajt = new AjouterSujet();
            ajt.Show();
            this.Close();
        }
    }
}
