using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsDetainedLicenses
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int _DetainID { set; get; }
        public int _LicenseID { set; get; }
        public DateTime _DetainDate { set; get; }

        public float  _FineFees { set; get; }
        public int _CreatedByUserID { set; get; }
        public clsUsers _CreatedByUserInfo { set; get; }
        public bool _IsReleased { set; get; }
        public DateTime _ReleaseDate { set; get; }
        public int _ReleasedByUserID { set; get; }
        public clsUsers ReleasedByUserInfo { set; get; }
        public int _ReleaseApplicationID { set; get; }
       
        public clsDetainedLicenses()

        {
            this._DetainID = -1;
            this._LicenseID = -1;
            this._DetainDate = DateTime.Now;
            this._FineFees = 0;
            this._CreatedByUserID = -1;
            this._IsReleased = false;
            this._ReleaseDate = DateTime.MaxValue;
            this._ReleasedByUserID = 0;
            this._ReleaseApplicationID = -1;



            Mode = enMode.AddNew;

        }

        public clsDetainedLicenses(int Detain_ID,
            int _License_ID,  DateTime DetainDate,
            float Fine_Fees,  int _CreatedByUser_ID,
            bool IsReleased,  DateTime ReleaseDate,
            int ReleasedByUser_ID,  int Release_Application_ID)

        {
            this._DetainID = Detain_ID;
            this._LicenseID = _License_ID;
            this._DetainDate = DetainDate;
            this._FineFees = Fine_Fees;
            this._CreatedByUserID = _CreatedByUser_ID;
            this._CreatedByUserInfo = clsUsers.Find_ByID(this._CreatedByUserID);
            this._IsReleased = IsReleased;
            this._ReleaseDate = ReleaseDate;
            this._ReleasedByUserID = ReleasedByUser_ID;
            this._ReleaseApplicationID = Release_Application_ID;
            this.ReleasedByUserInfo= clsUsers.Find_ByID(this._ReleasedByUserID);
            Mode = enMode.Update;
        }

        private bool _AddNewDetainedLicense()
        {
            //call DataAccess Layer 

            this._DetainID = clsDetainedLicenseData.AddNewDetainedLicense( 
                this._LicenseID,this._DetainDate,this._FineFees,this._CreatedByUserID);
            
            return (this._DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            //call DataAccess Layer 

            return clsDetainedLicenseData.UpdateDetainedLicense(
                this._DetainID,this._LicenseID,this._DetainDate,this._FineFees,this._CreatedByUserID);
        }

        public static clsDetainedLicenses Find_ByLicenseID(int Detain_ID)
        {
            int _License_ID = -1; DateTime DetainDate = DateTime.Now;
            float Fine_Fees= 0; int _CreatedByUser_ID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUser_ID = -1; int Release_Application_ID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoBy_ID(Detain_ID,
            ref _License_ID, ref DetainDate,
            ref Fine_Fees, ref _CreatedByUser_ID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUser_ID, ref Release_Application_ID))

                return new clsDetainedLicenses(Detain_ID,
                     _License_ID,  DetainDate,
                     Fine_Fees,  _CreatedByUser_ID,
                     IsReleased,  ReleaseDate,
                     ReleasedByUser_ID,  Release_Application_ID);
            else
                return null;

        }

        public static DataTable GetAll()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();

        }

        public static clsDetainedLicenses FindBy_License_ID(int _License_ID)
        {
            int Detain_ID = -1; DateTime DetainDate = DateTime.Now;
            float Fine_Fees = 0; int _CreatedByUser_ID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUser_ID = -1; int Release_Application_ID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoBy_License_ID(_License_ID,
            ref Detain_ID, ref DetainDate,
            ref Fine_Fees, ref _CreatedByUser_ID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUser_ID, ref Release_Application_ID))

                return new clsDetainedLicenses(Detain_ID,
                     _License_ID, DetainDate,
                     Fine_Fees, _CreatedByUser_ID,
                     IsReleased, ReleaseDate,
                     ReleasedByUser_ID, Release_Application_ID);
            else
                return null;

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }

            return false;
        }

        public static bool IsLicenseDetained(int _License_ID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(_License_ID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUser_ID, int Release_Application_ID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this._DetainID,
                   ReleasedByUser_ID, Release_Application_ID);
        }
    }
}
