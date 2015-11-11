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
using winFileSearchLib;

namespace winFileSearchApp
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
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

        private void textBoxFolderName_Enter(object sender, EventArgs e)
        {
            textBoxFileTemplate.Select();
        }

        private void textBoxFileTemplate_Enter(object sender, EventArgs e)
        {
            textBoxFileText.Select();
        }

        private void textBoxFileText_Enter(object sender, EventArgs e)
        {
            buttonStartSearch.Select();
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
    }
}
