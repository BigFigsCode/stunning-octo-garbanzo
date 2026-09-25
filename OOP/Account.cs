using System.Collections.Generic;
using System.Formats.Tar;
using System.ComponentModel.DataAnnotations;
namespace BankingProject
{

    public enum TypeOfAccount
    {
        Savings,
        Checking
        //Loans
    };

    #region Properties
    public abstract class Account
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<ChequeBookRequest> ChequeBookRequests { get; set; } = new List<ChequeBookRequest>();
        [Key]
        public int AccountNumber { get; set; }//primary key
        public TypeOfAccount AccountType { get; set; }
        public double AccountBalance { get; set; }
        public bool isActive { get; set; }
        public int CustomerID { get; set; }// foreign key
        
        //navigation property  
        //it lets c# navigate from Account to customer
        public Customer Customer { get; set; } = null!;
        


    #endregion

        #region Methods
        //method that will be overriden (polymorphism)
        public virtual double Withdraw(double amount)
        {
            if (amount > AccountBalance)
            {
                throw new Exception("The amount " + amount + " exceeds your current balance!");
            }
            else if (amount <= 0)
            {
                throw new Exception("You cannot withdraw " + amount + " it is of negative value!");
            }
            var transaction = new Transaction()
            {
                Account = this,
                customerTransaction = TypeOftransaction.Withdraw,
                amount = amount,
                local = DateTime.Now
            };
            AccountBalance -= amount;
            Transactions.Add(transaction);
            return AccountBalance;
        }

        public double Deposit(double amount)
        {
            if (amount <= 0)
            {
                throw new Exception("You cannot deposit " + amount + " it is of negative value!");
            }
            var transaction = new Transaction()
            {
                Account = this,
                customerTransaction = TypeOftransaction.Deposit,
                amount = amount,
                local = DateTime.Now
            };
            AccountBalance += amount;
            Transactions.Add(transaction);
            return AccountBalance;
        }

        public double CheckBalance()
        {
            return AccountBalance;
        }

        #endregion
    }
}