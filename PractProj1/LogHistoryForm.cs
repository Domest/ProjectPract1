using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PractProj1.Models;
using System.Collections;

namespace PractProj1
{
    public partial class LogHistoryForm : Form
    {
        public LogHistoryForm()
        {
            InitializeComponent();
        }
        public void LoadLogToDataGrid( List<LogHisModel> GetList)
        {
            dataGridLog.DataSource = GetList;
        }
    }
}
