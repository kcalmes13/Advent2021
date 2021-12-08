namespace UnionWireless.Utilities.ModelParser.Api.Domain
{
    /// <summary>
    /// Request for getting properities user not authorized to
    /// </summary>
    public class RequestUnAuthedProperties
    {
        /// <summary>
        /// Identifier of the user
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Api model lives in
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        /// Method creating model
        /// </summary>
        public string Method { get; set; }
    }
}
