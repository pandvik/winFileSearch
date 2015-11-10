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
    public partial class Form1 : Form
    {
        private Thread backgroundSearch;
        public Form1()
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

            backgroundSearch = new Thread(() => runSearch(folderName, fileTemplate, fileText));
            backgroundSearch.Start();
        }

        private void runSearch(string folderName, string fileTemplate, string fileText)
        { 
            var fileTextB = Encoding.UTF8.GetBytes(fileText);

            DirectoryTree tree = new DirectoryTree(folderName, fileTemplate);
            tree.OnFoundFile +=
                file => {
                    try
                    {
                        using (var ff = File.OpenRead(file.FullName))
                        {
                            // Show processing file
                            BeginInvoke(new Action<string>
                                    ((text) => LabelFileProcessing.Text = text)
                                , file.FullName);

                            // Search fileText in file
                            if (FileSearch.searchInFile(ff, fileTextB))
                            {
                                // Add file to tree
                                BeginInvoke(new Action<FileInfo>(addFileToTree), file);
                            }

                        }
                    }
                    catch (UnauthorizedAccessException e)
                    { }
                    catch (IOException e)
                    { }
                };
            tree.run();
        }

        /// <summary>
        /// Добавляет файл в дерево файлов
        /// </summary>
        /// <param name="file"></param>
        private void addFileToTree(FileInfo file)
        {
            var dir = file.Directory;
            treeViewResult.BeginUpdate();
            {
                TreeNode dirNode = getDirectoryNode(dir);
                // Add file node
                dirNode.Nodes.Add(file.FullName, file.Name);
            }
            treeViewResult.EndUpdate();
            treeViewResult.ExpandAll();
        }

        /// <summary>
        /// Find or create directory node in tree
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private TreeNode getDirectoryNode(DirectoryInfo dir)
        {
            var node = treeViewResult.Nodes.Find(dir.FullName, true).FirstOrDefault();
            if (node == null)
            {
                // if it is disk
                if (dir.Parent == null)
                {
                    node = treeViewResult.Nodes.Add(dir.FullName, dir.Name);
                    node.Expand();
                    return node;
                }

                var parentNode = getDirectoryNode(dir.Parent);
                node = parentNode.Nodes.Add(dir.FullName, dir.Name);
                
            }
            return node;
        }

        private void buttonStopSearch_Click(object sender, EventArgs e)
        {
            if (backgroundSearch != null && backgroundSearch.IsAlive)
                backgroundSearch.Abort();
            else
                MessageBox.Show("Поиск ещё не запущен");
        }
    }
}
