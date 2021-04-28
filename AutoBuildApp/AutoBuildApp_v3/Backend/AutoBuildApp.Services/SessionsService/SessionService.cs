using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AutoBuildApp.Services.SessionsService
{
    public class SessionService
    {

        public SessionService()
        {

        }


        public BigInteger GenerateUniqueSessionsIdentifier()
        {
            Guid guid= Guid.NewGuid();


            string myuuidAsString = guid.ToString();

            Console.WriteLine("Your GUID is: " + myuuidAsString);


            var bint = new BigInteger(guid.ToByteArray());

            Console.WriteLine("Your GUID is: " + bint );

            BigInteger bigInt = new BigInteger(guid.ToByteArray());
            var a = bigInt.ToString();
            var b =   bigInt.GetType();

            Console.WriteLine("a : " + a);
            Console.WriteLine("b : " + b);

            return bigInt;
        }

        public Guid GenerateUniqueSessionsIdentifierGUID()
        {
            Guid guid = Guid.NewGuid();


            string myuuidAsString = guid.ToString();

            Console.WriteLine("Your GUID is: " + myuuidAsString);


            var bint = new BigInteger(guid.ToByteArray());

            Console.WriteLine("Your GUID is: " + bint);

            BigInteger bigInt = new BigInteger(guid.ToByteArray());
            var a = bigInt.ToString();
            var b = bigInt.GetType();

            Console.WriteLine("a : " + a);
            Console.WriteLine("b : " + b);

            return guid;
        }





    }
}
