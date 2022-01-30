using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{ 
    public class Cities
    {
        #region Data Members

        private List<City> cities;

        #endregion

        #region Constructor

        public Cities()
        {
            cities = new List<City>();
        }

        #endregion

        #region Public Methods

        public bool Add(City city, out string result)
        {
            result = string.Empty;

            try
            {
                if (cities == null)
                {
                    result = "Citeis List Is Null";

                    return false;
                }

                cities.Add(city);

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        public bool GetCityNameByCityId(int cityId, out string cityName, out string result)
        {
            result = string.Empty;
            cityName = string.Empty;

            try
            {
                if ((cities == null) || (cities.Count == 0))
                {
                    result = "Citeis List Is Null Or Empty";

                    return false;
                }

                City city = cities.First(currentCity => currentCity.Id == cityId);
                cityName = city.Name;   

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        public int Count()
        {
            return (cities == null) ? Constants.NONE : cities.Count;
        }

        public List<City> CitiesList()
        {
            return cities;
        }

        #endregion
    }
}
