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
        public event EventHandler DalError;
        public event EventHandler DalMessage;

        private OleDbConnection m_OleDbConnection;
 
        public Dal()
        {
        }

        public void SetConnectionString(string sConnectionString)
        {
            OpenConnection(sConnectionString);
        }

        private bool OpenConnection(string sConnectionString)
        {
            try
            {
                m_OleDbConnection = new OleDbConnection();
                m_OleDbConnection.ConnectionString = sConnectionString;
                m_OleDbConnection.Open();

                OnDalMessage("Connection Opened With Connection String[" + sConnectionString + "]");

                return true;
            }
            catch (Exception e)
            {
                OnDalError(e.Message);

                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                m_OleDbConnection.Close();

                return true;
            }
            catch (Exception e)
            {
                OnDalError(e.Message);

                return false;
            }
        }

        public void OnDalError(string sErrorMessage)
        {
            if (DalError != null)
            {
                DalError(sErrorMessage, null);
            }
        }

        public void OnDalMessage(string sMessage)
        {
            if (DalMessage != null)
            {
                DalMessage(sMessage, null);
            }
        }

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
                OnDalError(e.Message);

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
                OnDalError(e.Message);

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
                OnDalError("Connection State[" + m_OleDbConnection.State + "]");

                return false;
            }

            odcOleDbCommand = new OleDbCommand(sSql, m_OleDbConnection);
            try
            {
                oddrOleDbDataReader = odcOleDbCommand.ExecuteReader();

                OnDalMessage("SQL[" + sSql + "] Executed");

                return true;
            }
            catch (OleDbException e)
            {
                OnDalError(e.Message);

                return false;
            }
        }
    }
}
