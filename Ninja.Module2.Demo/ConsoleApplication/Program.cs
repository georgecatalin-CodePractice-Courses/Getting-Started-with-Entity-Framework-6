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
            //QueryAndUpdateNinjaDisconnected();  //to work in disconnected situations -web sites, APIs, etc
            //RetrieveDataWithFind();
            //RetrieveDataWithSQLQuery();
            //DeleteNinja();
            //DeleteNinjaWithKeyValue();
            //DeleteNinjaWithSQLQuery();   
            //InsertNinjaWithEquipment();
            //SimpleNinjaGraphQuery();
            ProjectionQuery();
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

        private static void RetrieveDataWithFind()
        {
            var keyval = 3;
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                Ninja ninja = context.Ninjas.Find(keyval);

                Console.WriteLine("I have found this record in database "+ninja.Name);

                Ninja ninja1 = context.Ninjas.Find(keyval);
                Console.WriteLine("I have found in memory this " + ninja1.Name);
                ninja = null;
            }
        }


        private static void RetrieveDataWithSQLQuery()
        {
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas.SqlQuery("SELECT * FROM dbo.Ninjas WHERE dbo.Ninjas.Name='Ilie'");
                foreach (var item in ninjas)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }

        private static void DeleteNinja()
        {
            Ninja ninja;
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
                //context.Ninjas.Remove(ninja);
                //context.SaveChanges();
            }

            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }


        private static void DeleteNinjaWithKeyValue()
        {
            var keyval = 3;
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                Ninja ninja = context.Ninjas.Find(keyval);
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }
        
        private static void DeleteNinjaWithSQLQuery()
        {
            var keyval = 2;
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("DELETE FROM dbo.Ninjas WHERE dbo.Ninjas.Id={0}", keyval);
            }
        }


        private static void InsertNinjaWithEquipment()
        {
            using (NinjaContext context=new NinjaContext())
            {
                Ninja ninja = new Ninja()
                {
                    Name = "Elvis",
                    ServedInOniwaban = true,
                    DateOfBirth = new DateTime(1977, 07, 08),
                    ClainId = 1
                };

                NinjaEquipment fist = new NinjaEquipment()
                {
                    Name = "Fist",
                    Type = EquipmentType.Weapon
                };
                NinjaEquipment sword = new NinjaEquipment()
                {
                    Name = "Sword",
                    Type = EquipmentType.Tool
                };

                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(fist);
                ninja.EquipmentOwned.Add(sword);
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaGraphQuery()
        {
            using (NinjaContext context=new NinjaContext())
            {
                //Ninja specialNinja = context.Ninjas.Include(n=>n.EquipmentOwned).FirstOrDefault(n => n.Name.Contains("Elvis"));  //DbSet.Include() for eager Loading -fetch data in advance

                Ninja specialNinja = context.Ninjas.FirstOrDefault(n => n.Name.Contains("Elvis"));
                Console.WriteLine("I found this ninja ", specialNinja.Name);
                context.Entry(specialNinja).Collection(n => n.EquipmentOwned).Load(); // Explicit Loading
            }
        }


        private static void ProjectionQuery()
        {
            using (NinjaContext context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var query = context.Ninjas.Select(n => new { n.Name, n.ServedInOniwaban, n.EquipmentOwned }).ToList();
            }
        }

    }
}
