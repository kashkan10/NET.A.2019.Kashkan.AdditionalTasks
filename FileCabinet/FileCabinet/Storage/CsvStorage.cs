using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinet
{
    internal class CsvStorage : IStorage<User>
    {
        private string filePath;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CsvStorage()
        {
            FilePath = "doc.csv";
        }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="path"></param>
        public CsvStorage(string path)
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
                if (!Regex.IsMatch(value, @"\w*.csv$"))
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
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.Default))
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
        }

        /// <summary>
        /// Import list
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> Import()
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File is not exist");
            }

            List<User> list = new List<User>();
            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
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

                return list;
            }
        }
    }
}
