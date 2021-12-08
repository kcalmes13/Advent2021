using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnionWireless.Utilities.ModelParser.Api.Controllers
{
    /// <summary>
    /// Controller for interacting with alert rules
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ModelParser")]
    [Produces("application/json")]
    [Authorize]
    public class ModelParserController : Controller
    {
        /// <summary>
        /// Constructor making model controller
        /// </summary>
        public ModelParserController()
        {
        }

        /// <summary>
        /// Get alert types.  
        /// </summary>
        /// <param name="pageSize">amount of types to get</param>
        /// <param name="pageNum">current page location</param>
        /// <param name="endDate">pass in to only get active with end date after or null</param>
        /// <param name="id">pass in id to get</param>
        /// <returns>Ok - 200 for a successful fetch, BadRequest - 400 for failure to fetch</returns>
        [HttpGet("Types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        public async Task<ActionResult<string>> ParseModel(string token, string model)
        {
            string parsedValue = string.Empty;


            return Ok(parsedValue);
        }
    }
}
