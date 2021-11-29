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
        public IActionResult GetAllCompanies()
        {
            var companies = _repositoryManager.Company.FindAllCompanies(true);

            var value = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

            return Ok(value);
        }

        // GET api/<CompanysController>/5
        [HttpGet("{id}", Name = "GetCompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var companyFromDatabase = _repositoryManager.Company.FindCompany(id, trackChanges: false);

            if (companyFromDatabase == null)
            {
                _logImplementations.InfoMessage($"The company with the id {id} can't be found in the database.");

                return NotFound();
            }
            else
            {
                var valueToBeReturned = _mapper.Map<CompanyDTO>(companyFromDatabase);

                return Ok(valueToBeReturned);
            }
        }


        [Route("getmultiplecompaniesbyid")]
        [HttpPost]
        public IActionResult GetCompaniesById([FromBody] IEnumerable<Guid> id)
        {
            if (id == null)
            {
                _logImplementations.ErrorMessage("The id field is null");

                return BadRequest("Input valid IDs");
            }
            else
            {
                var companiesEntity = _repositoryManager.Company.FindMultipleCompanies(id, trackChanges: false);

                if (id.Count() != companiesEntity.Count())
                {
                    _logImplementations.DebugMessage("Some of the IDs provided by the client is not found in the database");

                    return NotFound();
                }

                var companiesToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companiesEntity);

                return Ok(companiesToReturn);
            }
        }


        // POST api/<CompanysController>
        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyInputDTO company)
        {
            if (company == null)
            {
                _logImplementations.InfoMessage("Company object sent from client is null");

                return BadRequest();
            }
                var valueToPost = _mapper.Map<Company>(company);

                _repositoryManager.Company.CreateCompany(valueToPost);

                _repositoryManager.Save();

                var valueToReturn = _mapper.Map<CompanyDTO>(valueToPost);

                return CreatedAtRoute("GetCompanyById", new { id = valueToReturn.Id }, valueToReturn);
        }

        [Route("createmultiplecompanies")]
        [HttpPost]
        public IActionResult CreateMultipleCompanies([FromBody] IEnumerable<CompanyInputDTO> companies)
        {
            if (companies == null)
            {
                _logImplementations.ErrorMessage("The companies object inputed by the client was null");

                return BadRequest("Kindly input the details of the companies you want to create");
            }
            else
            {
                var companiesToPost = _mapper.Map<IEnumerable<Company>>(companies);

                _repositoryManager.Company.CreateMultipleCompanies(companiesToPost);

                _repositoryManager.Save();

                var companiesToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companiesToPost);

                var ids = string.Join(",", companiesToReturn.Select(c => c.Id));

                return Ok(companiesToReturn);
            }
        }

        // PUT api/<CompanysController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] CompanyUpdateDTO companyUpdateDTO)
        {
            if(companyUpdateDTO == null)
            {
                _logImplementations.ErrorMessage($"The company object passed by the client is null");

                return BadRequest("Input the value of your company object");
            }
            else
            {
                var companyEntity = _repositoryManager.Company.FindCompany(id, trackChanges: true);

                if(companyEntity == null)
                {
                    _logImplementations.InfoMessage($"The company with id, {id} does not exist in the database");

                    return NotFound("Company not found in the database");
                }
                else
                {
                   var newObject =  _mapper.Map(companyUpdateDTO, companyEntity);

                    _repositoryManager.Save();

                    var valueToReturn = _mapper.Map<CompanyDTO>(newObject);

                    return Ok(valueToReturn);
                }
            }
        }

        // DELETE api/<CompanysController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var company = _repositoryManager.Company.FindCompany(id, trackChanges: false);

            if (company == null)
            {
                _logImplementations.ErrorMessage($"Company with id: {id} does mot exist in the database.");

                return BadRequest("Company does not exist in the database");
            }
            else
            {
                _repositoryManager.Company.DeleteCompany(company);

                _repositoryManager.Save();

                return NoContent();
            }
        }


        [HttpPatch("{id}")]
        public IActionResult PatchACompanysRecord(Guid id, [FromBody] JsonPatchDocument<CompanyUpdateDTO> companyUpdateDTO)
        {
            var company = _repositoryManager.Company.FindCompany(id, trackChanges: true);

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

                        _repositoryManager.Save();

                        return NoContent();
                    }
                }
            }
        }
    }
}
