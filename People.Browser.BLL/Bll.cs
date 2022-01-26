using System;
using System.Collections.Generic;
using People.Browser.Common;
using System.Data.OleDb;
using System.Reflection;
using People.Browser.DAL;
using System.Timers;
using System.Threading;

namespace People.Browser.BLL
{
    public class Bll
    {
        #region Events

        public event AuditMessage Message;
        public event LoadAllProgressMessage LoadAllProgress;

        #endregion

        #region Data Members

        private Dal dal;

        private System.Timers.Timer tmrIntervalTimer;

        private int loadAllPercentage;

        #endregion

        #region Constructor

        public Bll()
        {
            dal = new Dal();
            dal.Message += Dal_Message; ;
        }

        ~Bll()
        {
            dal.Message -= Dal_Message;
        }

        #endregion

        #region Actions

        #region Create

        private string CreateSiblingsSql(Person pPerson, out string result)
        {
            result = "";

            if (pPerson == null)
            {
                result = "Person Object Is Null";

                return "";
            }

            if (pPerson.Parent == null)
            {
                result = "Person Parents Information Is Null";

                return "";
            }

            if ((pPerson.Parent[0].Id <= 0) && (pPerson.Parent[1].Id <= 0))
            {
                result = "Person Has No Parents Information";

                return "";
            }

            if (pPerson.Parent[0].Id <= 0)
            {
                return "SELECT * FROM M WHERE ID_FATHER=" + pPerson.Parent[1].Id + " AND ID<>" + pPerson.Id;
            }

            if (pPerson.Parent[1].Id <= 0)
            {
                return "SELECT * FROM M WHERE ID_MOTHER=" + pPerson.Parent[0].Id + " AND ID<>" + pPerson.Id;
            }

            return "SELECT * FROM M WHERE ID_FATHER=" + pPerson.Parent[1].Id +
                   " AND ID_MOTHER=" + pPerson.Parent[0].Id +
                   " AND ID<>" + pPerson.Id + " ";
        }

        private string CreateDescendantsSql(Person pPerson, out string result)
        {
            result = string.Empty;

            if (pPerson == null)
            {
                result = "Person Object Is Null";

                return "";
            }

            if (pPerson.Id <= 0) 
            {
                result = "Person Has No Valid ID";

                return "";
            }

            string sParent;
            switch (pPerson.Sex)
            {
                case PersonSex.Male:
                    sParent = "ID_FATHER";
                    break;

                case PersonSex.Female:
                    sParent = "ID_MOTHER";
                    break;

                default:
                    return String.Empty;
            }

            return "SELECT * FROM M WHERE " + sParent + "=" + pPerson.Id;
        }

        #endregion

        #region Set

        public void SetConnectionString(string sConnectionString)
        {
            dal.SetConnectionString(sConnectionString);
        }

        #endregion

        #region Get

        public bool GetPersonInformation(int iId, ref Person pPerson)
        {
            GetPerson(iId, ref pPerson);
            GetParents(ref pPerson);
            GetSiblings(ref pPerson);
            GetDescendants(ref pPerson);

            for (int i = 0; i < pPerson.Parent.Count; i++)
            {
                int iCurrentId = pPerson.Parent[i].Id;
                Person currentPerson = new Person();
                GetPerson(iCurrentId, ref currentPerson);
                pPerson.Parent[i] = currentPerson;
            }

            for (int i = 0; i < pPerson.Sibling.Count; i++)
            {
                int iCurrentId = pPerson.Sibling[i].Id;
                Person currentPerson = new Person();
                GetPerson(iCurrentId, ref currentPerson);
                pPerson.Sibling[i] = currentPerson;
            }

            for (int i = 0; i < pPerson.Descendant.Count; i++)
            {
                int iCurrentId = pPerson.Descendant[i].Id;
                Person currentPerson = new Person();
                GetPerson(iCurrentId, ref currentPerson);
                pPerson.Descendant[i] = currentPerson;
            }

            return true;
        }

