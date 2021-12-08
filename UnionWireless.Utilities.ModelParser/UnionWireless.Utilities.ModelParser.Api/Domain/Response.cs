using System.Collections.Generic;
using System.Linq;
#nullable enable

namespace UnionWireless.Utilities.ModelParser.Api.Domain
{
    /// <summary>
    /// Object to contain additional details for request / response pattern
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Get or set the status of the response
        /// </summary>
        public Status Status { get; protected init; }

        /// <summary>
        /// Get or set the messages for the response. Nullable
        /// </summary>
        public IList<string> Messages { get; protected init; } = new List<string>();

        /// <summary>
        /// Get or set the error list for the response. Nullable
        /// </summary>
        public IList<string> Errors { get; protected init; } = new List<string>();

        /// <summary>
        /// Convenience method to create a success response object
        /// </summary>
        public static Response Success(params string[] messages) => new()
        {
            Status = Status.Success,
            Messages = messages
        };

        /// <summary>
        /// Convenience method to create a success response object
        /// </summary>
        public static Response Success(IEnumerable<string>? messages = null) => new()
        {
            Status = Status.Success,
            Messages = messages?.ToList() ?? new List<string>()
        };

        /// <summary>
        /// Convenience method to create a success response object with optional messages
        /// </summary>
        public static Response<T> Success<T>(T result, params string[] messages) => new()
        {
            Status = Status.Success,
            Result = result,
            Messages = messages
        };

        /// <summary>
        /// Convenience method to create a success response object with optional messages
        /// </summary>
        public static Response<T> Success<T>(T result, IEnumerable<string>? messages = null) => new()
        {
            Status = Status.Success,
            Result = result,
            Messages = messages?.ToList() ?? new List<string>()
        };

        /// <summary>
        /// Convenience method to create error response object
        /// </summary>
        /// <param name="errors"></param>
        public static Response Error(params string[] errors) => new()
        {
            Status = Status.Error,
            Errors = errors
        };

        /// <summary>
        /// Convenience method to create error response object
        /// </summary>
        /// <param name="errors"></param>
        public static Response Error(IEnumerable<string> errors) => new()
        {
            Status = Status.Error,
            Errors = errors.ToList()
        };

        /// <summary>
        /// Convenience method to create error response object
        /// </summary>
        /// <param name="errors">Vararg errors to be added to the error response</param>
        public static Response<T> Error<T>(params string[] errors) => new()
        {
            Status = Status.Error,
            Errors = errors
        };

        /// <summary>
        /// Convenience method to create error response object
        /// </summary>
        /// <param name="errors"></param>
        public static Response<T> Error<T>(IEnumerable<string> errors) => new()
        {
            Status = Status.Error,
            Errors = errors.ToList()
        };

        /// <summary>
        /// Convenience method to create an error response object with optional messages
        /// </summary>
        public static Response<T> Error<T>(T result, params string[] errors) => new()
        {
            Status = Status.Error,
            Result = result,
            Errors = errors
        };

        /// <summary>
        /// Convenience method to create an error response object with optional errors
        /// </summary>
        public static Response<T> Error<T>(T result, IEnumerable<string>? errors = null) => new()
        {
            Status = Status.Error,
            Result = result,
            Errors = errors?.ToList() ?? new List<string>()
        };
    }

    /// <summary>
    /// Object to contain additional details for request / response pattern with a generified Result
    /// </summary>
    public class Response<T> : Response
    {
        /// <summary>
        /// Get or set the resulting object for this response
        /// </summary>
        public T Result { get; internal init; } = default!;

        /// <summary>
        /// Convenience method to create a success response object with optional messages
        /// </summary>
        public static Response<T> Success(T result, params string[] messages) => new()
        {
            Status = Status.Success,
            Result = result,
            Messages = messages
        };

        /// <summary>
        /// Convenience method to create a success response object with optional messages
        /// </summary>
        public static Response<T> Success(T result, IEnumerable<string>? messages = null) => new()
        {
            Status = Status.Success,
            Result = result,
            Messages = messages?.ToList() ?? new List<string>()
        };

        /// <summary>
        /// Convenience method to create error response object
        /// </summary>
        /// <param name="errors">Vararg errors to be added to the error response</param>
        public new static Response<T> Error(params string[] errors) => new()
        {
            Status = Status.Error,
            Errors = errors
        };

        /// <summary>
        /// Convenience method to create error response object
        /// </summary>
        /// <param name="errors"></param>
        public new static Response<T> Error(IEnumerable<string> errors) => new()
        {
            Status = Status.Error,
            Errors = errors.ToList()
        };

        /// <summary>
        /// Convenience method to create an error response object with optional messages
        /// </summary>
        public static Response<T> Error(T result, params string[] errors) => new()
        {
            Status = Status.Error,
            Result = result,
            Errors = errors
        };

        /// <summary>
        /// Convenience method to create an error response object with optional messages
        /// </summary>
        public static Response<T> Error(T result, IEnumerable<string>? errors = null) => new()
        {
            Status = Status.Error,
            Result = result,
            Errors = errors?.ToList() ?? new List<string>()
        };
    }

    /// <summary>
    /// Status types for the response object
    /// </summary>
    public enum Status
    {
        Success,
        Error
    }
}