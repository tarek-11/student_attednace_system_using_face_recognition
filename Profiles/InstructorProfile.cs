using AutoMapper;
using DataAccessLayer.Etities;
using presentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.Profiles
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            CreateMap<Instructor, InstructorModel>().ReverseMap();
        }
    }
}
