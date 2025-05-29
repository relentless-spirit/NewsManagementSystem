using AutoMapper;
using BusinessObject.Entities;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Mapper;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<CreateAccountRequest, SystemAccount>();
    }
}