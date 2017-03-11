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
    public partial class AjouterSujet : Form
    {
        int nextTop;
        int nextLeft;
        int i = 0;
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public AjouterSujet()
        {
            InitializeComponent();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Sujets suj = new Sujets();
            suj.Show();
            this.Close();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (i < 6)
            {
                i++;
                TextBox t = new TextBox();
                t.Top = nextTop;
                t.Left = nextLeft;
                t.Width = 170;
                t.Name = "txt" + i.ToString();
                nextTop += 40;
                nextLeft += 00;
                panel.Controls.Add(t);
            }
            else
            {
                btnPlus.Enabled = false;
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            String idAjoute;
            if (txtSujet.Text != "" && txtDesc.Text != "" && (rdFacile.Checked || rdMoyen.Checked || rdDifficile.Checked))
            {
                String niveau = "";
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
                OleDbCommand ajout = new OleDbCommand("INSERT INTO Sujets ([Titre Du Sujet],[Description Du Sujet],[Difficulté]) values (\"" + txtSujet.Text + "\",\"" + txtDesc.Text + "\",'" + niveau + "')", cnx);
                try
                {
                    int nb = ajout.ExecuteNonQuery();
                    OleDbCommand num = new OleDbCommand("Select idSujet from Sujets where [Titre Du Sujet]=\"" + txtSujet.Text + "\" AND [Description Du Sujet]=\"" + txtDesc.Text + "\" AND [Difficulté]='" + niveau + "'",cnx);
                    OleDbDataReader rd = num.ExecuteReader();
                    while (rd.Read())
                    {
                        idAjoute = rd.GetValue(0).ToString();
                        if (panel.Controls.Count > 0)
                        {
                            for (int j = 0; j < panel.Controls.Count; j++)
                            {
                                if ((panel.Controls[j]).GetType() == typeof(TextBox) && panel.Controls[j].Text != "")
                                {
                                    OleDbCommand tach = new OleDbCommand("INSERT INTO Taches(Tache,idSujet) VALUES (\"" + panel.Controls[j].Text + "\"," + idAjoute + ")", cnx);
                                    int nb2 = tach.ExecuteNonQuery();
                                }
                                else
                                {
                                    this.errorProvider.SetError(panel.Controls[j], "Ce champ est Vide ");
                                    MessageBox.Show("Le Champ est Vide");
                                    
                                }
                            }
                            MessageBox.Show("Le Sujet et Ses Tâches Sont Ajouté");
                        }
                        else
                        {
                            MessageBox.Show("Sujet Ajouté \nMais Vous N'avez Pas Ajouté Les Tâches ! ");
                        }
                    }
                }
                catch(OleDbException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            else
            {
                MessageBox.Show("Vous Devez Remplir Tous Les Champs !");
            }
        }

        private void AjouterSujet_FormClosing(object sender, FormClosingEventArgs e)
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

        private void AjouterSujet_Load(object sender, EventArgs e)
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
    }
}
