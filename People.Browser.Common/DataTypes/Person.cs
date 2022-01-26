using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public class Person
    {
        public List<Person> Parent
        {
            get;
            set;
        }

        public List<Person> Sibling
        {
            get;
            set;
        }

        public List<Person> Spouse
        {
            get;
            set;
        }

        public List<Person> Descendant
        {
            get;
            set;
        }

        public Person()
        {
            Parent = new List<Person>();
            Sibling = new List<Person>();
            Spouse = new List<Person>();
            Descendant = new List<Person>();
        }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int FatherId
        {
            get;
            set;
        }

        public string FatherName
        {
            get;
            set;
        }

        public int MotherId
        {
            get;
            set;
        }

        public string MotherName
        {
            get;
            set;
        }

        public string Family
        {
            get;
            set;
        }

        public string OldFamily
        {
            get;
            set;
        }

        public string BirthDate
        {
            get;
            set;
        }

        public int CityId
        {
            get;
            set;
        }

        public int CountryId
        {
            get;
            set;
        }

        public string Street
        {
            get;
            set;
        }

        public int House
        {
            get;
            set;
        }

        public PersonSex Sex
        {
            get;
            set;
        }
    }
}
