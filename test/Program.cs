using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace test
{
    class Program
    {
        public static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                yield return file;
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                foreach (var file in EnumerateFilesRecursively(directory))
                    yield return file;
            }
        }

        //public static string FormatByteSize(long byteSize)
        //{

        //}

        //public static XDocument CreateReport(IEnumerable<string> files)
        //{

        //}

        static void Main(string[] args)
        {
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //var tempDir = Path.Combine(appDataDir, "Documents");
            Console.WriteLine(appDataDir);
            Console.WriteLine("hello");
            var files = EnumerateFilesRecursively(appDataDir.ToString() + "/Documents/School/CECS_342/Lab_04/");
            //Console.WriteLine(appDataDir.ToString() + "/School/");
            foreach (var f in files) Console.WriteLine(f);
        }
    }
}
