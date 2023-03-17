using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
                        MessageBox.Show("user removed");
                    }
                }
                catch (Exception t)
                {
                    logger.Text += "\n" + t.Message;
                    MessageBox.Show(t.Message, "ERROR");
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
                        MessageBox.Show("Date changed");
                    }
                }
                catch (Exception t)
                {
                    logger.Text += "\n" + t.Message;
                    MessageBox.Show(t.Message, "ERROR");
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (connected){
                string newuser = textBox5.Text, newpass = textBox6.Text, date = textBox8.Text, limit = textBox7.Text;
                try
                {
                    using (var myclient = new SshClient(ip, int.Parse(port), user, pass))
                    {
                        myclient.Connect();
                        ShellStream shells = myclient.CreateShellStream("test", 80, 24, 800, 600, 1024);
                        shells.WriteLine("useradd -s /sbin/nologin "+newuser);
                        shells.WriteLine("passwd " + newuser);
                        shells.WriteLine(newpass);
                        shells.WriteLine(newpass);
                        shells.WriteLine("chage -E "+date+" " + newuser);
                        shells.WriteLine("echo '"+newuser+" hard maxlogins "+int.Parse(limit)+"' >>/etc/security/limits.conf");
                        f = shells.Read();
                        logger.Text += "\n" + f;
                        myclient.Disconnect();
                        myclient.Dispose();
                        MessageBox.Show("users added");
                    }
                }
                catch (Exception t)
                {
                    logger.Text += "\n" + t.Message;
                    MessageBox.Show(t.Message,"ERROR");
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
