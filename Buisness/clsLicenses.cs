using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using DVLD_DataAccess;
using static System.Net.Mime.MediaTypeNames;

namespace Buisness
{
    public class clsLicenses
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum en_IssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public clsDrivers DriverInfo;
        public int _LicenseID { set; get; }
        public int _ApplicationID { set; get; }
        public int _DriverID { set; get; }
        public int _LicenseClass { set; get; }
        public clsLicenseClasses _LicenseClassIDIfo;
        public DateTime _IssueDate { set; get; }
        public DateTime _ExpirationDate { set; get; }
        public string _Notes { set; get; }
        public float _PaidFees { set; get; }
        public bool _IsActive { set; get; }
        public en_IssueReason _IssueReason { set; get; }
        public string _IssueReasonText
        { get 
            { 
                return Get_IssueReasonText(this._IssueReason); 
            } 
        }
        public clsDetainedLicenses DetainedInfo { set; get; }
        public int _CreatedByUserID { set; get; }
        public bool _IsDetained
        {
            get { return clsDetainedLicenses.IsLicenseDetained(this._LicenseID); }
        }

        public clsLicenses()

        {
            this._LicenseID = -1;
            this._ApplicationID= -1;
            this._DriverID = -1;
            this._LicenseClass = -1;
            this._IssueDate = DateTime.Now;
            this._ExpirationDate = DateTime.Now;
            this._Notes = "";
            this._PaidFees = 0;
            this._IsActive = true;
            this._IssueReason = en_IssueReason.FirstTime;
            this._CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        public clsLicenses(int _License_ID,int _Application_ID, int _Driver_ID, int _LicenseClassID,
            DateTime _IssueDate, DateTime _ExpirationDate, string _Notes,
            float _Pa_ID_Fees, bool _IsActive,en_IssueReason _IssueReason, int _CreatedByUser_ID)

        {
            this._LicenseID = _License_ID;
            this._ApplicationID = _Application_ID;
            this._DriverID = _Driver_ID;
            this._LicenseClass = _LicenseClassID;
            this._IssueDate = _IssueDate;
            this._ExpirationDate = _ExpirationDate;
            this._Notes = _Notes;
            this._PaidFees = _Pa_ID_Fees;
            this._IsActive = _IsActive;
            this._IssueReason = _IssueReason;
            this._CreatedByUserID = _CreatedByUser_ID;

            this.DriverInfo = clsDrivers.Find_ByID(this._DriverID);
            this._LicenseClassIDIfo = clsLicenseClasses.Find_ByID(this._LicenseClass);
            this.DetainedInfo=clsDetainedLicenses.FindBy_License_ID(this._LicenseID);   

            Mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            //call DataAccess Layer 

            this._LicenseID = clsLicenseData.AddNewLicense(this._ApplicationID, this._DriverID, this._LicenseClass,
               this._IssueDate, this._ExpirationDate, this._Notes, this._PaidFees,
               this._IsActive,(byte) this._IssueReason, this._CreatedByUserID);


            return (this._LicenseID != -1);
        }

        private bool _UpdateLicense()
        {
            //call DataAccess Layer 

            return clsLicenseData.UpdateLicense(this._ApplicationID, this._LicenseID, this._DriverID, this._LicenseClass,
               this._IssueDate, this._ExpirationDate, this._Notes, this._PaidFees,
               this._IsActive,(byte) this._IssueReason, this._CreatedByUserID);
        }

        public static clsLicenses Find_ByID(int _License_ID)
        {
            int _Application_ID = -1; int _Driver_ID = -1; int _LicenseClassID = -1;
            DateTime _IssueDate = DateTime.Now; DateTime _ExpirationDate = DateTime.Now;
            string _Notes = "";
            float _Pa_ID_Fees = 0; bool _IsActive = true; int _CreatedByUser_ID = 1;
            byte _IssueReason = 1;
            if (clsLicenseData.GetLicenseInfoBy_ID(_License_ID,ref _Application_ID, ref _Driver_ID, ref _LicenseClassID,
            ref _IssueDate, ref _ExpirationDate, ref _Notes,
            ref _Pa_ID_Fees, ref _IsActive,ref _IssueReason, ref _CreatedByUser_ID))

                return new clsLicenses(_License_ID,_Application_ID, _Driver_ID, _LicenseClassID,
                                     _IssueDate, _ExpirationDate, _Notes,
                                     _Pa_ID_Fees, _IsActive,(en_IssueReason) _IssueReason, _CreatedByUser_ID);
            else
                return null;

        }
        public static clsLicenses Find_ByAppID(int _Application_ID)
        {

            int _License_ID = -1; int _Driver_ID = -1; int _LicenseClassID = -1;
            DateTime _IssueDate = DateTime.Now; DateTime _ExpirationDate = DateTime.Now;
            string _Notes = "";
            float _Pa_ID_Fees = 0; bool _IsActive = true; int _CreatedByUser_ID = 1;
            byte _IssueReason = 1;
            if (clsLicenseData.Find_ByAppID(ref _License_ID,_Application_ID, ref _Driver_ID, ref _LicenseClassID,
            ref _IssueDate, ref _ExpirationDate, ref _Notes,
            ref _Pa_ID_Fees, ref _IsActive,ref _IssueReason, ref _CreatedByUser_ID))

                return new clsLicenses(_License_ID,_Application_ID, _Driver_ID, _LicenseClassID,
                                     _IssueDate, _ExpirationDate, _Notes,
                                     _Pa_ID_Fees, _IsActive,(en_IssueReason) _IssueReason, _CreatedByUser_ID);
            else
                return null;

        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }

            return false;
        }

