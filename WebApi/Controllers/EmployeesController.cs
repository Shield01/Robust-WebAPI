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
        public IActionResult GetEmployees(Guid companyId)
        {
            //Check if Company exists

            var company = _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            //If company exists proceed
            
            if(company == null)
            {
                _logImplementations.InfoMessage($"Company with id {companyId} does not exist");

                return NotFound();
            }
            else
            {
                var employees = _repositoryManager.Employee.GetAllEmployeesOfACompany(companyId, trackChanges: false);

                var employeeDTO = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

                return Ok(employeeDTO);
            }
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}", Name = "GetAnEmployeeFromACompany")]
        public IActionResult GetAnEmployeeFromACompany(Guid companyId, Guid id)
        {
            var company = _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if(company == null)
            {
                _logImplementations.ErrorMessage("The company Id provided does not exist in the database");

                return NotFound("Company not found");
            }
            else
            {
                var employee = _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: false);

                if (employee == null)
                {
                    _logImplementations.ErrorMessage("The employee Id provided does not exist in the database");

                    return NotFound("Employee not found");
                }
                else
                {
                    var employeeToReturn = _mapper.Map<EmployeeDTO>(employee);

                    return Ok(employeeToReturn);
                }
            }
        }

        // POST api/<EmployeesController>
        [HttpPost]
        public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeInputDTO employee)
        {
            var employeesCompany = _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if(employeesCompany == null)
            {
                _logImplementations.ErrorMessage("The Company Id provided does not exist in the database");

                return NotFound("Company not found");
            }
            else
            {
                if (employee == null)
                {
                    _logImplementations.ErrorMessage("The Employee object posted by client is null");

                    return BadRequest("Input a valid employee object");
                }
                else
                {
                    var employeeToPost = _mapper.Map<Employee>(employee);

                    _repositoryManager.Employee.CreateEmployee(companyId, employeeToPost);

                    _repositoryManager.Save();

                    var employeeToReturn = _mapper.Map<EmployeeDTO>(employeeToPost);

                    //return CreatedAtRoute("GetAnEmployeeFromACompany", new { id = employeeToReturn.Id }, employeeToReturn);

                    return Ok(employeeToReturn);
                }
            }
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid companyId, Guid id, [FromBody] EmployeeUpdateDTO employeeUpdateDTO)
        {
            var company = _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if(company == null)
            {
                _logImplementations.ErrorMessage($"Company with id, {companyId}, does not exist in the database");

                return BadRequest("Company does not exist in the database");
            }
            else
            {
                var employeeEntity = _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: true);

                if(employeeEntity == null)
                {
                    _logImplementations.InfoMessage($"Employe with id : {id}, does not exist in the database");

                    return NotFound("Employee not found in the database");
                }
                else
                {
                    var result = _mapper.Map(employeeUpdateDTO, employeeEntity);

                    _repositoryManager.Save();

                    var valueToReturn = _mapper.Map<EmployeeDTO>(result);

                    return Ok(valueToReturn);
                }
            }
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid companyId, Guid id)
        {
            var company = _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if (company == null)
            {
                _logImplementations.ErrorMessage($"Company with id {companyId} does not exist in the database");

                return BadRequest("Company Id does not exist");
            }
            else
            {
                var employeeForCompany = _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: false);

                _repositoryManager.Employee.DeleteEmployee(employeeForCompany);

                _repositoryManager.Save();

                return NoContent();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PatchEmployeeData(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeUpdateDTO> employeePatchObject)
        {
            if(employeePatchObject == null)
            {
                _logImplementations.ErrorMessage($"The patch object sent by the client is null");

                return BadRequest("Input a valid Patch object");
            }
            else
            {
                var company = _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

                if (company == null)
                {
                    _logImplementations.ErrorMessage($"The company with id: {companyId}, does not exist");

                    return NotFound($"The company with {id}, does not exist in the database");
                }
                else
                {
                    var employee = _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: true);

                    if (employee == null)
                    {
                        _logImplementations.InfoMessage($"Employee with the id: {id}, does not exist in the database");

                        return NotFound("Employee not found");
                    }
                    else
                    {
                        var objectToPatch = _mapper.Map<EmployeeUpdateDTO>(employee);

                        employeePatchObject.ApplyTo(objectToPatch, ModelState);

                        if (!TryValidateModel(objectToPatch))
                        {
                            return ValidationProblem(ModelState);
                        }
                        else
                        {
                            _mapper.Map(objectToPatch, employee);

                            _repositoryManager.Save();

                            return NoContent();
                        }
                    }
                }
            }
        }
    }
}
