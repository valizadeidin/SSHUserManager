using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSHMakerv1
{
    public partial class Form1 : Form
    {
        string ip=null, pass=null, user=null, port=null,f;
        bool connected = false;

        private void button3_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                string newuser = textBox9.Text;
                try
                {
                    using (var myclient = new SshClient(ip, int.Parse(port), user, pass))
                    {
                        myclient.Connect();
                        ShellStream shells = myclient.CreateShellStream("test", 80, 24, 800, 600, 1024);
                        shells.WriteLine("userdel "+newuser);
                        f = shells.Read();
                        logger.Text += "\n" + f;
                        myclient.Disconnect();
                        myclient.Dispose();
                        shells.Close();
                        MessageBox.Show("user removed");
                    }
                }
                catch (Exception t)
                {
                    logger.Text += "\n" + t.Message;
                    MessageBox.Show(t.Message, "ERROR");
                }
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

                    sqlite_cmd.CommandText = "DELETE FROM main.Users WHERE username='" + newuser+"'";

                    // Now lets execute the SQL ;D

                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn.Close();
                }
                catch (Exception t)
                {
                    logger.Text = t.Message;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                string newuser = textBox11.Text,date=textBox10.Text;
                try
                {
                    using (var myclient = new SshClient(ip, int.Parse(port), user, pass))
                    {
                        myclient.Connect();
                        ShellStream shells = myclient.CreateShellStream("test", 80, 24, 800, 600, 1024);
                        shells.WriteLine("chage -E " + date + " " + newuser);
                        f = shells.Read();
                        logger.Text += "\n" + f;
                        myclient.Disconnect();
                        myclient.Dispose();
                        shells.Close();
                        MessageBox.Show("Date changed");
                    }
                }
                catch (Exception t)
                {
                    logger.Text += "\n" + t.Message;
                    MessageBox.Show(t.Message, "ERROR");
                }


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

                    sqlite_cmd.CommandText = "UPDATE main.Users SET date='"+DataBindings+"' WHERE username='" + newuser + "'";

                    // Now lets execute the SQL ;D

                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn.Close();
                }
                catch (Exception t)
                {
                    logger.Text = t.Message;
                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                
                try
                {
                    using (var myclient = new SshClient(ip, int.Parse(port), user, pass))
                    {
                        myclient.Connect();
                        ShellStream shells = myclient.CreateShellStream("test", 80, 24, 800, 600, 1024);
                        shells.WriteLine("reboot");
                        logger.Text += "\n" + f;
                        myclient.Disconnect();
                        myclient.Dispose();
                        shells.Close();
                        MessageBox.Show("rebooted");
                    }
                }
                catch (Exception t)
                {
                    logger.Text += "\n" + t.Message;
                    MessageBox.Show(t.Message, "ERROR");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ123456789";
            string t = "";
            for (int i = 0; i < 12; i++)
            {
                t += allowedChars[rnd.Next(0, allowedChars.Length)];
            }
            textBox5.Text = t;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ123456789";
            string t = "";
            for (int i = 0; i < 12; i++)
            {
                t += allowedChars[rnd.Next(0, allowedChars.Length)];
            }
            textBox6.Text = t;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connected){
                string newuser = textBox5.Text, newpass = textBox6.Text, date = textBox8.Text, limit = textBox7.Text;
                int l = 0;
                try
                {
                    using (var myclient = new SshClient(ip, int.Parse(port), user, pass))
                    {
                        myclient.Connect();

                        var output = myclient.RunCommand("echo Connected successful").Result;
                        logger.Text += "\n" + output;

                        output = myclient.RunCommand("useradd -s /sbin/nologin " + newuser).Result;
                        logger.Text += "\n" + output;

                        output = myclient.RunCommand("chage -E " + date + " " + newuser).Result;
                        logger.Text += "\n" + output;

                        output = myclient.RunCommand("echo '" + newuser + " hard maxlogins " + int.Parse(limit) + "' >>/etc/security/limits.conf").Result;
                        logger.Text += "\n" + output;

                        output = myclient.RunCommand("echo -e '" + newpass + @"\n" + newpass+"' | passwd "+newuser).Result;
                        logger.Text += "\n" + output;

                        myclient.Disconnect();
                        myclient.Dispose();
                        MessageBox.Show("user added");
                    }

                    
                }
                catch (Exception t)
                {
                    logger.Text += "\n" + t.Message;
                    MessageBox.Show(t.Message,"ERROR");
                }

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

                    sqlite_cmd.CommandText = "INSERT into  main.Users values ('" + newuser+"','"+newpass+"','"+date+"',"+limit+",'"+ip+"')";

                    // Now lets execute the SQL ;D

                    sqlite_cmd.ExecuteNonQuery();

                    sqlite_conn.Close();
                }
                catch(Exception t)
                {
                    logger.Text = t.Message;
                }
                
            }
        }

       
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            user = textBox1.Text;
            pass = textBox2.Text;
            ip = textBox3.Text;
            port = textBox4.Text;

            try
            {
                using (var myclient = new SshClient(ip,int.Parse(port), user, pass))
                {
                    myclient.Connect();
                    ShellStream shells = myclient.CreateShellStream("test", 80, 24, 800, 600, 1024);
                    shells.WriteLine("echo Connected successful");
                    f = shells.Read();
                    logger.Text += "\n" + f;
                    myclient.Disconnect();
                    myclient.Dispose();
                    shells.Close();
                    connected = true;
                    MessageBox.Show("connected");
                }
            }
            catch(Exception t)
            {
                logger.Text += "\n" + t.Message;
                MessageBox.Show(t.Message, "ERROR");
            }

        }
    }
}
