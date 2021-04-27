using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{/// <summary>
 /// created a common respose object inheritance 
 /// </summary>
     class CommonResponse
    {

        /// <summary>
        /// the success string represents a success message
        /// upon success bool =  true
        /// </summary>
        string SuccessString { get; set; }
        /// <summary>
        /// the failure string corresponds to a failure message in the case of Success bool is false
        /// </summary>
        string FailureString { get; set; }
        /// <summary>
        /// a bool value to check incoming object if it succeeded or failed 
        /// to cast as necessary 
        /// </summary>
        public bool SuccessBool { get; set; }

        /// <summary>
        /// the response sttring encompases both success or fail string.
        /// </summary>
        public string ResponseString { get; set; }


    }
}
