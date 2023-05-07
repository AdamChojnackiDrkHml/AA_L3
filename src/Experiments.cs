using Microsoft.VisualBasic;

namespace AA_L3; 

public class Experiments
{
   static string path = Directory.GetParent(Environment.CurrentDirectory)!.FullName + "\\res";

   public static void Ex1()
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

   public static void Ex1b()
   {
      var qs = new[] { 0.001, 0.01, 0.1 };
      var ns = (new int[15]).Select((_, i) => i + 1).ToList();
      
      var resNakamuto = new List<List<double>>();
      var resGrunspan = new List<List<double>>();

      foreach (var q in qs)
      {
         var resNakamutoQ = new List<double>();
         var resGrunspanQ = new List<double>();
         
         foreach (var n in ns)
         {
            resNakamutoQ.Add(Nakamuto(q, n));
            resGrunspanQ.Add(Grunspan(q, n));
         }
         
         resNakamuto.Add(resNakamutoQ);
         resGrunspan.Add(resGrunspanQ);
      }
      
      for (var i = 0; i < qs.Length; i++)
      {
         var naks = resNakamuto[i];
         var gruns = resGrunspan[i].ToArray();

         var x = naks
            .Zip(gruns, (first, second) => first + " " + second)
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
                             .Select(_ =>
                             {
                                return SingleExperiment(q, n);
                             })
                             .Select(x => x ? 1.0 : 0.0)
                             .Sum() / (double)numOfTrials)
                         .ToList())
                     .ToList();
      
      
      foreach (var t in ns)
      {
         File.WriteAllText(path + "\\Ex9b" + t, string.Join("\n", res));
      }
   }

   public static bool SingleExperiment(double q, int n)
   {
      Random r = new Random();
      var threshold = 30;
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