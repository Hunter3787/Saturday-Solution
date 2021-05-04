using System;
namespace AutoBuildApp.Models.Interfaces
{
    /// <summary>
    /// A variation of commmon response with a message string and a bool.
    /// </summary>
    public interface IMessageResponse
    {
        /// <summary>
        /// Message string that carries the message response.
        /// </summary>
        string MessageString { get; set; }

        /// <summary>
        /// The boolean value representing the success value.
        /// </summary>
        bool SuccessBool { get; set; }
    }
}
