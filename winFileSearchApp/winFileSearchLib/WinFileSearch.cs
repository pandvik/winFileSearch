using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winFileSearchLib
{
    public class WinFileSearch : IWinFileSearch
    {
        /// <summary>
        /// Поиск подпоследовательности в потоке данных
        /// </summary>
        /// <remarks>
        /// Простейший вариант алгоритма Бойера — Мура
        /// </remarks>
        /// <param name="file">Поток данных</param>
        /// <param name="template">Подпоследовательность</param>
        /// <returns>true - если найдена</returns>
        public bool searchInFile(Stream file, IList<byte> template)
        {
            // incorrect input
            if (file == null
                || template == null
                || template.Count == 0)
                return false;

            CircularQueue<byte> buff = new CircularQueue<byte>(template.Count);

            for (int t = file.ReadByte(); t != -1; t = file.ReadByte())
            {
                buff.push((byte)t);

                if (template.Last().Equals((byte)t))
                {
                    if (buff.cmp(template))
                        return true;
                }
            }
            return false;
        }
    }
}
