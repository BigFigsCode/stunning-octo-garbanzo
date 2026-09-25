using BankingProject;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;


using BankContext db = new BankContext();//create database connection

DatabaseService databaseService = new DatabaseService(db);//create database service

Customer customerTest = databaseService.GetCustomer();

bool menu = true;//sets a flag for the  main menu so that we can exit the loop
bool customerMenu;//flag for the customer menu
bool adminMenu;//flag for the admin menu
string username = "";
string password = "";
while (menu)
{
    Console.Clear();
    //prompt a welcome screen to the console.
    //provide options for the customer, admin, and a way to exit
    Console.WriteLine("Welcome to the Figgy Bank!");
    Console.WriteLine("Please select one of the following options.");
    Console.WriteLine("1. Customer");
    Console.WriteLine("2. Admin");
    Console.WriteLine("3. Exit");
    string userInput = Console.ReadLine() ?? "";

    switch (userInput)
    {
        case "1":
            Console.WriteLine("Please Enter Username: (BigFig)");
            username = Console.ReadLine() ?? "";//makes it so it is not case sensitive
            Console.WriteLine("Please enter the password: "+ customerTest.Password);
            password = Console.ReadLine() ?? "";

            //lets check with an if statement for the password and username
            if (username.ToLower() == customerTest.UserName.ToLower() && password == customerTest.Password)
            {
                if (customerTest.MustChangePassword == true)
                {
                    Console.WriteLine("Your temporary password must be changed.");

                    customerTest.changePassword();

                    customerTest.MustChangePassword = false;

                    db.SaveChanges();

                    Console.WriteLine("Password changed successfully.");
                }
                customerMenu = true;
                DisplayCustomer();
            }
            else
            {
                Console.WriteLine("Invalid Credentials! Try Again!");
                Console.WriteLine("--------------------------------");
            }

            break;

        case "2":
            Console.WriteLine("Please Enter Username: (BigFig)");
            username = Console.ReadLine() ?? "";//makes it so it is not case sensitive
            Console.WriteLine("Please enter the password: (123)");
            password = Console.ReadLine() ?? "";

            //lets check with an if statement for the password and username
            if (username.ToLower() == "bigfig" && password == "123")
            {
                adminMenu = true;
                DisplayAdmin();
            }
            else
            {
                Console.WriteLine("Invalid Credentials! Try Again!");
                Console.WriteLine("--------------------------------");
            }


            break;

        case "3":

            menu = false;

            break;

        default:
            Console.WriteLine("Invalid Choice!");
            break;
    }
}

