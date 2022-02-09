using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public class Person
    {
        //public List<Person> Parent
        //{
        //    get;
        //    set;
        //}

        //public List<Person> Sibling
        //{
        //    get;
        //    set;
        //}

        //public List<Person> Spouse
        //{
        //    get;
        //    set;
        //}

        //public List<Person> Descendant
        //{
        //    get;
        //    set;
        //}

        #region Properties

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

        public string BirthDateYear { get; set; }

        public string BirthDateMonth { get; set; }    
        
        public string BirthDateDay { get; set; }

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

        #endregion

        #region Constructor

        public Person()
        {
            Id = Constants.NONE;
            FatherId = Constants.NONE;
            MotherId = Constants.NONE;

            Name = string.Empty;
            FatherName = string.Empty;
            MotherName = string.Empty;

            CityId = Constants.NONE;
            Street = string.Empty;
            House = Constants.NONE;

            CountryId = Constants.NONE;

            BirthDate = string.Empty;

            Sex = PersonSex.Unknown;
        }

        #endregion

        #region Public Methods


        #endregion
    }
}
