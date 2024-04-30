using AplicatieRebus.Models;
using AplicatieRebus.SqlDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

namespace AplicatieRebus.Forms
{
    public partial class Rebus___diversitate_și_transparență : Form
    {
        public UserModel user = new UserModel();
        List<RebusModel> rebusuri = new List<RebusModel>();
        int minute, secunde;
        bool timpDepasit = false;
        DataGridView selectedBoard = new DataGridView();
        public Rebus___diversitate_și_transparență()

        {
            InitializeComponent();
        }

        private void Rebus___diversitate_și_transparență_Load(object sender, EventArgs e)
        {
            if (user.Tip == 0)
            {
                administrareToolStripMenuItem.Enabled = false;
                tabControl1.SelectedIndex = 1;
            }
            dataGridViewRezolvari.Columns.Add("#", "#");
            dataGridViewRezolvari.Columns.Add("Directie", "Directie");
            dataGridViewRezolvari.Columns.Add("Cerinta", "Cerinta");
            dataGridViewOrizontal.Columns.Add("#", "#");
            dataGridViewOrizontal.Columns.Add("Cerinta", "Cerinta");
            dataGridViewVertical.Columns.Add("#", "#");
            dataGridViewVertical.Columns.Add("Cerinta", "Cerinta");
            VizualizareRebusuri();


        }

        private void VizualizareRebusuri()
        {
            comboBoxRebusuri.Items.Clear();
            comboBoxRebusuri2.Items.Clear();
            rebusuri = DatabaseHelper.GetRebusuri();
            foreach (var rebus in rebusuri) {
                comboBoxRebusuri.Items.Add(rebus.Name);
                comboBoxRebusuri2.Items.Add(rebus.Name);
            }
        }

        private void delogareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iesireDinAplicatieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void inregistrareUtilizatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInregistrare frmInregistrare = new FrmInregistrare();
            this.Hide();
            frmInregistrare.administrator = true;
            frmInregistrare.ShowDialog();
            this.Show();
        }

