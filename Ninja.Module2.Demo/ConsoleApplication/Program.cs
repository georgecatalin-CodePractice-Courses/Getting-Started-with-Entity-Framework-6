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
            //InsertNinja();
            //InsertMultipleNinjas();
            //SimpleNinjaQueries();
            //QueryAndUpdateNinja();
            QueryAndUpdateNinjaDisconnected();  //to work in disconnected situations -web sites, APIs, etc
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

        private static void SimpleNinjaQueries()
        { 
            using (NinjaContext context=new NinjaContext())
            {
                //var ninjas=context.Ninjas.Where(n=>n.DateOfBirth>=new DateTime(1980,1,1)).OrderBy(n=>n.Name).ToList();

                var ninjas = from ninja in context.Ninjas
                             where ninja.ServedInOniwaban == false
                             orderby ninja.Id ascending
                             select ninja;

                foreach (Ninja item in ninjas)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.Name = "Giorgio";
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.ServedInOniwaban = !ninja.ServedInOniwaban;

            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
