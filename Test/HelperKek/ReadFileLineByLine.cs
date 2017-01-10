using System.Collections.Generic;
using System.IO;

namespace Test.Helper
{
    public class ReadFileLineByLine
    {
        private List<string> _fileAsList;

        public ReadFileLineByLine()
        {

        }

        public List<string> ReadFile(string path)
        {
            _fileAsList = new List<string>();

            foreach (var line in File.ReadLines(path))
            {
                _fileAsList.Add(line);
            }

            return _fileAsList;
        }
    }
}