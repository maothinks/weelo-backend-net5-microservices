using Microsoft.Extensions.Configuration;
using System;
using Weelo.Microservices.Properties.Infrastructure.Data.Contexts;

namespace Weelo.Microservices.Properties.Infrastructure.Data
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating Database WeeloDBProperties if it does not exists");
            PropertyContext db = new ();
            db.Database.EnsureCreated();
            Console.WriteLine("Ready!!");
            Console.ReadKey();
        }
    }
}
