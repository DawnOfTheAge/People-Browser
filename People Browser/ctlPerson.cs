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
    public partial class ctlPerson : UserControl
    {
        #region Events

        public event AuditMessage Message;

        #endregion

        #region Properties

        public Cities cities { get; set; }

        public Countries countries { get; set; }

        #endregion

        #region Data Members

        private Person person;

        #endregion

        #region Constructor

        public ctlPerson()
        {
            InitializeComponent();

            person = null;
        }

        public ctlPerson(Person inPerson)
        {
            InitializeComponent();

            person = inPerson;
        }

        #endregion

        #region Startup

        private void ctlPerson_Load(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                btn.Visible = false;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        #endregion

        #region Gui

        public bool Fill(Person person, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            
            result = string.Empty;

            try
            {
                if (person != null)
                {
                    btn.Visible = true;

                    txtFamily.Text = person.Family;
                    txtOldFamily.Text = person.Family;
                    txtName.Text = person.Name;

                    dtBirthDate.Text = person.BirthDate;

                    string city = string.Empty;
                    if ((cities != null) && (cities.Count() > 0))
                    {
                        if (!cities.GetCityNameByCityId(person.CityId, out city, out result))
                        {
                            Audit(result, method, LINE(), AuditSeverity.Warning);
                        }
                    }
                    cboCity.Text = city;

                    txtStreet.Text = person.Street;
                    nudHouse.Value = person.House;

                    string country = string.Empty;
                    string countryNameInEnglish = string.Empty;
                    if ((countries != null) && (countries.Count() > 0))
                    {
                        if (!countries.GetCountryNameByCountryId(person.CountryId,
                                                                 out country,
                                                                 out countryNameInEnglish,
                                                                 out result))
                        {
                        }
                    }
                    cboCountry.Text = country;
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
