using AplicatieRebus.Models;
using AplicatieRebus.SqlDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplicatieRebus.Forms
{
    public partial class FrmLogare : Form
    {
        public FrmLogare()
        {
            InitializeComponent();
        }

        private void FrmLogare_Load(object sender, EventArgs e)
        {
            DatabaseHelper.Initialisation();
            textBoxNume.Text = "admin";
            textBoxParola.Text = "1234";
        }

        private void linkLabelInregistrare_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FrmInregistrare frmInregistrare = new FrmInregistrare();
            frmInregistrare.ShowDialog();
            this.Show();
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            UserModel user = new UserModel();
            user.Name = textBoxNume.Text;
            user.Password= textBoxParola.Text;
            user = DatabaseHelper.FindUser(user);
            if (user != null)
            {
                Rebus___diversitate_și_transparență rebusForm = new Rebus___diversitate_și_transparență();
                this.Hide();
                rebusForm.user = user;
                rebusForm.ShowDialog();
                if(Application.OpenForms.Count > 0) { this.Show(); }
            }
        }
    }
}
