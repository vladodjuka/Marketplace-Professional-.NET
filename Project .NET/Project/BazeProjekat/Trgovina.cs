using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

using Oracle.DataAccess.Client;

namespace BazeProjekat
{
    public partial class Trgovina : Form
    {

        bool delt = false;
        string usern = "";
        OracleConnection dc = new OracleConnection("User Id=luka; Password=luka1809; Data Source=localhost:1521/xe");
        Form1 f;
        string last = "";
        string sifr = "";
        public Trgovina(Form1 ff, string user)
        {
            InitializeComponent();
            f = ff;
            if (!user.Equals("admin"))
            {
                button2.Enabled = false;

                button11.Visible = false;
                
            }
           
            usern = user;
        }

        private void Trgovina_Load(object sender, EventArgs e)
        {

            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
           


            comboBox1.Items.Add("Odaberi");
            comboBox1.Items.Add("Klijenti");
            comboBox1.Items.Add("Proizvodi");
            comboBox1.Items.Add("Vrsta Proizvoda");
            if (button2.Enabled == true)
            {
                comboBox1.Items.Add("Zaposleni");
            }

            comboBox1.SelectedIndex = 0;

            fillCombo1();


            try
            {
                dc.Open();

                last = "proizvod";
                        string st = "select idproiz as ID, naziv, cijena, kolicina, sezona, naziv_vrste as vrsta from Proizvod";
                     
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

        public void fillCombo1()
        {
            try
            {

                dc.Open();
                OracleDataAdapter o = new OracleDataAdapter("select naziv_vrste from VrstaProizvoda", dc);
                DataTable td = new DataTable();

                o.Fill(td);

                DataRow dr = td.NewRow();
                dr["naziv_vrste"] = "Odaberi";
                dr["naziv_vrste"] = "Odaberi";


                td.Rows.InsertAt(dr, 0);

                comboBox2.DisplayMember = "naziv_vrste";
                comboBox2.ValueMember = "naziv_vrste";
                comboBox2.DataSource = td;
                dc.Close();



            }
            catch (OracleException ek)
            {


            }
        }

        private void Trgovina_FormClosing(object sender, FormClosingEventArgs e)
        {
            f.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
            f.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            resett();

            richTextBox1.Text = "";
            textBox8.Enabled = true;

            comboBox2.SelectedIndex = 0;
            fillGrid();

            
        }

        public void fillGrid()
        {
            try
            {
                dc.Open();
                getIdk();

                if (comboBox1.SelectedIndex != 0)
                {
                    string st = "select * from Klijent where idk<0";

                    if (comboBox1.SelectedIndex == 1)
                    {
                        st = "select idk, ime, prezime, broj_telefona as telefon, mail, ukupno from Klijent";
                        last = "klijent";
                    }

                    if (comboBox1.SelectedIndex == 2)
                    {
                        st = "select idproiz as ID, naziv, cijena, kolicina, sezona, naziv_vrste as vrsta from Proizvod";
                        last = "proizvod";
                    }

                    if (comboBox1.SelectedIndex == 3)
                    {
                        st = "select naziv_vrste as naziv, opis from VrstaProizvoda";
                        last = "vrsta";
                    }

                    if (button2.Enabled == true && comboBox1.SelectedIndex == 4)
                    {
                        st = "select jmbg, ime, prezime, broj_telefona as telefon, korisnicko_ime as \"KORISNICKO IME\", sifra from Zaposleni";
                        last = "zaposleni";
                    }
                    OracleDataAdapter od = new OracleDataAdapter(st, dc);
                    DataTable dt = new DataTable();

                    od.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
                else
                {
                    //MessageBox.Show("Izaberite sta zelite da prikazete");
                }
                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }
        }

        private int getIdk()
        {
            int idk = 0;

            try
            {
                

                string go = "select max(idk) from Klijent";

                OracleCommand rv = new OracleCommand();

                rv.CommandText = go;
                rv.Connection = dc;


                var read = rv.ExecuteScalar().ToString();
                if (read.Equals(""))
                {
                    idk = 0;
                }

                else
                {

                    idk = int.Parse(read) + 1;

                }
                //MessageBox.Show(idk + "");
    
                return idk;

            }


            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }

            return idk;

        }
        private int getProiz()
        {
            int proiz = 0;

            try
            {


                string go = "select max(idproiz) from Proizvod";

                OracleCommand rv = new OracleCommand();

                rv.CommandText = go;
                rv.Connection = dc;


                var read = rv.ExecuteScalar().ToString();
                if (read.Equals(""))
                {
                    proiz = 0;
                }

                else
                {

                    proiz = int.Parse(read) + 1;

                }
                //MessageBox.Show(idk + "");

                return proiz;

            }


            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }

            return proiz;

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
                        if (!textBox3.Text.Equals("") && IsValid(textBox3.Text))
                        {
                            if (!textBox7.Text.Equals("") && IsDigitsOnly(textBox7.Text))
                            {

                                if (!existEmail() && !existPhone())
                                {
                                    string add = "insert into Klijent(idk, ime, prezime, mail, broj_telefona ) values(" + getIdk() + ", '" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "','" + textBox7.Text + "')";

                                    OracleCommand or = new OracleCommand();
                                    or.CommandText = add;
                                    or.Connection = dc;
                                    or.ExecuteNonQuery();
                                    textBox1.Text = "";
                                    textBox2.Text = "";
                                    textBox3.Text = "";
                                    textBox7.Text = "";
                                }
                                else
                                {
                                    if (existPhone())
                                    {
                                        MessageBox.Show("Broj telefona vec postoji u bazu");
                                    }
                                    if (existEmail())
                                    {
                                        MessageBox.Show("Email vec postoji u bazu");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unesite ispravan broj telefona");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unesite ispravan mail");
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
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
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

        public bool checkDecimal(TextBox t)
        {
            decimal d;
            if (decimal.TryParse(t.Text, out d))
            {
                return true;
            }
            else
            {
                //invalid
                MessageBox.Show("Please enter a valid number");
                return false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();
                if (!textBox4.Text.Equals(""))
                {
                    if (!textBox5.Text.Equals("")&& checkDecimal(textBox5))
                    {
                        if (!textBox6.Text.Equals("") && checkDecimal(textBox6) && int.Parse(textBox6.Text)>0 && int.Parse(textBox6.Text) <= 20)
                        {
                            if (!textBox9.Text.Equals("") && IsDigitsOnly(textBox9.Text))
                            {
                                if (comboBox2.SelectedIndex!=0) {

                                    if (!existName())
                                    {

                                        string year = "01/jan/" + textBox9.Text;
                                        string add = "insert into Proizvod(idproiz, naziv, cijena, kolicina, sezona, naziv_vrste) values(" + getProiz() + ", '" + textBox4.Text + "', " + textBox5.Text + ", " + textBox6.Text + ",'" + year + "', '" + comboBox2.SelectedValue + "')";

                                 


                                    OracleCommand or = new OracleCommand();
                                    or.CommandText = add;
                                    or.Connection = dc;
                                    or.ExecuteNonQuery();
                                    textBox4.Text = "";
                                    textBox5.Text = "";
                                    textBox6.Text = "";
                                    textBox9.Text = "";
                                    comboBox2.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Proizvod sa tim imenom vec postoji u bazu");
                                    }
                                }
                                else {
                                    MessageBox.Show("Izaberite vrstu proizvoda");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unesite ispravnu godinu");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unesite ispravnu kolicinu. Minimalna kolicina: 1 Maksimalna kolicina: 20");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unesite ispravnu cijenu");
                    }

                }
                else
                {
                    MessageBox.Show("Unesite naziv");
                }




                dc.Close();
                fillGrid();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();
                if (!textBox8.Text.Equals(""))
                {

                    if (!existType())
                    {

                        string add = "insert into VrstaProizvoda(naziv_vrste, opis) values('" + textBox8.Text + "', '" + richTextBox1.Text + "')";

                        OracleCommand or = new OracleCommand();
                        or.CommandText = add;
                        or.Connection = dc;
                        or.ExecuteNonQuery();
                        textBox8.Text = "";
                        richTextBox1.Text = "";

                    }
                    else
                    {
                        MessageBox.Show("Vrsta vec postoji");
                    }
                    
                    

                     }
                else
                {
                    MessageBox.Show("Unesite naziv vrste");
                }




                dc.Close();
                fillGrid();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }

            fillCombo1();
        }

        public void deleteById(string what, string id)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da izvrsite brisanje ?", "Sure", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    

                    OracleCommand co = new OracleCommand();

                    string dlle = "";

                    if (what.Equals("klijent"))
                    {
                        dlle = "delete from proprod where idprod in (select idprod from prodaja where idk="+id+")";
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();

                        dlle = "delete from prodaja where idk=" + id;
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();

                        dlle = "delete from Klijent where idk=" + id;
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();

                    }

                    if (what.Equals("proizvod"))
                    {

                        /*dlle = "DELETE r.*, p.* FROM prodaja p INNER JOIN proprod r on r.idprod = p.idprod WHERE r.idproiz=" + id;
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();*/

                        dlle = "declare addr_nt sys.ODCINUMBERLIST; begin delete from proprod p where p.idproiz="+id+" returning p.idprod bulk collect into addr_nt; forall i in addr_nt.first..addr_nt.last delete from prodaja pr where pr.idprod = addr_nt(i); end;";
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();
                        /*
                        dlle = "delete from proprod where idproiz=" + id;
                        co.CommandText = dlle;
                        co.Connection = dc;

                        co.ExecuteNonQuery();

                        dlle = "delete from prodaja where idprod in (select idprod from proprod where idproiz=" + id + ")";
                        co.CommandText = dlle;
                       co.Connection = dc;

                      co.ExecuteNonQuery();*/


                        dlle = "delete from Proizvod where idproiz=" + id;
                        co.CommandText = dlle;
                        co.Connection = dc;

                       co.ExecuteNonQuery();

                    }

                    if (what.Equals("vrsta"))
                    {
                        if (MessageBox.Show("Brisanjem VRSTE PROIZVODA izbrisace se svi proizvodi koji pripadaju ovoj vrsti kao i sve trgovine za ovaj proizvod. Da li ste sigurni da zelite da nastavite ?", "Sure", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            dlle = "declare addr_tt sys.ODCINUMBERLIST; begin delete from proprod p where p.idproiz in (select idproiz from proizvod where naziv_vrste='"+id+"') returning p.idprod bulk collect into addr_tt; forall i in addr_tt.first..addr_tt.last delete from prodaja pr where pr.idprod = addr_tt(i); end;";
                            co.CommandText = dlle;
                            co.Connection = dc;

                            co.ExecuteNonQuery();

                           

                            dlle = "delete from Proizvod where naziv_vrste='" + id + "'";
                            co.CommandText = dlle;
                            co.Connection = dc;

                            co.ExecuteNonQuery();



                            dlle = "delete from VrstaProizvoda where naziv_vrste='" + id + "'";
                            co.CommandText = dlle;
                            co.Connection = dc;

                            co.ExecuteNonQuery();

                            resetAll();

                            
                        }
                    }

                }



                resetAll();


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

                sifr = jm;

                if (delt == true)
                {
                    deleteById(last, jm);
                    resett();
                    

                }
                else
                {
                    if (last.Equals("klijent"))
                    {

                        button8.Enabled = true;

                        string go = "select * from Klijent where idk=" + jm;

                        OracleCommand rv = new OracleCommand();

                        rv.CommandText = go;
                        rv.Connection = dc;


                        var read = rv.ExecuteReader();

                        while (read.Read())
                        {
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
                                textBox7.Text = read.GetString(3);
                            }
                            else
                            {
                                textBox7.Text = "";
                            }
                            if (read[4] != DBNull.Value)
                            {
                                textBox3.Text = read.GetString(4);
                            }
                            else
                            {
                                textBox3.Text = "";
                            }
                        }

                    }
                    else
                    {
                        if (last.Equals("proizvod"))
                        {
                            button7.Enabled = true;


                            string go = "select * from Proizvod where idproiz=" + jm;

                            OracleCommand rv = new OracleCommand();

                            rv.CommandText = go;
                            rv.Connection = dc;


                            var read = rv.ExecuteReader();

                            while (read.Read())
                            {
                                if (read[1] != DBNull.Value)
                                {
                                    textBox4.Text = read.GetString(1);
                                }
                                else
                                {
                                    textBox4.Text = "";
                                }
                                if (read[2] != DBNull.Value)
                                {
                                    textBox5.Text = read.GetDecimal(2) + "";
                                }
                                else
                                {
                                    textBox5.Text = "";
                                }
                                if (read[3] != DBNull.Value)
                                {
                                    textBox6.Text = read.GetDecimal(3) + "";
                                }
                                else
                                {
                                    textBox6.Text = "";
                                }
                                if (read[4] != DBNull.Value)
                                {
                                    string da = read.GetDateTime(4).ToShortDateString() + "";
                                    da = da.Split('/')[2];
                                    textBox9.Text = da;
                                }
                                else
                                {
                                    textBox9.Text = "";
                                }

                                if (read[5] != DBNull.Value)
                                {
                                    string sele = read.GetString(5);
                                    comboBox2.SelectedValue = sele;
                                }
                            }
                        }

                        else
                        {
                            if (last.Equals("vrsta"))
                            {
                                textBox8.Enabled = false;
                                button9.Enabled = true;





                                string go = "select * from VrstaProizvoda where naziv_vrste='" + jm + "'";

                                OracleCommand rv = new OracleCommand();

                                rv.CommandText = go;
                                rv.Connection = dc;


                                var read = rv.ExecuteReader();

                                while (read.Read())
                                {
                                    if (read[0] != DBNull.Value)
                                    {
                                        textBox8.Text = read.GetString(0);
                                    }
                                    else
                                    {
                                        textBox4.Text = "";
                                    }
                                    if (read[1] != DBNull.Value)
                                    {
                                        richTextBox1.Text = read.GetString(1) + "";
                                    }
                                    else
                                    {
                                        richTextBox1.Text = "";
                                    }

                                }


                            }
                            else
                            {
                                if (last.Equals("zaposleni"))
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Not Valid");
                                }
                            }
                        }
                    }
                }


               
                dc.Close();
                if (delt) { fillGrid(); }
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }


        public bool existPhone()
        {

            try
            {

                string sele = "select * from klijent where broj_telefona='" + textBox7.Text + "'";
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
        public bool existEmail()
        {
            try
            {

                string sele = "select * from klijent where mail='" +textBox3.Text+ "'";
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
        public bool existName()
        {

            try
            {

                string sele = "select * from proizvod where naziv='" + textBox4.Text + "'";
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
        public bool existType()
        {

            try
            {

                string sele = "select * from vrstaproizvoda where naziv_vrste='" + textBox8.Text + "'";
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

        private void button8_Click(object sender, EventArgs e)
        {

            try
            {
                dc.Open();
                if (!textBox1.Text.Equals(""))
                {
                    if (!textBox2.Text.Equals(""))
                    {
                        if (!textBox3.Text.Equals("") && IsValid(textBox3.Text))
                        {
                            if (!textBox7.Text.Equals("") && IsDigitsOnly(textBox7.Text))
                            {
                                string add = "update Klijent set ime='" + textBox1.Text + "', prezime='" + textBox2.Text + "', mail='" + textBox3.Text + "', broj_telefona='" + textBox7.Text + "' where idk=" + sifr;

                                    OracleCommand or = new OracleCommand();
                                    or.CommandText = add;
                                    or.Connection = dc;
                                    or.ExecuteNonQuery();
                                    textBox1.Text = "";
                                    textBox2.Text = "";
                                    textBox3.Text = "";
                                    textBox7.Text = "";
                                    button8.Enabled = false;
                               
                            }
                            else
                            {
                                MessageBox.Show("Unesite ispravan broj telefona");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unesite ispravan mail");
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

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();
                if (!textBox4.Text.Equals(""))
                {
                    if (!textBox5.Text.Equals("") && checkDecimal(textBox5))
                    {
                        if (!textBox6.Text.Equals("") && checkDecimal(textBox6) && int.Parse(textBox6.Text) > 0 && int.Parse(textBox6.Text) <= 20)
                        {
                            if (!textBox9.Text.Equals("") && IsDigitsOnly(textBox9.Text))
                            {
                                if (comboBox2.SelectedIndex != 0)
                                {

                                   

                                        string year = "01/jan/" + textBox9.Text;
                                    string add = "update Proizvod set naziv='" + textBox4.Text + "', cijena=" + textBox5.Text + ", kolicina=" + textBox6.Text + ", sezona='" + year + "', naziv_vrste='" + comboBox2.SelectedValue + "' where idproiz=" + sifr;




                                        OracleCommand or = new OracleCommand();
                                        or.CommandText = add;
                                        or.Connection = dc;
                                        or.ExecuteNonQuery();
                                        textBox4.Text = "";
                                        textBox5.Text = "";
                                        textBox6.Text = "";
                                        textBox9.Text = "";
                                        comboBox2.SelectedIndex = 0;
                                        button7.Enabled = false;
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Izaberite vrstu proizvoda");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unesite ispravnu godinu");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unesite ispravnu kolicinu. Minimalna kolicina: 1 Maksimalna: 20");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unesite ispravnu cijenu");
                    }

                }
                else
                {
                    MessageBox.Show("Unesite naziv");
                }


                

                dc.Close();
                fillGrid();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();
                if (!textBox8.Text.Equals(""))
                {

                   
                        OracleCommand or = new OracleCommand();

                    //string great = "UPDATE (SELECT vs.naziv_vrste as OLD, po.naziv_vrste as OLD2 FROM VrstaProizvoda vs INNER JOIN Proizvod po ON vs.naziv_vrste = po.naziv_vrste where po.naziv_vrste = '"+sifr+"') told set told.old ='" + textBox8.Text + "', told.old2='" + textBox8.Text + "'";
                    //textBox8.Text = great;
                    string add = "update VrstaProizvoda set opis='" + richTextBox1.Text + "' where naziv_vrste='" + sifr + "'";

                        or.CommandText = add;
                        or.Connection = dc;
                        or.ExecuteNonQuery();
                        textBox8.Text = "";
                        richTextBox1.Text = "";
                        textBox8.Enabled = true;
                        button9.Enabled = false;


                }
                else
                {
                    MessageBox.Show("Unesite naziv vrste");
                }


                

                dc.Close();


                fillGrid();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }

            fillCombo1();
        }

        public void resetAll()
        {
            textBox8.Enabled = true;

            textBox8.Text = "";
            richTextBox1.Text = "";

            button9.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            resetAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Zaposleni fy = new Zaposleni();
            fy.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Prodaja pr = new Prodaja(usern);
            pr.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (delt)
            {
                delt = false;
                button11.BackColor = Color.Red;

            }
            else
            {
                delt = true;
                button11.BackColor = Color.Green;
            }
        }
        public void resett()
        {
            

            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;

            textBox1.Text = "";
            textBox2.Text = "";

            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";

            textBox6.Text = "";
            textBox9.Text = "";
            textBox8.Text = "";

            textBox7.Text = "";
            comboBox2.SelectedIndex = 0;
            richTextBox1.Text = "";

        }
        private void button12_Click(object sender, EventArgs e)
        {
            resett();
        }
    }
}
