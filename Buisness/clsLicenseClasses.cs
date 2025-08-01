using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsLicenseClasses
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int _LicenseClassID { set; get; }
        public string _ClassName { set; get; }
        public string _ClassDescription { set; get; }
        public byte _MinimumAllowedAge { set; get; }
        public byte _DefaultValidityLength { set; get; }
        public float _Class_Fees { set; get; }

        public clsLicenseClasses()

        {
            this._LicenseClassID = -1;
            this._ClassName = "";
            this._ClassDescription = "";
            this._MinimumAllowedAge = 18;
            this._DefaultValidityLength = 10;
            this._Class_Fees = 0;
          
            Mode = enMode.AddNew;

        }

        public clsLicenseClasses(int _LicenseClassID_ID, string _ClassName,
            string _ClassDescription,
            byte _MinimumAllowedAge, byte _DefaultVal_IDityLength, float _Class_Fees)

        {
            this._LicenseClassID = _LicenseClassID_ID;
            this._ClassName = _ClassName;
            this._ClassDescription = _ClassDescription;
            this._MinimumAllowedAge = _MinimumAllowedAge;
            this._DefaultValidityLength = _DefaultVal_IDityLength;
            this._Class_Fees = _Class_Fees;
            Mode = enMode.Update;
        }

        private bool _AddNew_LicenseClassID()
        {
            //call DataAccess Layer 

            this._LicenseClassID = clsLicenseClassData.AddNew_LicenseClassID(this._ClassName,this._ClassDescription,
                this._MinimumAllowedAge,this._DefaultValidityLength,this._Class_Fees);
              

            return (this._LicenseClassID != -1);
        }

        private bool _Update_LicenseClassID()
        {
            //call DataAccess Layer 

            return clsLicenseClassData.Update_LicenseClassID(this._LicenseClassID, this._ClassName, this._ClassDescription,
                this._MinimumAllowedAge, this._DefaultValidityLength, this._Class_Fees);
        }

        public static clsLicenseClasses Find_ByID(int _LicenseClassID_ID)
        {
            string _ClassName = ""; string _ClassDescription = "";
            byte _MinimumAllowedAge = 18; byte _DefaultVal_IDityLength = 10; float _Class_Fees = 0;

            if (clsLicenseClassData.Get_LicenseClassIDInfoBy_ID(_LicenseClassID_ID, ref _ClassName, ref _ClassDescription,
                    ref  _MinimumAllowedAge,  ref _DefaultVal_IDityLength, ref _Class_Fees))

                return new clsLicenseClasses(_LicenseClassID_ID,_ClassName ,_ClassDescription,
                    _MinimumAllowedAge,_DefaultVal_IDityLength,_Class_Fees);
            else
                return null;

        }

        public static clsLicenseClasses Find(string _ClassName)
        {
            int _LicenseClassID_ID = -1; string _ClassDescription = "";
            byte _MinimumAllowedAge = 18; byte _DefaultVal_IDityLength = 10; float _Class_Fees = 0;

            if (clsLicenseClassData.Get_LicenseClassIDInfoBy_ClassName( _ClassName, ref  _LicenseClassID_ID, ref _ClassDescription,
                    ref _MinimumAllowedAge, ref _DefaultVal_IDityLength, ref _Class_Fees))

                return new clsLicenseClasses(_LicenseClassID_ID, _ClassName, _ClassDescription,
                    _MinimumAllowedAge, _DefaultVal_IDityLength, _Class_Fees);
            else
                return null;

        }

        public static DataTable GetAll()
        {
            return clsLicenseClassData.GetAll_LicenseClassIDes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew_LicenseClassID())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _Update_LicenseClassID();

            }

            return false;
        }

    }
}
