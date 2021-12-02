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
        public async Task<IActionResult> GetAnEmployeeFromACompany(Guid companyId, Guid id)
        {
            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if(company == null)
            {
                _logImplementations.ErrorMessage("The company Id provided does not exist in the database");

                return NotFound("Company not found");
            }
            else
            {
                var employee = await _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: false);

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
        public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeInputDTO employee)
        {
            var employeesCompany = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

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
                    if (!ModelState.IsValid)
                    {
                        _logImplementations.ErrorMessage("The model state of the employee object posted is not valid");

                        ModelState.AddModelError("age", "The age value is not valid.");

                        return UnprocessableEntity(ModelState);
                    }
                    else
                    {
                        var employeeToPost = _mapper.Map<Employee>(employee);

                        _repositoryManager.Employee.CreateEmployee(companyId, employeeToPost);

                        await _repositoryManager.SaveAsync();

                        var employeeToReturn = _mapper.Map<EmployeeDTO>(employeeToPost);

                        //return CreatedAtRoute("GetAnEmployeeFromACompany", new { id = employeeToReturn.Id }, employeeToReturn);

                        return Ok(employeeToReturn);
                    }
                }
            }
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid companyId, Guid id, [FromBody] EmployeeUpdateDTO employeeUpdateDTO)
        {
            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if(company == null)
            {
                _logImplementations.ErrorMessage($"Company with id, {companyId}, does not exist in the database");

                return BadRequest("Company does not exist in the database");
            }
            else
            {
                var employeeEntity = await _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: true);

                if(employeeEntity == null)
                {
                    _logImplementations.InfoMessage($"Employe with id : {id}, does not exist in the database");

                    return NotFound("Employee not found in the database");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        _logImplementations.ErrorMessage("Model state of new object is not valid");

                        return UnprocessableEntity(ModelState);
                    }
                    else
                    {
                        var result = _mapper.Map(employeeUpdateDTO, employeeEntity);

                       await  _repositoryManager.SaveAsync();

                        var valueToReturn = _mapper.Map<EmployeeDTO>(result);

                        return Ok(valueToReturn);
                    }
                }
            }
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid companyId, Guid id)
        {
            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if (company == null)
            {
                _logImplementations.ErrorMessage($"Company with id {companyId} does not exist in the database");

                return BadRequest("Company Id does not exist");
            }
            else
            {
                var employeeForCompany = await _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: false);

                _repositoryManager.Employee.DeleteEmployee(employeeForCompany);

                await _repositoryManager.SaveAsync();

                return NoContent();
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployeeData(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeUpdateDTO> employeePatchObject)
        {
            if(employeePatchObject == null)
            {
                _logImplementations.ErrorMessage($"The patch object sent by the client is null");

                return BadRequest("Input a valid Patch object");
            }
            else
            {
                var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

                if (company == null)
                {
                    _logImplementations.ErrorMessage($"The company with id: {companyId}, does not exist");

                    return NotFound($"The company with {id}, does not exist in the database");
                }
                else
                {
                    var employee = await _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges: true);

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
                            return UnprocessableEntity(ModelState);
                        }
                        else
                        {
                            _mapper.Map(objectToPatch, employee);

                            await _repositoryManager.SaveAsync();

                            return NoContent();
                        }
                    }
                }
            }
        }

        [Route("creatmultipleemployeesforacompany")]
        [HttpPost]
        public async Task<IActionResult> CreateMultipleEmployeesForACompany(Guid companyId, [FromBody] IEnumerable<EmployeeInputDTO> employeeInputDTOs)
        {
            var company = await _repositoryManager.Company.FindCompany(companyId, trackChanges: false);

            if (company == null)
            {
                _logImplementations.ErrorMessage($"The company Id passed by the client doesn't exist on the database");

                return BadRequest($"The company id {companyId} doesn't exist, so who employed the employees ?");
            }
            else
            {
                if (employeeInputDTOs == null)
                {
                    _logImplementations.ErrorMessage($"Employee objects passed by client is null");

                    return BadRequest("Kindly input an employee object");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        _logImplementations.ErrorMessage("The model state of employee objects posted is invalid");

                        ModelState.AddModelError("age", "The age value is not valid.");

                        return UnprocessableEntity(ModelState);
                    }
                    else
                    {
                        var employeeToCreate = _mapper.Map<IEnumerable<Employee>>(employeeInputDTOs);

                        _repositoryManager.Employee.CreateMultipleEmployee(companyId, employeeToCreate);

                        await _repositoryManager.SaveAsync();

                        var objectToReturn = _mapper.Map<IEnumerable<EmployeeDTO>>(employeeToCreate);

                        return Ok(objectToReturn);
                    }
                }
            }
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
