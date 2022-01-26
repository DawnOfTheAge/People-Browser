using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public static class Utils
    {
        public static bool ValidateDirectFamilyRelation2Ways(DirectFamilyRelation _directFamilyRelation1,
                                                             DirectFamilyRelation _directFamilyRelation2)
        {
            if ((ValidateDirectFamilyRelation(_directFamilyRelation1, _directFamilyRelation2) &&
                (ValidateDirectFamilyRelation(_directFamilyRelation1, _directFamilyRelation1))))
            {
                return true;
            }

            return false;
        }

        public static bool ValidateDirectFamilyRelation(DirectFamilyRelation _directFamilyRelation1,
                                                        DirectFamilyRelation _directFamilyRelation2)
        {
            switch (_directFamilyRelation1)
            {
                case DirectFamilyRelation.Father:
                case DirectFamilyRelation.Mother:
                    switch (_directFamilyRelation2)
                    {
                        case DirectFamilyRelation.Son:
                        case DirectFamilyRelation.Daughter:
                            return true;

                        default:
                            return false;
                    }

                case DirectFamilyRelation.Son:
                case DirectFamilyRelation.Daughter:
                    switch (_directFamilyRelation2)
                    {
                        case DirectFamilyRelation.Mother:
                        case DirectFamilyRelation.Father:
                            return true;

                        default:
                            return false;
                    }

                case DirectFamilyRelation.Brother:
                case DirectFamilyRelation.Sister:
                    switch (_directFamilyRelation2)
                    {
                        case DirectFamilyRelation.Brother:
                        case DirectFamilyRelation.Sister:
                            return true;

                        default:
                            return false;
                    }

                case DirectFamilyRelation.Husband:
                case DirectFamilyRelation.Wife:
                    switch (_directFamilyRelation2)
                    {
                        case DirectFamilyRelation.Husband:
                        case DirectFamilyRelation.Wife:
                            return true;

                        default:
                            return false;
                    }

                default:
                    return false;
            }
        }
    }
}
