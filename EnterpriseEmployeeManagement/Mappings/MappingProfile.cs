using AutoMapper;
using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.ViewModels;

namespace EnterpriseEmployeeManagement.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeFormViewModel, Employee>();
            CreateMap<Employee, EmployeeFormViewModel>();

            CreateMap<Employee, EmployeeListViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<UserRole, UserRoleViewModel>();

            CreateMap<User, UserListViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Role.Name)));

            CreateMap<Department, DepartmentListViewModel>();

            CreateMap<DepartmentFormViewModel, Department>();
            CreateMap<Department, DepartmentFormViewModel>();
        }
    }
}
