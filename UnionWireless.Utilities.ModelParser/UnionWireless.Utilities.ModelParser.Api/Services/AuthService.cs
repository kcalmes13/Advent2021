using System.Collections.Generic;
using System.Threading.Tasks;
using UnionWireless.Utilities.Logging.Services;
using UnionWireless.Utilities.ModelParser.Api.Domain;

namespace UnionWireless.Utilities.ModelParser.Api.Services
{
    /// <inheritdoc />
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Interface for interacting with logging util
        /// </summary>
        private readonly ILog _logging;

        /// <summary>
        /// Constructor for auth service
        /// </summary>
        /// <param name="logging">injecting logging utility</param>
        public AuthService(ILog logging)
        {
            _logging = logging;
        }

        /// <inheritdoc />
        public Task<Response<List<string>>> GetUnAuthedProperties(RequestUnAuthedProperties request)
        {
            throw new System.NotImplementedException();
        }
    }
}
