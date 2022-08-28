using System;
using static BankAPI.Models.Transaction;

namespace BankAPI.Models.DTO
{
    public class TransactionRequestDto
    {
        public decimal TransactionAmount { get; set; }
        public string TransactionSourceAmount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public TransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
