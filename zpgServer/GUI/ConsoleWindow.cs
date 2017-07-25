using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zpgServer
{
    public partial class ConsoleWindow : Form
    {
        static ConsoleWindow instance;
        static List<string> queuedMessages = new List<string>();

        static int commandHistoryPos = -1;
        static List<string> commandHistory = new List<string>();
        public ConsoleWindow()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            instance = this;
            this.FormClosing += OnClose;
            mainInput.ShortcutsEnabled = true;

            if (queuedMessages.Count > 0)
            {
                for (int i = 0; i < queuedMessages.Count; i++)
                {
                    mainOutput.AppendText(String.Format("{0}{1}", queuedMessages[i], Environment.NewLine));
                }
                queuedMessages.Clear();
            }
        }
        public static void WriteLine(string text)
        {
            if (instance == null)
                queuedMessages.Add(text);
            else
            {
                if (!instance.InvokeRequired)
                {
                    instance.mainOutput.AppendText(String.Format("{0}{1}", text, Environment.NewLine));
                    instance.mainOutput.ScrollToCaret();
                }
                else
                {
                    instance.Invoke(new Action<string>(WriteLine), text);
                }
            }
        }
        public static void CloseInstance()
        {
            if (instance == null)
                return;

            instance.Close();
        }
        private void OnClose(object sender, EventArgs e)
        {
            instance = null;
        }

        private void OnInput(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && mainInput.Text.Length > 0)
            {
                string cmd = mainInput.Text;
                commandHistory.Add(cmd);
                commandHistoryPos = commandHistory.Count;
                ConsoleEx.Log("> " + cmd);
                CmdParser.Handle(cmd);
                mainInput.Clear();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up && commandHistory.Count > 0)
            {
                if (commandHistoryPos > 0)
                {
                    commandHistoryPos -= 1;
                }
                mainInput.Text = commandHistory[commandHistoryPos];
                mainInput.Select(mainInput.Text.Length, 0);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (commandHistoryPos < commandHistory.Count)
                {
                    commandHistoryPos += 1;
                    if (commandHistoryPos < commandHistory.Count)
                    {
                        mainInput.Text = commandHistory[commandHistoryPos];
                        mainInput.Select(mainInput.Text.Length, 0);
                    }
                    else
                        mainInput.Text = "";
                }
                else
                {
                    mainInput.Text = "";
                }
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else
            {
                commandHistoryPos = commandHistory.Count;
            }
        }

        private void OnInputRedirect(object sender, KeyEventArgs e)
        {
            //mainInput.Focus();
            if (!(e.KeyCode == Keys.C && e.Control))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

    }
}
