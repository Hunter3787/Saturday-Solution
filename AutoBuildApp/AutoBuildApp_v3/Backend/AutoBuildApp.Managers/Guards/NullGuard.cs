using System;

namespace AutoBuildApp.Managers.Guards
{
    public static class NullGuard
    {
        public static void IsNotNull(Object toCheck)
        {
            if(toCheck == null)
            {
                throw new ArgumentException(nameof(toCheck));
            }
        }

        public static void IsNotNullOrEmpty(string toCheck)
        {
            if (String.IsNullOrEmpty(toCheck))
            {
                throw new ArgumentException(nameof(toCheck));
            }
        }
    }
}
