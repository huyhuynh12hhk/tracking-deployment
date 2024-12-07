using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Services;

namespace ProductTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchService searchService;

        public SearchController(SearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAll([FromQuery] string query)
        {
            List<SearchResult> results = new();
            if (!string.IsNullOrWhiteSpace(query)) {
                results.AddRange(await searchService.SearchAll(query));
            }
            

            return Ok(BaseResponse<List<SearchResult>>.Success(results));
        }
    }
}
