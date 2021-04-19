using System;
using System.Collections.Generic;

namespace AutoBuildApp.Models.Products
{
    public static class ProductGuard
    {

        /// <summary>
        /// Checks the object exists.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="arguementName"></param>
        public static void Exists(Object input, string arguementName)
        {
            if(input == null)
            {
                throw new MissingFieldException(arguementName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="arguementName"></param>
        public static void IsNotEmpty(string input, string arguementName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(arguementName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="input"></param>
        /// <param name="arguementName"></param>
        public static void ContainsElement(List<string> list,
                            string input, string arguementName)
        {
            if (!list.Contains(input))
            {
                throw new MissingMemberException(arguementName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="arguementName"></param>
        public static void IsInRange(List<string> input, int index, string arguementName)
        {
            var endOfList = input.Count - 1;

            if (index >= ProductGlobals.MIN_INDEX
                && input.Count >= ProductGlobals.MIN_LIST_SIZE
                && index <= endOfList)
            {
                throw new IndexOutOfRangeException(arguementName);
            }


        }

    }
}
