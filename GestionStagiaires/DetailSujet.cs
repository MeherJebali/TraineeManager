using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;

namespace GestionStagiaires
{
    public partial class DetailSujet : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        public DetailSujet(String idSujet, String titre, String desc, String dific)
        {
            InitializeComponent();
            lblTitre.Text = titre;
            txtDesc.Text = desc;
            txtId.Text = idSujet;
            lblDific.Text = dific;
        }

        private void DetailSujet_Load(object sender, EventArgs e)
        {
            try
            {
                cnx.Open();
                OleDbCommand cmd = new OleDbCommand("Select Tache From Taches WHERE idSujet=" + txtId.Text + "", cnx);
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    lstTaches.Items.Add("•" + rd.GetValue(0).ToString());
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

        private void btnGenerer_Click(object sender, EventArgs e)
        {
            try
            {
                Document doc = new Document(iTextSharp.text.PageSize.A4);
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(@"..\..\..\Resources\"+lblTitre.Text.ToString()+".pdf", FileMode.Create));
                doc.Open();
                var logo = iTextSharp.text.Image.GetInstance(@"..\..\..\Resources\Zitouna.png");
                logo.SetAbsolutePosition(35, 750);
                Paragraph banque = new Paragraph("                                     Banque Zitouna");
                banque.Font.Size = 20;
                Paragraph adresse = new Paragraph("                                                                                                                            2. Boulevard Qualité de la Vie 2015 le Kram - Tunisie\n                                                                                                                            contact@banquezitouna.com | Tél. : 81 10 55 55");
                adresse.Font.Size = 6;
                Paragraph ligne1 = new Paragraph("______________________________________________________________________________");
                Paragraph sujet = new Paragraph("\n                            "+lblTitre.Text.ToString());
                sujet.Font.Size = 23;
                Paragraph ligne2 = new Paragraph("\n______________________________________________________________________________");
                Paragraph p1 = new Paragraph("\n\n\n\nDescription : ");
                p1.Font.Size = 14;
                Paragraph desc = new Paragraph("    "+txtDesc.Text.ToString());
                Paragraph p2 = new Paragraph("\n\n\n\nNiveau de Difficulté : ");
                p2.Font.Size = 14;
                Paragraph dif = new Paragraph("    " + lblDific.Text.ToString());
                Paragraph p3 = new Paragraph("\n\n\n\nLes Tâches : ");
                p3.Font.Size = 14;
                OleDbCommand cmd = new OleDbCommand("Select Tache From Taches WHERE idSujet=" + txtId.Text + "", cnx);
                OleDbDataReader rd = cmd.ExecuteReader();
                List list = new List(List.ORDERED);
                while (rd.Read())
                {
                    list.Add("    " + rd.GetValue(0).ToString());

                }
                doc.Add(logo);
                doc.Add(banque);
                doc.Add(adresse);
                doc.Add(ligne1);
                doc.Add(sujet);
                doc.Add(ligne2);
                doc.Add(p1);
                doc.Add(desc);
                doc.Add(p2);
                doc.Add(dif);
                doc.Add(p3);
                doc.Add(list);
                doc.Close();
                Process mnfichier = new Process();
                mnfichier.StartInfo.FileName = lblTitre.Text.ToString() + ".pdf";
                mnfichier.StartInfo.WorkingDirectory = @"..\..\..\Resources\";
                mnfichier.Start(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailSujet_FormClosing(object sender, FormClosingEventArgs e)
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
