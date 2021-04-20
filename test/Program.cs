using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace test342
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
        private static XDocument CreateReport(IEnumerable<string> files)
                  => new XDocument(
                      new XElement("html",
                          new XElement("head",
                              new XElement("table",
                                new XElement("thead",
                                               new XElement("th", "CECS 342 Assignment 4"))
                                     ),
                                new XElement("style", "head, th, td { border: 2px solid black;padding: 5px;border-style: inset }")
                          ),
                          new XElement("body",

                              new XElement("table",

                                      new XElement("thead",
                                              new XElement("th", "Type",
                                                  new XAttribute("align", "center")),
                                              new XElement("th", "Count",
                                                   new XAttribute("align", "center")),
                                              new XElement("th", "Size",
                                                   new XAttribute("align", "center"))
                                      ),
                                      new XElement("tbody",
                                          from test in files
                                          group test by Path.GetExtension(test).ToLower() into testFile
                                          let fileSize = testFile.Sum(file => new FileInfo(file).Length)
                                          orderby fileSize ascending
                                          select new XElement("tr",
                                              new XElement("td", testFile.Key,
                                                   new XAttribute("align", "left")),
                                              new XElement("td", testFile.Count(),
                                                  new XAttribute("align", "right")),
                                              new XElement("td", FormatByteSize(fileSize),
                                                  new XAttribute("align", "right"))
                                          )//End select new XElement
                                      )//End new XElement tbody
                                  )//End new XElement Table
                              )//End new XElement body
                          )//End new XElement html

                      );

        // public static XDocument CreateReport(IEnumerable<string> files)
        //{

        //}
        /*
        public static void CreateReport(IEnumerable<string> files)
        {
            var fileQuery = from file in files select (new FileInfo(file));

            foreach (var fileData in fileQuery)
            {
                Console.WriteLine(fileData.Name + ": " + fileData.Length);
            }
        }*/

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

            string inputFolder;
            string outputFolder;
            Console.WriteLine("Enter a folder path");
            inputFolder = Console.ReadLine();

            Console.WriteLine("Enter a path for the  report.html:");
            outputFolder = Console.ReadLine();

            CreateReport(EnumerateFilesRecursively(inputFolder)).Save(outputFolder + "\\report.html");
        }
    }
}
