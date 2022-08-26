using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        Random rand = new Random();

        public Account()
        {
            AccountNumberGenerated = Convert.ToString((long) rand.NextDouble() * 9_000_000_000 + 1_000_000_000);
            AccountName = $"{FirstName} + {LastName}";
        }
    }

    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Gorvernment,
        Joint
    }
}
