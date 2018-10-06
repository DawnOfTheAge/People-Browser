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

namespace People_Browser
{
    public partial class frmMain : Form
    {
        private Bll m_Bll;
        //private List<Types.Person> m_PersonDb;

        //private OleDbConnection m_OleDbConnection;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            string myConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;";
            string sPath = "H:\\Stuff\\Db";
            string sDataSource = "Data Source=" + sPath + "\\TZ092005.mdb";

            myConnectionString += sDataSource;

            m_Bll = new Bll();
            m_Bll.BllError += m_Bll_BllError;
            m_Bll.BllMessage += m_Bll_BllMessage;
            m_Bll.SetConnectionString(myConnectionString);

            Types.Person pPerson;

            m_Bll.GetPerson(13297205, out pPerson);

            //OpenConnection();
        }

        void m_Bll_BllMessage(object sender, EventArgs e)
        {
            string sErrorMessage = (string)sender;

            Debug.WriteLine(sErrorMessage);
        }

        void m_Bll_BllError(object sender, EventArgs e)
        {
            string sMessage = (string)sender;

            Debug.WriteLine(sMessage);
        }

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
    }
}
