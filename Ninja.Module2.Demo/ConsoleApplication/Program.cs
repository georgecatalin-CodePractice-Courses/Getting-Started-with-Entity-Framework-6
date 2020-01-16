using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertNinja();
            InsertMultipleNinjas();
        }

        private static void InsertNinja()
        {
            Ninja ninja = new Ninja()
            {
                Name = "Alexey",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1980, 10, 13),
                ClainId = 1
            };

            using (NinjaContext context=new NinjaContext())
            {
                Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());  //avoid the initialization of the database in case database already exists in production
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }

        private static void InsertMultipleNinjas()
        {
            Ninja ninja1 = new Ninja()
            {
                Name = "Axente",
                ServedInOniwaban = true,
                DateOfBirth = new DateTime(1984, 07, 13),
                ClainId = 1
            };

            Ninja ninja2 = new Ninja()
            {
                Name = "Ilie",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1987, 02, 12),
                ClainId = 1
            };

            using (NinjaContext context=new NinjaContext())
            {
                Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());   //avoid the initialization of the database in case database already exists in production
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja>() { ninja1, ninja2 });
                context.SaveChanges();
            
            }
        }
    }
}
