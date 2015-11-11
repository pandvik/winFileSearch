using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace winFileSearchApp
{
    /// <summary>
    /// Класс, ответственный за хранение критерий поиска
    /// </summary>
    class SessionData
    {
        private string fileName;

        public string _folderName = "";
        public string _fileTemplate = "";
        public string _fileText = "";

        public SessionData(string fileName)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this.fileName = folder + "\\" + fileName;
        }

        /// <summary>
        /// Загрузить данные
        /// </summary>
        public void Load()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(this.fileName);
                _folderName = xml.SelectSingleNode("SessionData/folderName").InnerText;
                _fileTemplate = xml.SelectSingleNode("SessionData/fileTemplate").InnerText;
                _fileText = xml.SelectSingleNode("SessionData/fileText").InnerText;
            }
            catch (Exception e)
            { }
        }

        /// <summary>
        /// Сохранить данные
        /// </summary>
        public void Save()
        {
            try
            {
                XElement xml = new XElement("SessionData"
                    , new XElement("folderName", _folderName)
                    , new XElement("fileTemplate", _fileTemplate)
                    , new XElement("fileText", _fileText));
                xml.Save(fileName);
            }
            catch (Exception e)
            { }
        }


    }
}
