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
    public partial class FrmInregistrare : Form
    {
       public bool administrator = false;
        public FrmInregistrare()
        {
            InitializeComponent();
        }
        private bool ValidEmail(string email)
        {

            if (email != null)
            {
                string[] splitByA = email.Split('@');
                if (splitByA.Length == 2 && !string.IsNullOrEmpty(splitByA[0]) && !string.IsNullOrEmpty(splitByA[1]))
                {
                    foreach (char c in splitByA[0])
                    {
                        if (!char.IsLetterOrDigit(c) && c != '.' && c != '_' && c != '-')
                        {
                            return false;
                        }
                    }
                    string[] splitByP = splitByA[1].Split('.');
                    if (splitByP.Length != 2 || string.IsNullOrEmpty(splitByP[0]) || string.IsNullOrEmpty(splitByP[1]) || splitByA[0].First() == '.' || splitByA[0].Last() == '.')
                    {
                        return false;
                    }

                }
                else { return false; }
            }
            else { return false; }
            return true;
        }
        private void buttonSingUp_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxNume.Text) && !string.IsNullOrEmpty(textBoxParola.Text) && !string.IsNullOrEmpty(textBoxConf.Text) && !string.IsNullOrEmpty(textBoxEmail.Text) && comboBox1.SelectedItem!=null&&ValidEmail(textBoxEmail.Text)&&textBoxConf.Text==textBoxParola.Text) 
            { 
                UserModel user = DatabaseHelper.FindUserByNumeOrEmail(textBoxNume.Text, textBoxEmail.Text);
                if (user == null)
                {
                    int tip = 0;
                    if (comboBox1.SelectedItem.ToString() != "Utilizator")
                    {
                        tip = 1;
                    }
                    UserModel newUser = new UserModel()
                    {
                        Tip = tip,
                        Name = textBoxNume.Text,
                        Email = textBoxEmail.Text,
                        Password = textBoxParola.Text
                    };
                    DatabaseHelper.InsertNewUser(newUser);
                    MessageBox.Show("Utilizator adaugat cu succes!");
                    this.Close();
                }
                else { MessageBox.Show("Utilizatorul esti in baza de date!");
                    textBoxNume.Text = textBoxEmail.Text = textBoxConf.Text = textBoxParola.Text = "";
                    comboBox1.SelectedIndex = 0;
                }
            
            }else { MessageBox.Show("Eroare inregistrare: completati toate spatiile si folositi un email valid!"); }
        }

        private void buttonRenunta_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmInregistrare_Load(object sender, EventArgs e)
        {
            if (administrator == true)
            {
                comboBox1.Items.Add("Administrator");
            }
        }
    }
}
