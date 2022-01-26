using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Browser.Common
{
    public class Country
    {        
        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string NameInEnglish { get; set; }

        #endregion

        #region Constructor

        public Country(int id, string name, string nameInEnglish)
        {
            Id = id;
            Name = name;
            NameInEnglish = nameInEnglish;
        }

        #endregion
    }
}
