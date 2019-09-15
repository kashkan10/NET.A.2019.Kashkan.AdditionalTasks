using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace FileCabinet
{
    internal class FileCabinet
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private IRepository repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        public FileCabinet(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Create user
        /// </summary>
        public void Create()
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
                return;
            }

            repository.Create(user);
            Console.WriteLine("Record {0} is created", repository.Stat());
            logger.Info("Record is created");
        }

        /// <summary>
        /// List of users
        /// </summary>
        /// <param name="input">User input</param>
        public void List(string input)
        {
            if (repository.Stat() == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            logger.Trace("Start List");
            Console.WriteLine("List of users:");
            Console.WriteLine(new string('-', 25));
            string result = string.Empty;

            foreach (User us in repository)
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

        /// <summary>
        /// Remove user
        /// </summary>
        public void Remove()
        {
            if (repository.Stat() == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            logger.Trace("Start Remove");
            ShowList();
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
                return;
            }

            repository.Remove(id);
            Console.WriteLine("User removed successufully");
            logger.Info("User removed successufully");
        }

        /// <summary>
        /// Count of records
        /// </summary>
        public void Stat()
        {
            logger.Trace("Start Stat");
            Console.WriteLine("{0} records", repository.Stat());
            logger.Trace("End Stat");
        }

        /// <summary>
        /// Edit user
        /// </summary>
        public void Edit()
        {
            if (repository.Stat() == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            logger.Trace("Start Edit");
            ShowList();
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
                return;
            }

            Console.WriteLine(new string('-', 25));
            var user = repository[id];
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
                return;
            }

            Console.Clear();
            Console.WriteLine("User changed successufully");
            logger.Info("User changed successufully");
        }

        /// <summary>
        /// Find users
        /// </summary>
        public void Find()
        {
            if (repository.Stat() == 0)
            {
                Console.WriteLine("List is empty");
                return;
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

                users = repository[name];
            }
            else if (answer == "2")
            {
                Console.WriteLine("Enter name: ");
                name = Console.ReadLine();

                Console.WriteLine("Enter lastname:");
                lastName = Console.ReadLine();

                users = repository[name, lastName];
            }
            else
            {
                logger.Info("Incorrect option");
                Console.WriteLine("Incorrect option");
                return;
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

        /// <summary>
        /// Export list to CSV
        /// </summary>
        public void ExportCsv()
        {
            if (repository.Stat() == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            logger.Trace("Start ExportCSV");
            repository.Export(new CsvStorage());
            Console.WriteLine("Success!");
            logger.Trace("End ExportCSV");
        }

        /// <summary>
        /// Export list to XML
        /// </summary>
        public void ExportXml()
        {
            if (repository.Stat() == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            logger.Trace("Start ExportXML");
            repository.Export(new XmlStorage());
            Console.WriteLine("Success!");
            logger.Trace("End ExportXML");
        }

        /// <summary>
        /// Import list from CSV
        /// </summary>
        public void ImportCsv()
        {
            logger.Trace("Start ImportCSV");
            repository.Import(new CsvStorage());
            Console.WriteLine("Success!");
            logger.Trace("End ImportCSV");
        }

        /// <summary>
        /// Import list from XML
        /// </summary>
        public void ImportXml()
        {
            logger.Trace("Start ImportXML");
            repository.Import(new XmlStorage());
            Console.WriteLine("Success!");
            logger.Trace("End ImportCSV");
        }

        /// <summary>
        /// Purge of list
        /// </summary>
        public void Purge()
        {
            logger.Trace("Start Purge");
            repository.Purge();
            Console.WriteLine("List purged successfully!");
            logger.Trace("End Purge");
        }

        private void ShowList()
        {
            foreach (User us in repository)
            {
                Console.WriteLine(us.Id + "." + us.FirstName + " " + us.LastName);
            }
        }
    }
}