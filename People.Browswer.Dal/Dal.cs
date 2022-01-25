using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Reflection;
using System.Data;
using People.Browser.Common;

namespace People.Browser.Dal
{
    public class Dal
    {
        #region Events

        public event EventHandler DalMessage;

        #endregion

        #region Data Members

        private OleDbConnection m_OleDbConnection;

        private readonly string m_ModuleName = "DAL"; 

        #endregion

        public Dal()
        {
        }

        public void SetConnectionString(string sConnectionString)
        {
            OpenConnection(sConnectionString);
        }

        private bool OpenConnection(string sConnectionString)
        {
            string sMethod = MethodBase.GetCurrentMethod().Name;

            try
            {
                m_OleDbConnection = new OleDbConnection();
                m_OleDbConnection.ConnectionString = sConnectionString;
                m_OleDbConnection.Open();

                Audit("Connection Opened With Connection String[" + sConnectionString + "]",
                      sMethod,
                      AuditSeverity.Information);

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, sMethod, AuditSeverity.Error);

                return false;
            }
        }

        private bool CloseConnection()
        {
            string sMethod = MethodBase.GetCurrentMethod().Name;

            try
            {
                m_OleDbConnection.Close();

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, sMethod, AuditSeverity.Error);

                return false;
            }
        }

        public void OnDalMessage(Types.AuditMessage amAuditMessage)
        {
            if (DalMessage != null)
            {
                DalMessage(null, amAuditMessage);
            }
        }

        #region Queries Execution

        public bool ExecuteNonQuery(string sSql)
        {
            #region Data Members

            OleDbCommand odcOleDbCommand;

            string sMethod = MethodBase.GetCurrentMethod().Name;

            #endregion

            if (m_OleDbConnection.State != ConnectionState.Open)
            {
                return false;
            }

            odcOleDbCommand = new OleDbCommand(sSql, m_OleDbConnection);
            try
            {
                odcOleDbCommand.ExecuteNonQuery();

                return true;
            }
            catch (OleDbException e)
            {
                Audit(e.Message, sMethod, AuditSeverity.Error);

                return false;
            }
        }

        public bool ExecuteScalarQuery(string sSql, out int iScalar)
        {
            #region Data Members

            OleDbCommand odcOleDbCommand;

            string sMethod = MethodBase.GetCurrentMethod().Name;

            #endregion

            iScalar = Constants.NONE;

            if (m_OleDbConnection.State != ConnectionState.Open)
            {
                return false;
            }

            odcOleDbCommand = new OleDbCommand(sSql, m_OleDbConnection);
            try
            {
                bool bRc = int.TryParse(odcOleDbCommand.ExecuteScalar().ToString(), out iScalar);

                return bRc;
            }
            catch (OleDbException e)
            {
                Audit(e.Message, sMethod, AuditSeverity.Error);

                return false;
            }
        }

        public bool ExecuteReaderQuery(string sSql, out OleDbDataReader oddrOleDbDataReader)
        {
            #region Data Members

            OleDbCommand odcOleDbCommand;

            string sMethod = MethodBase.GetCurrentMethod().Name;

            #endregion

            oddrOleDbDataReader = null;

            if (m_OleDbConnection.State != ConnectionState.Open)
            {
                Audit("Connection State[" + m_OleDbConnection.State + "]", sMethod, AuditSeverity.Warning);

                return false;
            }

            odcOleDbCommand = new OleDbCommand(sSql, m_OleDbConnection);
            try
            {
                oddrOleDbDataReader = odcOleDbCommand.ExecuteReader();

                Audit("SQL[" + sSql + "] Executed", sMethod, AuditSeverity.Information);

                return true;
            }
            catch (OleDbException e)
            {
                Audit(e.Message, sMethod, AuditSeverity.Error);

                return false;
            }
        } 

        #endregion

        private void Audit(string sMessage, string sMethod, AuditSeverity asAuditSeverity)
        {
            OnDalMessage(new Types.AuditMessage(sMessage, m_ModuleName, sMethod, asAuditSeverity));
        }
    }
}
