using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace BankingProject
{
    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
        
    };

    public class ChequeBookRequest
    {
        [Key]
        public int RequestID { get; set; }

        public int AccountNumber { get; set; }

        public Account Account { get; set; } = null!;

        public DateTime local { get; set; } = DateTime.Now;

        public ApprovalStatus AdminApproval { get; set; }
            = ApprovalStatus.Pending;
    }
}