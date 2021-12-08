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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly ILogImplementations _logImplementations;

        private readonly IRepositoryManager _repositoryManager;

        public EmployeesController(

                IRepositoryManager repositoryManager,

                IMapper mapper,

                ILogImplementations logImplementations
            )
        {
            _mapper = mapper;

            _logImplementations = logImplementations;

            _repositoryManager = repositoryManager;
        }

        // GET: api/<EmployeesController>
        [HttpGet]
        public async Task<IActionResult> GetEmployees(Guid companyId)
        {
            //Check if Company exists

            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            //If company exists proceed
            
            if(company == null)
            {
                _logImplementations.InfoMessage($"Company with id {companyId} does not exist");

                return NotFound();
            }
            else
            {
                var employees = await _repositoryManager.Employee.GetAllEmployeesOfACompany(companyId, trackChanges: false);

                var employeeDTO = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

                return Ok(employeeDTO);
            }
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}", Name = "GetAnEmployeeFromACompany")]
        [ServiceFilter(typeof(ValidateCompanyExistForEmployeeController))]
        public IActionResult GetAnEmployeeFromACompany(Guid companyId, Guid id)
        {
            var employee = HttpContext.Items["employee"] as Employee;

            var employeeToReturn = _mapper.Map<EmployeeDTO>(employee);

            return Ok(employeeToReturn);
        }

        // POST api/<EmployeesController>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttributes))]
        public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeInputDTO employee)
        {
            var employeesCompany = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            
            var employeeToPost = _mapper.Map<Employee>(employee);

            _repositoryManager.Employee.CreateEmployee(companyId, employeeToPost);

            await _repositoryManager.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDTO>(employeeToPost);

            //return CreatedAtRoute("GetAnEmployeeFromACompany", new { id = employeeToReturn.Id }, employeeToReturn);

            return Ok(employeeToReturn);
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttributes))]
        public async Task<IActionResult> Put(Guid companyId, Guid id, [FromBody] EmployeeUpdateDTO employeeUpdateDTO)
        {
            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            var employeeEntity = await _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: true);

            var result = _mapper.Map(employeeUpdateDTO, employeeEntity);

            await _repositoryManager.SaveAsync();

            var valueToReturn = _mapper.Map<EmployeeDTO>(result);

            return Ok(valueToReturn);
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistForEmployeeController))]
        public async Task<IActionResult> Delete(Guid companyId, Guid id)
        {
            var employeeForCompany = HttpContext.Items["employee"] as Employee;

            _repositoryManager.Employee.DeleteEmployee(employeeForCompany);

            await _repositoryManager.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistForEmployeeController))]
        public async Task<IActionResult> PatchEmployeeData(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeUpdateDTO> employeePatchObject)
        {
            if(employeePatchObject == null)
            {
                _logImplementations.ErrorMessage($"The patch object sent by the client is null");

                return BadRequest("Input a valid Patch object");
            }

            var employee = HttpContext.Items["employee"] as Employee;


            var objectToPatch = _mapper.Map<EmployeeUpdateDTO>(employee);

            employeePatchObject.ApplyTo(objectToPatch, ModelState);

            if (!TryValidateModel(objectToPatch))
            {
                return UnprocessableEntity(ModelState);
            }
            else
            {
                _mapper.Map(objectToPatch, employee);

                await _repositoryManager.SaveAsync();

                return NoContent();
            }
        }

        [Route("creatmultipleemployeesforacompany")]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttributes))]
        public async Task<IActionResult> CreateMultipleEmployeesForACompany(Guid companyId, [FromBody] IEnumerable<EmployeeInputDTO> employeeInputDTOs)
        {
            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            var employeeToCreate = _mapper.Map<IEnumerable<Employee>>(employeeInputDTOs);

            _repositoryManager.Employee.CreateMultipleEmployee(companyId, employeeToCreate);

            await _repositoryManager.SaveAsync();

            var objectToReturn = _mapper.Map<IEnumerable<EmployeeDTO>>(employeeToCreate);

            return Ok(objectToReturn);
        }

        [Route("getmultipleemployeesfromacompanybyid")]
        [HttpPost]
        public async Task<IActionResult> GetMultipleEmployeesFromCompanyById(Guid companyId, [FromBody] IEnumerable<Guid> employeesId)
        {
            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if(company == null)
            {
                _logImplementations.ErrorMessage($"Company with id: {companyId} does not exist in the database");

                return BadRequest("Input a valid company id");
            }
            else
            {
                var employeeEntity = await _repositoryManager.Employee.GetMultipleEmployeesById(companyId, employeesId, trackChanges: false);

                var employeesToReturn = _mapper.Map<IEnumerable<EmployeeDTO>>(employeeEntity);

                if(employeesId.Count() != employeeEntity.Count())
                {
                    _logImplementations.InfoMessage("Not all the employee id passed exist in the database");

                    return Ok(employeesToReturn);
                }
                else
                {
                    return Ok(employeesToReturn);
                }

            }
        }
    }
}
