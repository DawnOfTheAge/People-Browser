using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public static class Utils
    {
        public static bool ValidateDirectFamilyRelation2Ways(Enums.DirectFamilyRelation _directFamilyRelation1,
                                                             Enums.DirectFamilyRelation _directFamilyRelation2)
        {
            if ((ValidateDirectFamilyRelation(_directFamilyRelation1, _directFamilyRelation2) &&
                (ValidateDirectFamilyRelation(_directFamilyRelation1, _directFamilyRelation1))))
            {
                return true;
            }

            return false;
        }

        public static bool ValidateDirectFamilyRelation(Enums.DirectFamilyRelation _directFamilyRelation1,
                                                        Enums.DirectFamilyRelation _directFamilyRelation2)
        {
            switch (_directFamilyRelation1)
            {
                case Enums.DirectFamilyRelation.Father:
                case Enums.DirectFamilyRelation.Mother:
                    switch (_directFamilyRelation2)
                    {
                        case Enums.DirectFamilyRelation.Son:
                        case Enums.DirectFamilyRelation.Daughter:
                            return true;

                        default:
                            return false;
                    }

                case Enums.DirectFamilyRelation.Son:
                case Enums.DirectFamilyRelation.Daughter:
                    switch (_directFamilyRelation2)
                    {
                        case Enums.DirectFamilyRelation.Mother:
                        case Enums.DirectFamilyRelation.Father:
                            return true;

                        default:
                            return false;
                    }

                case Enums.DirectFamilyRelation.Brother:
                case Enums.DirectFamilyRelation.Sister:
                    switch (_directFamilyRelation2)
                    {
                        case Enums.DirectFamilyRelation.Brother:
                        case Enums.DirectFamilyRelation.Sister:
                            return true;

                        default:
                            return false;
                    }

                case Enums.DirectFamilyRelation.Husband:
                case Enums.DirectFamilyRelation.Wife:
                    switch (_directFamilyRelation2)
                    {
                        case Enums.DirectFamilyRelation.Husband:
                        case Enums.DirectFamilyRelation.Wife:
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
