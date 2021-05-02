
namespace AutoBuildApp.DataAccess.Abstractions
{
    /// <summary>
    /// created a common respose object inheritance 
    /// </summary>
    public interface ICommonResponse
    {

        /// <summary>
        /// the success string represents a success message
        /// upon success bool =  true
        /// </summary>
        string ResponseString { get; set; }

        /// <summary>
        /// a bool value to check incoming object if it succeeded or failed 
        /// to cast as necessary 
        /// </summary>
        bool ResponseBool { get; set; }


    }
}