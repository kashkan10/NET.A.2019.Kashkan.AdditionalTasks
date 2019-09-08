using System;
using System.Text.RegularExpressions;

namespace FileCabinet
{
    public class User
    {
        private string firstName;
        private string lastName;
        private string birthDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="birthDate"></param>
        /// <param name="name"></param>
        /// <param name="secondName"></param>
        public User(string name, string secondName, string birthDate)
        {
            FirstName = name;
            LastName = secondName;
            BirthDate = birthDate;
        }

        /// <summary>
        /// Id auto-property
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// BirthDate property
        /// </summary>
        public string BirthDate
        {
            get
            {
                return birthDate;
            }

            set
            {
                if (!Regex.IsMatch(value, @"\d{2}.\d{2}.\d{4}"))
                {
                    throw new FormatException();
                }

                birthDate = value;
            }
        }

        /// <summary>
        /// FirstName property
        /// </summary>
        public string FirstName
        {
            get
            {
                return char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower();
            }

            set
            {
                if (value == string.Empty)
                {
                    throw new FormatException();
                }

                firstName = value;
            }
        }

        /// <summary>
        /// LastName property
        /// </summary>
        public string LastName
        {
            get
            {
                return char.ToUpper(lastName[0]) + lastName.Substring(1).ToLower();
            }

            set
            {
                if (value == string.Empty)
                {
                    throw new FormatException();
                }

                lastName = value;
            }
        }
    }
}
