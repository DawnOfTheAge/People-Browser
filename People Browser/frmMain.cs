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

namespace People_Browser
{
    public partial class frmMain : Form
    {
        #region Constants

        private const string CONNECTION_STRING_PREFIX = "Provider=Microsoft.Jet.OLEDB.4.0;";

        #endregion

        #region Data Members

        private Bll bll;
        
        private List<Person> Persons;
        
        private bool AuditOn;

        private string databaseFilePath;
        private string databaseConnectionString;

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

                //if (!CreateContextMenus(out result))
                //{
                //    return false;
                //}

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
                bll.SetConnectionString(databaseConnectionString);

                //if (!m_Bll.GetAllPersons(out Persons, out string result))
                //{
                //    Audit(result, method, LINE(), AuditSeverity.Warning);

                //    return;
                //}
                IAsyncResult asyncResult;

                asyncResult = Task.Run(() => bll.GetAllPersons(out Persons, out string result));

                while (!asyncResult.IsCompleted)
                {
                    Application.DoEvents();
                }


                Audit("Loaded All Persons", method, LINE(), AuditSeverity.Warning);
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
        
        #endregion

        //private bool OpenConnection()
        //{
        //    try
        //    {
        //        m_PersonDb = new List<Types.Person>();

        //        string myConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;";
        //        string sPath = "H:\\Stuff\\Db";
        //        string sDataSource = "Data Source=" + sPath + "\\TZ092005.mdb";

        //        myConnectionString += sDataSource;

        //        m_OleDbConnection = new OleDbConnection();
        //        m_OleDbConnection.ConnectionString = myConnectionString;
        //        m_OleDbConnection.Open();

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        //private bool CloseConnection()
        //{
        //    try
        //    {
        //        m_OleDbConnection.Close();

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        //private void Audit(string sLine, string sMethod, Enums.AuditSeverity asAuditSeverity)
        //{
        //    Console.WriteLine("[" + sMethod + "]:<" + asAuditSeverity + "> " + sLine);
        //}

        //private void Start()
        //{
        //    try
        //    {
        //        OpenConnection();

        //        string query = "Select count (*) From M";
        //        OleDbCommand command = new OleDbCommand(query, m_OleDbConnection);
        //        int iNumberOfRecords = int.Parse(command.ExecuteScalar().ToString());

        //        query = "Select * From M";
        //        command = new OleDbCommand(query, m_OleDbConnection);
        //        OleDbDataReader reader = command.ExecuteReader();

        //        int iPersonCounter = 0;

        //        while (reader.Read())
        //        {
        //            Types.Person p = new Types.Person();

        //            int iValue;

        //            p.ID = int.TryParse(reader["ID"].ToString(), out iValue) ? iValue : Constants.NONE;
        //            if (p.ID == Constants.NONE)
        //            {
        //                continue;
        //            }

        //            //p.MotherID = int.TryParse(reader["ID_MOTHER"].ToString(), out iValue) ? iValue : Constants.NONE;
        //            //p.FatherID = int.TryParse(reader["ID_FATHER"].ToString(), out iValue) ? iValue : Constants.NONE;

        //            //if (LegalId(p.FatherID) || LegalId(p.MotherID))
        //            //{
        //            //    p.Sibling = GetSiblings(p);
        //            //}

        //            p.Sex = (int.Parse(reader["SEX"].ToString()) == 1) ? true : false;

        //            p.Family = reader["L_NAME"].ToString();
        //            p.Name = reader["F_NAME"].ToString();
        //            p.OldFamily = reader["F_NAME_OLD"].ToString();

        //            p.Descendant = GetDescendants(p);

        //            m_PersonDb.Add(p);
        //            double dValue = ((double)iPersonCounter / (double)iNumberOfRecords) * 10000D;

        //            lblPercentage.Text = ((int)(dValue / 100D)).ToString() + "%";

        //            pbPersons.Value = (int)(dValue);

        //            iPersonCounter++;

        //            Application.DoEvents();
        //        }

        //        CloseConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private bool LegalId(int iId)
        //{
        //    if ((iId == Constants.NONE) || (iId == 0))
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private List<Types.Person> GetSiblings(Types.Person pPerson)
        //{
        //    string sQuery;
        //    string sMethod = MethodBase.GetCurrentMethod().Name;

