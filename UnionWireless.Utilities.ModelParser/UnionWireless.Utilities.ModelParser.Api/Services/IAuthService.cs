using System.Collections.Generic;
using System.Threading.Tasks;
using UnionWireless.Utilities.ModelParser.Api.Domain;

namespace UnionWireless.Utilities.ModelParser.Api.Services
{
    /// <summary>
    /// Service for interacting with auth util
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Method for getting properties users are unauthorized to
        /// </summary>
        /// <param name="request">request with information for user and model to check</param>
        /// <returns>Success with unauthorized properties or error</returns>
        Task<Response<List<string>>> GetUnAuthedProperties(RequestUnAuthedProperties request);
    }
}
