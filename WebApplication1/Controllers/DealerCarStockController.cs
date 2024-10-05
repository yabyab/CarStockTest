using System.Diagnostics;
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
        [Authorize]
        public async Task<ActionResult<List<DealerCarStock>>> GetAllDealerCarStockByDealerId()
        {
            try{
                int dealerId = await RetrieveDealerIdFromTokenAsync();
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
        [Authorize]
        public async Task<ActionResult> AddCarStockAsync(DealerCarStock dealerCarStock)
        {
            try{
                int dealerId = await RetrieveDealerIdFromTokenAsync();
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
        [Route("search")]
        [Authorize]
        public async Task<ActionResult<List<DealerCarStock>>> SearchDealerCarStock(DealerCarstockSearchRequest dealerCarstockSearchRequest)
        {
            try{
                int dealerId = await RetrieveDealerIdFromTokenAsync();
                await AuthorizationDealerAsync(dealerId);
                string make = (dealerCarstockSearchRequest.make == null) ? "": dealerCarstockSearchRequest.make;
                string model = (dealerCarstockSearchRequest.model == null) ? "": dealerCarstockSearchRequest.model;
                int year = (dealerCarstockSearchRequest.year == null) ? -1: (int)dealerCarstockSearchRequest.year;

                var result = await _dealerCarStockRepository.SearchDealerCarStockAsync(dealerId, make, model, year);
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
        [Route("AdjQty")]
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> UpdateCarStockQuantity(DealerCarStockQtyAdjRequest dealerCarStockQtyAdjRequest)
        {
            try{
                int dealerId = await RetrieveDealerIdFromTokenAsync();
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
        [Route("AdjPrice")]
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> UpdateCarStockPrice(DealerCarStockPriceAdjRequest dealerCarStockPriceAdjRequest)
        {
            try{
                int dealerId = await RetrieveDealerIdFromTokenAsync();
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
        [Route("AdjInfo")]
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> UpdateCarStockInfo(DealerCarStockInfoAdjRequest dealerCarStockInfoAdjRequest)
        {
            try{
                int dealerId = await RetrieveDealerIdFromTokenAsync();
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
        [Authorize]
        public async Task<ActionResult<DealerCarStock>> DeleteDealerCarStock(int stockid)
        {
            try{
                int dealerId = await RetrieveDealerIdFromTokenAsync();
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

        private async Task<int> RetrieveDealerIdFromTokenAsync(){
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if(authHeader == null){
                throw new Exception("Cannot find Token from header");
            }
            var accessToken = authHeader.Split(' ')[1];
            var jwtAuthenticationManager = new JwtAuthenticationManager();
            int resDealerId = await jwtAuthenticationManager.GetDealeridByToken(accessToken);
            if(resDealerId == -1)
            {
                throw new Exception("Given Token Cannot retrieve Dealer ");
            }
            else
            {
                return resDealerId;
            }
        }

        private async Task AuthorizationDealerAsync(int dealerId){
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if(authHeader == null){
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