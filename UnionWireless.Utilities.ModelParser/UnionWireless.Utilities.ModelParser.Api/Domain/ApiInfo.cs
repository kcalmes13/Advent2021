namespace UnionWireless.Utilities.ModelParser.Api.Domain
{
    /// <summary>
    /// Model holding api data
    /// </summary>
    public class ApiInfo
    {
        /// <summary>
        /// Group name of api
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// name of api
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of api
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Version of api
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Indicator on if api is depricated
        /// </summary>
        public bool Depricated { get; set; }
    }
}