//a method that displays options for the customer
//this will make the switch statement look cleaner and readable
//handles options as well
void DisplayCustomer()
{
    customerMenu = true;
    //Console.Clear();
    while (customerMenu)
    {
        
        Console.WriteLine("\n---------- CUSTOMER MENU ----------");
        Console.WriteLine("1. Check Account Details");
        Console.WriteLine("2. Withdraw");
        Console.WriteLine("3. Deposit");
        Console.WriteLine("4. Transfer");
        Console.WriteLine("5. Last 5 transactions");
        Console.WriteLine("6. Request Cheque Book");
        Console.WriteLine("7. Change Password");
        Console.WriteLine("8. Main menu");
        string customerInput = Console.ReadLine() ?? "";
        switch (customerInput)
        {
            case "1":
                Console.WriteLine("\n---------- ACCOUNT DETAILS ----------");
                Console.WriteLine("Name " + customerTest.firstName + " " + customerTest.lastName);
                foreach (var item in customerTest.Accounts)
                {
                    Console.WriteLine("Account number: " + item.AccountNumber);
                    Console.WriteLine("Account type: " + item.AccountType);
                    Console.WriteLine("Account balance: " + item.AccountBalance);
                    Console.WriteLine("Account active status: " + item.isActive);
                    Console.WriteLine("-----------------------------------------");
                }
                break;
            case "2":
                Console.WriteLine("\n---------- WITHDRAW ----------");
                Console.WriteLine("Please enter account number: ");
                int accNo = Convert.ToInt32(Console.ReadLine());
                bool withdrawAccount = false;
                foreach (var item in customerTest.Accounts)
                {
                    if (accNo == item.AccountNumber)
                    {
                        Console.WriteLine("Account Found!: " + item.AccountType);
                        Console.WriteLine("Current Balance: " + item.AccountBalance);
                        withdrawAccount = true;
                        Console.WriteLine("Enter Amount: ");
                        try
                        {
                            double amount = Convert.ToDouble(Console.ReadLine());
                            item.Withdraw(amount);
                            db.SaveChanges();
                            Console.WriteLine("Successful withdraw!");
                            Console.WriteLine("Remaining Balance after withdraw : " + item.AccountBalance);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    }
                }
                if (withdrawAccount == false)
                {
                    Console.WriteLine("Account Not Found!");
                }
                break;
            case "3":
                Console.WriteLine("\n---------- DEPOSIT ----------");
                Console.WriteLine("Please enter account number: ");
                accNo = Convert.ToInt32(Console.ReadLine());
                bool depositAccount = false;
                foreach (var item in customerTest.Accounts)
                {
                    if (accNo == item.AccountNumber)
                    {
                        Console.WriteLine("Account Found!: " + item.AccountType);
                        Console.WriteLine("Current Balance: " + item.AccountBalance);
                        depositAccount = true;
                        Console.WriteLine("Enter Amount: ");
                        try
                        {
                            double amount = Convert.ToDouble(Console.ReadLine());
                            item.Deposit(amount);
                            db.SaveChanges();
                            Console.WriteLine("Successful deposit!");
                            Console.WriteLine("New Balance after deposit: " + item.AccountBalance);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    }
                }
                if (depositAccount == false)
                {
                    Console.WriteLine("Account Not Found!");
                }
                break;
            case "4":
                try
                {

                    Console.WriteLine("\n---------- TRANSFER ----------");

                    Console.WriteLine("Enter your account number: ");
                    int fromAcc = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter account number you want to transfer to: ");
                    int toAcc = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Now enter the amount you want to transfer: ");
                    double transferAmount = Convert.ToDouble(Console.ReadLine());

                    Account? fromAccount = null;
                    Account? toAccount = null;

                    foreach (var item in customerTest.Accounts)
                    {
                        if (fromAcc == item.AccountNumber)
                        {
                            fromAccount = item;
                        }

                        if (toAcc == item.AccountNumber)
                        {
                            toAccount = item;
                        }
                    }

                    if (fromAccount != null && toAccount != null)
                    {
                        if (fromAccount.AccountNumber == toAccount.AccountNumber)
                        {
                            Console.WriteLine("Cannot transfer to the same account!");
                        }
                        else if (transferAmount > fromAccount.AccountBalance)
                        {
                            Console.WriteLine("The amount exceeds your balance!");
                            Console.WriteLine("Current balance: " + fromAccount.AccountBalance);
                        }
                        else
                        {
                            fromAccount.Withdraw(transferAmount);
                            toAccount.Deposit(transferAmount);

                            db.SaveChanges();

                            Console.WriteLine("Transfer Complete!");
                            Console.WriteLine(
                                "Current balance for " +
                                fromAccount.AccountType +
                                " is " +
                                fromAccount.AccountBalance);

                            Console.WriteLine(
                                "Current balance for " +
                                toAccount.AccountType +
                                " is " +
                                toAccount.AccountBalance);
                        }
                    }
                    else
                    {
                        Console.WriteLine("One or both accounts could not be found.");
                    }
                }
                catch (FormatException)//catches the input so only numeric values apply
                {
                    Console.WriteLine("Invalid input. Please enter numbers only.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                break;
            case "5":
                //here we search for the account number
                //then we check that it is found
                //we then use a for loop to process through the transactions
                //then we set a counter to keep track number of times we need which is 5
                //once we hit 5 we break because we only want the last 5 transaction

                //we dont need to savechanges here because this only reads data
                //there no operations here. 
                Console.WriteLine("---------------");
                Console.WriteLine("Enter account number: ");
                accNo = Convert.ToInt32(Console.ReadLine());
                bool transactionAccount = false;
                int transactionCount = 0;
                foreach (var item in customerTest.Accounts)
                {
                    if (accNo == item.AccountNumber)
                    {
                        transactionAccount = true;
                        Console.WriteLine("Account Found!");
                        if (item.Transactions.Count > 0)
                        {
                            Console.WriteLine("Number of Transactions: " + item.Transactions.Count);
                            for (int i = item.Transactions.Count - 1; i >= 0; i--)
                            {
                                Console.WriteLine(item.Transactions[i].ToString());
                                transactionCount++;
                                if (transactionCount == 5)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Transactions Found!");
                        }
                    }
                }
                if (transactionAccount == false)
                {
                    Console.WriteLine("Account not found!");
                }
                break;
            case "6":
                Console.WriteLine("\n---------- REQUEST CHEQUE BOOK ----------");
                Console.WriteLine("Enter account number: ");
                accNo = Convert.ToInt32(Console.ReadLine());
                bool requestBook = false;
                foreach (var item in customerTest.Accounts)
                {

                    if (accNo == item.AccountNumber)
                    {
                        requestBook = true;
                        bool pendingRequest = false;
                        foreach (var chequeRequest in item.ChequeBookRequests)
                        {
                            if (chequeRequest.AdminApproval == ApprovalStatus.Pending)
                            {
                                pendingRequest = true;
                                break;
                            }
                        }
                        if (pendingRequest == true)
                        {
                            Console.WriteLine("You already have a pending cheque book request.");
                        }
                        else
                        {


                            ChequeBookRequest request = new ChequeBookRequest()
                            {
                                Account = item,
                                local = DateTime.Now,
                                AdminApproval = ApprovalStatus.Pending,
                                //the counter is not longer needed we have a primary key in the
                                //check book class
                                //RequestID = requestCounter 

                            };
                            item.ChequeBookRequests.Add(request);

                            db.SaveChanges();

                            Console.WriteLine("Cheque book request submitted.");
                            Console.WriteLine("Status: Pending");
                            Console.WriteLine("RequestID: " + +request.RequestID);
                            //requestCounter++;
                        }

                    }
                }
                if (requestBook == false)
                {
                    Console.WriteLine("Account not found!");
                }


                break;
            case "7":
                Console.WriteLine("\n---------- CHANGE PASSWORD ----------");
                customerTest.changePassword();
                db.SaveChanges();//stores the new password in the database for the next log in
                Console.WriteLine("Password saved to the database");
                break;
            case "8":

                customerMenu = false;

                break;
            default:
                Console.WriteLine("Invalid!");
                break;
        }
    }


}

//a method that displays options for the admin
//this will make the switch statement look cleaner and readable
void DisplayAdmin()
{
    adminMenu = true;
    //Console.Clear();
    while (adminMenu)
    {
        Console.WriteLine("\n---------- ADMIN MENU ----------");
        Console.WriteLine("---------------");
        Console.WriteLine("1. Create New Account");
        Console.WriteLine("2. Delete Account");
        Console.WriteLine("3. Edit Account Details");
        Console.WriteLine("4. Display Summary");
        Console.WriteLine("5. Reset Customer Password");
        Console.WriteLine("6. Approve Cheque book request");
        Console.WriteLine("7. Main menu");

        string adminInput = Console.ReadLine() ?? "";
        switch (adminInput)
        {
            case "1":

                Console.WriteLine("\n---------- CREATE NEW ACCOUNT ----------");
                try
                {
                    Console.WriteLine("Enter Account Number:");
                    int newAccountNumber = Convert.ToInt32(Console.ReadLine());

                    // Make sure account number doesn't already exist
                    bool accountExists = false;

                    foreach (var account in customerTest.Accounts)
                    {
                        if (account.AccountNumber == newAccountNumber)
                        {
                            accountExists = true;
                            break;
                        }
                    }

                    if (accountExists)
                    {
                        Console.WriteLine("An account with this number already exists.");
                        break;
                    }

                    Console.WriteLine("Select Account Type:");
                    Console.WriteLine("1. Savings");
                    Console.WriteLine("2. Checking");

                    int accountChoice = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter Starting Balance:");
                    double startingBalance = Convert.ToDouble(Console.ReadLine());

                    if (startingBalance < 0)
                    {
                        Console.WriteLine("Starting balance cannot be negative.");
                        break;
                    }

                    if (accountChoice == 1)
                    {
                        Savings newSavings = new Savings()
                        {
                            AccountNumber = newAccountNumber,
                            AccountType = TypeOfAccount.Savings,
                            AccountBalance = startingBalance,
                            isActive = true,
                            Customer = customerTest//navigation property
                        };

                        customerTest.Accounts.Add(newSavings);

                        db.SaveChanges();

                        Console.WriteLine("Savings account created successfully!");
                    }
                    else if (accountChoice == 2)
                    {
                        Checking newChecking = new Checking()
                        {
                            AccountNumber = newAccountNumber,
                            AccountType = TypeOfAccount.Checking,
                            AccountBalance = startingBalance,
                            isActive = true,
                            Customer = customerTest//navigation property
                        };

                        customerTest.Accounts.Add(newChecking);

                        db.SaveChanges();

                        Console.WriteLine("Checking account created successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid account type.");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                }
                break;
            case "2":

                Console.WriteLine("\n---------- DELETE ACCOUNT ----------");
                try
                {
                    Console.WriteLine("Enter Account Number:");
                    int deleteAccountNumber = Convert.ToInt32(Console.ReadLine());

                    Account? accountToDelete = null;

                    foreach (var account in customerTest.Accounts)
                    {
                        if (account.AccountNumber == deleteAccountNumber)
                        {
                            accountToDelete = account;
                            break;
                        }
                    }

                    if (accountToDelete == null)
                    {
                        Console.WriteLine("Account not found!");
                    }
                    else
                    {
                        db.Accounts.Remove(accountToDelete);

                        db.SaveChanges();

                        Console.WriteLine("Account deleted successfully!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                break;
            case "3":
                Console.WriteLine("\n---------- EDIT ACCOUNT DETAILS ----------");
                try
                {
                    Console.WriteLine("Enter Account Number:");
                    int editAccountNumber = Convert.ToInt32(Console.ReadLine());

                    Account? accountToEdit = null;

                    foreach (var account in customerTest.Accounts)
                    {
                        if (account.AccountNumber == editAccountNumber)
                        {
                            accountToEdit = account;
                            break;
                        }
                    }

                    if (accountToEdit == null)
                    {
                        Console.WriteLine("Account not found!");
                    }
                    else
                    {
                        Console.WriteLine("Account Number: " + accountToEdit.AccountNumber);
                        Console.WriteLine("Account Type: " + accountToEdit.AccountType);
                        Console.WriteLine("Balance: $" + accountToEdit.AccountBalance);
                        Console.WriteLine("Active: " + accountToEdit.isActive);

                        Console.WriteLine("--------------------");
                        Console.WriteLine("1. Adjust Account Balance");
                        Console.WriteLine("2. Change Active Status");
                        Console.WriteLine("3. Back");

                        int editChoice = Convert.ToInt32(Console.ReadLine());

                        switch (editChoice)
                        {
                            case 1:
                                Console.WriteLine("Enter new balance:");
                                double newBalance = Convert.ToDouble(Console.ReadLine());

                                if (newBalance < 0)
                                {
                                    Console.WriteLine("Balance cannot be negative.");
                                }
                                else
                                {
                                    accountToEdit.AccountBalance = newBalance;
                                    db.SaveChanges();
                                    Console.WriteLine("Account balance updated successfully!");
                                }

                                break;

                            case 2:
                                accountToEdit.isActive = !accountToEdit.isActive;
                                db.SaveChanges();

                                Console.WriteLine(
                                    "Account active status changed to: "
                                    + accountToEdit.isActive
                                );

                                break;

                            case 3:
                                Console.WriteLine("Returning to Admin Menu...");
                                break;

                            default:
                                Console.WriteLine("Invalid choice!");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                break;
            case "4":

                Console.WriteLine("\n---------- SUMMARY ----------");
                int totalAccounts = 0;
                double totalBalance = 0;
                int activeAccounts = 0;
                int inactiveAccounts = 0;

                foreach (var account in customerTest.Accounts)
                {
                    totalAccounts++;

                    totalBalance += account.AccountBalance;

                    if (account.isActive == true)
                    {
                        activeAccounts++;
                    }
                    else
                    {
                        inactiveAccounts++;
                    }
                }

                Console.WriteLine("Total Accounts: " + totalAccounts);
                Console.WriteLine("Total Balance: $" + totalBalance);
                Console.WriteLine("Active Accounts: " + activeAccounts);
                Console.WriteLine("Inactive Accounts: " + inactiveAccounts);
                break;
            case "5":
                //we generate a random string of 5 so the customer can use to log in and set their own password
                Console.WriteLine("\n---------- RESET PASSWORD ----------");

                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                string temporaryPassword = "";

                for (int i = 0; i < 5; i++)
                {
                    int randomIndex = random.Next(characters.Length);
                    temporaryPassword += characters[randomIndex];
                }

                customerTest.resetPassword(temporaryPassword);

                customerTest.MustChangePassword = true;

                db.SaveChanges();

                Console.WriteLine("Customer password has been reset.");
                Console.WriteLine("Temporary Password: " + temporaryPassword);
                Console.WriteLine("Customer must change their password after login.");

                break;
            case "6":
                Console.WriteLine("\n---------- APPROVE REQUEST ----------");
                bool pendingRequestsFound = false;

                foreach (var account in customerTest.Accounts)
                {
                    foreach (var request in account.ChequeBookRequests)
                    {
                        if (request.AdminApproval == ApprovalStatus.Pending)
                        {
                            pendingRequestsFound = true;

                            Console.WriteLine(
                                "Request ID: " + request.RequestID +
                                " | Account: " + account.AccountNumber +
                                " | Status: " + request.AdminApproval
                            );
                        }
                    }
                }

                if (pendingRequestsFound == false)
                {
                    Console.WriteLine("No pending cheque book requests.");
                    break;
                }

                Console.WriteLine("--------------------");
                Console.WriteLine("Enter Request ID:");
                int requestID = Convert.ToInt32(Console.ReadLine());

                ChequeBookRequest? selectedRequest = null;

                foreach (var account in customerTest.Accounts)
                {
                    foreach (var request in account.ChequeBookRequests)
                    {
                        if (request.RequestID == requestID)
                        {
                            selectedRequest = request;
                            break;
                        }
                    }

                    if (selectedRequest != null)
                    {
                        break;
                    }
                }

                if (selectedRequest == null)
                {
                    Console.WriteLine("Request not found!");
                }
                else
                {
                    Console.WriteLine("1. Approve");
                    Console.WriteLine("2. Reject");

                    int approvalChoice = Convert.ToInt32(Console.ReadLine());

                    if (approvalChoice == 1)
                    {
                        selectedRequest.AdminApproval = ApprovalStatus.Approved;
                        db.SaveChanges();
                        Console.WriteLine("Cheque book request approved!");
                    }
                    else if (approvalChoice == 2)
                    {
                        selectedRequest.AdminApproval = ApprovalStatus.Rejected;
                        db.SaveChanges();
                        Console.WriteLine("Cheque book request rejected!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice!");
                    }
                }

                break;
            case "7":

                adminMenu = false;

                break;

            default:
                Console.WriteLine("Invalid Input!");
                break;
        }

    }


}




