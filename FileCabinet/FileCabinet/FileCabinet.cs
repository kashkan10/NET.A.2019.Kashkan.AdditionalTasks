using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using NLog;

namespace FileCabinet
{
    public class FileCabinet
    {      
        private Logger logger = LogManager.GetCurrentClassLogger();
        private List<User> list;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileCabinet()
        {
            list = new List<User>();
        }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        public User this[int id]
        {
            get
            {
                return list.Find(user => user.Id == id);
            }

            set
            {
                value.Id = id;
                list.Add(value);
            }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List of users</returns>
        public List<User> this[string name]
        {
            get
            {
                return list.FindAll(user => user.FirstName.ToLower() == name);
            }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <returns>List of users</returns>
        public List<User> this[string name, string lastName]
        {
            get
            {
                return list.FindAll(user => user.FirstName.ToLower() == name && user.LastName.ToLower() == lastName);
            }
        }

        /// <summary>
        /// Export list to CSV
        /// </summary>
        public void ExportCSV()
        {
            logger.Trace("Start ExportCSV");
            Console.WriteLine("Enter a file name in the format(filename.csv):");
            string fileName = Console.ReadLine();

            if (!Regex.IsMatch(fileName, @"\w*.csv$"))
            {
                logger.Info("Incorrect format");
                Console.WriteLine("Incorrect format");
                return;
            }

            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                string res = string.Empty;
                res = "Id, Name, LastName, Age";
                sw.WriteLine(res);
                foreach (User us in list)
                {
                    res = us.Id + "," + us.FirstName + "," + us.LastName + "," + us.BirthDate;
                    sw.WriteLine(res);
                    sw.Flush();
                }
            }

            Console.WriteLine("Success!");
            logger.Trace("End ExportCSV");
        }

        /// <summary>
        /// Import list from CSV
        /// </summary>
        public void ImportCSV()
        {
            logger.Trace("Start ImportCSV");
            Console.WriteLine("Enter a file name in the format(filename.csv):");
            string fileName = Console.ReadLine();

            if (!Regex.IsMatch(fileName, @"\w*.csv$") || !File.Exists(fileName))
            {
                logger.Info("Incorrect format or file does not exist");
                Console.WriteLine("Incorrect format or file does not exist");
                return;
            }

            using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                string[] arr;
                string line;
                int c = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    User user = new User();
                    arr = line.Split(',');
                    c++;
                    if (c == 1)
                    {
                        continue;
                    }

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == 0)
                        {
                            user.Id = int.Parse(arr[i]);
                        }
                        else if (i == 1)
                        {
                            user.FirstName = arr[i].Trim();
                        }
                        else if (i == 2)
                        {
                            user.LastName = arr[i].Trim();
                        }
                        else if (i == 3)
                        {
                            user.BirthDate = arr[i];
                            break;
                        }
                    }

