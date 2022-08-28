using AutoMapper;
using BankAPI.Models;
using BankAPI.Models.DTO;

namespace BankAPI.Profiles
{
    public class BankProfiles : Profile
    {
        public BankProfiles()
        {
            CreateMap<RegisterAccountViewModel, Account>().ReverseMap();
            CreateMap<UpdateAccountModel, Account>().ReverseMap();
            CreateMap<GetAccountModel, Account>().ReverseMap();
            CreateMap<TransactionRequestDto, Transaction>().ReverseMap();


        }
    }
}
