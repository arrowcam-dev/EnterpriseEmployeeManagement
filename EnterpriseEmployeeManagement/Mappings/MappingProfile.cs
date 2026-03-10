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
        }
    }
}
