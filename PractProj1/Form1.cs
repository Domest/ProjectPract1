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

                //XmlSerializer xsav = new XmlSerializer(typeof(ValCursValute));
                //string FileName = "datatest.xaml";
                //string WritePath = System.IO.Path.Combine(Environment.CurrentDirectory, FileName);
                //using (StreamWriter sw = new StreamWriter(WritePath, false, System.Text.Encoding.UTF8))
                //{
                //    xsav.Serialize(sw, html);
                //}

                FileStream file = new FileStream("data.xaml", FileMode.Create, FileAccess.ReadWrite);
                StreamWriter wData = new StreamWriter(file);
                wData.Write(html);
                wData.Close();

                ServiceReference1.DailyInfoSoapClient scr = new DailyInfoSoapClient();
                DataSet ds = scr.GetCursOnDate(DateTime.Now.Date);
                dataGridView1.DataSource = ds.Tables[0];
                rsl.SendList = null;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (rsl.SendList != null)
                    {
                        rsl.SendList.Add(new SendModel()
                        {
                            Name = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
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
                            Name = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            Nominal = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            Value = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            NumCode = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            CharCode = ds.Tables[0].Rows[i].ItemArray[4].ToString()
                        });
                    }
                }
            }
            catch
            {
                MessageBox.Show("Данные не получены");
            }

    //RBKServise.DailyInfo di = new RBKServise.DailyInfo();

    //System.DateTime DateFrom, DateTo;
    //DateFrom = dateTimePicker1.Value;
    //DateTo = dateTimePicker2.Value;

    ////Вызываем GetCursDynamic для получения таблицы с курсами заданной валютой
    //DataSet Ds = (System.Data.DataSet)di.GetCursDynamic(DateFrom, DateTo, "R01235");
    //Ds.Tables[0].Columns[0].ColumnName = "Дата";
    //Ds.Tables[0].Columns[1].ColumnName = "Вн.код валюты";
    //Ds.Tables[0].Columns[2].ColumnName = "Номинал";
    //Ds.Tables[0].Columns[3].ColumnName = "Курс";

    //dataGrid1.SetDataBinding(Ds, "ValuteCursDynamic");
}

        private void button2_Click(object sender, EventArgs e)
        {
            //ValCurs LoadSer;
            //XmlSerializer xloa = new XmlSerializer(typeof(ValCurs));
            //string f = @"C:\Users\Demid\Desktop\GitProj\ProjectPract1\PractProj1\bin\Debug\data.xaml";
            //using (StreamReader sr = new StreamReader(f))
            //{
            //    ValCurs ReadData = (ValCurs)xloa.Deserialize(sr);
            //    LoadSer = ReadData;
            //}

            //dataGridView1.Columns.Add("b1", "ID");
            //dataGridView1.Columns.Add("b2", "Название");
            //dataGridView1.Columns.Add("b3", "Ценность");
            //dataGridView1.Columns.Add("b4", "Кодовый номер");
            //dataGridView1.Columns.Add("b5", "Код");
            //dataGridView1.Columns.Add("b6", "Номинал");
            //for (int i = 0; i < LoadSer.Valute.Length-1; i++)
            //{

            //    dataGridView1.Rows.Add(LoadSer.Valute[i].ID, LoadSer.Valute[i].Name, LoadSer.Valute[i].Value, LoadSer.Valute[i].NumCode, LoadSer.Valute[i].CharCode, LoadSer.Valute[i].Nominal);
            //    //dataGridView1.Rows.Add(LoadSer.Valute[i].Value);
            //    //dataGridView1.Rows[i].Cells[i].Value = LoadSer.Valute[i].;
            //}
            if (rsl.SendList != null)
            {
                using (context)
                {
                    foreach (SendModel f in rsl.SendList)
                    {
                        var valute = new SendModel()
                        {
                            Name = f.Name,
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
            //using (context)
            //{
            //    foreach (SendModel f in rsl.SendList)
            //    {
            //        var valute = new SendModel()
            //        {
            //            Name = f.Name,
            //            Nominal = f.Nominal,
            //            Value = f.Value,
            //            NumCode = f.NumCode,
            //            CharCode = f.CharCode
            //        };
            //        context.ParsData.Add(valute);
            //    }

            //}
            context.ParsData.Load();
            var valutes = context.ParsData;
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

                for (int i = 0; CurrentTime < GetDateRangeEnd; i++) //while(CurrentTime > GetDateRangeEnd) 
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
                    string f = System.IO.Path.Combine(Environment.CurrentDirectory, "datarange.xaml"); //@"C:\Users\Demid\Desktop\GitProj\ProjectPract1\PractProj1\bin\Debug\datarange.xaml";
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
