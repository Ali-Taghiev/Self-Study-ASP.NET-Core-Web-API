using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(NZWalksDbContext dbContext, IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {   //Get data from database
            var regionsDomain = await regionRepository.GetAllasync();
            //Map domain models to DTOs
            //var regionsDto = new List<RegionDto>();
            //foreach (var region in regionsDomain)
            //{
            //    regionsDto.Add(
            //        new RegionDto
            //        {
            //            Id = region.Id,
            //            Name = region.Name,
            //            Code = region.Code,
            //            RegionImageUrl = region.RegionImageUrl,

            //        }

            //        );
            //}
           var regionsDto =  mapper.Map<List<RegionDto>>(regionsDomain);

            //return DTOs to client 
            return Ok(regionsDto);
        }



        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {   //First Method 
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map domain models to DTOs
            //var regionDto = new RegionDto()
            //{
            //    Id = regionDomain.Id,
            //    Name = regionDomain.Name,
            //    Code = regionDomain.Code,
            //    RegionImageUrl = regionDomain.RegionImageUrl,


            //};

           var regionDto= mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           
                //Map or Conver Dto to Domain  Model

                //var regionDomainModel = new Region
                //{
                //    Name = addRegionRequestDto.Name,
                //    Code = addRegionRequestDto.Code,
                //    RegionImageUrl = addRegionRequestDto.RegionImageUrl,
                //};
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Use Domain Model to create Region 

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
                //Map domainmodel back to DTO
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { Id = regionDto.Id }, regionDto);
           

        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
                //var regionDomainModel = new Region
                //{
                //    Code = updateRegionRequestDto.Code,
                //    Name = updateRegionRequestDto.Name,
                //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
                //};
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }


                //convert to dto

                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
           
        }


        [HttpDelete]

        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }



            ////return delete region back
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }


    }
}
