using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace FileCabinet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
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
                    logger.Trace("Start Create");
                    Console.WriteLine("User creation:");
                    Console.WriteLine(new string('-', 25));

                    User user = new User();

                    try
                    {
                        Console.WriteLine("Name:");
                        user.FirstName = Console.ReadLine();

                        Console.WriteLine("Last Name:");
                        user.LastName = Console.ReadLine();

                        Console.WriteLine("Date of birth (dd.mm.yyyy):");
                        user.BirthDate = Console.ReadLine();
                    }
                    catch (FormatException)
                    {
                        logger.Warn("FormatException");
                        Console.Clear();
                        Console.WriteLine("Incorrect input, user is not created");
                        continue;
                    }

                    cabinet.Create(user);
                    Console.WriteLine("Record {0} is created", cabinet.Stat());
                    logger.Info("Record is created");
                }
                else if (input.Contains("list") || input == "2")
                {
                    if (cabinet.IsEmpty)
                    {
                        Console.WriteLine("List is empty");
                        continue;
                    }

                    logger.Trace("Start List");
                    Console.WriteLine("List of users:");
                    Console.WriteLine(new string('-', 25));
                    string result = string.Empty;

                    foreach (User us in cabinet)
                    {
                        if (input.Split().Count() == 1)
                        {
                            Console.WriteLine(us.Id + "." + us.FirstName + " " + us.LastName);
                        }
                        else
                        {
                            result = string.Empty;
                            if (input.Contains("id"))
                            {
                                result += "#" + us.Id + ".";
                            }

                            if (input.Contains("firstname"))
                            {
                                result += us.FirstName + " ";
                            }

                            if (input.Contains("lastname"))
                            {
                                result += us.LastName + " ";
                            }

                            if (input.Contains("age"))
                            {
                                result += us.BirthDate;
                            }

                            if (result == string.Empty)
                            {
                                logger.Info("Incorrect command");
                                Console.WriteLine("Incorrect command");
                                break;
                            }

                            Console.WriteLine(result);
                            logger.Trace("End List");
                        }
                    }
                }
                else if (input == "remove" || input == "3")
                {
                    if (cabinet.IsEmpty)
                    {
                        Console.WriteLine("List is empty");
                        continue;
                    }

                    logger.Trace("Start Remove");
                    ShowList(cabinet);
                    Console.WriteLine(new string('-', 25));
                    Console.WriteLine("Enter № to remove:");

                    int id;
                    try
                    {
                        id = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        logger.Warn("FormatException");
                        Console.Clear();
                        Console.WriteLine("Incorrect input");
                        continue;
                    }

                    cabinet.Remove(id);
                    Console.WriteLine("User removed successufully");
                    logger.Info("User removed successufully");
                }
                else if (input == "stat" || input == "4")
                {
                    logger.Trace("Start Stat");
                    Console.WriteLine("{0} records", cabinet.Stat());
                    logger.Trace("End Stat");
                }
                else if (input == "edit" || input == "5")
                {
                    if (cabinet.IsEmpty)
                    {
                        Console.WriteLine("List is empty");
                        continue;
                    }

                    logger.Trace("Start Edit");
                    ShowList(cabinet);
                    Console.WriteLine(new string('-', 25));
                    Console.WriteLine("Enter № to edit:");

                    int id;
                    try
                    {
                        id = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        logger.Warn("FormatException");
                        Console.WriteLine("Incorrect input");
                        continue;
                    }

                    Console.WriteLine(new string('-', 25));
                    var user = cabinet[id];
                    Console.WriteLine(user.FirstName + " " + user.LastName + ", " + user.BirthDate);

                    try
                    {
                        Console.WriteLine("Name:");
                        user.FirstName = Console.ReadLine();

                        Console.WriteLine("Last Name:");
                        user.LastName = Console.ReadLine();

                        Console.WriteLine("Date of birth (dd.mm.yyyy):");
                        user.BirthDate = Console.ReadLine();
                    }
                    catch (FormatException)
                    {
                        logger.Warn("FormatException");
                        Console.WriteLine("Incorrect input");
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine("User changed successufully");
                    logger.Info("User changed successufully");
                }
                else if (input == "find" || input == "6")
                {
                    if (cabinet.IsEmpty)
                    {
                        Console.WriteLine("List is empty");
                        continue;
                    }

                    logger.Trace("Start Find");
                    Console.WriteLine("Search options: ");
                    Console.WriteLine("1 - by name | 2 - by name and lastname");
                    string answer = Console.ReadLine();

                    string name = string.Empty;
                    string lastName = string.Empty;
                    List<User> users = null;

                    Console.Clear();
                    if (answer == "1")
                    {
                        Console.WriteLine("Enter name:");
                        name = Console.ReadLine().ToLower();

                        users = cabinet[name];
                    }
                    else if (answer == "2")
                    {
                        Console.WriteLine("Enter name: ");
                        name = Console.ReadLine();

                        Console.WriteLine("Enter lastname:");
                        lastName = Console.ReadLine();

                        users = cabinet[name, lastName];
                    }
                    else
                    {
                        logger.Info("Incorrect option");
                        Console.WriteLine("Incorrect option");
                        continue;
                    }

                    Console.WriteLine(new string('-', 25));
                    if (users.Count() == 0)
                    {
                        logger.Info("Users are not found");
                        Console.WriteLine("Users are not found");
                    }
                    else
                    {
                        int i = 1;
                        foreach (User us in users)
                        {
                            Console.WriteLine("#" + i++ + " " + us.FirstName + " " + us.LastName);
                        }
                    }

                    logger.Trace("End Find");
                }
                else if (input == "export csv" || input == "7")
                {
                    if (cabinet.IsEmpty)
                    {
                        Console.WriteLine("List is empty");
                        continue;
                    }

                    logger.Trace("Start ExportCSV");
                    cabinet.Export(new CsvStorage());
                    Console.WriteLine("Success!");
                    logger.Trace("End ExportCSV");
                }
                else if (input == "export xml" || input == "8")
                {
                    if (cabinet.IsEmpty)
                    {
                        Console.WriteLine("List is empty");
                        continue;
                    }

                    logger.Trace("Start ExportXML");
                    cabinet.Export(new XmlStorage());
                    Console.WriteLine("Success!");
                    logger.Trace("End ExportXML");
                }
                else if (input == "import csv" || input == "9")
                {
                    logger.Trace("Start ImportCSV");
                    cabinet.Import(new CsvStorage());
                    Console.WriteLine("Success!");
                    logger.Trace("End ImportCSV");
                }
                else if (input == "import xml" || input == "10")
                {
                    logger.Trace("Start ImportXML");
                    cabinet.Import(new XmlStorage());
                    Console.WriteLine("Success!");
                    logger.Trace("End ImportCSV");
                }
                else if (input == "purge" || input == "11")
                {
                    logger.Trace("Start Purge");
                    cabinet.Purge();
                    Console.WriteLine("List purged successfully!");
                    logger.Trace("End Purge");
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

        private static void ShowList(FileCabinet cabinet)
        {
            foreach (User us in cabinet)
            {
                Console.WriteLine(us.Id + "." + us.FirstName + " " + us.LastName);
            }
        }
    }
}
