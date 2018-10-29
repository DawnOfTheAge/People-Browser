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

            public int ID
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

        public class FamilyRelation
        {
            public FamilyRelation(Person _personA, 
                                  Enums.DirectFamilyRelation _directFamilyRelationA, 
                                  Person _personB,
                                  Enums.DirectFamilyRelation _directFamilyRelationB)
            {
                PersonA = _personA;
                PersonB = _personB;
                DirectFamilyRelationA = _directFamilyRelationA;
                DirectFamilyRelationB = _directFamilyRelationB;                
            }

            public Person PersonA
            {
                get;
                set;
            }

            public Person PersonB
            {
                get;
                set;
            }

            public Enums.DirectFamilyRelation DirectFamilyRelationA
            {
                get;
                set;
            }

            public Enums.DirectFamilyRelation DirectFamilyRelationB
            {
                get;
                set;
            }

            public bool IsDirectFamilyRelationValid()
            {
                return Utils.ValidateDirectFamilyRelation2Ways(DirectFamilyRelationA, DirectFamilyRelationB);
            }
        }

        public class AuditMessage : EventArgs
        {
            public string Message
            {
                get;
                set;
            }

            public string Module
            {
                get;
                set;
            }

            public string Method
            {
                get;
                set;
            }

            public Enums.AuditSeverity Severity
            {
                get;
                set;
            }

            public AuditMessage(string sMessage,
                                string sModule,
                                string sMethod,
                                Enums.AuditSeverity asAuditSeverity)
            {
                Message = sMessage;
                Module = sModule;
                Method = sMethod;
                Severity = asAuditSeverity;
            }
        }
    }
}
