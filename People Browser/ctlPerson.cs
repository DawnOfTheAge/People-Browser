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
        public event SearchParametersMessage SearchParameters;

        #endregion

        #region Data Members

        public Cities cities { get; set; }

        public Countries countries { get; set; }
        
        #endregion

        #region Constructor

        public ctlPerson()
        {
            InitializeComponent();
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

        public bool SetForSearch(Cities inCities, Countries inCountries, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            result = string.Empty;

            try
            {
                btn.Visible = true;
                btn.Text = "Search";

                #region City

                if (inCities != null)
                {
                    cities = inCities;
                    foreach (City city in cities.CitiesList())
                    {
                        cboCity.Items.Add(city.Name);
                    }
                }

                #endregion

                #region Country

                if (inCountries != null)
                {
                    countries = inCountries;
                    foreach (Country country in countries.CountriesList())
                    {
                        cboCountry.Items.Add(country.Name);
                    }
                }

                #endregion

                #region Birth Date

                cboYear.Items.Clear();
                cboYear.Items.Add(string.Empty);
                for (int year = 1900; year < 2030; year++)
                {
                    cboYear.Items.Add(year.ToString());
                }

                cboMonth.Items.Clear();
                cboMonth.Items.Add(string.Empty);
                for (int month = 1; month < 13; month++)
                {
                    cboMonth.Items.Add(month.ToString());
                }

                cboDay.Items.Clear();
                cboDay.Items.Add(string.Empty);
                for (int day = 1; day < 32; day++)
                {
                    cboDay.Items.Add(day.ToString());
                }

                #endregion

                #region Sex

                cboSex.Items.Clear();
                cboSex.Items.Add("זכר");
                cboSex.Items.Add("נקבה");
                cboSex.Items.Add("");

                #endregion

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        #endregion

        #region Gui
        
        private void btn_Click(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result;

            try
            {
                #region City

                int cityId = Constants.NONE;
                if (!string.IsNullOrEmpty(cboCity.Text))
                {
                    if (!cities.GetCityIdByCityName(cboCity.Text, out cityId, out result))
                    {
                        Audit(result, method, LINE(), AuditSeverity.Warning);
                    }
                }

                #endregion

                #region Country

                int countryId = Constants.NONE;
                if (!string.IsNullOrEmpty(cboCountry.Text))
                {
                    if (!countries.GetCountryIdByCountryName(cboCountry.Text, out countryId, out result))
                    {
                        Audit(result, method, LINE(), AuditSeverity.Warning);
                    }
                }

                #endregion

                #region Sex

                PersonSex sex = new PersonSex();
                switch (cboSex.Text)
                {
                    case "זכר":
                        sex = PersonSex.Male;
                        break;

                    case "נקבה":
                        sex = PersonSex.Female;
                        break;

                    default:
                        sex = PersonSex.Unknown;
                        break;
                }

                #endregion

                #region ID

                int id = int.TryParse(txtId.Text, out id) ? id : Constants.NONE; 

                #endregion

                Person searchFilter = new Person()
                {
                    Id = id,

                    Name = txtName.Text,
                    Family = txtFamily.Text,
                    OldFamily = txtOldFamily.Text,

                    Street = txtStreet.Text,
                    House = (int)nudHouse.Value,

                    CityId = cityId,
                    CountryId = countryId,

                    Sex = sex,

                    BirthDate = GetBirthDatFromGui()
                };

                OnSearchParameter(searchFilter);
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        public bool Fill(Person person, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            
            result = string.Empty;

            try
            {
                if (person != null)
                {                 
                    txtId.Text = person.Id.ToString();

                    txtFamily.Text = person.Family;
                    txtOldFamily.Text = person.Family;
                    txtName.Text = person.Name;

                    #region Birth Date

                    if (!string.IsNullOrEmpty(person.BirthDate))
                    {
                        FillBirthDate(person.BirthDate);
                    }

                    #endregion

                    #region City

                    string city = string.Empty;
                    if ((cities != null) && (cities.Count() > 0))
                    {
                        if (!cities.GetCityNameByCityId(person.CityId, out city, out result))
                        {
                            Audit(result, method, LINE(), AuditSeverity.Warning);
                        }
                    }
                    cboCity.Text = city;

                    #endregion

                    txtStreet.Text = person.Street;
                    nudHouse.Value = person.House;

                    #region Country

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

                    #endregion

                    #region Sex

                    switch (person.Sex)
                    {
                        case PersonSex.Male:
                            cboSex.Text = cboSex.Items[0].ToString();
                            break;

                        case PersonSex.Female:
                            cboSex.Text = cboSex.Items[1].ToString();
                            break;

                        default:
                            cboSex.Text = string.Empty;
                            break;
                    }

                    #endregion
                }

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        private void FillBirthDate(string birthDate)
        {
            cboYear.Text = birthDate.Substring(0, 4);
            cboMonth.Text = birthDate.Substring(4, 2);
            cboDay.Text = birthDate.Substring(6, 2);
        }

        private string GetBirthDatFromGui()
        {
            string birthDate;

            string year = string.IsNullOrEmpty(cboYear.Text) ? "0000" : cboYear.Text;
            string month = string.IsNullOrEmpty(cboMonth.Text) ? "00" : cboMonth.Text;
            string day = string.IsNullOrEmpty(cboDay.Text) ? "00" : cboDay.Text;

            birthDate = $"{year, 4}{month, 2}{day, 2}";

            return birthDate;
        }

        #endregion

        #region Events Handlers

        public void OnSearchParameter(Person searchFilter)
        {
            SearchParameters?.Invoke(searchFilter);
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
