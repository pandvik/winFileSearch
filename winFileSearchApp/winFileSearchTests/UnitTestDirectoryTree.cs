using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using winFileSearchLib;
using System.Collections.Generic;

namespace winFileSearchTests
{
    [TestClass]
    public class UnitTestDirectoryTree
    {
        String baseDirName = "testDir_3244543234343r34r";
        String subDir1Name = "subDir1_rtewffwef";
        String subDir2Name = "subDir2_rfdesdsffvvsdf";
        String file1Name = "file1_sdfd";
        String file2Name = "file2_sdfvdvdfvdfd";
        String file3Name = "file3_sdfefefevd";
        String file4Name = "file4_sdfvdfdefdfd";

        String[] files;

        DirectoryInfo baseDir;
        public UnitTestDirectoryTree()
        {
            if (Directory.Exists(baseDirName))
                Directory.Delete(baseDirName, true);

            baseDir = Directory.CreateDirectory(baseDirName);
            Directory.CreateDirectory(baseDirName + "\\" + subDir1Name);
            Directory.CreateDirectory(baseDirName + "\\" + subDir2Name);

            File.CreateText(baseDirName + "\\" + file1Name).Close();
            File.CreateText(baseDirName + "\\" + file2Name).Close();
            File.CreateText(baseDirName + "\\" + subDir2Name + "\\" + file3Name).Close();
            File.CreateText(baseDirName + "\\" + subDir2Name + "\\" + file4Name).Close();

            files = new string[] { file1Name, file2Name, file3Name, file4Name };
        }

        ~UnitTestDirectoryTree()
        {
            //baseDir.Delete(true);
        }

        [TestMethod]
        public void DirectoryTree_Test1()
        {
            DirectoryTree tree = new DirectoryTree(baseDirName, "file*");
            tree.run();
        }
        [TestMethod]
        public void DirectoryTree_Test2()
        {
            DirectoryTree tree = new DirectoryTree(baseDirName, "file*");
            List<String> files = new List<string>();
            tree.OnFoundFile += x => files.Add(x.Name);
            tree.run();
            Assert.IsTrue(!files.TrueForAll(x => x != file1Name));
            Assert.IsTrue(!files.TrueForAll(x => x != file2Name));
            Assert.IsTrue(!files.TrueForAll(x => x != file3Name));
            Assert.IsTrue(!files.TrueForAll(x => x != file4Name));
        }

        [TestMethod]
        public void DirectoryTree_NotFoundFiles()
        {
            DirectoryTree tree = new DirectoryTree(baseDirName, "fi444444le*");
            List<String> files = new List<string>();
            tree.OnFoundFile += x => files.Add(x.Name);
            tree.run();
            Assert.IsTrue(files.Count == 0);
        }

    }
}
