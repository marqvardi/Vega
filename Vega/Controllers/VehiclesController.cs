using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Vega.Core;
 


namespace Vega.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;       
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;     
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(VehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var model = await context.Models.FindAsync(vehicleResource.ModelId);
            //if (model == null)
            //{
            //    ModelState.AddModelError("ModelId", "Invalid model Id");
            //    return BadRequest(ModelState);
            //}

            var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();


            vehicle = await repository.GetVehicles(vehicle.Id);

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, VehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var model = await context.Models.FindAsync(vehicleResource.ModelId);
            //if (model == null)
            //{
            //    ModelState.AddModelError("ModelId", "Invalid model Id");
            //    return BadRequest(ModelState);
            //}

            //var vehicle = await context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);

            var vehicle = await repository.GetVehicles(id);

            if (vehicle == null)
                return NotFound();

            mapper.Map<VehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();

            vehicle = await repository.GetVehicles(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle =  await repository.GetVehicles(id, includeRelated: false);
            if (vehicle == null)           
                return NotFound();

            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await repository.GetVehicles(id);

            if (vehicle == null)
                return NotFound();

            var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }
    }
}