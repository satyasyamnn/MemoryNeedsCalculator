using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestMemoryAllocation
{
    // 10L for GL
    // 10L for Inventory
    // UCC3 for Discrepancy

    [Serializable]
    public struct Ucc6Data
    {
        public string Source { get; set; }

        public string BusinessUnit { get; set; }

        public string E2KAccount { get; set; }

        public string Currency { get; set; }

        public string Department { get; set; }

        public string Affiliate { get; set; }

        public string ObjectCode { get; set; }

        public Decimal TransactionAmount { get; set; }

        public DateTime RunDate { get; set; }

        public DateTime InventoryDate { get; set; }
    }

    class Program
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void Main(string[] args)
        {
            string ret = "";
            var objectCounts = new long[] { 4_000_000L };
            foreach (var objectCount in objectCounts)
            {
                var testStrings = new string[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ" };
                foreach (var teststring in testStrings)
                {
                    if (true)
                    {
                        GC.GetTotalMemory(true);
                        long mem = GC.GetAllocatedBytesForCurrentThread();
                        var collection = new Dictionary<int, Ucc6Data>();
                        for (int i = 0; i < objectCount; i++)
                        {
                            Ucc6Data data = new Ucc6Data()
                            {
                                Affiliate = teststring + i, //RandomString(50) + i,
                                BusinessUnit = teststring + i, //RandomString(50) + i,
                                Currency = teststring + i, //RandomString(50) + i,
                                Department = teststring + i, //RandomString(50) + i,
                                E2KAccount = teststring + i, //RandomString(50) + i,
                                ObjectCode = teststring + i, //RandomString(50) + i,
                                InventoryDate = DateTime.Now,
                                RunDate = DateTime.Now,
                                Source = "GL",
                                TransactionAmount = 12345678901234 + i
                            };
                            collection.Add(i, data);
                        }
                        long totalmem = GC.GetAllocatedBytesForCurrentThread() - mem;
                        Console.WriteLine(collection.Count);
                        Console.Write(ret += $"Dictionary<int, Ucc6Data>: ".PadRight(50, ' ') + $"{totalmem.ToString("N0").PadLeft(20, ' ')} B\n");//{Size(collection).ToString("N0").PadLeft(20,' ')} B 
                    }
                }
            }
            Console.Write(ret += "Press any key to exit");
            File.WriteAllText("out.txt", ret);
            Console.ReadLine();
        }
    }
}