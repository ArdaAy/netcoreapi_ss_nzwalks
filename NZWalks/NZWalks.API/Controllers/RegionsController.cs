﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            //logger.LogInformation("GetAllRegions Action Method was invoked");
            try
            {
                throw new Exception("This is a custom exception");

                // Get Data From Database - Domain Models
                var regionsDomain = await regionRepository.GetAllAsync();

                // Map Domain Models to DTOs
                var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

                //Return DTOs
                logger.LogInformation($"Finished GetAllRegions request with data {JsonSerializer.Serialize(regionsDomain)}");
                return Ok(regionsDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            

            

            // Map Domain Models to DTOs
            /*
            var regionsDto = new List<RegionDto>();
            foreach (var regionsDomain in regions)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = regionsDomain.Id,
                    Code = regionsDomain.Code,
                    Name = regionsDomain.Name,
                    RegionImageUrl = regionsDomain.RegionImageUrl
                });
            }
            */

            
        }

        // GET SINGLE REGION (Get region By ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {


            // Get Region Domain Model From Database
            //var regionDomain = dbContext.Regions.Find(id);
            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            //Return DTO back to client
            return Ok(regionDto);
            */

            // Map/Convert Region Domain Model to Region DTO
            // Return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        // POST To Create New Region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
            // Map or Convert DTO to Domain Model
            /*
            var regionDomainModel = new Region

            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };
            */

            // Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to Create Region
            //await dbContext.Regions.AddAsync(regionDomainModel);
            ///await dbContext.SaveChangesAsync();
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain Model Back to DTO
            /*
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            */
            var regionDTO = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
            
            
        }


        // Update Region
        // Put: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            /*
            var regionDomainModel = new Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };
            */

            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            // Check if Region Exists
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);


            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to Domain model
            /*
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            */

            // Convert Domain Model to DTO
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            */
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
            
            
        }

        // Delete Region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            // return deleted Region back
            // Map Domain Model to DTO
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            */

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
