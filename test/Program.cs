using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

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
        public static void CreateReport(IEnumerable<string> files)
        {
            var fileQuery = from file in files select (new FileInfo(file));

            foreach(var fileData in fileQuery)
            {
                Console.WriteLine(fileData.Name + ": " + fileData.Length);
            }
        }

        static void Main(string[] args)
        {
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //var tempDir = Path.Combine(appDataDir, "Documents");
            Console.WriteLine(appDataDir);
            Console.WriteLine("hello");
            var files = EnumerateFilesRecursively(appDataDir.ToString() + args[0]);
            //Console.WriteLine(appDataDir.ToString() + "/School/");
            //foreach (var f in files) Console.WriteLine(f);
            CreateReport(files);
        }
    }
}
