using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Reflection;
using People.Browser.Common;
using People.Browser.BLL;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Globalization;
using People.Browser.UI;

namespace People_Browser
{
    public partial class frmMain : Form
    {
        #region Constants

        private const string CONNECTION_STRING_PREFIX = "Provider=Microsoft.Jet.OLEDB.4.0;";

        #endregion

        #region Data Members

        private Bll bll;
        
        private List<Person> persons;

        private Countries countries;
        
        private Cities cities;

        private bool AuditOn;

        private string databaseFilePath;
        private string databaseConnectionString;

        private ContextMenu personsContextMenu;

        #endregion

        #region Constructor

        public frmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region Startup

        private void frmMain_Load(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                Location = Cursor.Position;

                if (!Initialize(out string result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);
                }
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }

            //string myConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;";
            //string sPath = @"F:\Stuff\Db";
            //string sDataSource = "Data Source=" + sPath + "\\TZ092005.mdb";

            //myConnectionString += sDataSource;

            //m_Bll = new Bll();
            //m_Bll.BllMessage += m_Bll_BllMessage;
            //m_Bll.SetConnectionString(myConnectionString);

            //List<Types.Person> l1;
            //m_Bll.GetAllPerson("ספיר", out l1);

            //List<Types.Person> l2;
            //m_Bll.GetAllPerson("עמרי", out l2);

            //for (int i = 0; i < l1.Count; i++)
            //{
            //    for (int j = 0; j < l2.Count; j++)
            //    {
            //        if ((l1[i].FatherId == l2[j].FatherId) && (l1[i].FatherId != 0) && (l1[i].FatherId != Constants.NONE))
            //        {
            //            Console.WriteLine($"ID:{l1[i].FatherId}  NAME:{l1[i].FatherName}");
            //        }
            //    }
            //}

            //List<Types.Person> l3 = l1.Intersect(l2).ToList();

            //int x = 5;
            ////Types.Person pPerson;
            ////Types.FamilyRelation _familyRelation = new Types.FamilyRelation(new Types.Person(), 
            ////                                                                Enums.DirectFamilyRelation.Husband, 
            ////                                                                new Types.Person(), 
            ////                                                                Enums.DirectFamilyRelation.Sister);
            ////bool bRc = _familyRelation.IsDirectFamilyRelationValid();

            ////pPerson = new Types.Person();
            ////m_Bll.GetPersonInformation(27809870, ref pPerson);

            ////m_Bll.GetPerson(27809870, out pPerson);
            ////m_Bll.GetParents(ref pPerson);
            ////m_Bll.GetSiblings(ref pPerson);
            ////m_Bll.GetDescendants(ref pPerson);

            ////OpenConnection();
        }

