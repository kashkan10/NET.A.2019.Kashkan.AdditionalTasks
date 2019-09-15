using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FileCabinet
{
    internal class XmlStorage : IStorage<User>
    {
        private string filePath;

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlStorage()
        {
            FilePath = "doc.xml";
        }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="path"></param>
        public XmlStorage(string path)
        {
            FilePath = path;
        }

        /// <summary>
        /// FilePath property
        /// </summary>
        public string FilePath
        {
            get
            {
                return filePath;
            }

            private set
            {
                if (!Regex.IsMatch(value, @"\w*.xml$"))
                {
                    throw new FormatException();
                }

                filePath = value;
            }
        }

        /// <summary>
        /// Export list
        /// </summary>
        /// <param name="list"></param>
        public void Export(List<User> list)
        {
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
            xdoc.Save(filePath);
        }

        /// <summary>
        /// Import list
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> Import()
        {
            XmlDocument xDoc = new XmlDocument();
            if (!File.Exists(filePath))
            {
                throw new Exception("File is not exist");
            }

            List<User> list = new List<User>();
            xDoc.Load(filePath);
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

            return list;
        }
    }
}
