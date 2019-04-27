using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LINQJoining
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            var query = cars.Join(manufacturers,
                                    c => c.Manufacturer,
                                    m => m.Name, (c, m) => new
                                    {
                                        m.Headquarters,
                                        c.Name,
                                        c.Combined
                                    })
                                .OrderByDescending(c => c.Combined)
                                .ThenBy(c => c.Name);




            foreach (var item in query.Take(10))
            {
                Console.WriteLine($"{item.Headquarters} {item.Name} {item.Combined}");
            }

            Console.ReadLine();
        }

        private static List<Car> ProcessFile(string path)
        {
            return
                 File.ReadAllLines(path)
                  .Skip(1)
                  .Where(line => line.Length > 1)
                  .Select(Car.ParseFromCSV).ToList();

        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = 
                File.ReadAllLines(path)
                    .Where(l => l.Length > 1)
                    .Select(l =>
                    {
                        var columns = l.Split(',');
                        return new Manufacturer
                        {
                            Name = columns[0],
                            Headquarters = columns[1],
                            Year = int.Parse(columns[2])
                        };
                    });
            return query.ToList();
        }
    }
}
