using AutoMapper;
using BankAPI.Models;
using BankAPI.Models.DTO;
using BankAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BankAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("register_new_accunt")]
        public IActionResult RegisterNewAccount([FromBody] RegisterAccountViewModel newAccountModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(newAccountModel);

            var account = _mapper.Map<Account>(newAccountModel);
            return Ok(_accountService.Create(account, newAccountModel.Pin, newAccountModel.Pin));
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAccount()
        {
            var account = _accountService.GetAllAccounts();
            var cleanedAccountData = _mapper.Map<IList<GetAccountModel>>(account);
            return Ok(cleanedAccountData);
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel authenticateModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(authenticateModel);
            return Ok(_accountService.Authenticate(authenticateModel.AccountNumber, authenticateModel.Pin));
        }

        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account number must be 10 digits");

            var account = _accountService.GetByAccountNumber(AccountNumber);
            var cleanedAccountData = _mapper.Map<IList<GetAccountModel>>(account);
            return Ok(cleanedAccountData);
        }

        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int id)
        {
            var account = _accountService.GetById(id);
            var cleanedData = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedData);
        }

        [HttpPost]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel updateAccount)
        {
            if(!ModelState.IsValid)
                return BadRequest(updateAccount);
            var account = _mapper.Map<Account>(updateAccount);
            _accountService.Update(account, updateAccount.Pin);
            return Ok();
        }
    }
}
