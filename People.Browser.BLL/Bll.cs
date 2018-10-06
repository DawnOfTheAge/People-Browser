using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using People.Browser.Common;
using People.Browser.Dal;
using System.Data.OleDb;

namespace People.Browser.BLL
{
    public class Bll
    {
        public event EventHandler BllError;
        public event EventHandler BllMessage;

        private Dal.Dal m_Dal;

        public Bll()
        {
            m_Dal = new Dal.Dal();
            m_Dal.DalError += m_Dal_DalError;
            m_Dal.DalMessage += m_Dal_DalMessage;
        }

        public void SetConnectionString(string sConnectionString)
        {
            m_Dal.SetConnectionString(sConnectionString);
        }

        private void m_Dal_DalError(object sender, EventArgs e)
        {
            string sErrorMessage = (string)sender;

            OnBllError(sErrorMessage);
        }

        private void m_Dal_DalMessage(object sender, EventArgs e)
        {
            string sMessage = (string)sender;

            OnBllMessage(sMessage);
        }

        public bool GetPerson(int iId, out Common.Types.Person pPerson)
        {
            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            pPerson = null;

            try
            {
                if (!m_Dal.ExecuteReaderQuery("SELECT * FROM M WHERE ID=" + iId, out oddrOleDbDataReader))
                {
                    return false;
                }

                pPerson = new Types.Person();

                if (oddrOleDbDataReader.Read())
                {
                    pPerson.Sex = (int.Parse(oddrOleDbDataReader["SEX"].ToString()) == 1) ? true : false;

                    pPerson.Family = oddrOleDbDataReader["L_NAME"].ToString();
                    pPerson.Name = oddrOleDbDataReader["F_NAME"].ToString();
                    pPerson.OldFamily = oddrOleDbDataReader["F_NAME_OLD"].ToString();

                    pPerson.MotherID = int.TryParse(oddrOleDbDataReader["ID_MOTHER"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.FatherID = int.TryParse(oddrOleDbDataReader["ID_FATHER"].ToString(), out iValue) ? iValue : Constants.NONE;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                OnBllError(e.Message);

                return false;
            }
        }

        public void OnBllError(string sErrorMessage)
        {
            if (BllError != null)
            {
                BllError(sErrorMessage, null);
            }
        }

        public void OnBllMessage(string sMessage)
        {
            if (BllMessage != null)
            {
                BllMessage(sMessage, null);
            }
        }
    }
}
