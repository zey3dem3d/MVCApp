using AutoMapper;
using Route.MVCApp.BLL.DTOs.Departments;
using Route.MVCApp.PL.ViewModels.Department;

namespace Route.MVCApp.PL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Employees

            #endregion

            #region Departments

            CreateMap<DepartmentDetailsDto, DepartmentVM>();
            //.ForMember(dest => dest.Name, config => config.MapFrom(S => S.Name));

            CreateMap<DepartmentVM, UpdatedDepartmentDto>();

            CreateMap<DepartmentVM, CreatedDepartmentDto>();

            #endregion
        }
    }
}
