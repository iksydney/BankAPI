using BankAPI.Data;
using BankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly BankDbContext _dbcontext;
        public AccountService(BankDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Account Authenticate(string AccountNumber, string Pin)
        {
            var account = _dbcontext.Accounts.Where(opt => opt.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if(account == null)
                return null;
            if(!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                return null;
            
            return account;
        }
        public static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if(string.IsNullOrEmpty(Pin))
                throw new ArgumentNullException("Pin");
            using(var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                for(int i = 0; i < computedPinHash.Length; i++)
                {
                    if(computedPinHash[i] != pinHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<Account> Create(Account account, string Pin, string ConfirmPin)
        {
            if (_dbcontext.Accounts.Any(open => open.Email == account.Email))
                throw new ApplicationException("An account already exists with this account");
            if (!Pin.Equals(ConfirmPin))
                throw new ArgumentException("Pins do not match", "Pin");

            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);

            account.PinHash = pinHash;
            account.PinSalt = pinSalt;
            _dbcontext.Accounts.Add(account);
            await _dbcontext.SaveChangesAsync();

            return account;
        }

        private static void CreatePinHash(string Pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
            }
        }
        public void Delete(int Id)
        {
            var account = _dbcontext.Accounts.Find(Id);
            if(account != null)
            {
                _dbcontext.Accounts.Remove(account);
                _dbcontext.SaveChanges();
            }
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _dbcontext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                return null;
            return account;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _dbcontext.Accounts.ToList();
        }

        public Account GetById(int id)
        {
            var account = _dbcontext.Accounts.Where(x => x.Id == id).FirstOrDefault();
            if (account == null)
                return null;
            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            var accountTobeUpdated = _dbcontext.Accounts.Where(x => x.Id == account.Id).SingleOrDefault();
            if (accountTobeUpdated == null)
                throw new ApplicationException("Account doesnt Exist");
            if(!string.IsNullOrWhiteSpace(account.Email))
            {
                if(_dbcontext.Accounts.Any(x => x.Email == account.Email))
                    throw new ApplicationException("This Email " + account.Email + "already exists");
                accountTobeUpdated.Email = account.Email;
            }

            //Update Phone number
            if(!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if(_dbcontext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber))
                    throw new ApplicationException("This Phone number " + account.PhoneNumber + "already exists");
                accountTobeUpdated.PhoneNumber = account.PhoneNumber;
            }

            //Update Phone number
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);
                accountTobeUpdated.PinHash = pinHash;
                accountTobeUpdated.PinSalt = pinSalt;
            }
            accountTobeUpdated.DateLastUpdated = DateTime.Now;
            _dbcontext.Update(accountTobeUpdated);
            _dbcontext.SaveChanges();
        }
    }
}
