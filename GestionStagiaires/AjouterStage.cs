using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Net.Mail;

namespace GestionStagiaires
{
    public partial class AjouterStage : Form
    {
        OleDbConnection cnx = new OleDbConnection(Properties.Settings.Default.cnx);
        DataSet dts;
        OleDbDataAdapter adapt1;
        DataTable dt1;
        DataRow dtr1;
        OleDbDataAdapter adapt2;
        DataTable dt2;
        DataRow dtr2;
        OleDbDataAdapter adapt3;
        DataTable dt3;
        DataRow dtr3;
        public AjouterStage()
        {
            InitializeComponent();
        }

        private void txtDure_Validating(object sender, CancelEventArgs e)
        {
            float durre;
            if (txtDure.Text == "")
            {
                this.errorProvider.SetError(txtDure, "Ce Champ Est Obligatoire ");
            }
            else if (!float.TryParse(txtDure.Text, out durre))
            {
                e.Cancel = true;
                this.errorProvider.SetError(txtDure, "La Durée doit être de type Réel");
            }
            else
            {
                this.errorProvider.Clear();
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Stages stg = new Stages();
            stg.Show();
            this.Close();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (cboStagiaire.Text != "" && cboEncadreur.Text != "" && cboSujet.Text != "" && cboType.Text != "" && txtDate.Text != "" && txtDure.Text != "")
            {
                String email="";
                String emailEnc = "";
                try
                {
                    OleDbCommand ajout = new OleDbCommand("insert into Stages (Type,[Durée],[Date de Début],CIN,Identifiant,idSujet) values('" + cboType.Text.ToString() + "'," + txtDure.Text + ",'" + txtDate.Text + "','" + cboStagiaire.SelectedValue.ToString() + "'," + cboEncadreur.SelectedValue.ToString() + "," + cboSujet.SelectedValue.ToString() + ")", cnx);
                    OleDbCommand modif = new OleDbCommand("update Stagiaires set Astg=true WHERE CIN='" + cboStagiaire.SelectedValue.ToString() + "'", cnx);
                    OleDbCommand selct = new OleDbCommand("Select [Adresse Email] FROM Stagiaires WHERE CIN='" + cboStagiaire.SelectedValue.ToString() + "'", cnx);
                    OleDbCommand mailEn = new OleDbCommand("Select [Adresse Email] FROM Encadreurs WHERE Identifiant=" + cboEncadreur.SelectedValue.ToString() + "", cnx);
                    OleDbDataReader rd = selct.ExecuteReader();
                    OleDbDataReader rd2 = mailEn.ExecuteReader();
                    while (rd.Read())
                    {
                        email = rd.GetValue(0).ToString();
                    }
                    while (rd2.Read())
                    {
                        emailEnc = rd2.GetValue(0).ToString();
                    }
                    int nb = ajout.ExecuteNonQuery();
                    int nb2 = modif.ExecuteNonQuery();
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = true;
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("banquezitouna.stages@gmail.com","Banque Zitouna");
                    mail.To.Add(email);
                    mail.Subject = "Affectation De Stage";
                    mail.Body = "<font color=\"#006f8a\" size=4>Bonjour<i><u> " + cboStagiaire.Text.ToString() + "</u></i> Nous Venons Vous envoyer cet email pour vous informer que Votre Stage a été effectué.<br><br> Un Stage de <i><u>" + cboType.Text.ToString() + "</u></i> d'une Durée de <i><u>" + txtDure.Text.ToString() + "</u></i> Mois</font><br><br><font color=\"#006f8a\" size=4>Votre Stage Commencera le <i><u>" + txtDate.Text.ToString() + "</u></i><br><br>Votre Sujet est : <i><u>" + cboSujet.Text.ToString() + "</u></i> vous trouverez ci-joint un document détaillé sur le sujet<br><br>Votre Encadreur est :<i><u>" + cboEncadreur.Text.ToString() + "</u></i> vous pouvez lui contacter sur son email : " + emailEnc + "<br><br>Cordialement.</font>"; 
                    Attachment maPieceJointe = new Attachment(@"..\..\..\Resources\"+cboSujet.Text.ToString()+".pdf"); 
                    mail.Attachments.Add(maPieceJointe); 
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("YourGmailLogin", "YourGmailPassword");
                    SmtpServer.EnableSsl = true;
                    try
                    {
                        SmtpServer.Send(mail);
                        MessageBox.Show("Stage Ajouté et Un Email a été Envoyé au Stagiaire");
                    }
                    catch (SmtpException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vous Devez Remplir Tous Les Champs!");
            }
        }

        private void AjouterStage_Load(object sender, EventArgs e)
        {
            
            try
            {
                cnx.Open();
                OleDbCommand cmd1 = new OleDbCommand("Select CIN,Prenom+' '+Nom as name From Stagiaires WHERE Astg=false", cnx);
                OleDbCommand cmd2 = new OleDbCommand("Select Identifiant,Prenom+' '+Nom as nompre FROM Encadreurs",cnx);
                OleDbCommand cmd3 = new OleDbCommand("Select idSujet,[Titre Du Sujet] FROM Sujets",cnx);
                dts = new DataSet();
                adapt1 = new OleDbDataAdapter(cmd1);
                adapt2 = new OleDbDataAdapter(cmd2);
                adapt3 = new OleDbDataAdapter(cmd3);
                adapt1.Fill(dts,"Stagiaires");
                adapt2.Fill(dts, "Encadreurs");
                adapt3.Fill(dts, "Sujets");
                dt1 = dts.Tables["Stagiaires"];
                dt2 = dts.Tables["Encadreurs"];
                dt3 = dts.Tables["Sujets"];
                cboStagiaire.DataSource = dt1;
                cboStagiaire.DisplayMember = "Name";
                cboStagiaire.ValueMember = "CIN";
                cboEncadreur.DataSource = dt2;
                cboEncadreur.DisplayMember = "nompre";
                cboEncadreur.ValueMember = "Identifiant";
                cboSujet.DataSource = dt3;
                cboSujet.DisplayMember = "Titre Du Sujet";
                cboSujet.ValueMember = "idSujet";
                
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
