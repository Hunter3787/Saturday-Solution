using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models
{
    /// <summary>
    /// Response object that carries a boolean and a string.
    /// </summary>
    public class StringBoolResponse : IMessageResponse
    {
        /// <summary>
        /// Message response.
        /// </summary>
        public string MessageString { get ; set; }

        /// <summary>
        /// Bool response.
        /// </summary>
        public bool SuccessBool { get; set; }
    }
}
