using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbkReaderToPgn
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change The Path to your Folder where your abk file is located
            var path = @"C:\_ChessSuite-Project\ABKParser\testABKFile.abk";

            var reader = new AbkReader();
            reader.ReadAbkFile(path);
            return;
        }
    }
}
