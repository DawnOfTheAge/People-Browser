﻿using System;
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
using System.Runtime;
using General.Database.Common;
using System.ServiceProcess;

namespace People_Browser
{
    public partial class FrmMain : Form
    {
        #region Constants

        private const string CONNECTION_STRING_PREFIX = "Provider=Microsoft.Jet.OLEDB.4.0;";

        #endregion

        #region Data Members

        private MsAccessBll msAccessBll;

        private MongoDbBll mongoDbBll;

        private List<Person> persons;

        private Countries countries;
        
        private Cities cities;

        private bool AuditOn;

        private string databaseFilePath;
        private string databaseConnectionString;

        private ContextMenu personsContextMenu;

        #endregion

        #region Constructor

        public FrmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region Startup

        private void FrmMain_Load(object sender, EventArgs e)
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

                mnuConnect.Enabled = true;
                mnuSearch.Enabled = false;

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

        private void MnuConnectAccess_Click(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                MnuConnectAccess.Enabled = false;
                MnuConnectMongoDB.Enabled = false;

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

                //OpenDatabaseParameters openDatabaseParameters = new OpenDatabaseParameters()
                //{
                //    DatabaseName = "PersonsDb",
                //    DatabaseIpAddress = "127.0.0.1",
                //    DatabaseIpPort = "27017",
                //    DatabaseTables = "Persons:Cities:Countries"
                //};

                msAccessBll = new MsAccessBll();
                msAccessBll.Message += Bll_Message;
                msAccessBll.LoadAllProgress += Bll_LoadAllProgress;
                msAccessBll.SetConnectionString(databaseConnectionString);
                //if (!bll.OpenMongoDb(openDatabaseParameters, out result))
                //{
                //    Audit($"Failed Connecting Mongo DB. {result}", method, LINE(), AuditSeverity.Warning);
                //}

                lblMessage.Text = "Loading 'Persons' ...";
                if (!GetAllPersons())
                {
                    Audit("Failed Loading 'Persons'", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                lblMessage.Text = "Loading 'Countries' ...";
                if (!GetAllCountries())
                {
                    Audit("Failed Loading 'Countries'", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                lblMessage.Text = "Loading 'Cities' ...";
                if (!GetAllCities())
                {
                    Audit("Failed Loading 'Cities'", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                Thread.Sleep(1000);

                pbPercentage.Visible = false;
                lblPercentage.Visible = false;
                lblMessage.Visible = false;

                mnuConnect.Enabled = false;
                MnuConnectAccess.Enabled = false;
                MnuConnectMongoDB.Enabled = false;
                mnuSearch.Enabled = true;

                if (!ctlCurrentPerson.SetForSearch(cities, countries, out result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);
                }

                ctlCurrentPerson.Message += CtlCurrentPerson_Message;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }
        
        private void MnuConnectMongoDB_Click(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                if (!IsMongoDbServiceRunning(out result))
                {
                    Audit($"Mongo DB Service Is Not Running. Status[{result}]", method, LINE(), AuditSeverity.Warning);
                    DialogResult dialogResult = MessageBox.Show("Start Mongo DB Service?",
                                                                $"Mongo DB Service Is Not Running. Status[{result}]",
                                                                MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (!StartMongoDbService(out result))
                        {
                            Audit($"Failed Starting Mongo DB Service. {result}", method, LINE(), AuditSeverity.Warning);

                            return;
                        }

                        Audit("<<< Mongo DB Service Started >>>", method, LINE(), AuditSeverity.Information);
                    }
                    else
                    {
                        return;
                    }
                }

                MnuConnectAccess.Enabled = false;
                MnuConnectMongoDB.Enabled = false;

                OpenDatabaseParameters openDatabaseParameters = new OpenDatabaseParameters()
                {
                    DatabaseName = "PersonsDb",
                    DatabaseIpAddress = "127.0.0.1",
                    DatabaseIpPort = "27017",
                    DatabaseTables = "Persons:Cities:Countries"
                };

                mongoDbBll = new MongoDbBll();
                mongoDbBll.Message += Bll_Message;

                if (!mongoDbBll.OpenMongoDb(openDatabaseParameters, out result))
                {
                    Audit($"Failed Connecting Mongo DB. {result}", method, LINE(), AuditSeverity.Warning);
                }

                lblMessage.Text = "Loading 'Persons' ...";
                if (!GetAllPersons())
                {
                    Audit("Failed Loading 'Persons'", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                lblMessage.Text = "Loading 'Countries' ...";
                if (!GetAllCountries())
                {
                    Audit("Failed Loading 'Countries'", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                lblMessage.Text = "Loading 'Cities' ...";
                if (!GetAllCities())
                {
                    Audit("Failed Loading 'Cities'", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                Thread.Sleep(1000);

                pbPercentage.Visible = false;
                lblPercentage.Visible = false;
                lblMessage.Visible = false;

                mnuConnect.Enabled = false;
                MnuConnectAccess.Enabled = false;
                MnuConnectMongoDB.Enabled = false;
                mnuSearch.Enabled = true;

                if (!ctlCurrentPerson.SetForSearch(cities, countries, out result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);
                }

                ctlCurrentPerson.Message += CtlCurrentPerson_Message;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        private void MnuSearch_Click(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result = string.Empty;

            try
            {
                FrmSearch search = new FrmSearch(cities, countries);
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
                if (!ctlCurrentPerson.Clear(out string result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);

                    return;
                }

                if (!SearchByFilter(persons, searchFilter, out List<Person> personsSearchResult, out result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);

                    return;
                }

                if (!FillPersons(personsSearchResult, out result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);

                    return;
                }
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

        private void MnuExit_Click(object sender, EventArgs e)
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
                personsContextMenu.MenuItems.Add("הורים", new EventHandler(FindParentsEvent));
                personsContextMenu.MenuItems.Add("אחים", new EventHandler(FindSiblingsEvent));
                personsContextMenu.MenuItems.Add("ילדים", new EventHandler(FindChildrenEvent));
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

        private void FindChildrenEvent(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                int rowIndex = dgvPersons.SelectedRows[0].Index;
                if (rowIndex == Constants.NONE)
                {
                    return;
                }

                PersonSex personSex = GetPersonSex(dgvPersons.Rows[rowIndex].DefaultCellStyle.BackColor);
                
                int id = int.TryParse(dgvPersons.Rows[rowIndex].Cells[0].Value.ToString(), out id) ? id : Constants.NONE;
                if (!IsIdValid(id))
                {
                    Audit("No Id", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                Person childrenSearchFilter = new Person();
                switch (personSex)
                {
                    case PersonSex.Male:
                        childrenSearchFilter.FatherId = id;
                        break;

                    case PersonSex.Female:
                        childrenSearchFilter.MotherId = id;
                        break;

                    default:
                        Audit($"No Sex Defined. Can't Find Children", method, LINE(), AuditSeverity.Warning);
                        return;
                }

                Search_SearchParameters(childrenSearchFilter);
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        private void FindSiblingsEvent(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                int rowIndex = dgvPersons.SelectedRows[0].Index;
                if (rowIndex == Constants.NONE)
                {
                    return;
                }

                int fatherId = int.TryParse(dgvPersons.Rows[rowIndex].Cells[9].Value.ToString(), out fatherId) ? fatherId : Constants.NONE;
                if (!IsIdValid(fatherId))
                {
                    Audit("No Father Id", method, LINE(), AuditSeverity.Warning);
                }

                int motherId = int.TryParse(dgvPersons.Rows[rowIndex].Cells[11].Value.ToString(), out motherId) ? motherId : Constants.NONE;
                if (!IsIdValid(motherId))
                {
                    Audit("No Mother Id", method, LINE(), AuditSeverity.Warning);
                }

                if (!IsIdValid(fatherId) && !IsIdValid(motherId))
                {
                    Audit("No Mother&Father Ids", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                Person siblingsSearchFilter = new Person
                {
                    FatherId = fatherId,
                    MotherId = motherId
                };

                Search_SearchParameters(siblingsSearchFilter);
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        private void FindParentsEvent(object sender, EventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                int rowIndex = dgvPersons.SelectedRows[0].Index;
                if (rowIndex == Constants.NONE)
                {
                    return;
                }

                int fatherId = int.TryParse(dgvPersons.Rows[rowIndex].Cells[9].Value.ToString(), out fatherId) ? fatherId : Constants.NONE;
                if (!IsIdValid(fatherId))
                {
                    Audit("No Father Id", method, LINE(), AuditSeverity.Warning);
                }

                int motherId = int.TryParse(dgvPersons.Rows[rowIndex].Cells[11].Value.ToString(), out motherId) ? motherId : Constants.NONE;
                if (!IsIdValid(motherId))
                {
                    Audit("No Mother Id", method, LINE(), AuditSeverity.Warning);
                }

                if (!IsIdValid(fatherId) && !IsIdValid(motherId))
                {
                    Audit("No Mother&Father Ids", method, LINE(), AuditSeverity.Warning);

                    return;
                }

                List<Person> parents = new List<Person>();

                if (!GetPersonById(fatherId, out var father, out string result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);
                }

                if (father != null)
                {
                    parents.Add(father);
                }

                if (!GetPersonById(motherId, out var mother, out result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);
                }

                if (mother != null)
                {
                    parents.Add(mother);
                }

                if (!FillPersons(parents, out result))
                {
                    Audit(result, method, LINE(), AuditSeverity.Warning);

                    return;
                }
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
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
                    int newRowIndex = dgvPersons.Rows.Add(new string[] { person.Id.ToString(), 
                                                                         person.Family, 
                                                                         person.OldFamily, 
                                                                         person.Name,
                                                                         GetBirthDate(person.BirthDate), 
                                                                         GetCity(person.CityId), 
                                                                         person.Street, 
                                                                         person.House.ToString(), 
                                                                         GetCountry(person.CountryId),
                                                                         person.FatherId.ToString(),
                                                                         person.FatherName,
                                                                         person.MotherId.ToString(),
                                                                         person.MotherName });
                    dgvPersons.Rows[newRowIndex].DefaultCellStyle.BackColor = SexColor(person.Sex);
                }
                
                dgvPersons.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                return true;
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
             
                return false;
            }
        }
        
        private void DgvPersons_MouseDown(object sender, MouseEventArgs e)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string result;

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

                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    int rowIndex = dgvPersons.SelectedRows[0].Index;
                    if (rowIndex == Constants.NONE)
                    {
                        return;
                    }

                    int id = int.TryParse(dgvPersons.Rows[rowIndex].Cells[0].Value.ToString(), out id) ? id : Constants.NONE;
                    int fatherId = int.TryParse(dgvPersons.Rows[rowIndex].Cells[9].Value.ToString(), out fatherId) ? fatherId : Constants.NONE;
                    int motherId = int.TryParse(dgvPersons.Rows[rowIndex].Cells[11].Value.ToString(), out motherId) ? motherId : Constants.NONE;
                    string cityName = dgvPersons.Rows[rowIndex].Cells[5].Value.ToString();
                    string countryName = dgvPersons.Rows[rowIndex].Cells[8].Value.ToString();
                    int house = int.TryParse(dgvPersons.Rows[rowIndex].Cells[7].Value.ToString(), out house) ? house : Constants.NONE;

                    int cityId = Constants.NONE;
                    if (!string.IsNullOrEmpty(cityName))
                    {
                        if (!cities.GetCityIdByCityName(cityName, out cityId, out result))
                        {
                            Audit(result, method, LINE(), AuditSeverity.Warning);
                        }
                    }

                    int countryId = Constants.NONE;
                    if (!string.IsNullOrEmpty(countryName))
                    {
                        if (!countries.GetCountryIdByCountryName(countryName, out countryId, out result))
                        {
                            Audit(result, method, LINE(), AuditSeverity.Warning);
                        }
                    }

                    PersonSex personSex = GetPersonSex(dgvPersons.Rows[rowIndex].DefaultCellStyle.BackColor);

                    Person person = new Person()
                    {
                        Id = id,
                        Family = dgvPersons.Rows[rowIndex].Cells[1].Value.ToString(),
                        OldFamily = dgvPersons.Rows[rowIndex].Cells[2].Value.ToString(),
                        Name = dgvPersons.Rows[rowIndex].Cells[3].Value.ToString(),
                        BirthDate = dgvPersons.Rows[rowIndex].Cells[4].Value.ToString(),
                        CityId = cityId,
                        Street = dgvPersons.Rows[rowIndex].Cells[6].Value.ToString(),
                        House = house,
                        CountryId = countryId,
                        FatherId = fatherId,
                        MotherId = motherId,
                        FatherName = dgvPersons.Rows[rowIndex].Cells[10].Value.ToString(),
                        MotherName = dgvPersons.Rows[rowIndex].Cells[12].Value.ToString(),
                        Sex = personSex
                    };

                    if (!ctlCurrentPerson.Fill(person, out result))
                    {
                        Audit(result, method, LINE(), AuditSeverity.Warning);
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                Audit(ex.Message, method, LINE(), AuditSeverity.Error);
            }
        }

        private Color SexColor(PersonSex personSex)
        {
            switch (personSex)
            {
                case PersonSex.Male:
                    return Color.PaleTurquoise;

                case PersonSex.Female:
                    return Color.LightPink;

                default:
                    return Color.MintCream;
            }
        }

        private PersonSex GetPersonSex(Color color)
        {
            if (color == Color.PaleTurquoise)
            {
                return PersonSex.Male;
            }

            if (color == Color.LightPink)
            {
                return PersonSex.Female;
            }

            return PersonSex.Unknown;
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

                IAsyncResult asyncResult;

                switch (GetRunningDatabase())
                {
                    case RunningDatabase.MsAccess:
                        asyncResult = Task.Run(() => msAccessBll.GetAllPersons(out persons, out result));
                        break;

                    case RunningDatabase.MongoDB:
                        asyncResult = Task.Run(() => mongoDbBll.GetAllPersons(out persons, out result));
                        break;

                    default:
                        return false;
                }

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

        private bool GetPersonById(int id, out Person person, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            result = string.Empty;

            person = null;

            try
            {
                person = persons.FirstOrDefault(currentPerson => currentPerson.Id == id);   

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
                IAsyncResult asyncResult;

                switch (GetRunningDatabase())
                {
                    case RunningDatabase.MsAccess:
                        asyncResult = Task.Run(() => msAccessBll.GetAllCountries(out countries, out result));
                        break;

                    case RunningDatabase.MongoDB:
                        asyncResult = Task.Run(() => mongoDbBll.GetAllCountries(out countries, out result));
                        break;

                    default:
                        return false;
                }

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
                IAsyncResult asyncResult;

                switch (GetRunningDatabase())
                {
                    case RunningDatabase.MsAccess:
                        asyncResult = Task.Run(() => msAccessBll.GetAllCities(out cities, out result));
                        break;

                    case RunningDatabase.MongoDB:
                        asyncResult = Task.Run(() => mongoDbBll.GetAllCities(out cities, out result));
                        break;

                    default:
                        return false;
                }                

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

        private bool SearchByFilter(List<Person> persons, Person searchFilter, out List<Person> personsSearchResult, out string result)
        {
            string method = MethodBase.GetCurrentMethod().Name;
            string parmeterName;

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

                personsSearchResult = persons;

                #region Id

                if (searchFilter.Id != Constants.NONE)
                {
                    personsSearchResult = personsSearchResult.Where(person => person.Id.ToString().Contains(searchFilter.Id.ToString())).ToList();

                    parmeterName = nameof(searchFilter.Id);
                    Audit($"Filter[{parmeterName} - '{searchFilter.Id}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Father Id

                if (searchFilter.FatherId != Constants.NONE)
                {
                    personsSearchResult = personsSearchResult.Where(person => person.FatherId.ToString().Contains(searchFilter.FatherId.ToString())).ToList();

                    parmeterName = nameof(searchFilter.FatherId);
                    Audit($"Filter[{parmeterName} - '{searchFilter.FatherId}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Mother Id

                if (searchFilter.MotherId != Constants.NONE)
                {
                    personsSearchResult = personsSearchResult.Where(person => person.MotherId.ToString().Contains(searchFilter.MotherId.ToString())).ToList();

                    parmeterName = nameof(searchFilter.MotherId);
                    Audit($"Filter[{parmeterName} - '{searchFilter.MotherId}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Name

                if (!string.IsNullOrEmpty(searchFilter.Name))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.Name.Contains(searchFilter.Name)).ToList();

                    parmeterName = nameof(searchFilter.Name);
                    Audit($"Filter[{parmeterName} - '{searchFilter.Name}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Father Name

                if (!string.IsNullOrEmpty(searchFilter.FatherName))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.FatherName.Contains(searchFilter.FatherName)).ToList();

                    parmeterName = nameof(searchFilter.FatherName);
                    Audit($"Filter[{parmeterName} - '{searchFilter.FatherName}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Mother Name

                if (!string.IsNullOrEmpty(searchFilter.MotherName))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.MotherName.Contains(searchFilter.MotherName)).ToList();

                    parmeterName = nameof(searchFilter.MotherName);
                    Audit($"Filter[{parmeterName} - '{searchFilter.MotherName}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Family

                if (!string.IsNullOrEmpty(searchFilter.Family))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.Family.Contains(searchFilter.Family)).ToList();

                    parmeterName = nameof(searchFilter.Family);
                    Audit($"Filter[{parmeterName} - '{searchFilter.Family}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Old Family

                if (!string.IsNullOrEmpty(searchFilter.OldFamily))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.OldFamily.Contains(searchFilter.OldFamily)).ToList();

                    parmeterName = nameof(searchFilter.OldFamily);
                    Audit($"Filter[{parmeterName} - '{searchFilter.OldFamily}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region City

                if (searchFilter.CityId != Constants.NONE)
                {
                    personsSearchResult = personsSearchResult.Where(person => person.CityId == searchFilter.CityId).ToList();

                    parmeterName = nameof(searchFilter.CityId);

                    if (!cities.GetCityNameByCityId(searchFilter.CityId, out string cityName, out result))
                    {
                        Audit(result, method, LINE(), AuditSeverity.Warning);
                    }

                    Audit($"Filter[{parmeterName} - '{cityName}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Street

                if (!string.IsNullOrEmpty(searchFilter.Street))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.Street.Contains(searchFilter.Street)).ToList();

                    parmeterName = nameof(searchFilter.Street);
                    Audit($"Filter[{parmeterName} - '{searchFilter.Street}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region House

                if ((searchFilter.House != 0) && (searchFilter.House != Constants.NONE))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.House == searchFilter.House).ToList();

                    parmeterName = nameof(searchFilter.House);
                    Audit($"Filter[{parmeterName} - '{searchFilter.House}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Country

                if (searchFilter.CountryId != Constants.NONE)
                {
                    personsSearchResult = personsSearchResult.Where(person => person.CountryId == searchFilter.CountryId).ToList();

                    parmeterName = nameof(searchFilter.CountryId);

                    if (!countries.GetCountryNameByCountryId(searchFilter.CountryId, out string countryName, out _, out result))
                    {
                        Audit(result, method, LINE(), AuditSeverity.Warning);
                    }

                    Audit($"Filter[{parmeterName} - '{countryName}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Sex

                if (searchFilter.Sex != PersonSex.Unknown)
                {
                    personsSearchResult = personsSearchResult.Where(person => person.Sex == searchFilter.Sex).ToList();

                    parmeterName = nameof(searchFilter.Sex);
                    Audit($"Filter[{parmeterName} - '{searchFilter.Sex}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                #region Birth Date
                
                if (!string.IsNullOrEmpty(searchFilter.BirthDateYear))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.BirthDate.Contains(searchFilter.BirthDateYear)).ToList();

                    parmeterName = nameof(searchFilter.BirthDateYear);
                    Audit($"Filter[{parmeterName} - '{searchFilter.BirthDateYear}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                if (!string.IsNullOrEmpty(searchFilter.BirthDateMonth))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.BirthDate.Contains(searchFilter.BirthDateMonth)).ToList();

                    parmeterName = nameof(searchFilter.BirthDate);
                    Audit($"Filter[{parmeterName} - '{searchFilter.BirthDateMonth}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                if (!string.IsNullOrEmpty(searchFilter.BirthDateDay))
                {
                    personsSearchResult = personsSearchResult.Where(person => person.BirthDate.Contains(searchFilter.BirthDateDay)).ToList();

                    parmeterName = nameof(searchFilter.BirthDate);
                    Audit($"Filter[{parmeterName} - '{searchFilter.BirthDateDay}']  Number Of Hits[{personsSearchResult.Count}]", method, LINE(), AuditSeverity.Information);
                }

                #endregion

                lblNumberOfHits.Text = $"{personsSearchResult.Count} Records";

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

            if (!cities.GetCityNameByCityId(cityId, out string cityName, out _))
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

            if (!countries.GetCountryNameByCountryId(countryId, out string countryName, out _, out _))
            {
                countryName = string.Empty;
            }

            return countryName;
        }

        private bool IsIdValid(int id)
        {
            return ((id != 0) && (id != Constants.NONE));
        }

        private bool IsMongoDbServiceRunning(out string result)
        {
            result = string.Empty;

            try
            {
                ServiceController sc = new ServiceController("mongodb");

                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        return true;

                    default:
                        result = sc.Status.ToString();
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

        private bool StartMongoDbService(out string result)
        {
            result = string.Empty;

            try
            {
                ServiceController sc = new ServiceController("mongodb");

                if (sc.Status.Equals(ServiceControllerStatus.Stopped) ||
                    sc.Status.Equals(ServiceControllerStatus.StopPending))
                {
                    sc.Start();
                }
                else
                {
                    sc.Stop();
                }

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        private RunningDatabase GetRunningDatabase()
        {
            if (msAccessBll == null)
            {
                return (mongoDbBll == null) ? RunningDatabase.Unknown : RunningDatabase.MongoDB;
            }

            return RunningDatabase.MsAccess;
        }

        #endregion        

        #region Events Handlers

        private void CtlCurrentPerson_Message(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            Audit(message, method, module, line, auditSeverity);
        }
        
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
            Audit(message, method, "People Browser GUI", line, auditSeverity);
        }

        public static int LINE([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }

        #endregion
    }
}
