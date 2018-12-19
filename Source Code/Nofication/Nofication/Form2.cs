using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Nofication
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\notify.accdb";
            con.Open();
            OleDbDataAdapter sda = new OleDbDataAdapter("SELECT Title, Description, Due_Date,Days FROM [notify]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string title = dataGridView1.Rows[row.Index].Cells["Title"].Value + "";
                string desc = dataGridView1.Rows[row.Index].Cells["Description"].Value + "";
                Double d = Convert.ToDouble(dataGridView1.Rows[row.Index].Cells["Days"].Value + "");
                DateTime duedate = DateTime.Parse(dataGridView1.Rows[row.Index].Cells["Due_Date"].Value + "", System.Globalization.CultureInfo.CurrentCulture);
                DateTime subday = duedate.AddDays(-d);
                if (DateTime.Today.Date >= subday.Date)
                {
                    //label2.Text = label2.Text + "\nTitle: " + title + "\nDescription: " + desc + "\nDue in: " + duedate.ToString("dd-MM-yyyy") + "\n";
                    listBox1.Items.Add("Title: " + title);
                    listBox1.Items.Add("Description: " + desc);
                    listBox1.Items.Add("\nDue in: " + duedate.ToString("dd-MM-yyyy"));
                    listBox1.Items.Add("");
                }               
            }
        }
    }
}