        public static bool IsLicenseExistBy_Person_ID(int _Person_ID, int _LicenseClassID_ID)
        {
            return (GetActive_License_IDBy_Person_ID(_Person_ID, _LicenseClassID_ID) != -1);
        }

        public static int GetActive_License_IDBy_Person_ID(int _Person_ID, int _LicenseClassID_ID)
        {

            return clsLicenseData.GetActive_License_IDBy_Person_ID(_Person_ID, _LicenseClassID_ID);

        }

        public static DataTable GetAll(int _Driver_ID)
        {
            return clsLicenseData.GetDriverLicenses(_Driver_ID);
        }

        public Boolean IsLicenseExpired()
        {

            return ( this._ExpirationDate < DateTime.Now );

        }

        public bool DeactivateCurrentLicense()
        {
            return (clsLicenseData.DeactivateLicense(this._LicenseID));
        }

        public static string Get_IssueReasonText(en_IssueReason _IssueReason)
        {

            switch (_IssueReason)
            {
                case en_IssueReason.FirstTime:
                    return "First Time";
                case en_IssueReason.Renew:
                    return "Renew";
                case en_IssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case en_IssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public int Detain(float Fine_Fees,int _CreatedByUser_ID)
        {
            clsDetainedLicenses detainedLicense = new clsDetainedLicenses();
            detainedLicense._LicenseID = this._LicenseID;
            detainedLicense._DetainDate = DateTime.Now;
            detainedLicense._FineFees = Convert.ToSingle(Fine_Fees);
            detainedLicense._CreatedByUserID = _CreatedByUser_ID;

            if (!detainedLicense.Save())
            {
               
                return -1;
            }

            return detainedLicense._DetainID;

        }

        public bool ReleaseDetainedLicense(int ReleasedByUser_ID,ref int _Application_ID)
        {

            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application._ApplicantPersonID = this.DriverInfo._PersonID;
            Application._ApplicationDate = DateTime.Now;
            Application._ApplicationTypeID = (int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application._ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application._LastStatusDate = DateTime.Now;
            Application._PaidFees = clsAppsTypes.Find_ByID((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicsense)._ApplicationFees;
            Application._CreatedByUserID = ReleasedByUser_ID;

            if (!Application.Save())
            {
                _Application_ID = -1;
                return false;
            }

            _Application_ID = Application._ApplicationID;


            return this.DetainedInfo.ReleaseDetainedLicense(ReleasedByUser_ID, Application._ApplicationID);

        }

        public clsLicenses RenewLicense(string _Notes, int _CreatedByUser_ID)
        {

            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application._ApplicantPersonID = this.DriverInfo._PersonID;
            Application._ApplicationDate = DateTime.Now;
            Application._ApplicationTypeID = (int)clsApplications.enApplicationType.RenewDrivingLicense;
            Application._ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application._LastStatusDate = DateTime.Now;
            Application._PaidFees = clsAppsTypes.Find_ByID((int)clsApplications.enApplicationType.RenewDrivingLicense)._ApplicationFees;
            Application._CreatedByUserID = _CreatedByUser_ID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicenses NewLicense = new clsLicenses();

            NewLicense._ApplicationID = Application._ApplicationID;
            NewLicense._DriverID = this._DriverID;
            NewLicense._LicenseClass = this._LicenseClass;
            NewLicense._IssueDate = DateTime.Now;

            int _DefaultVal_IDityLength = this._LicenseClassIDIfo._DefaultValidityLength;

            NewLicense._ExpirationDate = DateTime.Now.AddYears(_DefaultVal_IDityLength);
            NewLicense._Notes = _Notes;
            NewLicense._PaidFees = this._LicenseClassIDIfo._Class_Fees;
            NewLicense._IsActive = true;
            NewLicense._IssueReason = clsLicenses.en_IssueReason.Renew;
            NewLicense._CreatedByUserID = _CreatedByUser_ID;


            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return NewLicense;    
        }

        public clsLicenses Replace(en_IssueReason _IssueReason, int _CreatedByUser_ID)
        {


            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application._ApplicantPersonID = this.DriverInfo._PersonID;
            Application._ApplicationDate = DateTime.Now;

            Application._ApplicationTypeID = (_IssueReason == en_IssueReason.DamagedReplacement) ? 
                (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense : 
                (int)clsApplications.enApplicationType.ReplaceLostDrivingLicense; 

            Application._ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application._LastStatusDate = DateTime.Now;
            Application._PaidFees = clsAppsTypes.Find_ByID(Application._ApplicationTypeID)._ApplicationFees;
            Application._CreatedByUserID = _CreatedByUser_ID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicenses NewLicense = new clsLicenses();

            NewLicense._ApplicationID = Application._ApplicationID;
            NewLicense._DriverID = this._DriverID;
            NewLicense._LicenseClass = this._LicenseClass;
            NewLicense._IssueDate = DateTime.Now;
            NewLicense._ExpirationDate = this._ExpirationDate;
            NewLicense._Notes = this._Notes;
            NewLicense._PaidFees = 0;// no _ApplicationFees for the license because it's a replacement.
            NewLicense._IsActive = true;
            NewLicense._IssueReason = _IssueReason;
            NewLicense._CreatedByUserID = _CreatedByUser_ID;



            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return NewLicense;
        }

    }
}
