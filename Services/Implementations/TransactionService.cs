using BankAPI.Models;
using BankAPI.Services.Interfaces;
using System;

namespace BankAPI.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public Response CreateNewTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Response FindTransactionByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }

        public Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }
    }
}