        //    try
        //    {
        //        if ((pPerson.MotherID == Constants.NONE) && (pPerson.FatherID != Constants.NONE))
        //        {
        //            sQuery = "Select * FROM M WHERE ID_FATHER=" + pPerson.FatherID + " ";
        //        }
        //        else
        //        {
        //            if ((pPerson.MotherID != Constants.NONE) && (pPerson.FatherID == Constants.NONE))
        //            {
        //                sQuery = "Select * FROM M WHERE ID_MOTHER=" + pPerson.MotherID + " ";
        //            }
        //            else
        //            {
        //                sQuery = "Select * FROM M WHERE ID_FATHER=" + pPerson.FatherID + " AND ID_MOTHER=" + pPerson.MotherID + " ";
        //            }
        //        }

        //        OleDbCommand command = new OleDbCommand(sQuery, m_OleDbConnection);
        //        OleDbDataReader reader = command.ExecuteReader();

        //        List<Types.Person> Sibling = new List<Types.Person>();

        //        while (reader.Read())
        //        {
        //            Types.Person p = new Types.Person();

        //            int iValue;

        //            p.ID = int.TryParse(reader["ID"].ToString(), out iValue) ? iValue : Constants.NONE;
        //            if (p.ID == Constants.NONE)
        //            {
        //                continue;
        //            }

        //            p.MotherID = int.TryParse(reader["ID_MOTHER"].ToString(), out iValue) ? iValue : Constants.NONE;
        //            p.FatherID = int.TryParse(reader["ID_FATHER"].ToString(), out iValue) ? iValue : Constants.NONE;

        //            p.Sex = (int.Parse(reader["SEX"].ToString()) == 1) ? true : false;

        //            p.Family = reader["L_NAME"].ToString();
        //            p.Name = reader["F_NAME"].ToString();
        //            p.OldFamily = reader["F_NAME_OLD"].ToString();

        //            Sibling.Add(p);
        //        }

        //        return Sibling;
        //    }
        //    catch (Exception e)
        //    {
        //        Audit(e.Message, sMethod, Enums.AuditSeverity.Error);

        //        return null;
        //    }
        //}

        //private List<Types.Person> GetDescendants(Types.Person pPerson)
        //{
        //    string sQuery;
        //    string sMethod = MethodBase.GetCurrentMethod().Name;

        //    try
        //    {
        //        if (pPerson.Sex == true)
        //        {
        //            sQuery = "Select * FROM M WHERE ID_FATHER=" + pPerson.ID + " ";
        //        }
        //        else
        //        {
        //            sQuery = "Select * FROM M WHERE ID_MOTHER=" + pPerson.ID + " ";
        //        }

        //        OleDbCommand command = new OleDbCommand(sQuery, m_OleDbConnection);
        //        OleDbDataReader reader = command.ExecuteReader();

        //        List<Types.Person> Descendants = new List<Types.Person>();

        //        while (reader.Read())
        //        {
        //            Types.Person p = new Types.Person();

        //            int iValue;

        //            p.ID = int.TryParse(reader["ID"].ToString(), out iValue) ? iValue : Constants.NONE;
        //            if (p.ID == Constants.NONE)
        //            {
        //                continue;
        //            }

        //            p.MotherID = int.TryParse(reader["ID_MOTHER"].ToString(), out iValue) ? iValue : Constants.NONE;
        //            p.FatherID = int.TryParse(reader["ID_FATHER"].ToString(), out iValue) ? iValue : Constants.NONE;

        //            p.Sex = (int.Parse(reader["SEX"].ToString()) == 1) ? true : false;

        //            p.Family = reader["L_NAME"].ToString();
        //            p.Name = reader["F_NAME"].ToString();
        //            p.OldFamily = reader["F_NAME_OLD"].ToString();

        //            Descendants.Add(p);
        //        }

        //        return Descendants;
        //    }
        //    catch (Exception e)
        //    {
        //        Audit(e.Message, sMethod, Enums.AuditSeverity.Error);

        //        return null;
        //    }
        //}

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

        private void Settings_Message(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            try
            {
                Audit(message, method, module, line, auditSeverity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH-mm-ss dd-MM-yyyy}]:<{auditSeverity}>:<{module}>:<{method}> {message + ". Error:" + ex.Message}");
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
