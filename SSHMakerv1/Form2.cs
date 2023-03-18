using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSHMakerv1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SQLiteConnection sqlite_conn;

            SQLiteCommand sqlite_cmd;

            SQLiteDataReader sqlite_datareader;

            try
            {

                // create a new database connection:

                sqlite_conn = new SQLiteConnection("Data Source=data.db;Version=3;New=True;Compress=True;");

                // open the connection:

                sqlite_conn.Open();

                // create a new SQL command:

                sqlite_cmd = sqlite_conn.CreateCommand();

                sqlite_cmd.CommandText = "SELECT * From main.Users";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                int i = 0;

                dataGridView1.Rows.Add(sqlite_datareader.StepCount);
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read

                {

                    // Print out the content of the text field:

                    //System.Console.WriteLine( sqlite_datareader["text"] );
                    dataGridView1[0, i].Value = sqlite_datareader["username"];
                    dataGridView1[1, i].Value = sqlite_datareader["password"];
                    dataGridView1[2, i].Value = sqlite_datareader["date"];
                    dataGridView1[3, i].Value = sqlite_datareader["limit"];
                    dataGridView1[4, i].Value = sqlite_datareader["ip"];

                    i++;

                }

                // We are ready, now lets cleanup and close our connection:

                sqlite_conn.Close();
            }
            catch (Exception t)
            {
                MessageBox.Show(t.Message);
            }
        }
    }
}
