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
    public partial class ModificationSujets : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public ModificationSujets(String idSujet,String titre,String desc)
        {
            InitializeComponent();
            txtIdSujet.Text = idSujet;
            txtSujet.Text = titre;
            txtDesc.Text = desc;
            
        }

        private void ModificationSujets_Load(object sender, EventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand("select [Difficulté] from Sujets WHERE idSujet=" + txtIdSujet.Text + "", cnx);
            try
            {
                cnx.Open();
                OleDbDataReader rd = cmd.ExecuteReader();
                while(rd.Read())
                {
                    String dific = rd.GetString(0);
                    if (dific.Equals("Facile"))
                    {
                        rdFacile.Checked = true;
                    }
                    else if (dific.Equals("Moyen"))
                    {
                        rdMoyen.Checked = true;
                    }
                    else if (dific.Equals("Difficile"))
                    {
                        rdDifficile.Checked = true;
                    }
                    else
                    {
                        rdFacile.Checked = false;
                        rdMoyen.Checked = false;
                        rdDifficile.Checked = false;
                    }

                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Sujets menu = new Sujets();
            menu.Show();
            this.Close();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            String niveau="";
            if (rdFacile.Checked)
            {
                niveau = "Facile";
            }
            else if (rdMoyen.Checked)
            {
                niveau = "Moyen";
            }
            else
            {
                niveau = "Difficile";
            }
            if(txtSujet.Text != "" && txtDesc.Text != "")
            {
                OleDbCommand cmd = new OleDbCommand("UPDATE Sujets set [Titre Du Sujet]=\"" + txtSujet.Text + "\" , [Description Du Sujet]=\"" + txtDesc.Text + "\" , [Difficulté]='" + niveau + "' WHERE idSujet=" + txtIdSujet.Text + "", cnx);
                try
                {
                    int nb = cmd.ExecuteNonQuery();
                    MessageBox.Show("Les Données Du Sujet ont été Modifié ");
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vous Devez Remplir Tous les Champs");
            }
        }

        private void ModificationSujets_FormClosing(object sender, FormClosingEventArgs e)
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
