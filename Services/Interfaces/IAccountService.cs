using BankAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAPI.Services
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Task<Account> Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account, string Pin = null);
        void Delete(int Id);
        Account GetById(int id);
        Account GetByAccountNumber(string AccountNumber);
    }
}
