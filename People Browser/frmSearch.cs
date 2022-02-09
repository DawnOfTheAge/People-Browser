using People.Browser.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace People.Browser.UI
{
    public partial class frmSearch : Form
    {
        #region Events

        public event AuditMessage Message;
        public event SearchParametersMessage SearchParameters;

        #endregion

        #region Data Members

        public Cities cities { get; set; }

        public Countries countries { get; set; }

        #endregion

        #region Constructor

        public frmSearch(Cities inCities, Countries inCountries)
        {
            InitializeComponent();

            cities = inCities;
            countries = inCountries;
        }

        #endregion

        #region Startup

        private void frmSearch_Load(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                Location = Cursor.Position;

                if (!SetForSearch(cities, countries, out string result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);
                }

                ctlSearchFilter.SearchParameters += CtlSearchFilter_SearchParameters;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        #endregion

        #region Public Methods

        private bool SetForSearch(Cities cities, Countries countries, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            result = string.Empty;

            try
            {
                if (!ctlSearchFilter.SetForSearch(cities, countries, out result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        #endregion

        #region Events Handlers
        
        private void CtlSearchFilter_SearchParameters(Person searchFilter, SpecialSearchFilter filter)
        {
            OnSearchParameter(searchFilter, filter);

            Close();
        }

        public void OnSearchParameter(Person searchFilter, SpecialSearchFilter filter)
        {
            SearchParameters?.Invoke(searchFilter, filter);
        }

        public void OnMessage(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            Message?.Invoke(message, method, module, line, auditSeverity);
        }

        #endregion

        #region Audit

        private void Audit(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            OnMessage(message, method, module, line, auditSeverity);
        }

        private void Audit(string message, string method, int line, AuditSeverity auditSeverity)
        {
            string module = "Person User Control";

            Audit(message, method, module, line, auditSeverity);
        }

        public static int LINE([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }

        #endregion        
    }
}
