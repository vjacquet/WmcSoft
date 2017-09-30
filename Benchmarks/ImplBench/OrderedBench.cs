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
    [Benchmark(Iterations = 1000)]
    public class OrderedBench
    {
        public struct NaiveOrdered
        {
            private readonly int _storage;

            public NaiveOrdered(int value)
            {
                _storage = value;
            }

            #region Operators

            public static implicit operator NaiveOrdered(int x)
            {
                return new NaiveOrdered(x);
            }

            public static implicit operator int(NaiveOrdered x)
            {
                return x._storage;
            }

            public static bool operator true(NaiveOrdered x)
            {
                return x._storage != 0;
            }

            public static bool operator false(NaiveOrdered x)
            {
                return x._storage == 0;
            }

            public static NaiveOrdered operator |(NaiveOrdered x, NaiveOrdered y)
            {
                if (x._storage != 0)
                    return x._storage;
                return y._storage;
            }

            #endregion

        }

        public struct Ordered
        {
            private readonly int _storage;

            public Ordered(int value)
            {
                _storage = value;
            }

            #region Operators

            public static implicit operator Ordered(int x)
            {
                return new Ordered(x);
            }

            public static implicit operator int(Ordered x)
            {
                return x._storage;
            }

            public static bool operator true(Ordered x)
            {
                return x._storage != 0;
            }

            public static bool operator false(Ordered x)
            {
                return x._storage == 0;
            }

            public static Ordered operator |(Ordered x, Ordered y)
            {
                //if (x._storage != 0)
                //    return x._storage;
                return y._storage;
            }

            #endregion

        }

        struct OrderedComparer : IComparer<Record>
        {
            public int Compare(Record x, Record y)
            {
                return (Ordered)x.Name.CompareTo(y.Name)
                    || x.DateOfBirth.CompareTo(y.DateOfBirth)
                    || x.PayRate.CompareTo(y.PayRate);
            }
        }

        struct NaiveOrderedComparer : IComparer<Record>
        {
            public int Compare(Record x, Record y)
            {
                return (NaiveOrdered)x.Name.CompareTo(y.Name)
                    || x.DateOfBirth.CompareTo(y.DateOfBirth)
                    || x.PayRate.CompareTo(y.PayRate);
            }
        }

        struct ClassicComparer : IComparer<Record>
        {
            public int Compare(Record x, Record y)
            {
                var result = x.Name.CompareTo(y.Name);
                if (result != 0)
                    return result;
                result = x.DateOfBirth.CompareTo(y.DateOfBirth);
                if (result != 0)
                    return result;
                result = x.PayRate.CompareTo(y.PayRate);
                return result;
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
        static List<Record> records;

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

            records = new List<Record>(book);
        }

        public static void Reset()
        {
            records = new List<Record>(book);
        }

        [Measure]
        public static void MeasureSortWithClassicComparer()
        {
            records.Sort(new ClassicComparer());
        }

        [Measure]
        public static void MeasureSortWithNaiveOrderedComparer()
        {
            records.Sort(new NaiveOrderedComparer());
        }

        [Measure]
        public static void MeasureSortWithOrderedComparer()
        {
            records.Sort(new OrderedComparer());
        }
    }
}
