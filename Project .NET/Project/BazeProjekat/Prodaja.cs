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
    public partial class Prodaja : Form
    {

        string id = "";
        int popust = 0;
        string username = "";
        
        OracleConnection dc = new OracleConnection("User Id=luka; Password=luka1809; Data Source=localhost:1521/xe");
        public Prodaja(string usernam)
        {
            InitializeComponent();
            var pos = this.PointToScreen(label1.Location);
            pos = pictureBox1.PointToClient(pos);
            label1.Parent = pictureBox1;
            label1.Location = pos;
            label1.BackColor = Color.Transparent;

            username = usernam;
            //MessageBox.Show(usernam);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Width = 604;
                string idkk = "";
                string jmbgg = "";
                string imeprezimekl = "";
                string imeprezimeza = "";
                bool df = false;
                
                dc.Open();

                string date = DateTime.Now.ToString("dd/MMM/yyyy");

                
                OracleCommand or = new OracleCommand();

                if (comboBox1.SelectedIndex != 0)
                {
                    if (comboBox2.SelectedIndex != 0)
                    {
                        idkk = comboBox1.SelectedValue.ToString();
                        jmbgg = comboBox2.SelectedValue.ToString();
                    }
                    else
                    {
                        if (!textBox2.Text.Equals(""))
                        {
                            idkk = comboBox1.SelectedValue.ToString();
                           
                                imeprezimeza = textBox2.Text;
                        }
                        else
                        {
                            MessageBox.Show("Izaberite zaposlenog");
                        }
                    }
                }
                else
                {
                    if (comboBox2.SelectedIndex != 0)
                    {
                        jmbgg = comboBox2.SelectedValue.ToString();

                        if (!textBox1.Text.Equals(""))
                        {
                            imeprezimekl = textBox1.Text;
                        }
                        else
                        {
                            MessageBox.Show("Izaberite klijenta");
                        }

                    }
                    else
                    {
                        if (!textBox2.Text.Equals(""))
                        {

                            imeprezimeza = textBox2.Text;
                        }
                        
                        else
                        {
                            MessageBox.Show("Izaberite zaposlenog");

                        }

                        if (!textBox1.Text.Equals(""))
                        {
                           
                            imeprezimekl = textBox1.Text;

                        }
                        else
                        {
                            MessageBox.Show("Izaberite klijenta");

                        }
                    }


                }

                if (!idkk.Equals("") && !jmbgg.Equals(""))
                {
                   // MessageBox.Show(getIdProd() + " " + idkk + " " + jmbgg);
                    id = getIdProd() + "";
                    string jy = "insert into Prodaja(idprod, jmbg, idk, datum) values(" + getIdProd() + ", '" + jmbgg + "'," + idkk + ",'"+date+"')";
                    or.CommandText = jy;
                    or.Connection = dc;
                    or.ExecuteNonQuery();
                    df = true;

                    
                }

                else
                {
                    if (idkk.Equals("") && !jmbgg.Equals(""))
                    {
                        if (imeprezimekl.Equals(""))
                        {
                            //-----

                        }

                        else
                        {
                            if (exiIdk())
                            {
                                id = getIdProd() + "";
                                string jy = "insert into Prodaja(idprod, jmbg, idk, datum) values(" + getIdProd() + ", '" + jmbgg + "'," + textBox1.Text + ",'"+date+"')";
                                or.CommandText = jy;
                                or.Connection = dc;
                                or.ExecuteNonQuery();
                                df = true;

                            }

                            else
                            {

                                MessageBox.Show("Ovaj idk ne postoji u bazu");
                            }

                        }
   
                    }

                    if (jmbgg.Equals("") && !idkk.Equals(""))
                    {
                        if (imeprezimeza.Equals(""))
                        {
                            //-----

                        }

                        else
                        {

                            if (exist())
                            {
                                id = getIdProd() + "";
                                string jy = "insert into Prodaja(idprod, jmbg, idk, datum) values(" + getIdProd() + ", '" + textBox2.Text + "'," + idkk + ",'"+date+"')";
                                or.CommandText = jy;
                                or.Connection = dc;
                                or.ExecuteNonQuery();
                                df = true;

                            }

                            else
                            {

                                MessageBox.Show("Ovaj jmbg ne postoji u bazu");
                            }

                        }
                    }



                    if (jmbgg.Equals("") && idkk.Equals(""))
                    {
                        if (imeprezimeza.Equals(""))
                        {
                            //-----

                        }

                        else
                        {

                            if (exist())
                            {
                                if (imeprezimekl.Equals(""))
                                {
                                    //-----

                                }

                                else
                                {
                                    if (exiIdk())
                                    {
                                        id = getIdProd() + "";
                                        string jy = "insert into Prodaja(idprod, jmbg, idk, datum) values(" + getIdProd() + ", '" + textBox2.Text + "'," + textBox1.Text + ", '"+date+"')";
                                        or.CommandText = jy;
                                        or.Connection = dc;
                                        or.ExecuteNonQuery();
                                        df = true;

                                    }

                                    else
                                    {

                                        MessageBox.Show("Ovaj idk ne postoji u bazu");
                                    }

                                }

                            }

                            else
                            {

                                MessageBox.Show("Ovaj jmbg ne postoji u bazu");
                            }

                        }
                    }

                }


                if (df == true)
                {
                    button6.Enabled = false;
                    button1.Enabled = false;

                    this.Height = 610;

                    groupBox1.Visible = true;



                    comboBoxFill4();

                    


                }

               


                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }
        }

        private int getIdProd()
        {
            int idprod = 0;

            try
            {


                string go = "select max(idprod) from Prodaja";

                OracleCommand rv = new OracleCommand();

                rv.CommandText = go;
                rv.Connection = dc;


                var read = rv.ExecuteScalar().ToString();
                if (read.Equals(""))
                {
                    idprod = 0;
                }

                else
                {

                   idprod = int.Parse(read) + 1;

                }
                //MessageBox.Show(idprod+"");

                return idprod;

            }

            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }


            //MessageBox.Show(idprod + "");
            return idprod;

        }

        private string getIdkByUsername()
        {
            string jmbg = "";

            try
            {
                dc.Open();


                string go = "select jmbg from Zaposleni where korisnicko_ime='" + username + "'";

                OracleCommand rv = new OracleCommand();

                rv.CommandText = go;
                rv.Connection = dc;

                //MessageBox.Show(username);


                var read = rv.ExecuteScalar().ToString();
                
                if (read.Equals(""))
                {
                    jmbg = "";
                }

                else
                {

                    jmbg = read.ToString();
                    //MessageBox.Show(jmbg);

                }
                //MessageBox.Show(idprod+"");

                dc.Close();
                return jmbg;

               
            }

            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                dc.Close();
            }


            //MessageBox.Show(idprod + "");
            return jmbg;

        }


        private double getUkupno()
        {
            double idprod = 0;

            try
            {


                string go = "select k.ukupno from Klijent k, Prodaja p where k.idk=p.idk and p.idprod=" + id;

                OracleCommand rv = new OracleCommand();

                rv.CommandText = go;
                rv.Connection = dc;


                var read = rv.ExecuteScalar().ToString();
                if (read.Equals(""))
                {
                    idprod = 0;
                }

                else
                {

                    idprod = int.Parse(read);

                }
                //MessageBox.Show(idprod+"");

                return idprod;

            }






            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }


            //MessageBox.Show(idprod + "");
            return idprod;

        }

        private double getPrice(string idup)
        {
            double idprod = 0;

            try
            {

                dc.Open();


                string go = "select cijena from proizvod where idproiz=" + idup;

                OracleCommand rv = new OracleCommand();

                rv.CommandText = go;
                rv.Connection = dc;


                var read = rv.ExecuteScalar().ToString();
                if (read.Equals(""))
                {
                    idprod = 0;
                }

                else
                {

                    idprod = int.Parse(read);

                }
                //MessageBox.Show(idprod+"");
                dc.Close();

                return idprod;
            }






            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }


            //MessageBox.Show(idprod + "");
            return idprod;

        }

        private double selectSum(string idp)
        {
            double  idprod = 0;

            try
            {


                string go = "select sum(p.cijena*pr.kolicina) from Proizvod p, Proprod pr where p.idproiz = pr.idproiz and pr.idprod = " + idp;

                OracleCommand rv = new OracleCommand();

                rv.CommandText = go;
                rv.Connection = dc;


                var read = rv.ExecuteScalar().ToString();
                if (read.Equals(""))
                {
                    idprod = 0;
                }

                else
                {

                    idprod = int.Parse(read);

                }
                //MessageBox.Show(idprod+"");

                return idprod;

            }


            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
            }


            //MessageBox.Show(idprod + "");
            return idprod;

        }

        private void comboBoxFill(ComboBox com)
        {
            try
            {

                OracleDataAdapter o = new OracleDataAdapter("select idk, ime||' '||prezime as na from Klijent", dc);
                DataTable td = new DataTable();

                o.Fill(td);

                DataRow dr = td.NewRow();
                dr["idk"] = -1;
                dr["na"] = "Odaberi";


                td.Rows.InsertAt(dr, 0);


                com.DisplayMember = "na";
                com.ValueMember = "idk";
                com.DataSource = td;

            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        public void comboBoxFill2()
        {
            try
            {

                OracleDataAdapter o = new OracleDataAdapter("select jmbg, ime||' '||prezime as na from Zaposleni", dc);
                DataTable td = new DataTable();

                o.Fill(td);

                DataRow dr = td.NewRow();
                dr["jmbg"] = -1;
                dr["na"] = "Odaberi";


                td.Rows.InsertAt(dr, 0);


                comboBox2.DisplayMember = "na";
                comboBox2.ValueMember = "jmbg";
                comboBox2.DataSource = td;

            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        public void comboBoxFill4()
        {
            try
            {

                OracleDataAdapter o = new OracleDataAdapter("select idproiz as id, naziv as na, kolicina as ko  from Proizvod", dc);
                DataTable td = new DataTable();

                o.Fill(td);

                

                foreach (DataRow row in td.Rows)
                {
                    //MessageBox.Show(row[2]+"");

                    if (int.Parse(row[2]+"")==0)
                    {
                        row.Delete();
                    }
                        
                }
            

                DataRow dr = td.NewRow();
                dr["id"] = -1;
                dr["na"] = "Odaberi";


                td.Rows.InsertAt(dr, 0);


                comboBox3.DisplayMember = "na";
                comboBox3.ValueMember = "id";
                comboBox3.DataSource = td;

            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        public void comboBoxFill5(string idpr)
        {
            try
            {

                comboBox4.Items.Clear();
                dc.Open();
                OracleCommand or = new OracleCommand("select kolicina as na from Proizvod where idproiz="+idpr, dc);
                var read = or.ExecuteScalar().ToString();

                int no = int.Parse(read);

                for (int i = 1; i <= no; i++)
                {
                    comboBox4.Items.Add(i);
                }


                if (comboBox4.Items.Count != 0)
                {
                    comboBox4.SelectedIndex = 0;
                }
                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        private void Prodaja_Load(object sender, EventArgs e)
        {
            this.Width = 604;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;

            textBox9.Enabled = false;
            
            button3.Enabled = false;
            this.Height = 300;


            if (!username.Equals("admin"))
            {
                comboBox2.Enabled = false;
            }
            

           

            

            groupBox2.Visible = false;

            groupBox1.Visible = false;
            try
            {
                dc.Open();

                comboBoxFill(comboBox1);

                comboBoxFill2();


                dc.Close();

                comboBox2.SelectedValue = getIdkByUsername();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }

        }

        public bool exist()
        {

            try
            {

                string sele = "select * from Zaposleni where jmbg='" + textBox2.Text + "'";
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
                MessageBox.Show(jj.GetBaseException() + "");
                Application.Exit();
            }
            return false;

        }

        public bool exiIdk()
        {

            try
            {

                string sele = "select * from Klijent where idk=" + textBox1.Text;
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
                MessageBox.Show(jj.GetBaseException() + "");
                Application.Exit();
            }
            return false;

        }


        public bool existProizvod(string tt)
        {

            try
            {

                string sele = "select * from proprod where idproiz=" + tt + " and idprod=" + id;
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
                MessageBox.Show(jj.GetBaseException() + "");
                Application.Exit();
            }
            return false;

        }



        private void comboBox3_DisplayMemberChanged(object sender, EventArgs e)
        {
            try
            {
                

                if (comboBox3.SelectedIndex != 0)
                {
                    comboBoxFill5(comboBox3.SelectedValue.ToString());

                    textBox9.Text = getPrice(comboBox3.SelectedValue.ToString()) + " €";


                }

               
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();

                if (comboBox3.SelectedIndex != 0)
                {
                    if (!existProizvod(comboBox3.SelectedValue.ToString()))
                {
                    //MessageBox.Show(comboBox3.SelectedValue.ToString() + ", " + id + ", " + comboBox4.SelectedItem.ToString());
                   
                        //MessageBox.Show()
                        string sele = "insert into proprod(idproiz, idprod, kolicina) values(" + comboBox3.SelectedValue.ToString() + ", " + id + ", " + comboBox4.SelectedItem.ToString() + ")";
                        OracleCommand or = new OracleCommand();
                        or.CommandText = sele;
                        or.Connection = dc;

                        //idpro.Add(comboBox3.SelectedValue.ToString());

                        or.ExecuteNonQuery();

                        sele = "update Proizvod set kolicina=kolicina-" + comboBox4.SelectedItem.ToString() + "where idproiz=" + comboBox3.SelectedValue.ToString();
                        or.CommandText = sele;
                        or.Connection = dc;
                        or.ExecuteNonQuery();

                        button3.Enabled = true;

                        comboBox3.SelectedIndex = 0;
                        comboBox4.Items.Clear();
                    }

                    else
                    {
                        MessageBox.Show("Vec ste dodali ovaj proizvod");
                    }


                }
                

                else
                    {
                    MessageBox.Show("Izaberite proizvod koji zelite da dodate");
                }






                dc.Close();

                comboBoxFill4();

                fillGrid(dataGridView1, id);
            }
            catch (Exception jj)
            {
                MessageBox.Show(jj.GetBaseException() + "");
                Application.Exit();
            }
        }

        public void fillGrid(DataGridView dg, string ii)
        {
            try
            {
                dc.Open();

                string st = "select p.naziv as naziv, p.cijena as \"Cijena Proizvoda\", pr.kolicina as kolicina, pr.kolicina*p.cijena as \"Ukupna Cijena\" from Proizvod p, Proprod pr where pr.idproiz = p.idproiz and pr.idprod=" + ii;
                OracleDataAdapter od = new OracleDataAdapter(st, dc);
                DataTable dt = new DataTable();

                od.Fill(dt);

                dg.DataSource = dt;

                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;

            groupBox1.Visible = false;

            groupBox2.Visible = true;

            this.Height = 674;


            try
            {
                dc.Open();
                double ukupnaCijena = selectSum(id);

                double ukupnoKlijent = getUkupno();

                if ((ukupnaCijena + ukupnoKlijent)>200)
                {
                    popust = 10;
                }

                if ((ukupnaCijena + ukupnoKlijent) > 500)
                {
                    popust = 20;
                }

                if ((ukupnaCijena + ukupnoKlijent) > 1000)
                {
                    popust = 30;
                }
                //MessageBox.Show(ukupnaCijena + "");
                double cijenaPopust = ukupnaCijena - ukupnaCijena * popust / 100;

                double popustCijena = ukupnaCijena * popust / 100;

                double updateUkupno = ukupnoKlijent + cijenaPopust;
                string prod = "update Prodaja set popust=" + popust + " where idprod=" + id;
                OracleCommand or = new OracleCommand();
                or.CommandText = prod;
                or.Connection = dc;
                int n= or.ExecuteNonQuery();
                //MessageBox.Show(n + "");

                or.CommandText = "update Klijent set ukupno=" + updateUkupno + " where idk in(select k.idk from Klijent k, Prodaja pr where k.idk=pr.idk and pr.idprod=" + id + ")";
                or.Connection = dc;
                or.ExecuteNonQuery();
                

                label14.Text = ukupnaCijena + "";
                label18.Text = popustCijena + "";
                label17.Text = cijenaPopust + "";

                dc.Close();

                fillData(id);
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
             
                
            }
           





        }

        public void fillData(string idp)
        {
            try
            {
                dc.Open();
                string te = "select p.ime||' '||p.prezime as kupac, r.ime||' '||r.prezime as prodavac, prod.popust from Prodaja prod, klijent p, ZAPOSLENI r where prod.IDK = p.idk and r.jmbg = prod.jmbg and prod.idprod =" + idp;

                OracleCommand rv = new OracleCommand();
                rv.CommandText = te;
                rv.Connection = dc; 

                var read = rv.ExecuteReader();

                while (read.Read())
                {
                    if (read[0] != DBNull.Value)
                    {
                        textBox3.Text = read.GetString(0);
                    }
                    else
                    {
                        textBox3.Text = "";
                    }
                    if (read[1] != DBNull.Value)
                    {
                        textBox4.Text = read.GetString(1);
                    }
                    else
                    {
                        textBox4.Text = "";
                    }
                }

                textBox5.Text = popust + " %";
                    dc.Close();
                fillGrid(dataGridView2, idp);
            }
            catch (Exception)
            {

               
            }
 
            }

        public void fillData2(string dd)
        {
            try
            {
                dc.Open();
                string te = "select p.ime||' '||p.prezime as kupac, r.ime||' '||r.prezime as prodavac, prod.popust from Prodaja prod, klijent p, ZAPOSLENI r where prod.IDK = p.idk and r.jmbg = prod.jmbg and prod.idprod =" + dd;

                OracleCommand rv = new OracleCommand();
                rv.CommandText = te;
                rv.Connection = dc;

                var read = rv.ExecuteReader();
                decimal popust = 0;
                while (read.Read())
                {
                    if (read[0] != DBNull.Value)
                    {
                        textBox8.Text = read.GetString(0);
                    }
                    else
                    {
                        textBox8.Text = "";
                    }
                    if (read[1] != DBNull.Value)
                    {
                        textBox7.Text = read.GetString(1);
                    }
                    else
                    {
                        textBox7.Text = "";
                    }

                    if (read[2] != DBNull.Value)
                    {
                        popust = read.GetDecimal(2);
                        textBox6.Text = read.GetDecimal(2) + " %";
                    }
                    else
                    {
                        textBox6.Text = "";
                    }
                }

                double ukupno = selectSum(dd);

                label21.Text = ukupno + "";


                double popustC = ukupno * (double)popust / 100;

                label5.Text = popustC + "";

                double bezP = ukupno - popustC;

                label7.Text = bezP + "";




               
                dc.Close();
                fillGrid(dataGridView4, dd);
            }
            catch (Exception)
            {


            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da zelite da otkazete trgovinu ?", "Sure", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                closeOrder();
            }
            
        }

        public void closeOrder()
        {

            try
            {
                dc.Open();

                
                    button3.Enabled = false;
                    string dlle = "select p.idproiz, pr.kolicina from Proizvod p, PROPROD pr where p.idproiz = pr.idproiz and pr.idprod = " + id;
                    OracleCommand co = new OracleCommand();
                    co.CommandText = dlle;
                    co.Connection = dc;

                    var reader = co.ExecuteReader();

                    while (reader.Read())
                    {
                        dlle = "update Proizvod set kolicina=kolicina+" + reader.GetDecimal(1) + " where idproiz=" + reader.GetDecimal(0);
                        co.CommandText = dlle;
                        co.Connection = dc;
                        co.ExecuteNonQuery();
                    }

                    string commi = "commit";
                    co.CommandText = commi;
                    co.Connection = dc;
                    co.ExecuteNonQuery();



                    dlle = "delete from proprod where idprod=" + id;


                    co.CommandText = dlle;
                    co.Connection = dc;


                    co.ExecuteNonQuery();


                    co.CommandText = commi;
                    co.Connection = dc;
                    co.ExecuteNonQuery();



                    dlle = "delete from prodaja where idprod=" + id;

                    co.CommandText = dlle;
                    co.Connection = dc;


                    co.ExecuteNonQuery();

                    co.CommandText = commi;
                    co.Connection = dc;
                    co.ExecuteNonQuery();

                    clearAll();

                    groupBox1.Visible = false;




                
                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        public void clearAll()
        {
            button1.Enabled = true;
            button6.Enabled = true;
            this.Width = 604;
            //idpro.Clear();
            popust = 0;
            comboBox1.SelectedIndex = 0;
            //comboBox2.SelectedIndex = 0;
            if (comboBox3.Items.Count != 0)
            {
                comboBox3.SelectedIndex = 0;
            }
            comboBox4.Items.Clear();
            

            textBox1.Text = "";
            textBox2.Text = "";

            dataGridView1.DataSource = null;

            button3.Enabled = false;
            groupBox2.Visible = false;

            this.Height = 300;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.Width == 1200)
            {
                this.Width = 604;
                clearAll();
            }
            else
            {
                this.Width = 1200;

            }

            


            try
            {
                dc.Open();
                comboBoxFill(comboBox5);


                OracleDataAdapter od = new OracleDataAdapter("select prod.idprod as ID, prod.datum, prod.popust from Prodaja prod, Klijent k where k.idk=prod.idk", dc);
                    DataTable dt = new DataTable();

                    od.Fill(dt);

                    dataGridView3.DataSource = dt;

                
                



                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Height = 644;
            try
            {
                dc.Open();

                int index = dataGridView3.SelectedCells[0].RowIndex;


                DataGridViewRow dr = dataGridView3.Rows[index];

                string jm = dr.Cells[0].Value.ToString();

                //MessageBox.Show(jm);




                dc.Close();

                fillData2(jm);
            }
            catch (Exception)
            {


            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                dc.Open();
                if (comboBox5.SelectedIndex != 0)
                {
                    OracleDataAdapter od = new OracleDataAdapter("select prod.idprod as ID, prod.datum, prod.popust from Prodaja prod, Klijent k where k.idk=prod.idk and k.idk=" + comboBox5.SelectedValue.ToString(), dc);
                    DataTable dt = new DataTable();

                    od.Fill(dt);

                    dataGridView3.DataSource = dt;

                }
                else
                {
                    string ss = "select prod.idprod as ID, prod.datum, prod.popust from Prodaja prod, Klijent k where k.idk=prod.idk";
                   OracleDataAdapter od = new OracleDataAdapter(ss, dc);
                    DataTable dt = new DataTable();

                    od.Fill(dt);

                    dataGridView3.DataSource = dt;

                }

                

                dc.Close();
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.GetBaseException() + "");
                Application.Exit();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void Prodaja_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Height == 610)
            {
                closeOrder();

            }
        }
    }
}
