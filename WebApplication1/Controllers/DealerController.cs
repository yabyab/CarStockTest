using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerRepository _dealerRepository;

        public DealerController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login(JwtAuthRequest jwtAuthRequest)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager();
            var result = jwtAuthenticationManager.Authenticate(jwtAuthRequest.dealername, jwtAuthRequest.dealeremail, null);
            if(result != null){
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> InsertDealerAsync(Dealer dealer)
        {
            try{
                var newDealerId = await _dealerRepository.InsertDealerAsync(dealer);
                
                var jwtAuthenticationManager = new JwtAuthenticationManager();
                var result = jwtAuthenticationManager.Authenticate(dealer.dealeremail, dealer.dealeremail, newDealerId);
                return Ok(result); 
            }
            catch(InvalidDataException idex)
            {
                return Ok(idex.Message); 
            }
            catch (Exception ex){
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet]
        [Route("{dealerId}")]
        [Authorize]
        public async Task<ActionResult<Dealer>> GetDealerById(int dealerId)
        {
            var result = await _dealerRepository.GetByIdAsync(dealerId);
            if(result == null)
                return BadRequest("Dealer Not Found");
            else
                return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateDealerAsync(Dealer dealer)
        {
            try{
                await _dealerRepository.UpdateDealerAsync(dealer);
                return Ok(); 
            }
            catch(InvalidDataException idex)
            {
                return Ok(idex.Message); 
            }
            catch (Exception e){
                return StatusCode(500, e.Message);
            }
            
        }
    }
}