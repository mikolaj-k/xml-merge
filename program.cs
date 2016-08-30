using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace MergeXML
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                int howManyFiles = 0;
                var watch = Stopwatch.StartNew();

                
                //Getting current .exe path
                var downloadFolder = AppDomain.CurrentDomain.BaseDirectory;
                var files = Directory.GetFiles(downloadFolder, "*.xml");

                //XML Namespace
                var firstFile = XElement.Load(files[0]);
                XNamespace ns = firstFile.Name.Namespace; ;

                //Nodes which content we want to merge
                #region Elements
                var firsNode = new XElement(ns + "NodeTitle");
                var secondNode = new XElement(ns + "NodeTitle1");
                #endregion

                
                //If first node has to be only once, it should be outside of a node
                var firstNodeElements = firstFile.Element(ns + "NodeTitle")?.Elements();
                firsNode.Add(firstNodeElements);

                foreach (var file in files)
                {
                    var source = XElement.Load(file);

                    //Looking for content and adding to XElement
                    #region Add Elements
                    var secondNodeElements = source.Element(ns + "NodeTitle1")?.Elements();
                    secondNode.Add(secondNodeElements);
                    #endregion

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(file);
                    Console.WriteLine("--------------------------------------------------");

                    howManyFiles++;
                }

                //Nulling empty nodes so they won't be in output - I bet there is better way to do it
                #region FirstNode == null
                if (secondNode.FirstNode == null)
                {
                    secondNode = null;
                }
                #endregion

                //New XML file
                var result = new XElement(ns + "dokumenty", firsNode, secondNode);
                result.Save(@"MergedXML.xml");

                watch.Stop();
                var elapsedS = watch.ElapsedMilliseconds * 0.001;


                Console.WriteLine("Merged {0} files in {1} seconds.", howManyFiles, elapsedS);
                Console.WriteLine("Press ENTER...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                //In case of fuckup

                Console.WriteLine(e.Message);
                Console.WriteLine("Press ENTER...");
                Console.ReadLine();
            }
        }
    }
}