        private bool Initialize(out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                if (!Prologue(out result))
                {
                    return false;
                }

                #region Initializations

                AuditOn = true;


                #endregion

                #region Get Configuration



                #endregion

                #region Audit

                dgvAudit.Visible = AuditOn;
                if (!AuditOn)
                {
                    splitContainer.Panel2Collapsed = true;
                }

                #endregion

                #region Context Menus

                if (!CreateContextMenus(out result))
                {
                    return false;
                }

                Audit("Create Context Menus", method, LINE(), AuditSeverity.Information);

                #endregion

                if (!Epilogue(out result))
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        private bool Prologue(out string result)
        {
            result = string.Empty;

            try
            {
                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        private bool Epilogue(out string result)
        {
            result = string.Empty;

            try
            {
                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        #endregion

        #region Gui
        
        #region Main Menu

        private void mnuConnect_Click(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                OpenFileDialog openFile = new OpenFileDialog
                {
                    Title = "Select Database File",
                    Filter = "All files (*.*)|*.*",
                    CheckFileExists = true,
                    CheckPathExists = true
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    databaseFilePath = openFile.FileName;
                }

                if (string.IsNullOrEmpty(databaseFilePath))
                {
                    Audit("Database File Name Is Null Or Empty", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                if (!File.Exists(databaseFilePath))
                {
                    Audit($"Database File Name[{databaseFilePath}] Does Not Exist", method, LINE(), AuditSeverity.Warning);
                    
                    return;
                }

                databaseConnectionString = $@"{CONNECTION_STRING_PREFIX}Data Source={databaseFilePath}";

                bll = new Bll();
                bll.Message += Bll_Message;
                bll.LoadAllProgress += Bll_LoadAllProgress;
                bll.SetConnectionString(databaseConnectionString);

                lblMessage.Text = "Loading Persons ...";
                GetAllPersons();

                lblMessage.Text = "Loading Countries ...";
                GetAllCountries();

                lblMessage.Text = "Loading Cities ...";
                GetAllCities();

                Thread.Sleep(1000);

                pbPercentage.Visible = false;
                lblPercentage.Visible = false;
                lblMessage.Visible = false;                
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }
        
        private void mnuSearch_Click(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                frmSearch search = new frmSearch(cities, countries);
                search.SearchParameters += Search_SearchParameters;
                search.ShowDialog();
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        private void Search_SearchParameters(Person searchFilter)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        private void Bll_LoadAllProgress(int percentage)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                SetProgressBar(percentage);
                SetProgressNumber(percentage);
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        #region Context Menus

        private bool CreateContextMenus(out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            result = string.Empty;

            try
            {
                #region Persons Context Menu

                personsContextMenu = new ContextMenu();
                //personsContextMenu.MenuItems.Add("Set Device Instance ID To Config File", new EventHandler(SetDevicesIdsEvent));
                //personsContextMenu.MenuItems.Add("-");

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

        #region Invoke Methods

        private void SetProgressNumber(int value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OneIntegerParameterDelegate(SetProgressNumber), value);
            }
            else
            {
                lblPercentage.Text = $"{value}%";
            }
        }

        private void SetProgressBar(int value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OneIntegerParameterDelegate(SetProgressBar), value);
            }
            else
            {
                pbPercentage.Value = value;
            }
        }

        #endregion

        #region Grids

        public bool FillPersons(List<Person> personsList, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            
            result = string.Empty;

            try
            {
                if ((personsList == null) || (personsList.Count == 0))
                {
                    result = "Persons List Is Null Or Empty";

                    return false;
                }

                dgvPersons.Rows.Clear();
                foreach (Person person in personsList)
                {
                    dgvPersons.Rows.Add(new string[] { person.Id.ToString(), 
                                                       person.Family, 
                                                       person.OldFamily, 
                                                       person.Name,
                                                       GetBirthDate(person.BirthDate), 
                                                       GetCity(person.CityId), 
                                                       person.Street, 
                                                       person.House.ToString(), 
                                                       GetCountry(person.CountryId) });

                    dgvPersons.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }

                return true;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
             
                return false;
            }
        }
        
        private void dgvPersons_MouseDown(object sender, MouseEventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            DataGridView.HitTestInfo hitTestInfo;

            try
            {
                if ((dgvPersons.Rows == null) || (dgvPersons.Rows.Count == 0))
                {
                    Audit("No Persons", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvPersons.HitTest(e.X, e.Y);
                    if ((hitTestInfo.Type == DataGridViewHitTestType.RowHeader) || (hitTestInfo.Type == DataGridViewHitTestType.Cell))
                    {
                        personsContextMenu.Show(dgvPersons, new Point(e.X, e.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        #endregion

        #endregion

        #region Get

        private bool GetAllPersons()
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                IAsyncResult asyncResult = Task.Run(() => bll.GetAllPersons(out persons, out result));

                while (!asyncResult.IsCompleted)
                {
                    Application.DoEvents();
                }

                if (string.IsNullOrEmpty(result))
                {
                    Audit($"Loaded All Persons. {persons.Count} Records", method, LINE(), AuditSeverity.Information);
                }
                else
                {
                    Audit($"Failed Loading All Persons. Loaded {persons.Count} Records. {result}", method, LINE(), AuditSeverity.Warning);
                }

                return true;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);

                return false;   
            }
        }

        private bool GetAllCountries()
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                IAsyncResult asyncResult = Task.Run(() => bll.GetAllCountries(out countries, out result));

                while (!asyncResult.IsCompleted)
                {
                    Application.DoEvents();
                }

                if (string.IsNullOrEmpty(result))
                {
                    Audit($"Loaded All Countries. {countries.Count()} Records", method, LINE(), AuditSeverity.Information);
                }
                else
                {
                    Audit($"Failed Loading All Countries. Loaded {countries.Count()} Records. {result}", method, LINE(), AuditSeverity.Warning);
                }

                return true;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        private bool GetAllCities()
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                IAsyncResult asyncResult = Task.Run(() => bll.GetAllCities(out cities, out result));

                while (!asyncResult.IsCompleted)
                {
                    Application.DoEvents();
                }

                if (string.IsNullOrEmpty(result))
                {
                    Audit($"Loaded All Cities. {cities.Count()} Records", method, LINE(), AuditSeverity.Information);
                }
                else
                {
                    Audit($"Failed Loading All Cities. Loaded {cities.Count()} Records. {result}", method, LINE(), AuditSeverity.Warning);
                }

                return true;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        #endregion

        #region Search

        private bool SearchByFilter(List<Person> persons, Person searchFilter, List<Person> personsSearchResult, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            personsSearchResult = null;
            result = string.Empty;

            try
            {
                if ((persons == null) || (persons.Count == 0))
                {
                    result = "Persons List Is Null Or Empty";

                    return false;
                }

                if (searchFilter == null)
                {
                    result = "Search Filter Is Null";

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

        private bool GetSiblings(int personId, out List<int> siblingsIds, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            List<Person> siblings;

            siblingsIds = null;
            result = string.Empty;

            try
            {
                if ((persons == null) || (persons.Count == 0))
                {
                    result = "Persons List Is Null Or Empty";

                    return false;
                }

                Person person = persons.First(currentPerson => currentPerson.Id == personId);
                if (person == null)
                {
                    result = $"Person With ID[{personId}] not Found";

                    return false;
                }

                if ((person.MotherId == Constants.NONE) && (person.FatherId != Constants.NONE))
                {
                    siblings = persons.Where(currentPerson => currentPerson.FatherId == person.FatherId).ToList();
                }
                else
                {
                    if ((person.MotherId != Constants.NONE) && (person.FatherId == Constants.NONE))
                    {
                        siblings = persons.Where(currentPerson => currentPerson.MotherId == person.MotherId).ToList();
                    }
                    else
                    {
                        siblings = persons.Where(currentPerson => ((currentPerson.MotherId == person.MotherId) && 
                                                                   (currentPerson.FatherId == person.FatherId))).ToList();
                    }
                }

                siblingsIds = siblings.Select(sibling => sibling.Id).ToList();

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        private bool GetDescendants(int personId, out List<int> descendantsIds, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            List<Person> descendants;

            descendantsIds = null;
            result = string.Empty;

            try
            {
                if ((persons == null) || (persons.Count == 0))
                {
                    result = "Persons List Is Null Or Empty";

                    return false;
                }

                Person person = persons.First(currentPerson => currentPerson.Id == personId);
                if (person == null)
                {
                    result = $"Person With ID[{personId}] not Found";

                    return false;
                }

                switch (person.Sex)
                {
                    case PersonSex.Male:
                        descendants = persons.Where(currentPerson => currentPerson.FatherId == person.Id).ToList();
                        break;

                    case PersonSex.Female:
                        descendants = persons.Where(currentPerson => currentPerson.MotherId == person.Id).ToList();
                        break;

                    default:
                        result = $"Person With ID[{person}] Has No Sex Defined";
                        return false;
                }

                descendantsIds = descendants.Select(descendant => descendant.Id).ToList();

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        #endregion

        #region Utils

        private bool LegalId(int iId)
        {
            if ((iId == Constants.NONE) || (iId == 0))
            {
                return false;
            }

            return true;
        }

        private string GetBirthDate(string birthDate)
        {
            if (string.IsNullOrEmpty(birthDate))
            {
                return string.Empty;
            }

            DateTime dtBirthDate = DateTime.TryParseExact(birthDate, 
                                                          "yyyyMMdd",
                                                          new CultureInfo("he-IL"),
                                                          DateTimeStyles.None, 
                                                          out dtBirthDate) ? dtBirthDate : new DateTime(2000, 1, 1);

            return dtBirthDate.ToString("dd/MM/yyyy");
        }

        private string GetCity(int cityId)
        {
            if (cityId == 0)
            {
                return string.Empty;
            }

            if ((cities == null) || (cities.Count() == 0))
            {
                return string.Empty;
            }

            if (cities.GetCityNameByCityId(cityId, out string cityName, out _))
            { 
                cityName = string.Empty;
            }

            return cityName;
        }

        private string GetCountry(int countryId)
        {
            if (countryId == 0)
            {
                return string.Empty;
            }

            if ((countries == null) || (countries.Count() == 0))
            {
                return string.Empty;
            }

            if (countries.GetCountryNameByCountryId(countryId, out string countryName, out _, out _))
            {
                countryName = string.Empty;
            }

            return countryName;
        }

        #endregion        

        #region Events Handlers

        private void Bll_Message(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            Audit(message, method, module, line, auditSeverity);
        }

        #endregion 

        #region Audit

        private Color SeverityColor(AuditSeverity auditSeverity)
        {
            switch (auditSeverity)
            {
                case AuditSeverity.Information:
                    return Color.SeaShell;

                case AuditSeverity.Important:
                    return Color.Aqua;

                case AuditSeverity.Warning:
                    return Color.Coral;

                case AuditSeverity.Error:
                    return Color.Red;

                case AuditSeverity.Critical:
                    return Color.Purple;

                default:
                    return Color.FromArgb(0, 0, 0, 0);
            }
        }

        private void Audit(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            string dateTime = DateTime.Now.ToString("HH:mm:ss.fff");

            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new AuditMessage(Audit), message, method, module, line, auditSeverity);
                }
                else
                {
                    if (AuditOn)
                    {
                        dgvAudit.Rows.Insert(0, new string[] { dateTime, auditSeverity.ToString(), module, method, line.ToString(), message });
                        dgvAudit.Rows[0].DefaultCellStyle.BackColor = SeverityColor(auditSeverity);

                        dgvAudit.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                    else
                    {
                        if ((auditSeverity == AuditSeverity.Error) || (auditSeverity == AuditSeverity.Warning))
                        {
                            MessageBox.Show(message,
                                            module,
                                            MessageBoxButtons.OK,
                                            (auditSeverity == AuditSeverity.Error) ?
                                            MessageBoxIcon.Error :
                                            MessageBoxIcon.Warning);
                        }
                    }

                    Console.WriteLine($"[{dateTime}]:<{auditSeverity}>:<{module}>:<{method}:{line}> {message}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"[{dateTime}]:<{auditSeverity}>:<{module}>:<{method}:{line}> {message + ". Audit Error:" + e.Message}");
            }
        }

        private void Audit(string message, string method, int line, AuditSeverity auditSeverity)
        {
            Audit(message, method, "MOPS Config Tool", line, auditSeverity);
        }

        public static int LINE([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }

        #endregion       
    }
}