                    list.Add(user);
                }
            }

            Console.WriteLine("Success!");
            logger.Trace("End ImportCSV");
        }

        /// <summary>
        /// Count of records
        /// </summary>
        public void Stat()
        {
            logger.Trace("Start Stat");
            Console.WriteLine("{0} records", list.Count());
            logger.Trace("End Stat");
        }

        /// <summary>
        /// Export list to XML
        /// </summary>
        public void ExportXml()
        {
            logger.Trace("Start ExportXml");       
            Console.WriteLine("Enter a file name in the format(filename.xml):");
            string fileName = Console.ReadLine();

            if (!Regex.IsMatch(fileName, @"\w*.xml$"))
            {
                logger.Info("Incorrect format");
                Console.WriteLine("Incorrect format");
                return;
            }

            XDocument xdoc = new XDocument();
            XElement users = new XElement("users");

            foreach (User us in list)
            {
                XElement user = new XElement("user");

                XAttribute userIdAttr = new XAttribute("id", us.Id);
                XElement userNameAttr = new XElement("name", us.FirstName);
                XElement userLastNameAttr = new XElement("lastName", us.LastName);
                XElement userAgeAttr = new XElement("age", us.BirthDate);

                user.Add(userIdAttr);
                user.Add(userNameAttr);
                user.Add(userLastNameAttr);
                user.Add(userAgeAttr);

                users.Add(user);
            }

            xdoc.Add(users);
            xdoc.Save(fileName);

            Console.WriteLine("Success!");
            logger.Trace("End ExportXml");
        }

        /// <summary>
        /// Import list from XML
        /// </summary>
        public void ImportXml()
        {
            logger.Trace("Start ImportXml");
            Console.WriteLine("Enter a file name in the format(filename.xml):");
            string fileName = Console.ReadLine();

            if (!Regex.IsMatch(fileName, @"\w*.xml$") || !File.Exists(fileName))
            {
                logger.Info("Incorrect format or file does not exist");
                Console.WriteLine("Incorrect format or file does not exist");
                return;
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileName);
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {
                User user = new User();

                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                    {
                        user.Id = int.Parse(attr.Value);
                    }
                }

                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "name")
                    {
                        user.FirstName = childnode.InnerText;
                    }

                    if (childnode.Name == "lastName")
                    {
                        user.LastName = childnode.InnerText;
                    }

                    if (childnode.Name == "age")
                    {
                        user.BirthDate = childnode.InnerText;
                    }
                }

                list.Add(user);
            }

            Console.WriteLine("Success!");
            logger.Trace("End ImportXml");
        }

        /// <summary>
        /// Remove user
        /// </summary>
        public void Remove()
        {
            logger.Trace("Start Remove");

            if (list.Count == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            Console.WriteLine("List of users:");
            foreach (User us in list)
            {
                Console.WriteLine(us.Id + "." + us.FirstName + " " + us.LastName + ", " + us.BirthDate);
            }

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
                Console.WriteLine("Incorrect input");
                return;
            }

            var user = this[id];

            list.Remove(user);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Id = i + 1;
            }

            Console.Clear();
            Console.WriteLine("User removed successufully");
            logger.Info("User removed successufully");
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

            this[list.Count + 1] = user;

            Console.Clear();
            Console.WriteLine("Record {0} is created", list.Count);
            logger.Info("Record is created");
        }

        /// <summary>
        /// List of users
        /// </summary>
        /// <param name="input">User input</param>
        public void List(string input)
        {
            logger.Trace("Start List");
            string result = string.Empty;
            if (list.Count == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            Console.WriteLine("List of users:");
            Console.WriteLine(new string('-', 25));

            foreach (User us in list)
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
                }
            }

            logger.Trace("End List");
        }

        /// <summary>
        /// Find users
        /// </summary>
        public void Find()
        {
            logger.Trace("Start Find");

            if (list.Count == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            Console.WriteLine("Search options: ");
            Console.WriteLine("1 - by name | 2 - by name and lastname");

            string name = string.Empty;
            string lastName = string.Empty;
            List<User> users = null;
            string answer = Console.ReadLine();

            Console.Clear();
            if (answer == "1")
            {
                Console.WriteLine("Enter name:");
                name = Console.ReadLine().ToLower();
                users = this[name];
            }
            else if (answer == "2")
            {
                Console.WriteLine("Enter name: ");
                name = Console.ReadLine();

                Console.WriteLine("Enter lastname:");
                lastName = Console.ReadLine();
                users = this[name, lastName];
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
        /// Edit user
        /// </summary>
        public void Edit()
        {
            logger.Trace("Start Edit");

            if (list.Count == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            Console.WriteLine("List of users:");
            foreach (User us in list)
            {
                Console.WriteLine(us.Id + "." + us.FirstName + " " + us.LastName + ", " + us.BirthDate);
            }

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
            var user = this[id];
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
        /// Purge of list
        /// </summary>
        public void Purge()
        {
            logger.Trace("Start Purge");
            list.Clear();
            Console.WriteLine("List purged successfully!");
            logger.Trace("End Purge");
        }
    }
}
