using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using winFileSearchLib;

namespace winFileSearchApp
{
    public partial class Form
    {
        /// <summary>
        /// Thread for background search
        /// </summary>
        private Thread backgroundSearch;

        /// <summary>
        /// The time of starting search
        /// </summary>
        private DateTime timeStartSearch;

        /// <summary>
        /// Search function
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileTemplate"></param>
        /// <param name="fileText"></param>
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
                            // Hide processing file
                            BeginInvoke(new Action<string>
                                    ((text) => LabelFileProcessing.Text = "")
                                , file.FullName);

                        }
                    }
                    catch (UnauthorizedAccessException e)
                    { }
                    catch (IOException e)
                    { }
                };
            tree.run();

            stopTimer();
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
        /// <remarks>recursive function</remarks>
        /// <param name="dir"></param>
        /// <returns></returns>
        private TreeNode getDirectoryNode(DirectoryInfo dir)
        {
            var node = treeViewResult.Nodes.Find(dir.FullName, true).FirstOrDefault();
            if (node == null)
            {

                //if (dir.Parent == null) // if it is disk
                if (dir.FullName == textBoxFolderName.Text) // If it is base dir
                {
                    node = treeViewResult.Nodes.Add(dir.FullName, dir.FullName);
                    return node;
                }

                var parentNode = getDirectoryNode(dir.Parent);
                node = parentNode.Nodes.Add(dir.FullName, dir.Name);

            }
            return node;
        }

        private void stopTimer()
        {
            timerSearch.Stop();
        }

    }
}
