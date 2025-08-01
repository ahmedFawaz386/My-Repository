using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_Buisness;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsDrivers
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsPeople PersonInfo;

        public int _DriverID { set; get; }
        public int _PersonID { set; get; }
        public int _CreatedByUserID { set; get; }
        public DateTime _CreatedDate {  get; }

        public clsDrivers()

        {
            this._DriverID = -1;
            this._PersonID = -1;
            this._CreatedByUserID = -1;
            this._CreatedDate=DateTime.Now;
            Mode = enMode.AddNew;

        }

        public clsDrivers(int _Driver_ID, int _Person_ID,int _CreatedByUser_ID, DateTime _CreatedDate)

        {
            this._DriverID = _Driver_ID;
            this._PersonID = _Person_ID;
            this._CreatedByUserID = _CreatedByUser_ID;
            this._CreatedDate = _CreatedDate;
            this.PersonInfo = clsPeople.Find_ByID(_Person_ID);

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            //call DataAccess Layer 

            this._DriverID = clsDriverData.AddNewDriver( _PersonID,  _CreatedByUserID);
              

            return (this._DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            //call DataAccess Layer 

            return clsDriverData.UpdateDriver(this._DriverID,this._PersonID,this._CreatedByUserID);
        }

        public static clsDrivers Find_ByID(int _Driver_ID)
        {
            
            int _Person_ID = -1; int _CreatedByUser_ID = -1;DateTime _CreatedDate= DateTime.Now; 

            if (clsDriverData.GetDriverInfoBy_Driver_ID(_Driver_ID, ref _Person_ID,ref _CreatedByUser_ID,ref _CreatedDate))

                return new clsDrivers(_Driver_ID,  _Person_ID,  _CreatedByUser_ID,  _CreatedDate);
            else
                return null;

        }

        public static clsDrivers Find_ByPersonID(int _Person_ID)
        {

            int _Driver_ID = -1; int _CreatedByUser_ID = -1; DateTime _CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoBy_Person_ID( _Person_ID, ref _Driver_ID,  ref _CreatedByUser_ID, ref _CreatedDate))

                return new clsDrivers(_Driver_ID, _Person_ID, _CreatedByUser_ID, _CreatedDate);
            else
                return null;

        }

        public static DataTable GetAll()
        {
            return clsDriverData.GetAllDrivers();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }

        public static DataTable GetLicenses(int _Driver_ID)
        {
            return clsLicenses.GetAll(_Driver_ID);
        }

        public static DataTable GetInternationalLicenses(int _Driver_ID)
        {
            return clsInternationalLicenses.GetAll(_Driver_ID);
        }

    }
}
