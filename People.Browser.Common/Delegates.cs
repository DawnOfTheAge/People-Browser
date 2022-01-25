using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public delegate void AuditMessage(string message, string method, string module, int line, AuditSeverity auditSeverity);
}
