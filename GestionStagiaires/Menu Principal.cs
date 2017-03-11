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
    public partial class Menu_Principal : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public Menu_Principal()
        {
            InitializeComponent();
        }

        private void Menu_Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Menu_Principal_Load(object sender, EventArgs e)
        {
            lblDate.Text = "Aujourd'hui : " + DateTime.Now.ToString("dd/MM/yyyy");
            timer_Tick(sender, e);
            try
            {
                cnx.Open();
                OleDbCommand cmd1 = new OleDbCommand("select count(*) from Stagiaires",cnx);
                OleDbCommand cmd2 = new OleDbCommand("select count(*) from Encadreurs", cnx);
                OleDbCommand cmd3 = new OleDbCommand("select count(*) from Sujets", cnx);
                OleDbCommand cmd4 = new OleDbCommand("select count(*) from Stages", cnx);
                int nb1 = (int)cmd1.ExecuteScalar();
                int nb2 = (int)cmd2.ExecuteScalar();
                int nb3 = (int)cmd3.ExecuteScalar();
                int nb4 = (int)cmd4.ExecuteScalar();
                lblnbStgr.Text = nb1.ToString();
                lblnbEnc.Text = nb2.ToString();
                lblnbSuj.Text = nb3.ToString();
                lblnbStages.Text = nb4.ToString();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Start();
            lblHeure.Text = "Heure Actuelle : " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnStagiaires_Click(object sender, EventArgs e)
        {
            Stagiaires stgiares = new Stagiaires();
            stgiares.Show();
            this.Hide();
        }

        private void btnEncadreur_Click(object sender, EventArgs e)
        {
            Encadreurs encdr = new Encadreurs();
            encdr.Show();
            this.Hide();
        }

        private void btnSujets_Click(object sender, EventArgs e)
        {
            Sujets sujets = new Sujets();
            sujets.Show();
            this.Hide();
        }

        private void btnStages_Click(object sender, EventArgs e)
        {
            Stages stg = new Stages();
            stg.Show();
            this.Hide();
        }

        private void Menu_Principal_FormClosing(object sender, FormClosingEventArgs e)
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
