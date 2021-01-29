using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using PractProj1.ServiceReference1;
using System.Data.Entity;

namespace PractProj1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadXMLData("", "", false);
            comboBox1.DataSource = rsl.SendList;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Name";
            dateTimePicker1_ValueChanged(dateTimePicker1, null);
            dateTimePicker2_ValueChanged(dateTimePicker2, null);
        }
        public DateTime GetDateRangeBegin;
        public DateTime GetDateRangeEnd;
        public RBKSendList rsl = new RBKSendList();
        DBContext context = new DBContext();
        public string SelectedValute;
        string UrlBase = "https://www.cbr.ru/scripts/XML_daily.asp?date_req=";
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string html;
                string url = "http://www.cbr.ru/scripts/XML_daily.asp";

                WebClient web = new WebClient();
                web.Encoding = Encoding.GetEncoding(1251);
                Stream data = web.OpenRead(url);
                StreamReader reader = new StreamReader(data, Encoding.GetEncoding(1251));
                html = reader.ReadLine();
                data.Close();
                reader.Close();

                FileStream file = new FileStream("data.xaml", FileMode.Create, FileAccess.ReadWrite);
                StreamWriter wData = new StreamWriter(file);
                wData.Write(html);
                wData.Close();

                ServiceReference1.DailyInfoSoapClient scr = new DailyInfoSoapClient();
                DataSet ds = scr.GetCursOnDate(DateTime.Now.Date);
                rsl.SendList = null;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (rsl.SendList != null)
                    {
                        rsl.SendList.Add(new SendModel()
                        {
                            //ID = i,
                            Name = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            Date = DateTime.Now.Date,
                            Nominal = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            Value = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            NumCode = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            CharCode = ds.Tables[0].Rows[i].ItemArray[4].ToString()
                        });
                    }
                    else
                    {
                        rsl.SendList = new List<SendModel>();
                        rsl.SendList.Add(new SendModel()
                        {
                            ID = i,
                            Name = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            Date = DateTime.Now.Date,
                            Nominal = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            Value = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            NumCode = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            CharCode = ds.Tables[0].Rows[i].ItemArray[4].ToString()
                        });
                    }
                }
                dataGridView1.DataSource = rsl.SendList;
            }
            catch
            {
                MessageBox.Show("Данные не получены");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [valute_curs]");
            if (rsl.SendList != null)
            {
                using (context)
                {
                    foreach (SendModel f in rsl.SendList)
                    {
                        var valute = new SendModel()
                        {
                            Name = f.Name,
                            Date = f.Date,
                            Nominal = f.Nominal,
                            Value = f.Value,
                            NumCode = f.NumCode,
                            CharCode = f.CharCode
                        };
                        context.ParsData.Add(valute);
                        context.SaveChanges();
                    }
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GetDateRangeBegin = dateTimePicker1.Value;
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            GetDateRangeEnd = dateTimePicker2.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            //GetDateRangeBegin.ToString("MM/dd/yyyy").Replace(".","/");
            string UrlBegin = UrlBase + GetDateRangeBegin.ToString("dd/MM/yyyy").Replace(".", "/");
            GetDateRangeEnd = dateTimePicker2.Value;
            string UrlEnd = UrlBase + GetDateRangeEnd.ToString("dd/MM/yyyy").Replace(".", "/");
            LoadXMLData(UrlBegin, UrlEnd, true);
            dataGridView1.DataSource = rsl.SendList;
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                if (dc.ValueType == typeof(System.DateTime))
                {
                    dc.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            context = new DBContext();
            context.ParsData.Load();

            
            //var valutes = context.ParsData;
            //using (context)
            //{
            //    var GetValutes = context.ParsData.Include(x => x.ID).ToList();
            //    rsl.SendList = GetValutes;
            //}
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedValute = rsl.SendList[comboBox1.SelectedIndex].Name;
        }

        void LoadXMLData(string UBegin, string UEnd, bool IsRange)
        {
            string html;
            string url = "http://www.cbr.ru/scripts/XML_daily.asp";
            if (IsRange != true)
            {
                WebClient web = new WebClient();
                web.Encoding = Encoding.GetEncoding(1251);
                Stream data = web.OpenRead(url);
                StreamReader reader = new StreamReader(data, Encoding.GetEncoding(1251));
                html = reader.ReadLine();
                data.Close();
                reader.Close();

                FileStream file = new FileStream("data.xaml", FileMode.Create, FileAccess.ReadWrite);
                StreamWriter wData = new StreamWriter(file);
                wData.Write(html);
                wData.Close();

                ValCurs LoadSer;
                XmlSerializer xloa = new XmlSerializer(typeof(ValCurs));
                string f = @"C:\Users\Demid\Desktop\GitProj\ProjectPract1\PractProj1\bin\Debug\data.xaml";
                using (StreamReader sr = new StreamReader(f))
                {
                    ValCurs ReadData = (ValCurs)xloa.Deserialize(sr);
                    LoadSer = ReadData;
                }
                for (int i = 0; i < LoadSer.Valute.Length - 1; i++)
                {
                    rsl.SendList.Add(new SendModel()
                    {
                        Name = LoadSer.Valute[i].Name,
                        Value = LoadSer.Valute[i].Value,
                        NumCode = Convert.ToString(LoadSer.Valute[i].NumCode),
                        CharCode = LoadSer.Valute[i].CharCode,
                        Nominal = Convert.ToString(LoadSer.Valute[i].Nominal)
                    });
                }
            }
            else
            {
                DateTime CurrentTime = GetDateRangeBegin;
                string CurrentUrl = UBegin;
                rsl.SendList = null;

                for (int i = 0; CurrentTime < GetDateRangeEnd; i++)
                {
                    if (i > 0)
                    {
                        CurrentTime = CurrentTime.AddDays(1);
                        CurrentUrl = UrlBase + CurrentTime.ToString("dd/MM/yyyy").Replace(".", "/");
                    }
                    WebClient web = new WebClient();
                    web.Encoding = Encoding.GetEncoding(1251);
                    Stream data = web.OpenRead(CurrentUrl);
                    StreamReader reader = new StreamReader(data, Encoding.GetEncoding(1251));
                    html = reader.ReadLine();
                    data.Close();
                    reader.Close();

                    FileStream file = new FileStream("datarange.xaml", FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter wData = new StreamWriter(file);
                    wData.Write(html);
                    wData.Close();

                    ValCurs LoadSer;
                    XmlSerializer xloa = new XmlSerializer(typeof(ValCurs));
                    string f = System.IO.Path.Combine(Environment.CurrentDirectory, "datarange.xaml");
                    using (StreamReader sr = new StreamReader(f))
                    {
                        ValCurs ReadData = (ValCurs)xloa.Deserialize(sr);
                        LoadSer = ReadData;
                    }

                    foreach (ValCursValute v in LoadSer.Valute)
                    {
                        if (rsl.SendList != null)
                        {
                            if (SelectedValute == v.Name)
                                rsl.SendList.Add(new SendModel() { Date = CurrentTime, Name = v.Name, Nominal = Convert.ToString(v.Nominal), Value = v.Value, NumCode = Convert.ToString(v.NumCode), CharCode = v.CharCode });
                        }
                        else
                        {
                            rsl.SendList = new List<SendModel>();
                            rsl.SendList.Add(new SendModel() { Date = CurrentTime, Name = v.Name, Nominal = Convert.ToString(v.Nominal), Value = v.Value, NumCode = Convert.ToString(v.NumCode), CharCode = v.CharCode });
                        }
                    }
                }
            }
        }
    }
}
