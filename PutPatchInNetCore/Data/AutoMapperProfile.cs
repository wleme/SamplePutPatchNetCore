using AutoMapper;
using PutPatchInNetCore.Dtos;
using PutPatchInNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutPatchInNetCore.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>();

        }
    }
}
