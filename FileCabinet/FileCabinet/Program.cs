using System;

namespace FileCabinet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileCabinet cabinet = new FileCabinet();
            while (true)
            {
                Console.WriteLine("List of available commands: \n0.Exit \n1.Create \n2.List \n3.Remove \n4.Stat \n5.Edit \n6.Find  \n7.Export csv \n8.Export xml \n9.Import csv \n10.Import xml \n11.Purge");
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("Enter command: ");

                string input = Console.ReadLine().ToLower();
                Console.Clear();

                if (input == "create" || input == "1")
                {
                    cabinet.Create();
                }
                else if (input.Contains("list") || input == "2")
                {
                    cabinet.List(input);
                }
                else if (input == "remove" || input == "3")
                {
                    cabinet.Remove();
                }
                else if (input == "stat" || input == "4")
                {
                    cabinet.Stat();
                }
                else if (input == "edit" || input == "5")
                {
                    cabinet.Edit();
                }
                else if (input == "find" || input == "6")
                {
                    cabinet.Find();
                }
                else if (input == "export csv" || input == "7")
                {
                    cabinet.ExportCSV();
                }
                else if (input == "export xml" || input == "8")
                {
                    cabinet.ExportXml();
                }
                else if (input == "import csv" || input == "9")
                {
                    cabinet.ImportCSV();
                }
                else if (input == "import xml" || input == "10")
                {
                    cabinet.ImportXml();
                }
                else if (input == "purge" || input == "11")
                {
                    cabinet.Purge();
                }
                else if (input == "0" || input == "Exit")
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Incorrect command, please try again");
                    continue;
                }

                Console.WriteLine("\nPress any key to back..");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
