using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;

namespace winFileSearchLib
{
    /// <summary>
    /// Рекурсивный обход дерева каталогов и поиск файлов по заданному шаблону
    /// </summary>
    public class DirectoryTree
    {
        String baseDirName;
        String fileNameTemplate;

        public DirectoryTree(String baseDirName, String fileNameTemplate)
        {
            this.baseDirName = baseDirName;
            this.fileNameTemplate = fileNameTemplate;
        }


        /// <summary>
        /// Вызывается при нахождении файла, соответствующего шаблону fileNameTemplate
        /// </summary>
        /// <param name="file"> FileInfo </param>
        public Action<FileInfo> OnFoundFile;
        /// <summary>
        /// Вызывается при возникновении исключений: SecurityException, DirectoryNotFoundException
        /// </summary>
        public Action<Exception> OnException;

        /// <summary>
        /// Запустить обход дерева каталогов
        /// </summary>
        public void run()
        {
            DirectoryInfo baseDir = new DirectoryInfo(baseDirName);
            run(baseDir);
        }

        private void run(DirectoryInfo baseDir)
        {
            try
            {
                foreach (var file in baseDir.EnumerateFiles(fileNameTemplate))
                {
                    if (OnFoundFile != null)
                        OnFoundFile(file);
                }
                foreach (var dir in baseDir.EnumerateDirectories())
                {
                    run(dir);
                }
            }
            catch (SecurityException e)
            {
                if (OnException != null)
                    OnException(e);
            }
            catch (DirectoryNotFoundException e)
            {
                if (OnException != null)
                    OnException(e);
            }
        }

    }
}
