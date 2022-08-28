using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.DTO;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BankAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly BankDbContext _dbContext;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public TransactionsController(BankDbContext dbContext, IMapper mapper, ITransactionService transactionService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("create_a_transaction")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDto transactionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(transactionRequestDto);

            var transaction = _mapper.Map<Transaction>(transactionRequestDto);

            return Ok(_transactionService.CreateNewTransaction(transaction));
        }

        [HttpPost]
        [Route("make_deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account number must be 10 digits");
            return Ok(_transactionService.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("make_withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account number must be 10 digits");
            return Ok(_transactionService.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("transfer_funds")]
        public IActionResult MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(FromAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") || !Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account number must be 10 digits");

            return Ok(_transactionService.MakeFundTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }
    }
}
