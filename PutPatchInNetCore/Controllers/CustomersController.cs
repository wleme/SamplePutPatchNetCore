using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PutPatchInNetCore.Data.Repositories;
using PutPatchInNetCore.Dtos;
using PutPatchInNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutPatchInNetCore.Controllers
{
    [Route("/api/customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly ILogger<CustomersController> _log;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepo customerRepo,
            ILogger<CustomersController> log,
            IMapper mapper)
        {
            this._customerRepo = customerRepo;
            this._log = log;
            this._mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerDto customerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var model = _mapper.Map<CustomerDto, Customer>(customerDto);
                await _customerRepo.AddAsync(model);
                await _customerRepo.SaveAllAsync();
                var output = _mapper.Map<Customer, CustomerResponseDto>(model);
                return Created($"/api/customers/{model.Id}", output);

            }
            catch (Exception e)
            {
                _log.LogError($"error adding customer {e}");
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _customerRepo.GetAsync();
                return Ok(_mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(customers));
            }
            catch (Exception e)
            {
                _log.LogError($"error getting customers {e}");
            }

            return BadRequest();
        }

        [Route("{customerId:int}")]
        [HttpGet]
        public async Task<IActionResult> Get(int customerId)
        {
            try
            {
                var customer = await _customerRepo.GetAsync(customerId);
                if (customer == null) return NotFound();
                return Ok(_mapper.Map<Customer, CustomerDto>(customer));
            }
            catch (Exception e)
            {
                _log.LogError($"error getting customer {e}");
            }

            return BadRequest();
        }

        [Route("{customerId:int}")]
        [HttpPut]
        public async Task<IActionResult> Put(int customerId, [FromBody] CustomerDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var oldCustomer = await _customerRepo.GetAsync(customerId);
                if (oldCustomer == null) return NotFound();
                _mapper.Map(dto, oldCustomer);
                await _customerRepo.SaveAllAsync();

                return Ok(_mapper.Map<Customer, CustomerResponseDto>(oldCustomer));
            }
            catch (Exception e)
            {
                _log.LogError($"error updating customer {e}");
            }

            return BadRequest("Error updating customer");
        }

        [Route("{customerId:int}")]
        [HttpPatch]
        public async Task<IActionResult> Patch(int customerId, [FromBody] JsonPatchDocument<CustomerDto> patchModel)
        {
            try
            {
                var customerDb = await _customerRepo.GetAsync(customerId);
                if (customerDb == null) return NotFound();
                var customerDbDto = _mapper.Map<Customer, CustomerDto>(customerDb);

                patchModel.ApplyTo(customerDbDto);
                _mapper.Map(customerDbDto, customerDb);

                await _customerRepo.UpdateAsync(customerDb);
                await _customerRepo.SaveAllAsync();

                return Ok(_mapper.Map<Customer, CustomerResponseDto>(customerDb));
            }
            catch (Exception e)
            {
                _log.LogError($"error updating customer {e}");
            }

            return BadRequest("Error updating customer");
        }

    }
}
