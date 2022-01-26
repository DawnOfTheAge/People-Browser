using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public class City
    {
        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region Constructor

        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion
    }
}