        public bool GetPerson(int iId, ref Person pPerson)
        {
            OleDbDataReader oddrOleDbDataReader;

            string method = MethodBase.GetCurrentMethod().Name;


            pPerson = null;

            try
            {
                if (!dal.ExecuteReaderQuery("SELECT * FROM M WHERE ID=" + iId, out oddrOleDbDataReader))
                {
                    return false;
                }

                pPerson = new Person();

                if (oddrOleDbDataReader.Read())
                {
                    pPerson.Id = iId;
                    //pPerson.Sex = (int.Parse(oddrOleDbDataReader["SEX"].ToString()) == 1) ? true : false;

                    pPerson.Family = oddrOleDbDataReader["L_NAME"].ToString();
                    pPerson.Name = oddrOleDbDataReader["F_NAME"].ToString();
                    pPerson.OldFamily = oddrOleDbDataReader["F_NAME_OLD"].ToString();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        public bool GetAllPersons(out List<Person> allPersons, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string sql;

            int count = 0;
            int personId = 0;

            OleDbDataReader oddrOleDbDataReader;


            result = string.Empty;

            allPersons = null;

            try
            {
                tmrIntervalTimer = new System.Timers.Timer();
                tmrIntervalTimer.Interval = 2000;
                tmrIntervalTimer.Elapsed += new ElapsedEventHandler(tmrIntervalTimer_Elapsed);

                sql = "SELECT COUNT(*) FROM M";
                if (!dal.ExecuteScalarQuery(sql, out int numberOfPersons))
                {
                    result = $"Failed Preforming SQL[{sql}]";

                    return false;
                }

                Audit($"{numberOfPersons} Persons In Total", method, LINE(), AuditSeverity.Information);

                sql = "SELECT * FROM M";
                if (!dal.ExecuteReaderQuery(sql, out oddrOleDbDataReader))
                {
                    result = $"Failed Preforming SQL[{sql}]";

                    return false;
                }

                allPersons = new List<Person>();

                tmrIntervalTimer.Start();

                count = 0;
                while (oddrOleDbDataReader.Read())
                {
                    Person person = new Person();

                    try
                    {
                        personId = int.TryParse(oddrOleDbDataReader["ID"].ToString(), out personId) ? personId : Constants.NONE;
                        person.Id = personId;

                        int personSex = int.TryParse(oddrOleDbDataReader["SEX"].ToString(), out personSex) ? personSex : Constants.NONE;
                        switch (personSex)
                        {
                            case 1:
                                person.Sex = PersonSex.Male;
                                break;

                            case 2:
                                person.Sex = PersonSex.Female;
                                break;

                            default:
                                person.Sex = PersonSex.Unknown;
                                break;
                        }

                        person.Family = oddrOleDbDataReader["L_NAME"].ToString();
                        person.Name = oddrOleDbDataReader["F_NAME"].ToString();
                        person.OldFamily = oddrOleDbDataReader["F_NAME_OLD"].ToString();

                        int fatherId = int.TryParse(oddrOleDbDataReader["ID_FATHER"].ToString(), out fatherId) ? fatherId : Constants.NONE;
                        person.FatherId = fatherId;
                        person.FatherName = oddrOleDbDataReader["FATHER_NAME"].ToString();

                        int motherId = int.TryParse(oddrOleDbDataReader["ID_MOTHER"].ToString(), out motherId) ? motherId : Constants.NONE;
                        person.MotherId = motherId;
                        person.MotherName = oddrOleDbDataReader["MOTHER_NAME"].ToString();

                        person.BirthDate = oddrOleDbDataReader["B_YEAR"].ToString();
                        person.Street = oddrOleDbDataReader["STREET"].ToString();

                        int house = int.TryParse(oddrOleDbDataReader["HOUSE"].ToString(), out house) ? house : Constants.NONE; ;
                        person.House = house;

                        int cityId = int.TryParse(oddrOleDbDataReader["ID_CITY"].ToString(), out cityId) ? cityId : Constants.NONE; ;
                        person.CityId = cityId;

                        int countryId = int.TryParse(oddrOleDbDataReader["B_COUNTRY"].ToString(), out countryId) ? countryId : Constants.NONE; ;
                        person.CountryId = countryId;
                    }
                    catch (Exception ex)
                    {
                        Audit($"Error For Person ID[{personId}]. {ex.Message}", method, LINE(), AuditSeverity.Error);

                        continue;
                    }

                    allPersons.Add(person);

                    ++count;

                    loadAllPercentage = (100 * count) / numberOfPersons;
                }

                return true;
            }
            catch (Exception e)
            {
                result = $"Count[{count}] Person ID[{personId}] {e.Message}";

                return false;
            }
            finally
            {
                tmrIntervalTimer.Stop();
            }
        }

        public bool GetParents(ref Person pPerson)
        {
            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                if (!dal.ExecuteReaderQuery("SELECT * FROM M WHERE ID=" + pPerson.Id, out oddrOleDbDataReader))
                {
                    return false;
                }

                if (oddrOleDbDataReader.Read())
                {
                    Person _person = new Person();

                    _person.Id = int.TryParse(oddrOleDbDataReader["ID_MOTHER"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.Parent.Add(_person);

                    _person = new Person();
                    _person.Id = int.TryParse(oddrOleDbDataReader["ID_FATHER"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.Parent.Add(_person);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        public bool GetSiblings(ref Person pPerson)
        {
            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            string method = MethodBase.GetCurrentMethod().Name;
            string sSql;
            string result;

            try
            {
                sSql = CreateSiblingsSql(pPerson, out result);
                if (string.IsNullOrEmpty(sSql))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);

                    return false;
                }

                if (!dal.ExecuteReaderQuery(sSql, out oddrOleDbDataReader))
                {
                    return false;
                }

                while (oddrOleDbDataReader.Read())
                {
                    Person _person = new Person();

                    _person.Id = int.TryParse(oddrOleDbDataReader["ID"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.Sibling.Add(_person);
                }

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        public bool GetDescendants(ref Person pPerson)
        {
            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            string method = MethodBase.GetCurrentMethod().Name;
            string sSql;
            string result;

            try
            {
                sSql = CreateDescendantsSql(pPerson, out result);
                if (string.IsNullOrEmpty(sSql))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);

                    return false;
                }

                if (!dal.ExecuteReaderQuery(sSql, out oddrOleDbDataReader))
                {
                    return false;
                }

                while (oddrOleDbDataReader.Read())
                {
                    Person _person = new Person();

                    _person.Id = int.TryParse(oddrOleDbDataReader["ID"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.Descendant.Add(_person);
                }

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        public bool GetAllCountries(out Countries allCountries, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string sql;

            int count = 0;
            int countryId = 0;

            OleDbDataReader oddrOleDbDataReader;


            result = string.Empty;

            allCountries = null;

            try
            {
                tmrIntervalTimer = new System.Timers.Timer();
                tmrIntervalTimer.Interval = 2000;
                tmrIntervalTimer.Elapsed += new ElapsedEventHandler(tmrIntervalTimer_Elapsed);

                sql = "SELECT COUNT(*) FROM COUNTRY";
                if (!dal.ExecuteScalarQuery(sql, out int numberOfCountries))
                {
                    result = $"Failed Preforming SQL[{sql}]";

                    return false;
                }

                Audit($"{numberOfCountries} Countries In Total", method, LINE(), AuditSeverity.Information);

                sql = "SELECT * FROM COUNTRY";
                if (!dal.ExecuteReaderQuery(sql, out oddrOleDbDataReader))
                {
                    result = $"Failed Preforming SQL[{sql}]";

                    return false;
                }

                allCountries = new Countries();

                tmrIntervalTimer.Start();

                count = 0;
                while (oddrOleDbDataReader.Read())
                {
                    Country country = new Country();

                    try
                    {
                        countryId = int.TryParse(oddrOleDbDataReader["ID"].ToString(), out countryId) ? countryId : Constants.NONE;
                        country.Id = countryId;

                        country.Name = oddrOleDbDataReader["NAME_HEB"].ToString();
                        country.NameInEnglish = oddrOleDbDataReader["NAME_ENG"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Audit($"Error For Country ID[{countryId}]. {ex.Message}", method, LINE(), AuditSeverity.Error);

                        continue;
                    }

                    if (allCountries.Add(country, out result))
                    {
                        ++count;
                    }
                    else
                    {
                        Audit($"Failed Adding Country. {result}", method, LINE(), AuditSeverity.Warning);
                    }

                    loadAllPercentage = (100 * count) / numberOfCountries;
                }

                return true;
            }
            catch (Exception e)
            {
                result = $"Count[{count}] Country ID[{countryId}] {e.Message}";

                return false;
            }
            finally
            {
                tmrIntervalTimer.Stop();
            }
        }

        public bool GetAllCities(out Cities allCities, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string sql;

            int count = 0;
            int cityId = 0;

            OleDbDataReader oddrOleDbDataReader;


            result = string.Empty;

            allCities = null;

            try
            {
                tmrIntervalTimer = new System.Timers.Timer();
                tmrIntervalTimer.Interval = 2000;
                tmrIntervalTimer.Elapsed += new ElapsedEventHandler(tmrIntervalTimer_Elapsed);

                sql = "SELECT COUNT(*) FROM CITY";
                if (!dal.ExecuteScalarQuery(sql, out int numberOfCities))
                {
                    result = $"Failed Preforming SQL[{sql}]";

                    return false;
                }

                Audit($"{numberOfCities} Cities In Total", method, LINE(), AuditSeverity.Information);

                sql = "SELECT * FROM CITY";
                if (!dal.ExecuteReaderQuery(sql, out oddrOleDbDataReader))
                //if (!Dal.ExecuteReaderQuery($"SELECT * FROM M WHERE F_NAME = '{name}'", out oddrOleDbDataReader))
                {
                    result = $"Failed Preforming SQL[{sql}]";

                    return false;
                }

                allCities = new Cities();

                tmrIntervalTimer.Start();

                count = 0;
                while (oddrOleDbDataReader.Read())
                {
                    City city = new City();

                    try
                    {
                        cityId = int.TryParse(oddrOleDbDataReader["ID"].ToString(), out cityId) ? cityId : Constants.NONE;
                        city.Id = cityId;

                        city.Name = oddrOleDbDataReader["NAME_CITY"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Audit($"Error For City ID[{cityId}]. {ex.Message}", method, LINE(), AuditSeverity.Error);

                        continue;
                    }

                    if (allCities.Add(city, out result))
                    {
                        ++count;
                    }
                    else
                    {
                        Audit($"Failed Adding City. {result}", method, LINE(), AuditSeverity.Warning);
                    }

                    loadAllPercentage = (100 * count) / numberOfCities;
                }

                return true;
            }
            catch (Exception e)
            {
                result = $"Count[{count}] City ID[{cityId}] {e.Message}";

                return false;
            }
            finally
            {
                tmrIntervalTimer.Stop();
            }
        }

        #endregion

        #endregion

        #region Timers

        void tmrIntervalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmrIntervalTimer.Stop();

            LoadAllProgress?.Invoke(loadAllPercentage);

            tmrIntervalTimer.Start();
        }

        #endregion

        #region Dal Events Handlers

        private void Dal_Message(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            Audit(message, method, module, line, auditSeverity);
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
            string module = "BLL";

            Audit(message, method, module, line, auditSeverity);
        }

        public static int LINE([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }

        #endregion
    }
}
