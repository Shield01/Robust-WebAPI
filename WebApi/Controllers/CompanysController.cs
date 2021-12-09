using AutoMapper;
using Core.Models;
using Infrastructure.Data_Transfer_Objects;
using Infrastructure.Database_Context;
using Infrastructure.Repository_Manager;
using LogService.Abstractions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Action_Filters;
using WebApi.ModelBinders;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanysController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        private readonly IMapper _mapper;

        private readonly ILogImplementations _logImplementations;

        public CompanysController(

            IRepositoryManager repositoryManager, 

            IMapper mapper, 

            ILogImplementations logImplementations)
        {
            _repositoryManager = repositoryManager;

            _mapper = mapper;

            _logImplementations = logImplementations;
        }

        // GET: api/<CompanysController>
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _repositoryManager.Company.FindAllCompanies(true);

            var value = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

            return Ok(value);
        }

        // GET api/<CompanysController>/5
        [HttpGet("{id}", Name = "GetCompanyById")]
        [ServiceFilter(typeof(ValidateCompanyExistAttribute))]
        public IActionResult GetCompany(Guid id)
        {
            var companyFromDatabase = HttpContext.Items["company"] as Company;

            var valueToBeReturned = _mapper.Map<CompanyDTO>(companyFromDatabase);

            return Ok(valueToBeReturned);
        }


        [Route("getmultiplecompaniesbyid")]
        [HttpPost]
        public async Task<IActionResult> GetCompaniesById([FromBody] IEnumerable<Guid> id)
        {
            if (id == null)
            {
                _logImplementations.ErrorMessage("The id field is null");

                return BadRequest("Input valid IDs");
            }
            else
            {
                var companiesEntity = await _repositoryManager.Company.FindMultipleCompanies(id, trackChanges: false);

                var companiesToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companiesEntity);

                if (id.Count() != companiesEntity.Count())
                {
                    _logImplementations.DebugMessage("Some of the IDs provided by the client is not found in the database");

                    return Ok(companiesToReturn);
                }

                return Ok(companiesToReturn);
            }
        }


        // POST api/<CompanysController>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttributes))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyInputDTO company)
        {
            var valueToPost = _mapper.Map<Company>(company);

            _repositoryManager.Company.CreateCompany(valueToPost);

            await _repositoryManager.SaveAsync();

            var valueToReturn = _mapper.Map<CompanyDTO>(valueToPost);

            return CreatedAtRoute("GetCompanyById", new { id = valueToReturn.Id }, valueToReturn);
        }

        [Route("createmultiplecompanies")]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttributes))]
        public async Task<IActionResult> CreateMultipleCompanies([FromBody] IEnumerable<CompanyInputDTO> companies)
        {
            var companiesToPost = _mapper.Map<IEnumerable<Company>>(companies);

            _repositoryManager.Company.CreateMultipleCompanies(companiesToPost);

            await _repositoryManager.SaveAsync();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companiesToPost);

            var ids = string.Join(",", companiesToReturn.Select(c => c.Id));

            return Ok(companiesToReturn);
        }

        // PUT api/<CompanysController>/5
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttributes))]
        [ServiceFilter(typeof(ValidateCompanyExistAttribute))]
        public async Task<IActionResult> Put(Guid id, [FromBody] CompanyUpdateDTO companyUpdateDTO)
        {
            var companyEntity = HttpContext.Items["company"] as Company;

            var newObject = _mapper.Map(companyUpdateDTO, companyEntity);

            await _repositoryManager.SaveAsync();

            var valueToReturn = _mapper.Map<CompanyDTO>(newObject);

             return Ok(valueToReturn);
        }

        // DELETE api/<CompanysController>/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistAttribute))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var company = HttpContext.Items["company"] as Company;

            _repositoryManager.Company.DeleteCompany(company);

            await _repositoryManager.SaveAsync();

            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchACompanysRecord(Guid id, [FromBody] JsonPatchDocument<CompanyUpdateDTO> companyUpdateDTO)
        {
            var company = await _repositoryManager.Company.FindCompany(id, trackChanges: true);

            if(company == null)
            {
                _logImplementations.ErrorMessage($"The company with id: {id}, does not exist in the database");

                return BadRequest("Input a valid company id");
            }
            else
            {
                if(companyUpdateDTO == null)
                {
                    _logImplementations.InfoMessage($"Patch object inputed by client is null");

                    return BadRequest("Input a valid patch object");
                }
                else
                {
                    var objectToPatch = _mapper.Map<CompanyUpdateDTO>(company);

                    companyUpdateDTO.ApplyTo(objectToPatch, ModelState);

                    if (!TryValidateModel(objectToPatch))
                    {
                        return ValidationProblem(ModelState);
                    }
                    else
                    {
                        _mapper.Map(objectToPatch, company);

                        await _repositoryManager.SaveAsync();

                        return NoContent();
                    }
                }
            }
        }
    }
}
