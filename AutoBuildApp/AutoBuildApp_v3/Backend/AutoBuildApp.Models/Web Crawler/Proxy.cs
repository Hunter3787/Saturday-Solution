﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Web_Crawler
{
    public class Proxy
    {
        public string IPAddress { get; private set; }
        public int Port { get; private set; }

        public string Country { get; private set; }

        public Proxy(string ipAddress, int port, string country)
        {
            IPAddress = ipAddress;
            Port = port;
            Country = country;
        }

        public override int GetHashCode()
        {
            int hash = 19;
            hash = hash * 29 + IPAddress.GetHashCode();
            hash = hash * 29 + Port.GetHashCode();
            hash = hash * 29 + Country.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return obj is Proxy && Equals((Proxy)obj);
        }

        public bool Equals(Proxy p)
        {
            return p.IPAddress.Equals(IPAddress) && p.Port == Port && p.Country.Equals(Country);
        }
    }
}
