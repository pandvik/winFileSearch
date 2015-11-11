using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using winFileSearchApp.Properties;
using winFileSearchLib;

namespace winFileSearchApp
{
    public partial class Form : System.Windows.Forms.Form
    {
        /// <summary>
        /// For load and save session data
        /// </summary>
        private SessionData sessionData;

        public Form()
        {
            InitializeComponent();

            // Load session data
            sessionData = new SessionData(Resources.SessionDataFileName);
            sessionData.Load();
            textBoxFolderName.Text = sessionData._folderName;
            textBoxFileTemplate.Text = sessionData._fileTemplate;
            textBoxFileText.Text = sessionData._fileText;
        }

        private void textBoxFolderName_Click(object sender, EventArgs e)
        {
            if (folderSelectDialog.ShowDialog() == DialogResult.OK)
                textBoxFolderName.Text = folderSelectDialog.SelectedPath;
        }

        private void buttonStartSearch_Click(object sender, EventArgs e)
        {
            string folderName = textBoxFolderName.Text;
            string fileTemplate = textBoxFileTemplate.Text;
            string fileText = textBoxFileText.Text;

            if (folderName == ""
                || fileTemplate == ""
                || fileText == "" )
            {
                MessageBox.Show("Для начала поиска необходимо заполнить все поля");
                return;
            }

            if (backgroundSearch != null && backgroundSearch.IsAlive)
            {
                MessageBox.Show("Поиск уже запущен");
                return;
            }

            // Init timer
            labelTime.Text = "00:00";
            timeStartSearch = DateTime.Now;
            timerSearch.Start();

            // Run background search
            backgroundSearch = new Thread(() => runSearch(folderName, fileTemplate, fileText));
            backgroundSearch.Start();
        }

        private void buttonStopSearch_Click(object sender, EventArgs e)
        {
            if (backgroundSearch != null && backgroundSearch.IsAlive)
            {
                backgroundSearch.Abort();
                stopTimer();
            }
            else
                MessageBox.Show("Поиск ещё не запущен");
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            if (timeStartSearch == null)
            {
                labelTime.Text = "--:--";
                return;
            }

            var deltaTime = DateTime.Now - timeStartSearch;
            labelTime.Text = deltaTime.ToString(@"mm\:ss");
        }

        private void textBoxFolderName_TextChanged(object sender, EventArgs e)
        {
            sessionData._folderName = textBoxFolderName.Text;
            sessionData.Save();
        }

        private void textBoxFileTemplate_TextChanged(object sender, EventArgs e)
        {
            sessionData._fileTemplate = textBoxFileTemplate.Text;
            sessionData.Save();
        }

        private void textBoxFileText_TextChanged(object sender, EventArgs e)
        {
            sessionData._fileText = textBoxFileText.Text;
            sessionData.Save();
        }
    }
}
