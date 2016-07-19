using System;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        DataTable mydt = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // comment
            status.Text = "Loading...";
            button1.Enabled = false;
            Process cmd = new Process();
            DataTable mydt = new DataTable();
            mydt.Columns.Add(new DataColumn("ID", typeof(int)));
            mydt.Columns.Add(new DataColumn("Wifi name", typeof(string)));
            mydt.Columns.Add(new DataColumn("Password", typeof(string)));
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("netsh wlan show profiles");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            MatchCollection mcUser = Regex.Matches(cmd.StandardOutput.ReadToEnd(), @"All User Profile.+: (.+)", RegexOptions.IgnoreCase);
            
            for (int i = 0; i < mcUser.Count; i++)            {
                
                Process cmd2 = new Process();
                cmd2.StartInfo.FileName ="cmd.exe";
                cmd2.StartInfo.RedirectStandardInput = true;
                cmd2.StartInfo.RedirectStandardOutput = true;
                cmd2.StartInfo.CreateNoWindow = true;
                cmd2.StartInfo.UseShellExecute = false;
                cmd2.Start();
                //Console.Write(mcUser[i].Groups[1].Value + "|");
                cmd2.StandardInput.WriteLine("netsh wlan show profiles name=\"" + mcUser[i].Groups[1].Value + "\" key=clear");
                cmd2.StandardInput.Close();
                cmd2.WaitForExit();
                Match match = Regex.Match(cmd2.StandardOutput.ReadToEnd(), @"Key Content.+: (.+)", RegexOptions.IgnoreCase);
                //Console.WriteLine(match.Groups[1].Value);
                mydt.Rows.Add(i + 1, mcUser[i].Groups[1].Value, match.Groups[1].Value);
                status.Text = "Loading..." + (i + 1) + "/" + mcUser.Count;
            }
            status.Text = "Done";
            dataGridView1.DataSource = mydt;
            button1.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
