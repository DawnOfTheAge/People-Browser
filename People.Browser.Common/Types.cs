using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public class Types
    {
        public class Person
        {
            public List<RelatedPerson> Parent
            {
                get;
                set;
            }

            public List<RelatedPerson> Sibling
            {
                get;
                set;
            }

            public List<RelatedPerson> Descendant
            {
                get;
                set;
            }

            public Person()
            {
                Parent = new List<RelatedPerson>();
                Sibling = new List<RelatedPerson>();
                Descendant = new List<RelatedPerson>();
            }

            public int ID
            {
                get;
                set;
            }

            public int FatherID
            {
                get;
                set;
            }

            public int MotherID
            {
                get;
                set;
            }

            public string Name
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

            public bool Sex
            {
                get;
                set;
            }
        }

        public class RelatedPerson : Person
        {
            public Enums.DirectFamilyRelation Relation
            {
                get;
                set;
            }

            public RelatedPerson() 
            { 
            }

            public RelatedPerson(Enums.DirectFamilyRelation dfrRelation)
            {
                Relation = dfrRelation;
            }
        }
    }
}
