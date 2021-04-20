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
            // Generator for creating IEnumerable of file paths
            foreach (var file in Directory.GetFiles(path))
            {
                yield return file;
            }
            // Recursive call for all subdirectories from the main directory passed
            foreach (var directory in Directory.GetDirectories(path))
            {
                foreach (var file in EnumerateFilesRecursively(directory))
                    yield return file;
            }
        }
        public static string FormatByteSize(long byteSize)
        {
            // array to hold different byte sizes
            string[] size = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteSize == 0)
                return "0" + size[0];

            long bytes = Math.Abs(byteSize);
            // gets correct byte size by finding log with 1024 which is the separation for each byte size 
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            // rounds to 2 decimal places after converting to correct byte size
            double number = Math.Round(bytes / Math.Pow(1024, place), 2);
            // return string representation
            return (Math.Sign(byteSize) * number).ToString() + size[place];
        }
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

        static void Main(string[] args)
        {
            // setup home directory so filePath will be correct across devices
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            Console.WriteLine("Grabbing files from: " + appDataDir.ToString() + args[0]);
            // sets up IEnumerable interface with C# generators
            var files = EnumerateFilesRecursively(appDataDir.ToString() + args[0]);

            Console.WriteLine("Saving report.html to: " + appDataDir.ToString() + args[1]);
            // saves file to report.html in the directory specified by the second console argument
            CreateReport(files).Save(appDataDir.ToString() + args[1] + "report.html");
        }
    }
}
