using System;
namespace AutoBuildApp.Models.Builds
{
    public static class BuildGuards
    {
        /// <summary>
        /// Checks the object exists.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="arguementName"></param>
        public static void Exists(Object input, string arguementName)
        {
            if (input == null)
            {
                throw new MissingFieldException(arguementName);
            }
        }



    }
}
