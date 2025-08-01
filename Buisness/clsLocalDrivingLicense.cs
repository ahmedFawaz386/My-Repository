using System;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Xml.Linq;
using DVLD_DataAccess;
//using static System.Net.Mime.MediaTypeNames;
//using static Buisness.clsTestType;


namespace Buisness
{
    public   class clsLocalDrivingLicense : clsApplications

    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int _LocalDrivingLicenseApplicationID { set; get; }
        public int _LicenseClassID { set; get; }
        public clsLicenseClasses _LicenseClassIDInfo;
        public string PersonFullName   
        {
            get { 
                return clsPeople.Find_ByID(_ApplicantPersonID).FullName; 
            }   
            
        }

        public clsLocalDrivingLicense()

        {
            this._LocalDrivingLicenseApplicationID = -1;
            this._LicenseClassID = -1;
            
           
            Mode = enMode.AddNew;

        }

        private clsLocalDrivingLicense(int _LocalDrivingLicense_Application_ID, int _Application_ID, int _Applicant_Person_ID, 
            DateTime _ApplicationDate, int _ApplicationType_ID,
             enApplicationStatus _ApplicationStatus, DateTime _LastStatusDate,
             float _Pa_ID_Fees, int _CreatedByUser_ID, int _LicenseClassID_ID)

        {
            this._LocalDrivingLicenseApplicationID= _LocalDrivingLicense_Application_ID; ;
            this._ApplicationID = _Application_ID;
            this._ApplicantPersonID = _Applicant_Person_ID;
            this._ApplicationDate = _ApplicationDate;
            this._ApplicationTypeID = (int) _ApplicationType_ID;
            this._ApplicationStatus = _ApplicationStatus;
            this._LastStatusDate = _LastStatusDate;
            this._PaidFees = _Pa_ID_Fees;
            this._CreatedByUserID = _CreatedByUser_ID;
            this._LicenseClassID = _LicenseClassID_ID;
            this._LicenseClassIDInfo = clsLicenseClasses.Find_ByID(_LicenseClassID_ID);
            Mode = enMode.Update;
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 
            
            this._LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication
                (
                this._ApplicationID, this._LicenseClassID);

            return (this._LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication
                (
                this._LocalDrivingLicenseApplicationID ,this._ApplicationID, this._LicenseClassID);
           
        }

        public static clsLocalDrivingLicense  Find_ByID(int _LocalDrivingLicense_Application_ID)
        {
            // 
            int _Application_ID=-1, _LicenseClassID_ID=-1;

            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoBy_ID
                (_LocalDrivingLicense_Application_ID, ref _Application_ID, ref _LicenseClassID_ID);


            if (IsFound)
            { 
               //now we find the base application
                clsApplications Application = clsApplications.FindBaseApplication(_Application_ID);

                //we return new object of that person with the right data
                return new clsLocalDrivingLicense(
                    _LocalDrivingLicense_Application_ID, Application._ApplicationID, 
                    Application._ApplicantPersonID,
                                     Application._ApplicationDate, Application._ApplicationTypeID,
                                    (enApplicationStatus)Application._ApplicationStatus, Application._LastStatusDate,
                                     Application._PaidFees, Application._CreatedByUserID,_LicenseClassID_ID);
            }
            else
                return null;
          

        }

        public static clsLocalDrivingLicense FindBy_Application_ID(int _Application_ID)
        {
            // 
            int _LocalDrivingLicense_Application_ID = -1, _LicenseClassID_ID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoBy_Application_ID 
                (_Application_ID, ref _LocalDrivingLicense_Application_ID, ref _LicenseClassID_ID);


            if (IsFound)
            {
                //now we find the base application
                clsApplications Application = clsApplications.FindBaseApplication(_Application_ID);

                //we return new object of that person with the right data
                return new clsLocalDrivingLicense(
                    _LocalDrivingLicense_Application_ID, Application._ApplicationID,
                    Application._ApplicantPersonID,
                                     Application._ApplicationDate, Application._ApplicationTypeID,
                                    (enApplicationStatus)Application._ApplicationStatus, Application._LastStatusDate,
                                     Application._PaidFees, Application._CreatedByUserID, _LicenseClassID_ID);
            }
            else
                return null;


        }

        public bool  Save()
        {
          
           
            base.Mode = (clsApplications.enMode) Mode;
          if (!base.Save()) 
                return false ;


          
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLocalDrivingLicenseApplication();

            }

            return false;
        }

