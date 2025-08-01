using System;
using System.Data;
using System.Xml.Linq;
using DVLD_DataAccess;


namespace Buisness
{
    public  class clsPeople
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int    _PersonID { set; get; }
        public string _FirstName { set; get; }
        public string _SecondName { set; get; }
        public string _ThirdName { set; get; }
        public string _LastName { set; get; }
        public string FullName
        {
            get { return _FirstName + " " + _SecondName + " " + _ThirdName + " " + _LastName; }

        }
        public string _NationalNo { set; get; }
        public DateTime _DateOfBirth { set; get; }
        public short _Gendor { set; get; }
        public string _Address { set; get; }
        public string _Phone { set; get; }
        public string _Email { set; get; }
        public int _NationalityCountryID { set; get; }

        public clsCountry CountryInfo;

        private string _ImagePath;
      
        public string ImagePath   
        {
            get { return _ImagePath; }   
            set { _ImagePath = value; }  
        }

        public clsPeople()

        {
            this._PersonID = -1;
            this._FirstName = "";
            this._SecondName = "";
            this._ThirdName = "";
            this._LastName = "";
            this._DateOfBirth = DateTime.Now;
            this._Address = "";
            this._Phone = "";
            this._Email = "";
            this._NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }

        private clsPeople(int _Person_ID, string _FirstName,string _SecondName, string _ThirdName,
            string _LastName,string _NationalNo, DateTime _DateOfBirth,short _Gendor,
             string _Address, string _Phone, string _Email,
            int _NationalityCountry_ID, string ImagePath)

        {
            this._PersonID = _Person_ID;
            this._FirstName = _FirstName;
            this._SecondName= _SecondName;
            this._ThirdName = _ThirdName;
            this._LastName = _LastName;
            this._NationalNo = _NationalNo;   
            this._DateOfBirth = _DateOfBirth;
            this._Gendor= _Gendor;
            this._Address = _Address;
            this._Phone = _Phone;
            this._Email = _Email;
            this._NationalityCountryID = _NationalityCountry_ID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find_ByID(_NationalityCountry_ID);
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            //call DataAccess Layer 

            this._PersonID = clsPersonData.AddNewPerson(
                this._FirstName,this._SecondName ,this._ThirdName,
                this._LastName,this._NationalNo,
                this._DateOfBirth, this._Gendor, this._Address, this._Phone, this._Email,
                this._NationalityCountryID, this.ImagePath);

            return (this._PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            //call DataAccess Layer 

            return clsPersonData.UpdatePerson(
                this._PersonID, this._FirstName,this._SecondName,this._ThirdName,
                this._LastName, this._NationalNo, this._DateOfBirth, this._Gendor,
                this._Address, this._Phone, this._Email, 
                  this._NationalityCountryID, this.ImagePath);
        }

        public static clsPeople Find_ByID(int _Person_ID)
        {

            string _FirstName = "", _SecondName = "", _ThirdName = "", _LastName = "",_NationalNo="", _Email = "", _Phone = "", _Address = "", ImagePath = "";
            DateTime _DateOfBirth = DateTime.Now;
            int _NationalityCountry_ID = -1;
            short _Gendor = 0;

            bool IsFound = clsPersonData.GetPersonInfoBy_ID 
                                (
                                    _Person_ID, ref _FirstName, ref _SecondName,
                                    ref _ThirdName, ref _LastName, ref _NationalNo, ref _DateOfBirth,
                                    ref _Gendor, ref _Address, ref _Phone, ref _Email,
                                    ref _NationalityCountry_ID, ref ImagePath
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsPeople(_Person_ID, _FirstName,_SecondName ,_ThirdName, _LastName,
                          _NationalNo, _DateOfBirth,_Gendor, _Address, _Phone, _Email,_NationalityCountry_ID, ImagePath);
            else
                return null;
        }

        public static clsPeople Find(string _NationalNo)
        {
            string _FirstName = "", _SecondName = "", _ThirdName = "", _LastName = "",  _Email = "", _Phone = "", _Address = "", ImagePath = "";
            DateTime _DateOfBirth = DateTime.Now;
            int _Person_ID=-1,_NationalityCountry_ID = -1;
            short _Gendor = 0;

            bool IsFound = clsPersonData.GetPersonInfoBy_NationalNo
                                (
                                    _NationalNo, ref _Person_ID, ref _FirstName, ref _SecondName,
                                    ref _ThirdName, ref _LastName, ref _DateOfBirth,
                                    ref _Gendor,ref _Address, ref _Phone, ref _Email,
                                    ref _NationalityCountry_ID, ref ImagePath
                                );

            if (IsFound)

                return new clsPeople(_Person_ID, _FirstName, _SecondName, _ThirdName, _LastName,
                          _NationalNo, _DateOfBirth,_Gendor, _Address, _Phone, _Email, _NationalityCountry_ID, ImagePath);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }

            return false;
        }

        public static DataTable GetAll()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool Delete(int _ID)
        {
            return clsPersonData.DeletePerson(_ID); 
        }

        public static bool isPersonExist(int _ID)
        {
           return clsPersonData.IsPersonExist(_ID);
        }

        public static bool isPersonExist(string NationlNo)
        {
            return clsPersonData.IsPersonExist(NationlNo);
        }

    }
}
