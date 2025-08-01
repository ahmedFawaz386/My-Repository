using System;
using System.Data;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsCountry
    {

        public int _ID { set; get; }
        public string _CountryName { set; get; }
   
        public clsCountry()

        {
            this._ID = -1;
            this._CountryName = "";

        }

        private clsCountry(int _ID, string CountryName)

        {
            this._ID = _ID;
            this._CountryName = CountryName;
        }

        public static clsCountry Find_ByID(int _ID)
        {
            string CountryName = "";

            if (clsCountryData.GetCountryInfoBy_ID(_ID, ref CountryName))

                return new clsCountry(_ID, CountryName);
            else
                return null;

        }

        public static clsCountry Find(string CountryName)
        {

            int _ID = -1;
           
            if (clsCountryData.GetCountryInfoByName(CountryName, ref _ID ))

                return new clsCountry(_ID, CountryName);
            else
                return null;

        }

        public static DataTable GetAll()
        {
            return clsCountryData.GetAllCountries();

        }

    }
}
