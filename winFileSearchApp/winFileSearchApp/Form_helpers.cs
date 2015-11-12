using System;
using System.Collections.Concurrent;
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
        /// File in processing
        /// </summary>
        private string processingFileName;

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
                            /*
                            BeginInvoke(new Action<string>
                                    ((text) => LabelFileProcessing.Text = text)
                                , file.FullName);
                            */
                            processingFileName = file.FullName;
                            // Search fileText in file
                            if (FileSearch.searchInFile(ff, fileTextB))
                            {
                                // Add file to tree
                                // BeginInvoke(new Action<FileInfo>(addFileToTree), file);
                                addFileToTree(file);
                            }
                            // Hide processing file
                            processingFileName = "--";
                        }
                    }
                    catch (UnauthorizedAccessException e)
                    { }
                    catch (IOException e)
                    { }
                };
            tree.run();

            stopTimer();
            MessageBox.Show("Поиск завершен");
        }

        Queue<Action> delayedTask = new Queue<Action>();

        /// <summary>
        /// Добавляет файл в дерево файлов
        /// </summary>
        /// <param name="file"></param>
        private void addFileToTree(FileInfo file)
        {
            lock (delayedTask)
            {
                delayedTask.Enqueue(() =>
                {
                    var dir = file.Directory;
                    TreeNode dirNode = getDirectoryNode(dir);
                    // Add file node if not exist
                    if (treeViewResult.Nodes.Find(file.FullName, true).FirstOrDefault() == null)
                    {
                        dirNode.Nodes.Add(file.FullName, file.Name);
                    }
                });
            }
        }

        /// <summary>
        /// Find or create directory node in tree
        /// </summary>
        /// <remarks>recursive function</remarks>
        /// <param name="dir"></param>
        /// <returns></returns>
        private TreeNode getDirectoryNode(DirectoryInfo dir)
        {
            var node = treeViewResult.Nodes.Find(dir.FullName.GetHashCode().ToString(), true).FirstOrDefault();
            if (node == null)
            {
                //if (dir.Parent == null) // if it is disk
                if (dir.FullName == textBoxFolderName.Text) // If it is base dir
                {
                    node = treeViewResult.Nodes.Add(dir.FullName.GetHashCode().ToString(), dir.FullName);
                    return node;
                }

                var parentNode = getDirectoryNode(dir.Parent);
                node = parentNode.Nodes.Add(dir.FullName.GetHashCode().ToString(), dir.Name);

            }
            return node;
        }

        /// <summary>
        /// Остановить таймер
        /// </summary>
        private void stopTimer()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    timerSearch.Stop();
                }));
            }
            else
            {
                timerSearch.Stop();
            }
        }

        /// <summary>
        /// Выполнить отсроченные задачи по добавлению файлов в дерево
        /// </summary>
        private void runDelayedTask()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    _runDelayedTask();
                }));
            }
            else
            {
                _runDelayedTask();
            }
        }
        private void _runDelayedTask()
        {
            if (delayedTask.Count != 0)
            {
                DateTime now = DateTime.Now;
                treeViewResult.BeginUpdate();
                int i =0;
                lock (delayedTask)
                {
                    for ( i = 0; (DateTime.Now - now).Milliseconds < 500 && delayedTask.Count != 0; i++)
                    {
                        Action tt= delayedTask.Dequeue();
                        tt();
                    }
                }
                treeViewResult.EndUpdate();
                Console.WriteLine("Time2: " + (DateTime.Now - now).Seconds+"   Count:" + delayedTask.Count+"   Done:"+i);
            }
        }
    }
}
