using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinet
{
    public class FileCabinet : IEnumerable
    {      
        private List<User> list;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileCabinet()
        {
            list = new List<User>();
        }

        /// <summary>
        /// IsEmpty property
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (list.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
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
        /// Export list
        /// </summary>
        /// <param name="storage"></param>
        public void Export(IStorage<User> storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException();
            }

            storage.Export(list);
        }

        /// <summary>
        /// Import list
        /// </summary>
        /// <param name="storage"></param>
        public void Import(IStorage<User> storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException();
            }

            list = storage.Import();
        }

        /// <summary>
        /// Count of records
        /// </summary>
        public int Stat()
        {
            return list.Count();
        }

        /// <summary>
        /// Remove user
        /// </summary>
        public void Remove(int id)
        {
            if (!list.Contains(this[id]))
            {
                throw new Exception("User not found");
            }

            list.Remove(this[id]);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Id = i + 1;
            }
        }

        /// <summary>
        /// Create user
        /// </summary>
        public void Create(User user)
        {
            this[list.Count + 1] = user ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Purge of list
        /// </summary>
        public void Purge()
        {
            list.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (User us in list)
            {
                yield return us;
            }
        }
    }
}
