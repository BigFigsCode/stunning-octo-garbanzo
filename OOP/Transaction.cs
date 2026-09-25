using System.Collections.Generic;
using System;

namespace BankingProject
{
    public enum TypeOftransaction
    {
        Deposit,
        Withdraw,
        Transfer
        //Loans
    };
    public class Transaction
    {
        public int TransactionID { get; set; }

        // Foreign Key
        public int AccountNumber { get; set; }

        // Navigation Property
        public Account Account { get; set; } = null!;

        public TypeOftransaction customerTransaction { get; set; }

        public DateTime local { get; set; } = DateTime.Now;

        public double amount { get; set; }



        public override string ToString()
        {
            return "|date/Time: " + local + " |Type: " + customerTransaction + " |Amount: " + amount;
        }
    }
}