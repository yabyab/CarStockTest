using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers{

    [Route("api/Stock")]
    [ApiController]
    public class DealerCarStockController : ControllerBase
    {
        private readonly IDealerCarStockRepository _dealerCarStockRepository;

        public DealerCarStockController(IDealerCarStockRepository dealerCarStockRepository)
        {
            _dealerCarStockRepository = dealerCarStockRepository;
        }

        [HttpGet]
        [Route("{dealerId}")]
        [Authorize]
        public async Task<ActionResult<List<DealerCarStock>>> GetAllDealerCarStockByDealerId(int dealerId)
        {
            try{
                await AuthorizationDealerAsync(dealerId);
                var result = await _dealerCarStockRepository.GetAllDealerCarStockByDealerIdAsync(dealerId);
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

        [HttpPost]
        [Route("{dealerId}")]
        [Authorize]
        public async Task<ActionResult> AddCarStockAsync([FromRoute] int dealerId, DealerCarStock dealerCarStock)
        {
            try{
                await AuthorizationDealerAsync(dealerId);

                dealerCarStock.dealerid = dealerId;
                var result = await _dealerCarStockRepository.InsertDealerCarStockAsync(dealerCarStock);
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

        [HttpGet]
        [Route("{dealerId}/search")]
        [Authorize]
        public async Task<ActionResult<List<DealerCarStock>>> SearchDealerCarStock([FromRoute]int dealerId, [FromQuery]string? make, [FromQuery]string? model, [FromQuery] int? year)
        {
            try{
                await AuthorizationDealerAsync(dealerId);

                if(make == null)
                    make = "";
                if(model == null)
                    model = "";
                if(year == null)
                    year = -1;
                var result = await _dealerCarStockRepository.SearchDealerCarStockAsync(dealerId, make, model, (int)year);
                return Ok(result);
            }
            catch(UnauthorizedAccessException uaex)
            {
                return Unauthorized(uaex.Message);
            }
            catch(ArgumentException argex)
            {
                return BadRequest(argex.Message); 
            }
            catch (Exception e){
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("{dealerId}/AdjQty")]
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> UpdateCarStockQuantity([FromRoute]int dealerId, DealerCarStockQtyAdjRequest dealerCarStockQtyAdjRequest)
        {
            try{
                await AuthorizationDealerAsync(dealerId);

                var result = await _dealerCarStockRepository.UpdateDealerCarStockQtyAsync(dealerId, dealerCarStockQtyAdjRequest);
                return Ok(result); 
            }
            catch(UnauthorizedAccessException uaex)
            {
                return Unauthorized(uaex.Message);
            }
            catch(ArgumentException argex)
            {
                return BadRequest(argex.Message); 
            }
            catch(InvalidDataException idex)
            {
                return BadRequest(idex.Message); 
            }
            catch (Exception e){
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("{dealerId}/AdjPrice")]
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> UpdateCarStockPrice([FromRoute]int dealerId, DealerCarStockPriceAdjRequest dealerCarStockPriceAdjRequest)
        {
            try{
                await AuthorizationDealerAsync(dealerId);

                var result = await _dealerCarStockRepository.UpdateDealerCarStockPriceAsync(dealerId, dealerCarStockPriceAdjRequest);
                return Ok(result); 
            }
            catch(UnauthorizedAccessException uaex)
            {
                return Unauthorized(uaex.Message);
            }
            catch(ArgumentException argex)
            {
                return BadRequest(argex.Message); 
            }
            catch(InvalidDataException idex)
            {
                return BadRequest(idex.Message); 
            }
            catch (Exception e){
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("{dealerId}/AdjInfo")]
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> UpdateCarStockInfo([FromRoute]int dealerId, DealerCarStockInfoAdjRequest dealerCarStockInfoAdjRequest)
        {
            try{
                await AuthorizationDealerAsync(dealerId);

                var result = await _dealerCarStockRepository.UpdateDealerCarStockInfoAsync(dealerId, dealerCarStockInfoAdjRequest);
                return Ok(result); 
            }
            catch(UnauthorizedAccessException uaex)
            {
                return Unauthorized(uaex.Message);
            }
            catch(ArgumentException argex)
            {
                return BadRequest(argex.Message); 
            }
            catch(InvalidDataException idex)
            {
                return BadRequest(idex.Message); 
            }
            catch (Exception e){
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{dealerId}")]
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> DeleteDealerCarStock([FromRoute]int dealerId, int stockid)
        {
            try{
                await AuthorizationDealerAsync(dealerId);

                await _dealerCarStockRepository.DeleteDealerCarStockAsync(dealerId, stockid);
                return Ok(); 
            }
            catch(UnauthorizedAccessException uaex)
            {
                return Unauthorized(uaex.Message);
            }
            catch(ArgumentException argex)
            {
                return BadRequest(argex.Message); 
            }
            catch(InvalidDataException idex)
            {
                return BadRequest(idex.Message); 
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