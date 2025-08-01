using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsTestTypes
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public  enum enTestType { VisionTest = 1, WrittenTest = 2,StreetTest=3 };

        public clsTestTypes.enTestType _TestTypeID { set; get; }
        public string _TestTypeTitle { set; get; }
        public string _TestTypeDescription { set; get; } 
        public float _TestTypeFees { set; get; }
        public clsTestTypes()

        {
            this._TestTypeID = clsTestTypes.enTestType.VisionTest;
            this._TestTypeTitle = "";
            this._TestTypeDescription = "";
            this._TestTypeFees = 0;
            Mode = enMode.AddNew;

        }

        public clsTestTypes(clsTestTypes.enTestType _ID, string TestTypeTitel,string Description,float TestType_Fees)

        {
            this._TestTypeID = _ID;
            this._TestTypeTitle = TestTypeTitel;
            this._TestTypeDescription = Description;

            this._TestTypeFees = TestType_Fees;
            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            //call DataAccess Layer 

            this._TestTypeID =(clsTestTypes.enTestType) clsTestTypeData.AddNewTestType(this._TestTypeTitle,this._TestTypeDescription, this._TestTypeFees);
              
            return (this._TestTypeTitle !="");
        }

        private bool _UpdateTestType()
        {
            //call DataAccess Layer 

            return clsTestTypeData.UpdateTestType((int) this._TestTypeID,this._TestTypeTitle,this._TestTypeDescription,this._TestTypeFees);
        }

        public static clsTestTypes Find(clsTestTypes.enTestType TestType_ID)
        {
            string _Title = "", Description=""; float _Fees=0;

            if (clsTestTypeData.GetTestTypeInfoBy_ID((int) TestType_ID, ref _Title,ref Description, ref _Fees))

                return new clsTestTypes(TestType_ID, _Title, Description,_Fees);
            else
                return null;

        }

        public static DataTable GetAll()
        {
            return clsTestTypeData.GetAllTestTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestType();

            }

            return false;
        }

    }
}
