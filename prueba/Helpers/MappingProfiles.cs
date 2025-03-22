using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba.Entities;
using prueba.Dto;
using AutoMapper;

namespace prueba.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Vehiculo, VehicleDTO>();
            CreateMap<VehicleDTO, Vehiculo>();
        }
    }
}