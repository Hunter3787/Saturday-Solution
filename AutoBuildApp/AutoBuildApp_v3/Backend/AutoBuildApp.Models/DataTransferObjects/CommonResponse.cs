using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{/// <summary>
 /// created a common respose object inheritance 
 /// </summary>
     public class CommonResponse
     {

        /// <summary>
        /// a bool value to check incoming object if it succeeded or failed 
        /// to cast as necessary 
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// the response sttring encompases both success or fail string.
        /// </summary>
        public string ResponseString { get; set; }


     }
}
