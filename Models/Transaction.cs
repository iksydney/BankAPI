using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }
        public decimal TransactionAmount { get; set; }
        public TransStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TransStatus.Success);
        public string TransactionSourceAmount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string Narration { get; set; }
        public TransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}";
        }
    }



    public enum TransStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TransType
    {
        Deposit,
        Transfer,
        Withdrawal
    }
}
