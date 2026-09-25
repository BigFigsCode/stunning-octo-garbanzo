using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BankingProject
{
    public class Customer
    {
        #region Properties
        public int CustomerID {get;set;}//primary key 
        public string firstName{get;set;}= "";
        public string lastName{get;set;} = "";
        public List<Account> Accounts { get; set;} = new List<Account>(); 
        public string UserName{get;set;} = "";
        public string Password{get;set;}="";
        public bool MustChangePassword { get; set; }

        #endregion

        #region Methods

        public void changePassword()
        {
            bool passwordMenu = true;
            while (passwordMenu)
            {
                Console.WriteLine("Please enter the new password: " );
                string newPass = Console.ReadLine()??"";
                if(newPass == Password)
                {
                    Console.WriteLine("Your new password matches the old password try again!");
                    continue;
                }else if(newPass != Password)
                {
                    Password = newPass;
                    Console.WriteLine("Password has been changed!");
                    passwordMenu = false;
                }
                
            }
            
        }

        public void resetPassword(string newPassword)
        {
            Password = newPassword;
        }

        #endregion

    }
}

