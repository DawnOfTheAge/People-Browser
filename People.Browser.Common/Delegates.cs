using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public delegate void AuditMessage(string message, string method, string module, int line, AuditSeverity auditSeverity);
    public delegate void LoadAllProgressMessage(int percentage);
    public delegate void SearchParametersMessage(Person searchFilter, SpecialSearchFilter filter);
    public delegate void OneIntegerParameterDelegate(int value);
}
