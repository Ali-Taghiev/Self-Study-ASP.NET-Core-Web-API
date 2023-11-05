using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddWalkRequest addWalkRequestDto)
        {if(ModelState.IsValid)
            {

                //Map dto to domainmodel

                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                //Map Domain model to Dto for sending it back to client 



                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
        else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet]

        public async Task<IActionResult> GetAllAsync()
        {
           var walksDomainModel =  await walkRepository.GetAllAsync();

            //Map Domain to dto


            return Ok( mapper.Map<List<WalkDto>>(walksDomainModel));

        }


        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
             var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }


        
        [HttpPut("{id:Guid}")]

        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,UpdateWalkRequestDto updateWalkRequestDto)
        {
           if(ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);


                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("{id:Guid}")]
        
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {  
            var deletedwalk = await walkRepository.DeleteAsync(id);
            if(deletedwalk == null)
            {
                return NotFound();
            }


            return Ok(deletedwalk); 
        }
    }


}
