using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace quicksort
{
    class Program
    {
        public static int Instances = int.MaxValue;
        static void Main(string[] args)
        {
            Random r = new();
            List<int> test = new List<int>();
            List<int> test2 = new List<int>();
            for(int i = 0; i < 20; i++)
            {
                test.Add(r.Next(1000));
                test2.Add(r.Next(1000)+1);
            }
            test = sort(test);
            foreach (int i in test) Console.Write(" " + i);
            Console.WriteLine();
            test2 = SortAsync(test2,4).Result;
            Console.WriteLine();
            foreach (int i in test2) Console.Write(" " + i);
            try
            {
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Source);
                Console.WriteLine(Instances);
            }
        }

        public async static Task<List<int>> SortAsync(List<int> unsorted,int instance = 2)
        {
            if(Instances == int.MaxValue)
            {
                Instances = instance;
            }
            if (unsorted.Count < 1)
            {
                return unsorted;
            }
            Random r = new();
            List<int> high = new List<int>();
            List<int> low = new List<int>();
            List<int> piv = new List<int>();
            int pi = await Task.FromResult(unsorted[r.Next(unsorted.Count - 1)]);
            List<int> maybesorted = new List<int>();
            Task<List<int>>[] tasks = new Task<List<int>>[2];
            foreach (int i in unsorted)
            {
                if (i == pi)
                {
                    piv.Insert(piv.Count, i);
                }
                else if (i > pi)
                {
                    high.Insert(high.Count, i);
                }
                else
                {
                    low.Insert(low.Count, i);
                }
            }
            maybesorted = await Task.Run(() => RecreateArray(low,piv,high));
            if (low == null || piv == null || high == null) return await Task.Run(() => RecreateArray(low, piv, high));
            for (int i = 1; i < maybesorted.Count; await Task.Run(() => i++))
            {

                if (maybesorted[i - 1] > maybesorted[i])
                {
                    if (low != null && Instances >= 1)
                    {
                        try
                        {
                            tasks[0] = Task.Run(() => SortAsync(low, --Instances));
                        }catch(Exception e)
                        {
                            return maybesorted;
                        }
                    }
                    else
                    {
                        tasks[0] = Task.Run(() => sort(low));
                    }
                    if (high != null && instance >= 1)
                    {
                        try
                        {
                            tasks[1] = Task.Run(() => SortAsync(high, --instance));
                        }catch(Exception e)
                        {
                            return maybesorted;
                        }
                    }
                    else
                    {
                        tasks[1] = Task.Run(() => sort(high));
                    }
                    break;
                }

            }
            try
            {
                var results = await Task.WhenAll(tasks.Where(t => t is not null));
                return await Task.Run(()=> RecreateArray(results[0], piv, results[1]));
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return await Task.Run(() => RecreateArray(low, piv, high));
            }
        }
        
        public static List<int> RecreateArray(List<int> first,List<int> middle, List<int> last)
        {
            List<int> result = new List<int>();
            foreach (int i in first)
            {
                result.Insert(result.Count, i);
            }
            foreach (int i in middle)
            {
                result.Insert(result.Count, i);
            }
            foreach (int i in last)
            {
                result.Insert(result.Count, i);
            }
            return result;
        }

        public static List<int> sort(List<int> unsorted)
        {
            if (unsorted.Count < 1)
            {
                return unsorted;
            }
            Random r = new();
            List<int> high = new List<int>();
            List<int> low = new List<int>();
            List<int> piv = new List<int>();
            int pi = unsorted[r.Next(unsorted.Count - 1)];
            List<int> maybesorted = new List<int>();
            foreach (int i in unsorted)
            {
                if (i == pi)
                {
                    piv.Insert(piv.Count, i);
                }
                else if (i > pi)
                {
                    high.Insert(high.Count, i);
                }
                else
                {
                    low.Insert(low.Count, i);
                }
            }
            foreach (int i in low)
            {
                maybesorted.Insert(maybesorted.Count, i);
            }
            foreach (int i in piv)
            {
                maybesorted.Insert(maybesorted.Count, i);
            }
            foreach (int i in high)
            {
                maybesorted.Insert(maybesorted.Count, i);
            }
            for (int i = 1; i < maybesorted.Count; i++)
            {

                if (maybesorted[i - 1] > maybesorted[i])
                {
                    low = sort(low);
                    high = sort(high);
                    break;
                }
            }
            List<int> sorted = RecreateArray(low,piv,high);
            return sorted;
        }
        //public async static Task<List<int>> SortAsync(List<int> unsorted)
        //{
        //    if(await Task.FromResult(unsorted.Count) < 1 || unsorted == null)
        //    {
        //        return await Task.FromResult(unsorted);
        //    }
        //    return await Task.FromResult(sort(unsorted).Result);
        //}
    }
}
