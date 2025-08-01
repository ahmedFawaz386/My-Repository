using Buisness;
using DVLD_DataAccess;
using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using static Buisness.clsApplications;

namespace DVLD_Buisness
{
    public class clsInternationalLicenses : clsApplications
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsDrivers DriverInfo;
        public int _InternationalLicenseID { set; get; }
        public int _DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime _IssueDate { set; get; }
        public DateTime _ExpirationDate { set; get; }
        public bool _IsActive { set; get; }


        public clsInternationalLicenses()

        {
            //here we set the applicaiton type to New International License.
            this._ApplicationTypeID = (int)clsApplications.enApplicationType.NewInternationalLicense;

            this._InternationalLicenseID = -1;
            this._DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this._IssueDate = DateTime.Now;
            this._ExpirationDate = DateTime.Now;

            this._IsActive = true;


            Mode = enMode.AddNew;

        }

        public clsInternationalLicenses(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID,
             int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive)

        {
            //this is for the base clase
            base._ApplicationID = ApplicationID;
            base._ApplicantPersonID = ApplicantPersonID;
            base._ApplicationDate = ApplicationDate;
            base._ApplicationTypeID = (int)clsApplications.enApplicationType.NewInternationalLicense;
            base._ApplicationStatus = ApplicationStatus;
            base._LastStatusDate = LastStatusDate;
            base._PaidFees = PaidFees;
            base._CreatedByUserID = CreatedByUserID;

            this._InternationalLicenseID = InternationalLicenseID;
            this._ApplicationID = ApplicationID;
            this._DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this._IssueDate = IssueDate;
            this._ExpirationDate = ExpirationDate;
            this._IsActive = IsActive;
            this._CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDrivers.Find_ByID(this._DriverID);

            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            

            this._InternationalLicenseID =
                clsInternationalLicenseData.AddNewInternationalLicense(this._ApplicationID, this._DriverID, this.IssuedUsingLocalLicenseID,
               this._IssueDate, this._ExpirationDate,
               this._IsActive, this._CreatedByUserID);


            return (this._InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            

            return clsInternationalLicenseData.UpdateInternationalLicense(
                this._InternationalLicenseID, this._ApplicationID, this._DriverID, this.IssuedUsingLocalLicenseID,
               this._IssueDate, this._ExpirationDate,
               this._IsActive, this._CreatedByUserID);
        }
        public static bool IsTherePreviousLicense(int driver_id)
        {
            
            return clsInternationalLicenseData.IsTherePreviousLicense(driver_id);
        }

        public static clsInternationalLicenses Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1; int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            bool IsActive = true; int CreatedByUserID = 1;

            if (clsInternationalLicenseData.GetInternationalLicenseInfoBy_ID(InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssuedUsingLocalLicenseID,
            ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                
                clsApplications Application = clsApplications.FindBaseApplication(ApplicationID);


                return new clsInternationalLicenses(Application._ApplicationID,
                    Application._ApplicantPersonID,
                                     Application._ApplicationDate,
                                    (enApplicationStatus)Application._ApplicationStatus, Application._LastStatusDate,
                                     Application._PaidFees, Application._CreatedByUserID,
                                     InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID,
                                         IssueDate, ExpirationDate, IsActive);

            }

            else
                return null;

        }

        public static DataTable GetAll()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();

        }

        public bool Save()
        {

            
            base.Mode = (clsApplications.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateInternationalLicense();

            }

            return false;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {

            return clsInternationalLicenseData.GetActiveInternational_License_IDBy_Driver_ID(DriverID);

        }

        public static DataTable GetAll(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
    }
}
