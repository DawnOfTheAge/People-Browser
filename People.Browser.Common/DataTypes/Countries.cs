using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common.DataTypes
{
    public class Countries
    {
        #region Data Members

        private List<Country> countries;

        #endregion

        #region Constructor

        public Countries()
        {
            countries = new List<Country>();
        }

        #endregion

        #region Public Methods

        public bool Add(Country country, out string result)
        {
            result = string.Empty;

            try
            {
                if (countries == null)
                {
                    result = "Countries List Is Null";

                    return false;
                }

                countries.Add(country);

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        public bool GetCountryNameByCountryId(int countryId, out string countryName, out string countryNameInEnglish, out string result)
        {
            result = string.Empty;
            countryName = string.Empty;
            countryNameInEnglish = string.Empty;

            try
            {
                if ((countries == null) || (countries.Count == 0))
                {
                    result = "Countries List Is Null Or Empty";

                    return false;
                }

                Country country  = countries.First(currentCountry => currentCountry.Id == countryId);
                countryName = country.Name;
                countryNameInEnglish = country.NameInEnglish;

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        #endregion
    }
}
