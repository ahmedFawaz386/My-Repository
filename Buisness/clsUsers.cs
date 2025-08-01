using System;
using System.Data;
using System.Runtime.InteropServices;
using DVLD_DataAccess;

namespace Buisness
{
    public  class clsUsers
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int _UserID { set; get; }
        public int _PersonID { set; get; }
        public clsPeople PersonInfo;
        public string _UserName { set; get; }
        public string _Password { set; get; }
        public bool _IsActive { set; get; }
     
        public clsUsers()

        {     
            this._UserID = -1;
            this._UserName = "";
            this._Password = "";
            this._IsActive = true;
            Mode = enMode.AddNew;
        }

        private clsUsers(int User_ID, int _Person_ID, string Username,string Password,
            bool _IsActive)

        {
            this._UserID = User_ID; 
            this._PersonID = _Person_ID;
            this.PersonInfo = clsPeople.Find_ByID(_Person_ID);
            this._UserName = Username;
            this._Password = Password;
            this._IsActive = _IsActive;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            //call DataAccess Layer 

            this._UserID = clsUserData.AddNewUser(this._PersonID,this._UserName,
                this._Password,this._IsActive);

            return (this._UserID != -1);
        }
        private bool _UpdateUser()
        {
            //call DataAccess Layer 

            return clsUserData.UpdateUser(this._UserID,this._PersonID,this._UserName,
                this._Password,this._IsActive);
        }
        public static clsUsers Find_ByID(int User_ID)
        {
            int _Person_ID = -1;
            string UserName = "", Password = "";
            bool _IsActive = false;

            bool IsFound = clsUserData.GetUserInfoByUser_ID
                                ( User_ID,ref _Person_ID, ref UserName,ref Password,ref _IsActive);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUsers(User_ID,_Person_ID,UserName,Password,_IsActive);
            else
                return null;
        }
        public static clsUsers Find_ByPersonID(int _Person_ID)
        {
            int User_ID = -1;
            string UserName = "", Password = "";
            bool _IsActive = false;

            bool IsFound = clsUserData.GetUserInfoBy_Person_ID
                                (_Person_ID, ref User_ID, ref UserName, ref Password, ref _IsActive);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUsers(User_ID, User_ID, UserName, Password, _IsActive);
            else
                return null;
        }
        public static clsUsers FindByUserNameAndPassword(string UserName,string Password)
        {
            int User_ID = -1;
            int _Person_ID=-1;

            bool _IsActive = false;

            bool IsFound = clsUserData.GetUserInfoByUsernameAndPassword
                                (UserName , Password,ref User_ID,ref _Person_ID, ref _IsActive);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUsers(User_ID, _Person_ID, UserName, Password, _IsActive);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }

            return false;
        }

        public static DataTable GetAll()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool Delete(int User_ID)
        {
            return clsUserData.DeleteUser(User_ID); 
        }

        public static bool isUserExist(int User_ID)
        {
           return clsUserData.IsUserExist(User_ID);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool isUserExistFor_Person_ID(int _Person_ID)
        {
            return clsUserData.IsUserExistFor_Person_ID(_Person_ID);
        }


    }
}
