
namespace PractProj1
{
    partial class LogHistoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridLog = new System.Windows.Forms.DataGridView();
            this.NameC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogTypeC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLog)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridLog
            // 
            this.dataGridLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameC,
            this.LogTypeC,
            this.TimeC});
            this.dataGridLog.Location = new System.Drawing.Point(12, 12);
            this.dataGridLog.Name = "dataGridLog";
            this.dataGridLog.Size = new System.Drawing.Size(452, 426);
            this.dataGridLog.TabIndex = 0;
            
            // 
            // NameC
            // 
            this.NameC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NameC.DataPropertyName = "Name";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NameC.DefaultCellStyle = dataGridViewCellStyle1;
            this.NameC.HeaderText = "Описание";
            this.NameC.Name = "NameC";
            this.NameC.Width = 200;
            // 
            // LogTypeC
            // 
            this.LogTypeC.DataPropertyName = "LogType";
            this.LogTypeC.HeaderText = "Тип лога";
            this.LogTypeC.Name = "LogTypeC";
            this.LogTypeC.Width = 90;
            // 
            // TimeC
            // 
            this.TimeC.DataPropertyName = "LogTime";
            dataGridViewCellStyle2.Format = "G";
            dataGridViewCellStyle2.NullValue = "-";
            this.TimeC.DefaultCellStyle = dataGridViewCellStyle2;
            this.TimeC.HeaderText = "Время";
            this.TimeC.Name = "TimeC";
            
            this.TimeC.Width = 117;
            this.dataGridLog.Columns["TimeC"].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LogHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 450);
            this.Controls.Add(this.dataGridLog);
            this.Name = "LogHistoryForm";
            this.Text = "LogHistoryForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameC;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogTypeC;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeC;
    }
}