        public static DataTable GetAll()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public  bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;
            
            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this._LocalDrivingLicenseApplicationID);
           
            if (!IsLocalDrivingApplicationDeleted)
                return false;
            
            IsBaseApplicationDeleted = base.Delete();
            return IsBaseApplicationDeleted;

        }

        public bool DoesPassTestType(clsTestTypes.enTestType  TestType_ID)

        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType( this._LocalDrivingLicenseApplicationID,(int) TestType_ID);
        }

        public bool DoesPassPreviousTest(clsTestTypes.enTestType CurrentTestType)
        {

            switch (CurrentTestType)
            {
                case clsTestTypes.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    return true;

                case clsTestTypes.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.

                    return this.DoesPassTestType(clsTestTypes.enTestType.VisionTest);
                   

                case clsTestTypes.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    return this.DoesPassTestType(clsTestTypes.enTestType.WrittenTest);

                default: 
                    return false;
            }
        }

        public static bool DoesPassTestType(int _LocalDrivingLicense_Application_ID, clsTestTypes.enTestType TestType_ID)

        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(_LocalDrivingLicense_Application_ID,(int) TestType_ID);
        }

        public  bool DoesAttendTestType( clsTestTypes.enTestType TestType_ID)

        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this._LocalDrivingLicenseApplicationID, (int)TestType_ID);
        }

        public  byte TotalTrialsPerTest( clsTestTypes.enTestType TestType_ID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this._LocalDrivingLicenseApplicationID, (int)TestType_ID);
        }

        public static byte TotalTrialsPerTest(int _LocalDrivingLicense_Application_ID, clsTestTypes.enTestType TestType_ID)

        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(_LocalDrivingLicense_Application_ID, (int)TestType_ID);
        }

        public static bool AttendedTest(int _LocalDrivingLicense_Application_ID, clsTestTypes.enTestType TestType_ID)

        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(_LocalDrivingLicense_Application_ID, (int)TestType_ID) >0;
        }

        public  bool AttendedTest( clsTestTypes.enTestType TestType_ID)

        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this._LocalDrivingLicenseApplicationID, (int)TestType_ID) > 0;
        }

        public static bool IsThereAnActiveScheduledTest(int _LocalDrivingLicense_Application_ID, clsTestTypes.enTestType TestType_ID)

        {
            
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(_LocalDrivingLicense_Application_ID, (int)TestType_ID);
        }

        public  bool IsThereAnActiveScheduledTest( clsTestTypes.enTestType TestType_ID)

        {

            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this._LocalDrivingLicenseApplicationID, (int)TestType_ID);
        }
        public static bool Check(int Person_id,int Class_id)
        {
            return clsLocalDrivingLicenseApplicationData.Check(Person_id,Class_id);
        }

        public clsTests GetLastTestPerTestType(clsTestTypes.enTestType TestType_ID)
        {
            return clsTests.FindLastTestPerPersonAnd_LicenseClassID(this._ApplicantPersonID, this._LicenseClassID, TestType_ID);
        }

        public byte GetPassedTestCount()
        {
            return clsTests.GetPassedTests(this._LocalDrivingLicenseApplicationID);
        }

        public static byte GetPassedTestCount(int _LocalDrivingLicense_Application_ID)
        {
            return clsTests.GetPassedTests(_LocalDrivingLicense_Application_ID);
        }

        public  bool PassedAllTests()
        {
             return clsTests.PassedAllTests(this._LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int _LocalDrivingLicense_Application_ID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return clsTests.PassedAllTests (_LocalDrivingLicense_Application_ID) ;
        }
        
        public int IssueLicenseForTheFirtTime(string _Notes, int _CreatedByUser_ID)
        {
            int _Driver_ID = -1;

            clsDrivers Driver =clsDrivers.Find_ByPersonID(this._ApplicantPersonID);

            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDrivers();
               
                Driver._PersonID= this._ApplicantPersonID;
                Driver._CreatedByUserID= _CreatedByUser_ID;
                if (Driver.Save())
                {
                    _Driver_ID= Driver._DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                _Driver_ID= Driver._DriverID;
            }
            //now we diver is there, so we add new licesnse
            
            clsLicenses License= new clsLicenses();
            License._ApplicationID = this._ApplicationID;
            License._DriverID= _Driver_ID;
            License._LicenseClass = this._LicenseClassID;
            License._IssueDate=DateTime.Now;
            License._ExpirationDate = DateTime.Now.AddYears(this._LicenseClassIDInfo._DefaultValidityLength);
            License._Notes = _Notes;
            License._PaidFees = this._LicenseClassIDInfo._Class_Fees;
            License._IsActive= true;
            License._IssueReason = clsLicenses.en_IssueReason.FirstTime;
            License._CreatedByUserID= _CreatedByUser_ID;

            if (License.Save())
            {
                //now we should set the application status to complete.
                this.SetComplete();

                return License._LicenseID;
            }
               
            else
                return -1;
        }

        public bool IsLicenseIssued()
        {
            return (GetActive_License_ID() !=-1);
        }

        public int GetActive_License_ID()
        {//this will get the license _ApplicationTypeID that belongs to this application
            return  clsLicenses.GetActive_License_IDBy_Person_ID(this._ApplicantPersonID, this._LicenseClassID);
        }
    }
}
