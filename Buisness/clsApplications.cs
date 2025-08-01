using System;
using System.Data;
using DVLD_DataAccess;


namespace Buisness
{
    public   class clsApplications
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enApplicationType { NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense=3,
            ReplaceDamagedDrivingLicense=4, ReleaseDetainedDrivingLicsense=5, NewInternationalLicense=6,RetakeTest=7
        };

        public enMode Mode = enMode.AddNew;
        public enum enApplicationStatus { New=1, Cancelled=2,Completed=3};

        public int _ApplicationID { set; get; }
        public int _ApplicantPersonID { set; get; }
        public string ApplicantFullName
        {
            get
            {
                return clsPeople.Find_ByID(_ApplicantPersonID).FullName;
            }
        }
        public DateTime _ApplicationDate { set; get; }
        public int _ApplicationTypeID { set; get; }
        public clsAppsTypes ApplicationTypeInfo;
        public enApplicationStatus _ApplicationStatus { set; get; } 
        public string _StatusText   
        {
            get { 
            
                switch (_ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";  
                }
            }   
           
        }
        public DateTime _LastStatusDate { set; get; }
        public float _PaidFees { set; get; }
        public int _CreatedByUserID { set; get; }
        public clsUsers _CreatedByUserInfo;

        public clsApplications()

        {
            this._ApplicationID = -1;
            this._ApplicantPersonID = -1;
            this._ApplicationDate = DateTime.Now;
            this._ApplicationTypeID = -1;
            this._ApplicationStatus = enApplicationStatus.New;
            this._LastStatusDate = DateTime.Now;
            this._PaidFees = 0;
            this._CreatedByUserID = -1;
           
            Mode = enMode.AddNew;

        }

        private clsApplications(int _Application_ID, int _Applicant_Person_ID, 
            DateTime _ApplicationDate, int _ApplicationType_ID,
             enApplicationStatus _ApplicationStatus, DateTime _LastStatusDate,
             float _Pa_ID_Fees, int _CreatedByUser_ID)

        {
            this._ApplicationID = _Application_ID;
            this._ApplicantPersonID = _Applicant_Person_ID;
            this._ApplicationDate = _ApplicationDate;
            this._ApplicationTypeID = _ApplicationType_ID;
            this.ApplicationTypeInfo = clsAppsTypes.Find_ByID(_ApplicationType_ID);
            this._ApplicationStatus = _ApplicationStatus;
            this._LastStatusDate = _LastStatusDate;
            this._PaidFees = _Pa_ID_Fees;
            this._CreatedByUserID = _CreatedByUser_ID;
            this._CreatedByUserInfo = clsUsers.Find_ByID(_CreatedByUser_ID);
            Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {
            //call DataAccess Layer 

            this._ApplicationID = clsApplicationData.AddNewApplication(
                this._ApplicantPersonID, this._ApplicationDate,
                this._ApplicationTypeID, (byte) this._ApplicationStatus,
                this._LastStatusDate, this._PaidFees, this._CreatedByUserID);

            return (this._ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            //call DataAccess Layer 

            return clsApplicationData.UpdateApplication(this._ApplicationID, this._ApplicantPersonID, this._ApplicationDate,
                this._ApplicationTypeID, (byte) this._ApplicationStatus,
                this._LastStatusDate, this._PaidFees, this._CreatedByUserID);
           
        }

        public  static clsApplications FindBaseApplication(int _Application_ID)
        {
            int _Applicant_Person_ID=-1;
            DateTime _ApplicationDate=DateTime.Now ;  int _ApplicationType_ID=-1;
            byte _ApplicationStatus =1; DateTime _LastStatusDate= DateTime.Now;
            float _Pa_ID_Fees = 0  ;  int _CreatedByUser_ID = -1;

            bool IsFound = clsApplicationData.GetApplicationInfoBy_ID 
                                (
                                    _Application_ID, ref  _Applicant_Person_ID, 
                                    ref  _ApplicationDate, ref  _ApplicationType_ID,
                                    ref   _ApplicationStatus, ref  _LastStatusDate,
                                    ref  _Pa_ID_Fees, ref  _CreatedByUser_ID
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsApplications(_Application_ID,  _Applicant_Person_ID,
                                     _ApplicationDate,  _ApplicationType_ID,
                                    (enApplicationStatus) _ApplicationStatus,  _LastStatusDate,
                                     _Pa_ID_Fees,  _CreatedByUser_ID);
            else
                return null;
        }

        public bool Cancel()

        {
            return clsApplicationData.UpdateStatus (_ApplicationID,2);
        }

        public bool SetComplete()

        {
            return clsApplicationData.UpdateStatus(_ApplicationID, 3);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplication();

            }

            return false;
        }

        public  bool Delete()
        {
            return clsApplicationData.DeleteApplication(this._ApplicationID); 
        }

        public static bool IsApplicationExist(int _Application_ID)
        {
           return clsApplicationData.IsApplicationExist(_Application_ID);
        }

        public static bool DoesPersonHaveActiveApplication(int _Person_ID,int _ApplicationType_ID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(_Person_ID,_ApplicationType_ID);
        }

        public  bool DoesPersonHaveActiveApplication( int _ApplicationType_ID)
        {
            return DoesPersonHaveActiveApplication(this._ApplicantPersonID, _ApplicationType_ID);
        }

        public static int GetActive_Application_ID(int _Person_ID, clsApplications.enApplicationType  _ApplicationType_ID)
        {
            return clsApplicationData.GetActive_Application_ID(_Person_ID,(int) _ApplicationType_ID);
        }

        public static int GetActive_Application_IDFor_LicenseClassID(int _Person_ID, clsApplications.enApplicationType _ApplicationType_ID,int _LicenseClassID_ID)
        {
            return clsApplicationData.GetActive_Application_IDFor_LicenseClassID(_Person_ID, (int)_ApplicationType_ID,_LicenseClassID_ID );
        }
       
        public  int GetActive_Application_ID(clsApplications.enApplicationType _ApplicationType_ID)
        {
            return GetActive_Application_ID(this._ApplicantPersonID, _ApplicationType_ID);
        }

    }
}
