using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotAuto
{
    public partial class Form1 : Form
    {
        private string cfgFile = @"dash.cfg";
        private string username = "Driver";
        private string ipaddr = "172.22.11.2";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\" + username + @"\wpilib\java\current\ant\auto.cfg";
			string cmd = @"pscp -unsafe " + path + @" lvuser@"+ipaddr+ @":/home/lvuser/auto.cfg";
			string select = "";
            string text = "";
            string[] options =
            {
                "agearleft",
                "agearright",
                "agear",
                "adrive"
            };
            if (radioButton1.Checked)
            {
                text = radioButton1.Text;
                select = options[0];
            }
            else
            if (radioButton2.Checked)
            {
                text = radioButton2.Text;
                select = options[1];
            }
            else
            if (radioButton3.Checked)
            {
                text = radioButton3.Text;
                select = options[2];
            }
            else
            {
                text = radioButton4.Text;
                select = options[3];
            }
            // Wite the file out
            System.IO.File.WriteAllText(path, select);
            label1.Text = text + " : " + select;
			label1.Text = cmd;

			// Transfer the file to the roborio
			ExecuteCommand(cmd);
        }

		void ExecuteCommand(string command)
		{
			int exitCode;
			ProcessStartInfo processInfo;
			Process process;

			processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			// *** Redirect the output ***
			//processInfo.RedirectStandardError = true;
			//processInfo.RedirectStandardOutput = true;

			process = Process.Start(processInfo);
			process.WaitForExit();

			// *** Read the streams ***
			// Warning: This approach can lead to deadlocks, see Edit #2
			//string output = process.StandardOutput.ReadToEnd();
			//string error = process.StandardError.ReadToEnd();

			exitCode = process.ExitCode;

			//label2.Text = ("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
			//label3.Text = ("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
			label2.Text = "Roborio Copy Status : " + exitCode.ToString();
			process.Close();
		}

        private void Form1_Load(object sender, EventArgs e)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines(cfgFile);
            username = readText[0];
            ipaddr = readText[1];
            /*
            foreach (string s in readText)
            {
                Console.WriteLine(s);
            }
            */
            label1.Text = username;
            label2.Text = ipaddr;

        }
    }

	
}
