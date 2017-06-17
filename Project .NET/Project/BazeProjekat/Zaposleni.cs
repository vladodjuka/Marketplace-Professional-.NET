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
    public partial class Zaposleni : Form
    {
        bool delt = false;
        OracleConnection dc = new OracleConnection("User Id=luka; Password=luka1809; Data Source=localhost:1521/xe");
        string jmbg = "";
        string username = "";
        public Zaposleni()
        {
            InitializeComponent();
        }

        private void Zaposleni_Load(object sender, EventArgs e)
        {
            fillGrid();
            button8.Enabled = false;
        }

        public void fillGrid()
        {
            try
            {
                dc.Open();



                string st = "select jmbg, ime, prezime, broj_telefona as telefon, korisnicko_ime as \"KORISNICKO IME\", sifra from Zaposleni"; 



                    OracleDataAdapter od = new OracleDataAdapter(st, dc);
                    DataTable dt = new DataTable();

                    od.Fill(dt);

                    dataGridView1.DataSource = dt;
                
               
                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }
        }

        public void deleteById(string id)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da izvrsite brisanje ?", "Sure", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    OracleCommand co = new OracleCommand();

                    string dlle = "";

                   
                        dlle = "delete from proprod where idprod in (select idprod from prodaja where jmbg='" + id + "')";
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();

                    dlle = "delete from prodaja where jmbg='" + id + "'";
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();

                    dlle = "delete from ZapoSleni where jmbg='" + id + "'";
                        co.CommandText = dlle;
                        co.Connection = dc;

                    co.ExecuteNonQuery();
                        resetAll();
                }


            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }



        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                dc.Open();



                int index = dataGridView1.SelectedCells[0].RowIndex;


                DataGridViewRow dr = dataGridView1.Rows[index];

                string jm = dr.Cells[0].Value.ToString();

                jmbg = jm;


                if (delt == true)
                {
                    deleteById(jm);


                }

                else
                {



                    string go = "select * from Zaposleni where jmbg='" + jm + "'";

                    OracleCommand rv = new OracleCommand();

                    rv.CommandText = go;
                    rv.Connection = dc;


                    var read = rv.ExecuteReader();

                    while (read.Read())
                    {
                        if (read[0] != DBNull.Value)
                        {
                            textBox4.Text = read.GetString(0);
                            textBox4.Enabled = false;
                        }
                        else
                        {
                            textBox4.Text = "";
                        }

                        if (read[1] != DBNull.Value)
                        {
                            textBox1.Text = read.GetString(1);
                        }
                        else
                        {
                            textBox1.Text = "";
                        }
                        if (read[2] != DBNull.Value)
                        {
                            textBox2.Text = read.GetString(2);
                        }
                        else
                        {
                            textBox2.Text = "";
                        }
                        if (read[3] != DBNull.Value)
                        {
                            textBox5.Text = read.GetString(3);
                        }
                        else
                        {
                            textBox5.Text = "";
                        }
                        if (read[4] != DBNull.Value)
                        {
                            textBox7.Text = read.GetString(4);
                            username = textBox7.Text;
                            //textBox7.Enabled = false;
                        }
                        else
                        {
                            textBox7.Text = "";
                        }
                        if (read[5] != DBNull.Value)
                        {
                            textBox3.Text = read.GetString(5);
                        }
                        else
                        {
                            textBox3.Text = "";
                        }
                    }

                    button8.Enabled = true;
                }

                dc.Close();
                if (delt==true) {
                    fillGrid();
                 }
                
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        public void resetAll()
        {
            textBox4.Enabled = true;
            textBox7.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            button8.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resetAll();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();

                if (!textBox1.Text.Equals(""))
                {
                    if (!textBox2.Text.Equals(""))
                    {
                        if (!textBox3.Text.Equals(""))
                        {
                            if (!textBox4.Text.Equals("") && textBox4.TextLength==13 && IsDigitsOnly(textBox4.Text))
                            {
                                if (!textBox5.Text.Equals("") && IsDigitsOnly(textBox5.Text))
                                {
                                    if (!textBox7.Text.Equals(""))
                                    {
                                        if (!exist())
                                        {
                                            if (!existUser())
                                            {
                                                //string year = "01/jan/" + textBox9.Text;
                                                string add = "insert into Zaposleni(jmbg, ime, prezime, broj_telefona, korisnicko_ime, sifra) values('" + textBox4.Text + "', '" + textBox1.Text + "','" + textBox2.Text + "', '" + textBox5.Text + "','" + textBox7.Text + "', '" + textBox3.Text + "')";


                                                OracleCommand or = new OracleCommand();
                                                or.CommandText = add;
                                                or.Connection = dc;
                                                or.ExecuteNonQuery();
                                                resetAll();
                                            }
                                            else
                                            {
                                                MessageBox.Show("Korisnicko ime vec postoji");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Ovaj jmbg vec postoji u bazu");
                                        }
                                       
                                    }
                                    else
                                    {
                                        MessageBox.Show("Unesite korisnicko ime");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Unesite ispravan broj telefona");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unesite jedinstveni maticni broj (sastoji se od 13 brojeva)");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unesite sifru");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unesite prezime");
                    }
                }
                else
                {
                    MessageBox.Show("Unesite ime");
                }


                dc.Close();
                fillGrid();
            }
            catch (Exception)
            {

            }
        }

        public bool exist()
        {

            try
            {

                string sele = "select * from Zaposleni where jmbg='" + textBox4.Text + "'";
                OracleCommand or = new OracleCommand();
                or.CommandText = sele;
                or.Connection = dc;
                int num = -1;
                var reader = or.ExecuteReader();

                while (reader.Read())
                {
                    num = 1;
                }

                if (num == 1)
                {
                    return true;
                }


            }
            catch (Exception jj)
            {

            }
            return false;

        }

        public bool existUser()
        {

            try
            {

                string sele = "select * from Zaposleni where korisnicko_ime='" + textBox7.Text + "'";
                OracleCommand or = new OracleCommand();
                or.CommandText = sele;
                or.Connection = dc;
                int num = -1;
                var reader = or.ExecuteReader();

                while (reader.Read())
                {
                    num = 1;
                }

                if (num == 1)
                {
                    return true;
                }


            }
            catch (Exception jj)
            {

            }
            return false;

        }

        public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if ((c < '0' || c > '9'))

                    return false;

            }

            return true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();

                if (!textBox1.Text.Equals(""))
                {
                    if (!textBox2.Text.Equals(""))
                    {
                        if (!textBox3.Text.Equals(""))
                        {
                            if (!textBox4.Text.Equals("") && textBox4.TextLength == 13 && IsDigitsOnly(textBox4.Text))
                            {
                                if (!textBox5.Text.Equals("") && IsDigitsOnly(textBox5.Text))
                                {
                                    if (!textBox7.Text.Equals(""))
                                    {

                                        if (!existUser() || textBox7.Text.Equals(username))
                                        {

                                            //string year = "01/jan/" + textBox9.Text;
                                            string add = "update Zaposleni set ime='" + textBox1.Text + "', prezime='" + textBox2.Text + "', broj_telefona='" + textBox5.Text + "',korisnicko_ime='"+textBox7.Text+"',sifra='" + textBox3.Text + "' where jmbg='" + jmbg + "'";


                                            OracleCommand or = new OracleCommand();
                                            or.CommandText = add;
                                            or.Connection = dc;
                                            or.ExecuteNonQuery();
                                            resetAll();

                                        }
                                        else
                                        {
                                            MessageBox.Show("Korisnicko ime vec postoji");
                                        }
                                        

                                    }
                                    else
                                    {
                                        MessageBox.Show("Unesite korisnicko ime");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Unesite ispravan broj telefona");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unesite jedinstveni maticni broj (sastoji se od 13 brojeva)");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unesite sifru");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unesite prezime");
                    }
                }
                else
                {
                    MessageBox.Show("Unesite ime");
                }



                dc.Close();

                fillGrid();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (delt)
            {
                delt = false;
                button5.BackColor = Color.Red;

            }
            else
            {
                delt = true;
                button5.BackColor = Color.Green;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Width = 1000;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Width = 838;
        }
    }
}
