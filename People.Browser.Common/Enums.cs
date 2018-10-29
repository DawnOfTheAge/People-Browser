using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public class Enums
    {
        public enum DirectFamilyRelation
        {
            Unknown,
            Father,
            Mother,
            Son,
            Daughter,
            Brother,
            Sister,
            Husband,
            Wife
        };

        public enum AuditSeverity
        {
            Error,
            Warning,
            Information,
            Important
        };
    }
}
