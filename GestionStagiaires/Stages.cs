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
    public partial class Stages : Form
    {
        OleDbDataAdapter adapt;
        DataSet dts;
        DataTable dt;
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        int index;
        public Stages()
        {
            InitializeComponent();
        }

        private void Stages_Load(object sender, EventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT stg.idStage,sj.[Titre Du Sujet] as Sujet,st.Prenom+' '+st.Nom as Stagiaire,en.Prenom+' '+en.Nom as Encadreur,stg.Type as Type,stg.[Date de Début] as [Date de Début],stg.[Durée]as Durée FROM Sujets sj,Stagiaires st,Encadreurs en,Stages stg WHERE sj.idSujet=stg.idSujet AND st.CIN=stg.CIN AND en.Identifiant=stg.Identifiant", cnx);
            try
            {
                cnx.Open();
                dts = new DataSet();
                adapt = new OleDbDataAdapter(cmd);
                adapt.Fill(dts, "Stages");
                dt = dts.Tables["Stages"];
                dgvStages.DataSource = dt;
                dgvStages.Columns[0].Visible = false;
                dgvStages.Columns[1].Width = 130;
                dgvStages.Columns[2].Width = 90;
                dgvStages.Columns[3].Width = 90;
                dgvStages.Columns[4].Width = 130;
                dgvStages.Columns[5].Width = 70;
                dgvStages.Columns[6].Width = 49;
            }
            catch(OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRetour_Click(object sender, EventArgs e)
        {
            Menu_Principal menu = new Menu_Principal();
            menu.Show();
            this.Close();
        }

        private void dgvStages_Click(object sender, EventArgs e)
        {
            try
            {
                index = dgvStages.CurrentRow.Index;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = ("Sujet like '" + textBox1.Text + "%'");
            dgvStages.DataSource = dv;
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Voulez-Vous Vraiment annuler Ce Stage ?", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    String cin = "";
                    index = dgvStages.CurrentRow.Index;
                    String ch = dt.Rows[index][0].ToString();
                    OleDbCommand trouver = new OleDbCommand("SELECT st.CIN FROM Stagiaires st,Stages stg WHERE st.CIN=stg.CIN AND stg.idStage=" + ch, cnx);
                    OleDbDataReader rd = trouver.ExecuteReader();
                    while (rd.Read())
                    {
                        cin = rd.GetValue(0).ToString();
                    }

                    OleDbCommand del = new OleDbCommand("delete from Stages where idStage=" + ch, cnx);
                    OleDbCommand updt = new OleDbCommand("UPDATE Stagiaires set Astg=false WHERE CIN='" + cin + "'", cnx);
                    int nb = del.ExecuteNonQuery();
                    int nb2 = updt.ExecuteNonQuery();
                    dts.Clear();
                    adapt.Fill(dts, "Stages");
                    dt = dts.Tables["Stages"];
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            ModificationStages mdfstg = new ModificationStages();
            mdfstg.Show();
            this.Close();
        }

        private void Stages_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterStage add = new AjouterStage();
            add.Show();
            this.Close();
        }
    }
}
