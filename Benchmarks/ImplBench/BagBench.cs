using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Benchmark;
using WmcSoft.Collections.Generic;

namespace ImplBench
{
    [Benchmark(Iterations = 3000)]
    public class BagBench
    {
        const int Count = 4096;
        static List<int>[] lists;
        static Bag<int>[] bags;

        static int Values;

        public static void Init(string[] args) {
            lists = new List<int>[Count];
            bags = new Bag<int>[Count];

            var random = new Random(1664);
            for (int i = 0; i < Count; i++) {
                var count = random.Next(1, 1200);
                var list = new List<int>(count);
                while (count-- > 0)
                    list.Add(random.Next(16 * i, 256 * i));
                lists[i] = new List<int>(list);
                bags[i] = new Bag<int>(list);
            }
        }

        public static void Reset() {
            Values = 0;
        }

        [Measure]
        public static void MeasureForEachOnList() {
            int sum = 0;
            for (int n = 0; n < Count; n++) {
                foreach (var value in lists[n]) {
                    sum += value;
                }
            }
            Values = sum;
        }

        [Measure]
        public static void MeasureForEachOnBag() {
            int sum = 0;
            for (int n = 0; n < Count; n++) {
                foreach (var value in bags[n]) {
                    sum += value;
                }
            }
            Values = sum;
        }
    }
}
