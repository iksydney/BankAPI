using BankAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace BankAPI.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
