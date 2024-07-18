﻿using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        //Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges);
        //Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId,
        //    EmployeeParameter employeeParameters, bool trackChanges);
        //Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid 
        //    companyId, EmployeeParameter employeeParameters, bool trackChanges);
        Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId,
            EmployeeParameter employeeParameters, bool trackChanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
        Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);

        Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, 
            bool compTrackChanges, bool empTrackChanges);

        //patch
        Task <(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, 
            Guid id, bool compTrackChanges, bool empTrackChanges);

        Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee
        employeeEntity);

      
    }
}
