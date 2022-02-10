using System;
using System.Data.OleDb;
using System.Reflection;
using System.Data;
using People.Browser.Common;

namespace People.Browser.DAL
{
    public class MsAccessDal
    {
        #region Events

        public event AuditMessage Message;

        #endregion

        #region Data Members

        private OleDbConnection m_OleDbConnection;

        #endregion

        #region Constructor

        public MsAccessDal()
        {
        }

        #endregion

        #region Connection

        public void SetConnectionString(string sConnectionString)
        {
            OpenConnection(sConnectionString);
        }

        private bool OpenConnection(string sConnectionString)
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                m_OleDbConnection = new OleDbConnection
                {
                    ConnectionString = sConnectionString
                };
                m_OleDbConnection.Open();

                Audit("Connection Opened With Connection String[" + sConnectionString + "]",
                      method,
                      LINE(),
                      AuditSeverity.Information);

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        private bool CloseConnection()
        {
            string method = MethodBase.GetCurrentMethod().Name;

            try
            {
                m_OleDbConnection.Close();

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        #endregion

        #region Queries Execution

        public bool ExecuteNonQuery(string sSql)
        {
            OleDbCommand odcOleDbCommand;

            string method = MethodBase.GetCurrentMethod().Name;

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
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        public bool ExecuteScalarQuery(string sql, out int scalar)
        {
            OleDbCommand odcOleDbCommand;

            string method = MethodBase.GetCurrentMethod().Name;


            scalar = Constants.NONE;

            if (m_OleDbConnection.State != ConnectionState.Open)
            {
                return false;
            }

            odcOleDbCommand = new OleDbCommand(sql, m_OleDbConnection);
            try
            {
                bool bRc = int.TryParse(odcOleDbCommand.ExecuteScalar().ToString(), out scalar);

                return bRc;
            }
            catch (OleDbException e)
            {
                Audit(e.Message, method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        public bool ExecuteReaderQuery(string sSql, out OleDbDataReader oddrOleDbDataReader)
        {
            OleDbCommand odcOleDbCommand;

            string method = MethodBase.GetCurrentMethod().Name;


            oddrOleDbDataReader = null;

            if (m_OleDbConnection.State != ConnectionState.Open)
            {
                Audit("Connection State[" + m_OleDbConnection.State + "]", method, LINE(), AuditSeverity.Warning);

                return false;
            }

            odcOleDbCommand = new OleDbCommand(sSql, m_OleDbConnection);
            try
            {
                oddrOleDbDataReader = odcOleDbCommand.ExecuteReader();

                Audit("SQL[" + sSql + "] Executed", method, LINE(), AuditSeverity.Information);

                return true;
            }
            catch (OleDbException e)
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
            string module = "DAL";

            Audit(message, method, module, line, auditSeverity);
        }

        public static int LINE([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }

        #endregion
    }
}
