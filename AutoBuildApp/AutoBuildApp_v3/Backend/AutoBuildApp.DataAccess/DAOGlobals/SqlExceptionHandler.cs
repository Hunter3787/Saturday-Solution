using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess
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

                case 137:
                    return AutoBuildSystemCodes.UndeclaredVariable;
                default:
                    return AutoBuildSystemCodes.DefaultError;
            }
        }
    }
}
