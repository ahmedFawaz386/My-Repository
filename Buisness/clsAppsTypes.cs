using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsAppsTypes
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int _ApplicationTypeID { set; get; }
        public string _ApplicationTypeTitle { set; get; }
        public float _ApplicationFees { set; get; }

        public clsAppsTypes()

        {
            this._ApplicationTypeID = -1;
            this._ApplicationTypeTitle = "";
            this._ApplicationFees = 0;
            Mode = enMode.AddNew;

        }

        public clsAppsTypes(int _ID, string ApplicationTypeTitel,float ApplicationType_Fees)

        {
            this._ApplicationTypeID =  _ID;
            this._ApplicationTypeTitle = ApplicationTypeTitel;
            this._ApplicationFees = ApplicationType_Fees;
            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            //call DataAccess Layer 

            this._ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType( this._ApplicationTypeTitle, this._ApplicationFees);
              

            return (this._ApplicationTypeID != -1);
        }

        private bool _UpdateApplicationType()
        {
            //call DataAccess Layer 

            return clsApplicationTypeData.UpdateApplicationType(this._ApplicationTypeID,this._ApplicationTypeTitle,this._ApplicationFees);
        }

        public static clsAppsTypes Find_ByID(int _ID)
        {
            string _Title = "";float _Fees=0;

            if (clsApplicationTypeData.GetApplicationTypeInfoBy_ID((int) _ID, ref _Title, ref _Fees))

                return new clsAppsTypes(_ID, _Title,_Fees);
            else
                return null;

        }

        public static DataTable GetAll()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplicationType();

            }

            return false;
        }

    }
}
