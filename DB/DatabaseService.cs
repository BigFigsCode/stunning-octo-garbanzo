using Microsoft.EntityFrameworkCore;

namespace BankingProject
{
    public class DatabaseService
    {
        private readonly BankContext db;

        public DatabaseService(BankContext db)
        {
            this.db = db;
        }

        public Customer GetCustomer()
        {
            Customer? customer = db.Customers
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.Transactions)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.ChequeBookRequests)
                .FirstOrDefault(c => c.UserName == "BigFig");

            if (customer == null)
            {
                customer = CreateInitialCustomer();
            }

            return customer;
        }

        private Customer CreateInitialCustomer()
        {
            Customer customer = new Customer()
            {
                firstName = "brandon",
                lastName = "figueroa",
                UserName = "BigFig",
                Password = "123"
            };

            Savings savings = new Savings()
            {
                AccountNumber = 111,
                AccountType = TypeOfAccount.Savings,
                AccountBalance = 500,
                isActive = true,
                Customer = customer
            };

            Checking checking = new Checking()
            {
                AccountNumber = 112,
                AccountType = TypeOfAccount.Checking,
                AccountBalance = 1000,
                isActive = true,
                Customer = customer
            };

            customer.Accounts.Add(savings);
            customer.Accounts.Add(checking);

            db.Customers.Add(customer);
            db.SaveChanges();

            return customer;
        }
    }
}