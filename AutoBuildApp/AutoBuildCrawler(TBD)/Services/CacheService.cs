using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services
{
    public sealed class CacheService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> vendorsProducts;
        private static CacheService _instance = null; // Initializes the logger object to zero, it has not been called yet.
        private static readonly object _lock = new object();
        public int f = 0;
        public static CacheService GetInstance
        {
            get
            {
                // there is a lock taken on the shared object
                /**
                 * in our case we are implementing a "thread 
                 * saftey singleton" by locking the thread in the shared 
                 * logger object. this takes care of the case if 
                 * two different thread both checked " instance == null" 
                 * and the result for both showed as true causing them both
                 * to create instances, THIS VIOLATES THE SINGLETON PATTERn!!
                 * 
                 * now at to address that^ is with the use of a lock, 
                 * problem with this is that it takes a hit on performance
                 * because of the lock required each time...
                 * 
                 */

                lock (_lock)
                {
                    // then checks whether an instance has been created before 
                    // creating the instance.
                    // If there is no instance of logger, then a new one will be creates, and only one.
                    if (_instance == null)
                    {
                        _instance = new CacheService();
                    }

                    return _instance;
                }
            }
        }

        public void Do()
        {
            lock (_lock)
            {
                f++;
                Console.WriteLine(f);
            }
        }
    }
}
