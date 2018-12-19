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
    public partial class Form1 : Form
    {        
        public Form1()
        {           
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\notify.accdb";
            con.Open();
            OleDbDataAdapter sda = new OleDbDataAdapter("SELECT Title, Description, Due_Date,Days FROM [notify]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Double d = Convert.ToDouble(dataGridView1.Rows[row.Index].Cells["Days"].Value + "");               
                DateTime duedate = DateTime.Parse(dataGridView1.Rows[row.Index].Cells["Due_Date"].Value + "", System.Globalization.CultureInfo.CurrentCulture);
                DateTime subday = duedate.AddDays(-d);
                if (DateTime.Today.Date >= subday.Date)
                {
                    Form2 f2 = new Form2();
                    f2.ShowDialog();
                    break;
                }
                else
                {                    
                    WindowState = FormWindowState.Minimized;                  
                    Hide();
                }
            }
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Title can't be blank.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (richTextBox1.Text == "")
            {
                MessageBox.Show("Description Required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (textBox2.Text == "")
            {
            }
            else
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\notify.accdb";
                con.Open();
                String query = "INSERT INTO [notify] (Title,Description,Due_Date,Days) values(?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("?", textBox1.Text);
                cmd.Parameters.AddWithValue("?", richTextBox1.Text);
                cmd.Parameters.AddWithValue("?", dateTimePicker1.Text);
                cmd.Parameters.AddWithValue("?", textBox2.Text);
                cmd.ExecuteNonQuery();
                con.Close();
               
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\notify.accdb";
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("SELECT Title, Description, Due_Date,Days FROM [notify]", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

                MessageBox.Show("Record successfully added", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = "";
                richTextBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure?\nYou want remove this notification?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    OleDbConnection con = new OleDbConnection();
                    con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\notify.accdb";
                    con.Open();
                    String query = "DELETE FROM [notify] WHERE Title=?";
                    OleDbCommand cmd = new OleDbCommand(query, con);
                    cmd.Parameters.AddWithValue("?", dataGridView1.Rows[e.RowIndex].Cells[1].Value + "");
                    cmd.ExecuteNonQuery();
                    con.Close();

                    con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\notify.accdb";
                    con.Open();
                    OleDbDataAdapter sda = new OleDbDataAdapter("SELECT Title, Description, Due_Date,Days FROM [notify]", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.DataSource = dt;
                    con.Close();

                    MessageBox.Show("Record Deleted", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int j;
            if (int.TryParse(textBox2.Text, out j))
            {
                if (j > 10)
                { textBox2.Text = ""; MessageBox.Show("Max day is 10", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                else if (j < 1) { textBox2.Text = ""; MessageBox.Show("Min day is 1", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            else { textBox2.Text = ""; }
        }

        private void showDuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }
    }
}
