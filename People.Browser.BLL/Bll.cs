using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using People.Browser.Common;
using People.Browser.Dal;
using System.Data.OleDb;
using System.Reflection;

namespace People.Browser.BLL
{
    public class Bll
    {
        #region Events

        public event EventHandler BllMessage;

        #endregion

        #region Data Members

        private readonly string m_ModuleName = "BLL";

        private Dal.Dal m_Dal; 

        #endregion

        public Bll()
        {
            m_Dal = new Dal.Dal();
            m_Dal.DalMessage += m_Dal_DalMessage;
        }

        ~Bll()
        {
            m_Dal.DalMessage -= m_Dal_DalMessage;
        }

        public void SetConnectionString(string sConnectionString)
        {
            m_Dal.SetConnectionString(sConnectionString);
        }

        public bool GetPerson(int iId, out Common.Types.Person pPerson)
        {
            #region Data Members

            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            string sMethod = MethodBase.GetCurrentMethod().Name;

            #endregion

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
                Audit(e.Message, sMethod, Enums.AuditSeverity.Error);

                return false;
            }
        }

        #region Dal Events Handlers

        private void m_Dal_DalMessage(object sender, EventArgs e)
        {
            OnBllMessage((Types.AuditMessage)e);
        }

        #endregion

        #region Events Handlers

        public void OnBllMessage(Types.AuditMessage amAuditMessage)
        {
            if (BllMessage != null)
            {
                BllMessage(null, amAuditMessage);
            }
        }

        #endregion

        private void Audit(string sMessage, string sMethod, Enums.AuditSeverity asAuditSeverity)
        {
            OnBllMessage(new Types.AuditMessage(sMessage, m_ModuleName, sMethod, asAuditSeverity));
        }
    }
}
