using System.Collections.Generic;
using System.Windows.Forms;
using PractProj1.Models;

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
