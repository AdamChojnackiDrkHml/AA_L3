using Microsoft.VisualBasic;

namespace AA_L3; 

public class Experiments
{
   static string path = Directory.GetParent(Environment.CurrentDirectory)!.FullName + "\\res";
   static Random r = new();
   public static void Ex9aa()
   {
      var ns = new[] {1, 3, 6, 12, 24, 48};
      var qsCount = (int)(0.5 / 0.02);
      var qs = (new int[qsCount]).Select((_, i) => (double)(i+1) * 0.02).ToList();
      var resNakamuto = new List<List<double>>();
      var resGrunspan = new List<List<double>>();

      foreach (var n in ns)
      {
         var resNakamutoQ = new List<double>();
         var resGrunspanQ = new List<double>();
         
         foreach (var q in qs)
         {
            resNakamutoQ.Add(Nakamuto(q, n));
            resGrunspanQ.Add(Grunspan(q, n));
         }
         
         resNakamuto.Add(resNakamutoQ);
         resGrunspan.Add(resGrunspanQ);
      }


      for (var i = 0; i < ns.Length; i++)
      {
         var naks = resNakamuto[i];
         var gruns = resGrunspan[i].ToArray();

         var x = naks
            .Zip(gruns, (first, second) => first + " " + second)
            .ToArray();
         
         File.WriteAllText(path + "\\Ex9aa" + ns[i], Strings.Join(x, "\n"));
      }


   }

   public static void Ex9ab()
   {
      const int qsCount = (int)(0.5 / 0.02);
      var qs = (new int[qsCount]).Select((_, i) => (double)(i+1) * 0.02).ToList();
      var Ps = new[] { 0.001, 0.01, 0.1 };
      
      var resNakamuto = new List<List<int>>();
      var resGrunspan = new List<List<int>>();
      var resMonteCarlo = new List<List<int>>();
      
      foreach (var p in Ps)
      {
         var resN = new List<int>();
         var resG = new List<int>();
         var resMC = new List<int>();
         foreach (var q in qs)
         {
            var low = 0;
            var high = 200;

            var mid = high / 2;

            while (low < high)
            {
               mid = (low + high) / 2;

               var res = Nakamuto(q, mid);


               if (res <= p)
               {
                  high = mid;
               }
               else
               {
                  low = mid + 1;
               }

            }
            resN.Add(mid);
            
            low = 0;
            high = 200;

            mid = high / 2;

            while (low < high)
            {
               mid = (low + high) / 2;

               var res = Grunspan(q, mid);


               if (res <= p)
               {
                  high = mid;
               }
               else
               {
                  low = mid + 1;
               }

            }
            resG.Add(mid);
            
            low = 0;
            high = 200;

            mid = high / 2;

            while (low < high)
            {
               mid = (low + high) / 2;

               var res = SingleMonteCarlo(q, mid);


               if (res <= p)
               {
                  high = mid;
               }
               else
               {
                  low = mid + 1;
               }

            }
            resMC.Add(mid);
         }
         
         resNakamuto.Add(resN);
         resGrunspan.Add(resG);
         resMonteCarlo.Add(resMC);
      }
      
      for (var i = 0; i < Ps.Length; i++)
      {
         var naks = resNakamuto[i].ToArray();
         var gruns = resGrunspan[i].ToArray();
         var mc = resMonteCarlo[i].ToArray();
      
         var x = naks
            .Zip(gruns, (first, second) => first + " " + second)
            .Zip(mc, (first, second) => first + " " + second)
            .ToArray();
         
         File.WriteAllText(path + "\\Ex9ab" + i, Strings.Join(x, "\n"));
      }
   }

   public static void Ex9b()
   {
      const int numOfTrials = 10_000;
      var qs = Enumerable.Range(1, 25).Select(i => 0.02 * i).ToList();
      var ns = new[] {1, 3, 6, 12, 24, 48};
      var res = ns.Select(n => qs.Select(q => Enumerable.Range(0, numOfTrials)
                             .Select(_ => SingleExperiment(q, n))
                             .Select(x => x ? 1.0 : 0.0)
                             .Sum() / (double)numOfTrials)
                         .ToList())
                     .ToList();
      
      
      for (var i = 0; i < ns.Length; i++)
      {
         File.WriteAllText(path + "\\Ex9b" + ns[i], string.Join("\n", res[i]));
      }
   }

   public static double SingleMonteCarlo(double q, int n)
   {
      const int numOfTrials = 10_000;
      return Enumerable.Range(0, numOfTrials)
            .Select(_ => SingleExperiment(q, n))
            .Select(x => x ? 1.0 : 0.0)
            .Sum() / (double)numOfTrials;
   }

   public static bool SingleExperiment(double q, int n)
   {
      
      var threshold = 100;
      var legitimateBranch = 0;
      var adversaryBranch = 0;

      while (true)
      {
         if (r.NextDouble() <= q)
         {
            adversaryBranch++;
         }
         else
         {
            legitimateBranch++;
         }

         if (legitimateBranch < n)
         {
            continue;
         }

         if (adversaryBranch >= legitimateBranch)
         {
            return true;
         }
         
         if (legitimateBranch >= n + threshold)
         {
            return false;
         }
      }
   }
   private static double Grunspan(double q, int n)
   {
      var p = 1.0 - q;
      var sum = 1.0;
      var pn = Math.Pow(p, (double)n);
      var qn = Math.Pow(q, (double)n);
      var pk = 1.0;
      var qk = 1.0;
      var newton = 1.0;
      for (var k = 0; k < n; k++)
      {
         sum -= (pn * qk - qn * pk) * newton; 
         newton = newton * (double)(k + n) / (double)(k+1);
         qk *= q;
         pk *= p;
      }

      return sum;
   }

   private static double Nakamuto(double q, int n)
   {
      var p = 1.0 - q;
      var lambda = (double)n * (q / p);
      var expMinusLambda = Math.Exp(-lambda);
      var currFact = 1.0;

      var sum = 1.0;

      for (var k = 0; k < n; k++)
      {
         if (k != 0)
         {
            currFact *= (double)k;
         }
         var temp1 = Math.Pow(lambda, k) * expMinusLambda / currFact;
         
         var temp3 = Math.Pow(q / p, n - k);
         sum -= temp1 * (1.0 - temp3);
      }

      return sum;
   }
}