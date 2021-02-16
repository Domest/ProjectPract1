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
using PractProj1.Lists;
using PractProj1.Models;

namespace PractProj1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            try
            {
                InitializeComponent();
                LoggerProc.createLoggerConfig();
                LoadXMLData("", "", false);
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "NumCode";
                comboBox1.DataSource = cbl.CBList;
                dateTimePicker1_ValueChanged(dateTimePicker1, null);
                dateTimePicker2_ValueChanged(dateTimePicker2, null);
                LoggerProc.logger.Info("Инициализация компонентов завершена");
                AddLogToHistory("Инициализация компонентов завершена", false);
            }
            catch(Exception e)
            {
                LoggerProc.logger.Error("Ошибка инициализации. " + e);
                AddLogToHistory("Ошибка инициализации. " + e, true);
            }
            
        }
        LoggerProc log = new LoggerProc();
        DateTime GetDateRangeBegin;
        DateTime GetDateRangeEnd;
        RBKSendList rsl = new RBKSendList();
        ComboBoxList cbl = new ComboBoxList();
        LogHistoryForm lhf = new LogHistoryForm();
        DBContext context = new DBContext();
        LogHistoryList lhl = new LogHistoryList();
        string SelectedValute;
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
                            ID = i+1,
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
                            ID = i+1,
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
                LoggerProc.logger.Info("Загрузка валют с сервиса завершена. Загружено " + rsl.SendList.Count + " объектов");
                AddLogToHistory("Загрузка валют с сервиса завершена. Загружено " + rsl.SendList.Count + " объектов", false);
            }
            catch(Exception f)
            {
                MessageBox.Show("Данные не получены");
                LoggerProc.logger.Error("Загрузка валютных данных с сервиса ОСТАНОВЛЕНА. " + f);
                AddLogToHistory("Загрузка валютных данных с сервиса ОСТАНОВЛЕНА. " + f, true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                context = new DBContext();
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
                LoggerProc.logger.Info("Загрузка данных в БД успешно завершена");
                AddLogToHistory("Загрузка данных в БД успешно завершена", false);
            }
            catch(Exception f)
            {
                LoggerProc.logger.Error("Произошла ошибка загрузки данных в БД. " + f);
                AddLogToHistory("Произошла ошибка загрузки данных в БД. " + f, true);
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
            string UrlBegin = UrlBase + GetDateRangeBegin.ToString("dd/MM/yyyy").Replace(".", "/");
            GetDateRangeEnd = dateTimePicker2.Value;
            string UrlEnd = UrlBase + GetDateRangeEnd.ToString("dd/MM/yyyy").Replace(".", "/");
            LoadXMLData(UrlBegin, UrlEnd, true);
            dataGridView1.DataSource = rsl.SendList;
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                if (dc.ValueType == typeof(System.DateTime))
                    dc.DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new DBContext())
                {
                    rsl.SendList = new List<SendModel>();
                    for (int i = 1; ; i++)
                    {
                        var query = context.ParsData.Where(s => s.ID == i).FirstOrDefault();
                        if (query != null)
                        {
                            rsl.SendList.Add(new SendModel() { ID = query.ID, Name = query.Name, Date = query.Date, Nominal = query.Nominal, Value = query.Value, NumCode = query.NumCode, CharCode = query.CharCode });
                        }
                        else
                            break;
                    }
                    dataGridView1.DataSource = rsl.SendList;
                }
                LoggerProc.logger.Info("Данные из БД успешно выгружены в таблицу. Получено " + rsl.SendList.Count + " объектов");
                AddLogToHistory("Данные из БД успешно выгружены в таблицу. Получено " + rsl.SendList.Count + " объектов", false);
            }
            catch(Exception f)
            {
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBox.Show(Convert.ToString(f), "Ошибка", btn);
                LoggerProc.logger.Error("Ошибка загрзуки данных из БД. " + f);
                AddLogToHistory("Ошибка загрзуки данных из БД. " + f, true);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ComboBoxModel sd in cbl.CBList)
            {
                if (sd.NumCode == Convert.ToString(comboBox1.SelectedValue))
                {
                    SelectedValute = sd.Name;
                    break;
                }
            }
        }

        void LoadXMLData(string UBegin, string UEnd, bool IsRange)
        {
            string html;
            string url = "http://www.cbr.ru/scripts/XML_daily.asp";
            if (IsRange != true)
            {
                try
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
                    string f = System.IO.Path.Combine(Environment.CurrentDirectory, "data.xaml");
                    using (StreamReader sr = new StreamReader(f))
                    {
                        ValCurs ReadData = (ValCurs)xloa.Deserialize(sr);
                        LoadSer = ReadData;
                    }
                    for (int i = 0; i < LoadSer.Valute.Length - 1; i++)
                    {
                        cbl.CBList.Add(new ComboBoxModel()
                        {
                            NumCode = Convert.ToString(LoadSer.Valute[i].NumCode),
                            Name = LoadSer.Valute[i].Name
                        });
                    }
                    LoggerProc.logger.Info("Данные с сервиса успешно скачаны");
                    AddLogToHistory("Данные с сервиса успешно скачаны", false);
                }
                catch(Exception e)
                {
                    LoggerProc.logger.Error("Ошибка скачивания данных с сервиса. " + e);
                    AddLogToHistory("Ошибка скачивания данных с сервиса. " + e, true);
                }
            }
            else
            {
                try
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
                                {
                                    rsl.SendList.Add(new SendModel() { Date = CurrentTime, Name = v.Name, Nominal = Convert.ToString(v.Nominal), Value = v.Value, NumCode = Convert.ToString(v.NumCode), CharCode = v.CharCode });
                                    break;
                                }
                            }
                            else
                            {
                                rsl.SendList = new List<SendModel>();
                                if (SelectedValute == v.Name)
                                {
                                    rsl.SendList.Add(new SendModel() { Date = CurrentTime, Name = v.Name, Nominal = Convert.ToString(v.Nominal), Value = v.Value, NumCode = Convert.ToString(v.NumCode), CharCode = v.CharCode });
                                }
                            }
                        }
                    }
                    LoggerProc.logger.Info("Диапазон по валюте " + SelectedValute + " с " + GetDateRangeBegin + " по " + GetDateRangeEnd + " успешно получены. Загружено " + rsl.SendList.Count + " объектов");
                    AddLogToHistory("Диапазон по валюте " + SelectedValute + " с " + GetDateRangeBegin + " по " + GetDateRangeEnd + " успешно получены. Загружено " + rsl.SendList.Count + " объектов", false);
                }
                catch(Exception e)
                {
                    LoggerProc.logger.Error("При попытке получить диапазон по " + SelectedValute + " с " + GetDateRangeBegin + " по " + GetDateRangeEnd + " произошла ошибка. " + e);
                    AddLogToHistory("При попытке получить диапазон по " + SelectedValute + " с " + GetDateRangeBegin + " по " + GetDateRangeEnd + " произошла ошибка. " + e, true);
                }
            }
        }
        public void AddLogToHistory(string GetLogMessage, bool isError)
        {
            DateTime LogT = DateTime.Now;
            if (lhl.LogList != null)
            {
                if (isError == false)
                {
                    lhl.LogList.Add(new LogHisModel(GetLogMessage, "Информация", LogT));
                }
                else
                    lhl.LogList.Add(new LogHisModel(GetLogMessage, "Ошибка", LogT));
            }
            lhf.LoadLogToDataGrid(lhl.LogList);
        }

        private void History_Click(object sender, EventArgs e)
        {
            lhf.ShowDialog();
            lhf.Hide();
        }
    }
}
