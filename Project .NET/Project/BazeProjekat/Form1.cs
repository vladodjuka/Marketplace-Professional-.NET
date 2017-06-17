using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Oracle.DataAccess.Client;

namespace BazeProjekat
{
    public partial class Form1 : Form
    {
        string username = "";
        OracleConnection dc = new OracleConnection("User Id=luka; Password=luka1809; Data Source=localhost:1521/xe");

        public Form1()
        {
            InitializeComponent();
            var pos = this.PointToScreen(label1.Location);
            pos = pictureBox1.PointToClient(pos);
            label1.Parent = pictureBox1;
            label1.Location = pos;
            label1.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();
                string sele = "select * from Zaposleni where korisnicko_ime='" + textBox1.Text + "' and sifra='" + textBox2.Text + "'";
                OracleCommand or = new OracleCommand();
                or.CommandText = sele;
                or.Connection = dc;
                int num = -1;
                var reader = or.ExecuteReader();

                while (reader.Read())
                {
                    num = 1;
                    username = textBox1.Text;
                }
                textBox1.Text = "";
                textBox2.Text = "";
                if (num == 1)
                {
                    Trgovina tr = new Trgovina(this, username);
                    tr.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Greska u korisnickom imenu ili sifri");
                    
                }

               


                dc.Close();
            }
            catch (Exception)
            {

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