        private void adaugareRebusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT|*.txt";
            openFileDialog.InitialDirectory = "C:\\Users\\Miriam\\Documents\\Aplicatii C\\CSHARP Nationala\\AplicatieRebus\\Resurse\\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog.FileName;
            }
            DatabaseHelper.InsertRebusuri(file);
            VizualizareRebusuri();
        }

        private void comboBoxRebusuri_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewBoard.Columns.Clear();
            dataGridViewRezolvari.Rows.Clear();
            RebusModel rebus = rebusuri[comboBoxRebusuri.SelectedIndex];
            List<RezolvareModel> rezolvari = DatabaseHelper.GetRezolvari(rebus.Id);
            int width = dataGridViewBoard.Width / rebus.NrColoane;
            int height = dataGridViewBoard.Height / rebus.NrLinii;
            for (int i = 0; i < rebus.NrColoane; i++)
            {
                dataGridViewBoard.Columns.Add("Coloana" + i, "Coloana" + i);

            }
            for (int i = 0; i < rebus.NrLinii; i++)
            {
                dataGridViewBoard.Rows.Add();
            }
            foreach (DataGridViewColumn cell in dataGridViewBoard.Columns)
            {
                cell.Width = width;

            }
            foreach (DataGridViewRow cell in dataGridViewBoard.Rows)
            {
                cell.Height = height;
            }
            char[][] solutii = new char[rebus.NrLinii][];
            for (int i = 0; i < rebus.NrLinii; i++)
            {
                solutii[i] = new char[rebus.NrColoane];
            }
            foreach (RezolvareModel rezolvare in rezolvari)
            {
                string orientare = rezolvare.Orientare;
                string cuvant = rezolvare.Solutie.Trim();

                if (orientare.Trim() == "orizontal")
                {
                    dataGridViewRezolvari.Rows.Add(rezolvare.LinieStart, rezolvare.Orientare, rezolvare.Definitie);
                    int ind = 0;
                    for (int j = rezolvare.ColoanaStart - 1; j < rezolvare.ColoanaStart + cuvant.Length - 1; j++)
                    {
                        solutii[rezolvare.LinieStart - 1][j] = cuvant[ind]; ind++;
                    }
                }
                else
                {
                    dataGridViewRezolvari.Rows.Add(rezolvare.ColoanaStart, rezolvare.Orientare, rezolvare.Definitie);
                    int ind = 0;
                    for (int j = rezolvare.LinieStart - 1; j < rezolvare.LinieStart + cuvant.Length - 1; j++)
                    {
                        solutii[j][rezolvare.ColoanaStart - 1] = cuvant[ind]; ind++;
                    }
                }

            }
            for (int i = 0; i < rebus.NrLinii; i++)
            {
                for (int j = 0; j < rebus.NrColoane; j++)
                {
                    DataGridViewCell c = dataGridViewBoard.Rows[i].Cells[j];
                    if (solutii[i][j] != '\0') { c.Value = solutii[i][j]; }
                    else
                    {
                        c.Style.BackColor = Color.Black;
                    }

                }
            }
        }

        private void comboBoxRebusuri2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewBoard2.Columns.Clear();
            dataGridViewOrizontal.Rows.Clear();
            dataGridViewVertical.Rows.Clear();
            timerJoc.Stop();
            RebusModel rebus = rebusuri[comboBoxRebusuri2.SelectedIndex];
            int timp = rebus.NrSecunde;
            minute = timp / 60;
            secunde = timp % 60;

            if (minute >= 10)
            {
                if (secunde >= 10) { labelEstimat.Text = "00:" + minute + ":" + secunde; }
                else { labelEstimat.Text = "00:" + minute + ":0" + secunde; }
            }
            else
            {
                if (secunde >= 10) { labelEstimat.Text = "00:0" + minute + ":" + secunde; }
                else { labelEstimat.Text = "00:0" + minute + ":0" + secunde; }
            }
            labelRamas.Text = labelEstimat.Text;
            timerJoc.Start();
            List<RezolvareModel> rezolvari = DatabaseHelper.GetRezolvari(rebus.Id);
            int width = dataGridViewBoard2.Width / rebus.NrColoane;
            int height = dataGridViewBoard2.Height / rebus.NrLinii;
            for (int i = 0; i < rebus.NrColoane; i++)
            {
                dataGridViewBoard2.Columns.Add("Coloana" + i, "Coloana" + i);

            }
            for (int i = 0; i < rebus.NrLinii; i++)
            {
                dataGridViewBoard2.Rows.Add();
            }
            foreach (DataGridViewColumn cell in dataGridViewBoard2.Columns)
            {
                cell.Width = width;

            }
            foreach (DataGridViewRow cell in dataGridViewBoard2.Rows)
            {
                cell.Height = height;
            }
            char[][] solutii = new char[rebus.NrLinii][];
            for (int i = 0; i < rebus.NrLinii; i++)
            {
                solutii[i] = new char[rebus.NrColoane];
            }
            foreach (RezolvareModel rezolvare in rezolvari)
            {
                string orientare = rezolvare.Orientare;
                string cuvant = rezolvare.Solutie.Trim();

                if (orientare.Trim() == "orizontal")
                {
                    dataGridViewOrizontal.Rows.Add(rezolvare.LinieStart, rezolvare.Definitie);
                    int ind = 0;
                    for (int j = rezolvare.ColoanaStart - 1; j < rezolvare.ColoanaStart + cuvant.Length - 1; j++)
                    {
                        solutii[rezolvare.LinieStart - 1][j] = cuvant[ind]; ind++;
                    }
                }
                else
                {
                    dataGridViewVertical.Rows.Add(rezolvare.ColoanaStart, rezolvare.Definitie);
                    int ind = 0;
                    for (int j = rezolvare.LinieStart - 1; j < rezolvare.LinieStart + cuvant.Length - 1; j++)
                    {
                        solutii[j][rezolvare.ColoanaStart - 1] = cuvant[ind]; ind++;
                    }
                }

            }
            for (int i = 0; i < rebus.NrLinii; i++)
            {
                for (int j = 0; j < rebus.NrColoane; j++)
                {
                    DataGridViewCell c = dataGridViewBoard2.Rows[i].Cells[j];
                    if (solutii[i][j] != '\0') { c.Tag = solutii[i][j]; }
                    else
                    {
                        c.Style.BackColor = Color.Black;
                        c.ReadOnly = true;
                    }

                }
            }
            selectedBoard = dataGridViewBoard2;
        }

        private void timerJoc_Tick(object sender, EventArgs e)
        {

            if (secunde == 0)
            {
                secunde = 60; minute--;
            } else
            {
                secunde--;
            }

            if (minute >= 10)
            {
                if (secunde >= 10) { labelRamas.Text = "00:" + minute + ":" + secunde; }
                else { labelRamas.Text = "00:" + minute + ":0" + secunde; }
            }
            else
            {
                if (secunde >= 10) { labelRamas.Text = "00:0" + minute + ":" + secunde; }
                else { labelRamas.Text = "00:0" + minute + ":0" + secunde; }
            }
            if (minute == 0 && secunde == 0)
            {
                timerJoc.Stop();
                timpDepasit = true;
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            timerJoc.Stop();
            int nrGresite = 0;
            int stare = 0;
            RebusModel rebus = rebusuri[comboBoxRebusuri2.SelectedIndex];
            for (int i = 0; i < rebus.NrLinii; i++)
            {
                for (int j = 0; j < rebus.NrColoane; j++)
                {
                    DataGridViewCell c = dataGridViewBoard2.Rows[i].Cells[j];
                    if (c.Tag != null)
                    {
                        if (c.Value != null)
                        {
                            if (c.Tag.ToString().Trim().ToUpper() != c.Value.ToString().Trim())
                            {
                                nrGresite++;
                            }
                        }
                        else nrGresite++;
                    }

                }
            }
            if (nrGresite == 0 && timpDepasit == false)
            {
                stare = 1;
            }
            else { stare = 2; }
            int nrsecunde = minute * 60 + secunde;
            MessageBox.Show("Litere gresite:" + nrGresite);
            DatabaseHelper.InsertStatistica(user.Id, rebus.Id, nrsecunde, nrGresite, stare);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0 && user.Tip == 0)
            {
                tabControl1.SelectedIndex = 1;
            }
            if (tabControl1.SelectedIndex == 2)
            {
                if (comboBoxRebusuri2.SelectedItem != null)
                {
                    CuvinteOrdonate.Items.Clear();
                    RebusModel rebus = rebusuri[comboBoxRebusuri2.SelectedIndex];
                    List<RezolvareModel> rezolvari = DatabaseHelper.GetRezolvari(rebus.Id);
                    foreach (var rezolvare in rezolvari)
                    {
                        int nrvocale = 0;
                        foreach (char c in rezolvare.Solutie)
                        {
                            if ("aeiou".Contains(c)) { nrvocale++; }

                        }
                        rezolvare.NrVocale = nrvocale;
                    }
                    rezolvari.OrderBy(i => i.NrVocale);
                    foreach (var rezolvare in rezolvari)
                    {
                        CuvinteOrdonate.Items.Add(rezolvare.Solutie);
                    }
                }
            }
            if (tabControl1.SelectedIndex == 3)
            {
                chartRebus.ChartAreas.Clear();
                chartRebus.Series.Clear();
                List<RebusModel> rebusuriChart = DatabaseHelper.GetRebusuri();
                chartRebus.ChartAreas.Add(new ChartArea());
                Series timpRebus = new Series();
                foreach (RebusModel rebus in rebusuriChart)
                {
                    timpRebus.Points.AddXY(rebus.Name, rebus.NrSecunde);

                }
                chartRebus.ChartAreas[0].AxisX.Interval = 1;
                chartRebus.Series.Add(timpRebus);

            }
        
    } 

        private void tiparireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxRebusuri2.SelectedItem != null)
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += printDocument1_PrintPage;
                printPreviewDialog1.Document = pd;
                printPreviewDialog1.ShowDialog();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            RebusModel rebus = rebusuri[comboBoxRebusuri2.SelectedIndex];
            Bitmap bitmap = new Bitmap(selectedBoard.Width, selectedBoard.Height);
            Rectangle rectangle = new Rectangle( 0,0, selectedBoard.Width, selectedBoard.Height);
            selectedBoard.DrawToBitmap(bitmap,rectangle);
            e.Graphics.DrawImage(bitmap, 100, 10);

            e.Graphics.DrawString(rebus.Name, label1.Font, new SolidBrush(Color.Black), 100, selectedBoard.Height + 20);
            e.Graphics.DrawLine(new Pen(Color.Black), 100, selectedBoard.Height + 40, bitmap.Width + 10, selectedBoard.Height + 40);
            int x = 10;
            int y = selectedBoard.Height + 60;
            foreach (DataGridViewRow row in dataGridViewOrizontal.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string text = row.Cells[0].Value.ToString() + " " + "orizontal" + " " + row.Cells[1].Value.ToString() + " ";
                    e.Graphics.DrawString(text, label1.Font, new SolidBrush(Color.Black), x, y);
                }
                y += 15;
            }
            foreach (DataGridViewRow row in dataGridViewVertical.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string text = row.Cells[0].Value.ToString() + " " + "vertical"+ " " + row.Cells[1].Value.ToString() + " ";
                    e.Graphics.DrawString(text, label1.Font, new SolidBrush(Color.Black), x, y);
                }
                y += 15;
            }

        }

        

        private void dataGridViewBoard2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell c = dataGridViewBoard2.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if(c.Value.ToString().Length >1 )
            {
                c.Value = "";
            }
            c.Value = c.Value.ToString().ToUpper();
            
        }
    }
}
