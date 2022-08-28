using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using BankAPI.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace BankAPI.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly BankDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;
        public TransactionService(BankDbContext context,
            ILogger<TransactionService> logger,
            IOptions<AppSettings> settings,
            IAccountService accountService)
        {
            _context = context;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;
        }
        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            response.Data = null;
            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var findTransaction = _context.Transactions.Where(x => x.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            response.Data = findTransaction;

            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;

            Transaction transaction = new Transaction();

            //Check the validity of account valid

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid Credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured => {ex.Message}");
            }
            transaction.TransactionType = TransType.Deposit;
            transaction.TransactionSourceAmount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.Narration = $"NEW Transaction FROM SOURCE " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAmount)} TO DESTINATION => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }

        public Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;

            Transaction transaction = new Transaction();

            //Check the validity of account valid

            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid Credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                destinationAccount = _accountService.GetByAccountNumber(ToAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured => {ex.Message}");
            }
            transaction.TransactionType = TransType.Transfer;
            transaction.TransactionSourceAmount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.Narration = $"NEW Transaction FROM SOURCE " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAmount)} TO DESTINATION => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON " +
                $"{transaction.TransactionDate} TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;

            Transaction transaction = new Transaction();

            //Check the validity of account valid

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid Credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured => {ex.Message}");
            }
            transaction.TransactionType = TransType.Withdrawal;
            transaction.TransactionSourceAmount = AccountNumber;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.Narration = $"NEW Transaction FROM SOURCE " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAmount)} TO DESTINATION => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }
    }
}