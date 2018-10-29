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

        public bool GetPersonInformation(int iId, ref Types.Person pPerson)
        {
            GetPerson(iId, ref pPerson);
            GetParents(ref pPerson);
            GetSiblings(ref pPerson);
            GetDescendants(ref pPerson);

            for (int i = 0; i < pPerson.Parent.Count; i++)
            {
                int iCurrentId = pPerson.Parent[i].ID;
                Types.Person currentPerson = new Types.Person();
                GetPerson(iCurrentId, ref currentPerson);
                pPerson.Parent[i] = currentPerson;
            }

            for (int i = 0; i < pPerson.Sibling.Count; i++)
            {
                int iCurrentId = pPerson.Sibling[i].ID;
                Types.Person currentPerson = new Types.Person();
                GetPerson(iCurrentId, ref currentPerson);
                pPerson.Sibling[i] = currentPerson;
            }

            for (int i = 0; i < pPerson.Descendant.Count; i++)
            {
                int iCurrentId = pPerson.Descendant[i].ID;
                Types.Person currentPerson = new Types.Person();
                GetPerson(iCurrentId, ref currentPerson);
                pPerson.Descendant[i] = currentPerson;
            }

            return true;
        }

        public bool GetPerson(int iId, ref Types.Person pPerson)
        {
            #region Data Members

            OleDbDataReader oddrOleDbDataReader;

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
                    pPerson.ID = iId;
                    pPerson.Sex = (int.Parse(oddrOleDbDataReader["SEX"].ToString()) == 1) ? true : false;

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
                Audit(e.Message, sMethod, Enums.AuditSeverity.Error);

                return false;
            }
        }

        public bool GetParents(ref Types.Person pPerson)
        {
            #region Data Members

            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            string sMethod = MethodBase.GetCurrentMethod().Name;

            #endregion

            try
            {
                if (!m_Dal.ExecuteReaderQuery("SELECT * FROM M WHERE ID=" + pPerson.ID, out oddrOleDbDataReader))
                {
                    return false;
                }

                if (oddrOleDbDataReader.Read())
                {
                    Types.Person _person = new Types.Person();

                    _person.ID = int.TryParse(oddrOleDbDataReader["ID_MOTHER"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.Parent.Add(_person);

                    _person = new Types.Person();
                    _person.ID = int.TryParse(oddrOleDbDataReader["ID_FATHER"].ToString(), out iValue) ? iValue : Constants.NONE;
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
                Audit(e.Message, sMethod, Enums.AuditSeverity.Error);

                return false;
            }
        }

        public bool GetSiblings(ref Types.Person pPerson)
        {
            #region Data Members

            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            string sMethod = MethodBase.GetCurrentMethod().Name;
            string sSql;
            string sResult;

            #endregion

            try
            {
                sSql = CreateSiblingsSql(pPerson, out sResult);
                if (string.IsNullOrEmpty(sSql))
                {
                    Audit(sResult, sMethod, Enums.AuditSeverity.Warning);

                    return false;
                }

                if (!m_Dal.ExecuteReaderQuery(sSql, out oddrOleDbDataReader))
                {
                    return false;
                }

                while (oddrOleDbDataReader.Read())
                {
                    Types.Person _person = new Types.Person();

                    _person.ID = int.TryParse(oddrOleDbDataReader["ID"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.Sibling.Add(_person);
                }

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, sMethod, Enums.AuditSeverity.Error);

                return false;
            }
        }

        public bool GetDescendants(ref Types.Person pPerson)
        {
            #region Data Members

            OleDbDataReader oddrOleDbDataReader;

            int iValue;

            string sMethod = MethodBase.GetCurrentMethod().Name;
            string sSql;
            string sResult;

            #endregion

            try
            {
                sSql = CreateDescendantsSql(pPerson, out sResult);
                if (string.IsNullOrEmpty(sSql))
                {
                    Audit(sResult, sMethod, Enums.AuditSeverity.Warning);

                    return false;
                }

                if (!m_Dal.ExecuteReaderQuery(sSql, out oddrOleDbDataReader))
                {
                    return false;
                }

                while (oddrOleDbDataReader.Read())
                {
                    Types.Person _person = new Types.Person();

                    _person.ID = int.TryParse(oddrOleDbDataReader["ID"].ToString(), out iValue) ? iValue : Constants.NONE;
                    pPerson.Descendant.Add(_person);
                }

                return true;
            }
            catch (Exception e)
            {
                Audit(e.Message, sMethod, Enums.AuditSeverity.Error);

                return false;
            }
        }

        private string CreateSiblingsSql(Types.Person pPerson, out string sResult)
        {
            sResult = "";

            if (pPerson == null)
            {
                sResult = "Person Object Is Null";

                return "";
            }

            if (pPerson.Parent == null)
            {
                sResult = "Person Parents Information Is Null";

                return "";
            }

            if ((pPerson.Parent[0].ID <= 0) && (pPerson.Parent[1].ID <= 0))
            {
                sResult = "Person Has No Parents Information";

                return "";
            }

            if (pPerson.Parent[0].ID <= 0)
            {
                return "SELECT * FROM M WHERE ID_FATHER=" + pPerson.Parent[1].ID + " AND ID<>" + pPerson.ID;
            }

            if (pPerson.Parent[1].ID <= 0)
            {
                return "SELECT * FROM M WHERE ID_MOTHER=" + pPerson.Parent[0].ID + " AND ID<>" + pPerson.ID;
            }

            return "SELECT * FROM M WHERE ID_FATHER=" + pPerson.Parent[1].ID +
                   " AND ID_MOTHER=" + pPerson.Parent[0].ID +
                   " AND ID<>" + pPerson.ID + " ";
        }

        private string CreateDescendantsSql(Types.Person pPerson, out string sResult)
        {
            sResult = "";

            if (pPerson == null)
            {
                sResult = "Person Object Is Null";

                return "";
            }

            if (pPerson.ID <= 0) 
            {
                sResult = "Person Has No Valid ID";

                return "";
            }

            string sParent = (pPerson.Sex) ? "ID_FATHER" : "ID_MOTHER";

            return "SELECT * FROM M WHERE " + sParent + "=" + pPerson.ID;
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
