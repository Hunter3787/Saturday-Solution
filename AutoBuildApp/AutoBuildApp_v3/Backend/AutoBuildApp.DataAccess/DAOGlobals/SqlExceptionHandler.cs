using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models
{
    public static class SqlExceptionHandler
    {
        public static AutoBuildSystemCodes GetCode(int exceptionNumber)
        {
            switch (exceptionNumber)
            {
                case -2:
                    return AutoBuildSystemCodes.DatabaseTimeout;

                case 2627:
                    return AutoBuildSystemCodes.DuplicateValue;

                default:
                    return AutoBuildSystemCodes.DefaultError;
            }
        }
    }
}
