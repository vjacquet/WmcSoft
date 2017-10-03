using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Benchmark;
using WmcSoft.Collections.Generic;

namespace ImplBench
{
    [Benchmark(Iterations = 5000)]
    public class HasherBench
    {
        struct HasherComparer : IEqualityComparer<Record>
        {
            public bool Equals(Record x, Record y)
            {
                return x.Name == y.Name
                    && x.DateOfBirth == y.DateOfBirth
                    && x.PayRate == y.PayRate;
            }

            public int GetHashCode(Record obj)
            {
                return (Hasher)obj.Name
                    ^ obj.DateOfBirth
                    ^ obj.PayRate;
            }
        }

        struct ClassicComparer : IEqualityComparer<Record>
        {
            public bool Equals(Record x, Record y)
            {
                return x.Name == y.Name
                    && x.DateOfBirth == y.DateOfBirth
                    && x.PayRate == y.PayRate;
            }

            public int GetHashCode(Record obj)
            {
                int h = obj.Name == null ? 0 : obj.Name.GetHashCode();
                h = ((h << 5) + h) ^ obj.DateOfBirth.GetHashCode();
                h = ((h << 5) + h) ^ obj.PayRate.GetHashCode();
                return h;
            }
        }

        struct CombineComparer : IEqualityComparer<Record>
        {
            public bool Equals(Record x, Record y)
            {
                return x.Name == y.Name
                    && x.DateOfBirth == y.DateOfBirth
                    && x.PayRate == y.PayRate;
            }

            public int GetHashCode(Record obj)
            {
                return EqualityComparer.CombineHashCodes(obj.Name == null ? 0 : obj.Name.GetHashCode()
                      , obj.DateOfBirth.GetHashCode()
                      , obj.PayRate.GetHashCode());
            }
        }

        static readonly string[] FirstNames = { "John", "Jane", "Mickael", "Paul", "Arthur", "Marc", "Vincent", "Sandrine", "Alicia", "Mary", "Benedict", "Bruno", "Barbara" };
        static readonly string[] LastNames = { "Doe", "Dupont", "Martin", "Lewis", "Perez", "Walker", "King", "Wright", "Green", "Davis", "Anderson", "Parker", "Campbell", "Flores", "Morris" };

        class Record
        {
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
            public decimal PayRate { get; set; }
        }

        const int Count = 8192;
        static List<Record> book;
        static HashSet<Record> records;

        public static void Init(string[] args)
        {
            book = new List<Record>(Count);

            var epoch = new DateTime(1973, 05, 02);
            var payRate = 10m;
            var random = new Random(1664);
            var half = Count / 2;
            for (int i = 0; i < half; i++) {
                book.Add(new Record {
                    Name = FirstNames[random.Next(FirstNames.Length)] + ' ' + LastNames[random.Next(LastNames.Length)],
                    DateOfBirth = epoch.AddDays(random.Next(365)),
                    PayRate = payRate + 0.01m * random.Next(1000),
                });
            }
            book.AddRange(book); // ensure collision.
        }

        public static void Reset()
        {
            records = null;
        }

        [Measure]
        public static void MeasureHashWithClassicComparer()
        {
            records = new HashSet<Record>(book, new ClassicComparer());
        }

        [Measure]
        public static void MeasureSortWithCombineComparer()
        {
            records = new HashSet<Record>(book, new CombineComparer());
        }

        [Measure]
        public static void MeasureSortWithHasherComparer()
        {
            records = new HashSet<Record>(book, new HasherComparer());
        }
    }
}
