using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using winFileSearchLib;
using System.IO;
using System.Text;

namespace winFileSearchTests
{
    [TestClass]
    public class UnitTestWinFileSearch
    {
        String fileText1 = "Это простой тестовый текст для файла. Могут быть и другие тексты.";
        String fileText2 = "Это простой тететекстыстовый текст для файла. Могут быть и другие тек.";

        [TestMethod]
        public void FileSearch_searchInFile_Test1()
        {
            Assert.IsTrue(
                FileSearch_searchInFile(fileText1, "тексты"));
        }

        [TestMethod]
        public void FileSearch_searchInFile_Test2()
        {
            Assert.IsFalse(
                FileSearch_searchInFile(fileText1, "текстыы"));
        }

        [TestMethod]
        public void FileSearch_searchInFile_RealFile_Test1()
        {
            Assert.IsTrue(
                FileSearch_searchInFile_RealFile(fileText1, "тексты"));
        }

        [TestMethod]
        public void FileSearch_searchInFile_RealFile_Test2()
        {
            Assert.IsFalse(
                FileSearch_searchInFile_RealFile(fileText1, "текстыы"));
        }

        [TestMethod]
        public void FileSearch_searchInFile_RealFile_Test3()
        {
            Assert.IsFalse(
                FileSearch_searchInFile_RealFile(fileText2, "текстыы"));
        }

        [TestMethod]
        public void FileSearch_searchInFile_RealFile_Test4()
        {
            Assert.IsTrue(
                FileSearch_searchInFile_RealFile(fileText2, "тексты"));
        }

        public bool FileSearch_searchInFile(string fileText, string template)
        {
            MemoryStream file
                = new MemoryStream(Encoding.UTF8.GetBytes(fileText));
            file.Seek(0, SeekOrigin.Begin);
            byte[] tem = Encoding.UTF8.GetBytes(template);

            return FileSearch.searchInFile(file, tem);
        }

        public bool FileSearch_searchInFile_RealFile(string fileText, string template)
        {
            
            var file = File.CreateText("test.txt");
            file.Write(fileText);
            file.Close();
            var file1 = File.OpenRead("test.txt");

            byte[] tmp = Encoding.UTF8.GetBytes(template);

            bool ret = FileSearch.searchInFile(file1, tmp);
            file1.Close();
            return ret;
        }
    }
}
