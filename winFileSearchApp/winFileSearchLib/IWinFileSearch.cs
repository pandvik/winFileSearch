using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace winFileSearchLib
{
    interface IWinFileSearch
    {
        /// <summary>
        /// Поиск подпоследовательности в потоке данных
        /// </summary>
        /// <param name="file">Поток данных</param>
        /// <param name="template">Подпоследовательность</param>
        /// <returns>true - если найдена</returns>
        bool  searchInFile(Stream file, IList<byte> template);
    }
}
