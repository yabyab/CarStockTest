using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            var result = await _dealerCarStockRepository.GetAllDealerCarStockByDealerIdAsync(dealerId);
            return Ok(result);
        }

        [HttpPost]
        [Route("{dealerId}")]
        [Authorize]
        public async Task<ActionResult> AddCarStockAsync([FromRoute] int dealerId, DealerCarStock dealerCarStock)
        {
            try{
                dealerCarStock.dealerid = dealerId;
                var result = await _dealerCarStockRepository.InsertDealerCarStockAsync(dealerCarStock);
                return Ok(result);            
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
                if(make == null)
                    make = "";
                if(model == null)
                    model = "";
                if(year == null)
                    year = -1;
                var result = await _dealerCarStockRepository.SearchDealerCarStockAsync(dealerId, make, model, (int)year);
                return Ok(result);
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
                var result = await _dealerCarStockRepository.UpdateDealerCarStockQtyAsync(dealerId, dealerCarStockQtyAdjRequest);
                return Ok(result); 
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
                var result = await _dealerCarStockRepository.UpdateDealerCarStockPriceAsync(dealerId, dealerCarStockPriceAdjRequest);
                return Ok(result); 
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
                var result = await _dealerCarStockRepository.UpdateDealerCarStockInfoAsync(dealerId, dealerCarStockInfoAdjRequest);
                return Ok(result); 
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
                await _dealerCarStockRepository.DeleteDealerCarStockAsync(dealerId, stockid);
                return Ok(); 
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
    }
}