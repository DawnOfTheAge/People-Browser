using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public class FamilyRelation
    {
        public FamilyRelation(Person _personA,
                              DirectFamilyRelation _directFamilyRelationA,
                              Person _personB,
                              DirectFamilyRelation _directFamilyRelationB)
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

        public DirectFamilyRelation DirectFamilyRelationA
        {
            get;
            set;
        }

        public DirectFamilyRelation DirectFamilyRelationB
        {
            get;
            set;
        }

        public bool IsDirectFamilyRelationValid()
        {
            return Utils.ValidateDirectFamilyRelation2Ways(DirectFamilyRelationA, DirectFamilyRelationB);
        }
    }
}
