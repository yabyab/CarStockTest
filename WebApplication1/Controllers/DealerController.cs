using Microsoft.AspNetCore.Authorization;
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
            try{
            await this.AuthorizationDealerAsync(dealerId);
            var result = await _dealerRepository.GetByIdAsync(dealerId);
            if(result == null)
                return BadRequest("Dealer Not Found");
            else
                return Ok(result);

            }
            catch(UnauthorizedAccessException uaex)
            {
                return Unauthorized(uaex.Message);
            }
            catch (Exception e){
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("{dealerId}")]
        [Authorize]
        public async Task<ActionResult<Dealer>> UpdateDealerAsync([FromRoute]int dealerId, Dealer dealer)
        {
            try{
                await this.AuthorizationDealerAsync(dealerId);
                var result = await _dealerRepository.UpdateDealerAsync(dealerId, dealer);
                return Ok(result); 
            }
            catch(UnauthorizedAccessException uaex)
            {
                return Unauthorized(uaex.Message);
            }
            catch(InvalidDataException idex)
            {
                return Ok(idex.Message); 
            }
            catch (Exception e){
                return StatusCode(500, e.Message);
            }
            
        }
        private async Task AuthorizationDealerAsync(int dealerId){
            var headerctx = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if(headerctx == null){
                throw new Exception("Cannot find Token from header");
            }
            var accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
            var jwtAuthenticationManager = new JwtAuthenticationManager();
            bool isValidAuthorized = await jwtAuthenticationManager.matchDealerToken(accessToken, dealerId);
            if(!isValidAuthorized)
                throw new UnauthorizedAccessException("Given Dealer id in URI not match with its token");
        }
    }
}