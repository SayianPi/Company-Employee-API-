﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        //extracting common code for replacing repeatitive code in three methods (GetCompanyAsync, DeleteCompanyAsync, and UpdateCompanyAsync)
        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }

        //public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        //{          
        //    var companies = _repository.Company.GetAllCompanies(trackChanges);
        //    // mannual mapping
        //    // var companiesDto = companies.Select(c => new CompanyDto(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country))).ToList();

        //    // using Automapper
        //    var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        //    return companiesDto;
        //}
        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        }

        // getting a single resource(company) from db
        //public CompanyDto GetCompany(Guid id, bool trackChanges)
        //{
        //    var company = _repository.Company.GetCompany(id, trackChanges);          
        //    if (company is null)
        //        throw new CompanyNotFoundException(id);
        //    var companyDto = _mapper.Map<CompanyDto>(company);
        //    return companyDto;
        //}
        public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
        {
        //    var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
        //    if (company is null)
        //        throw new CompanyNotFoundException(id);

            var company = await GetCompanyAndCheckIfItExists(id, trackChanges);
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        //POST method
        //public CompanyDto CreateCompany(CompanyForCreationDto company)
        //{
        //    var companyEntity = _mapper.Map<Company>(company);
        //    _repository.Company.CreateCompany(companyEntity);
        //    _repository.Save();
        //    var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        //    return companyToReturn;
        //}
        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        //for creating a collection of resources
        //public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        //{
        //    if (ids is null)
        //        throw new IdParametersBadRequestException();
        //    var companyEntities = _repository.Company.GetByIds(ids, trackChanges);
        //    if (ids.Count() != companyEntities.Count())
        //        throw new CollectionByIdsBadRequestException();
        //    var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        //    return companiesToReturn;
        //}

        //public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        //{
        //    if (companyCollection is null)
        //        throw new CompanyCollectionBadRequest();

        //    var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        //    foreach (var company in companyEntities)
        //    {
        //        _repository.Company.CreateCompany(company);
        //    }

        //    _repository.Save();

        //    var companyCollectionToReturn =
        //   _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        //    var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        //    return (companies: companyCollectionToReturn, ids: ids);
        //}
        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = await _repository.Company.GetByIdsAsync(ids,
            trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }
        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);
        }

        //public void DeleteCompany(Guid companyId, bool trackChanges)
        //{
        //    var company = _repository.Company.GetCompany(companyId, trackChanges);
        //    if (company is null)
        //        throw new CompanyNotFoundException(companyId);
        //    _repository.Company.DeleteCompany(company);
        //    _repository.Save();
        //}
        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
        }

        //public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        //{
        //    var companyEntity = _repository.Company.GetCompany(companyId, trackChanges);
        //    if (companyEntity is null)
        //        throw new CompanyNotFoundException(companyId);

        //    _mapper.Map(companyForUpdate, companyEntity);
        //    _repository.Save();
        //}
        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            //var companyEntity = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            //if (companyEntity is null)
            //    throw new CompanyNotFoundException(companyId);
            //_mapper.Map(companyForUpdate, companyEntity);
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            _mapper.Map(companyForUpdate, company);
            await _repository.SaveAsync();
        }
    }
}
