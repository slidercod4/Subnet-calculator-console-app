using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HalozatBeadando
{

    class Program
    {

        static List<Halozat> alhalozatok = new List<Halozat>();

        static void Main(string[] args)
        {
            string[] cimMaszk = HalozatCimBeker().Split();
            string kiinduloCim = Halozat.BinaryToDec(Halozat.CheckIfNetwork(cimMaszk[0], cimMaszk[1]));
            Console.WriteLine($"A kiinduló hálózati cím {kiinduloCim}");
            int alhalozatokSzama = GetDbBeker("Adja meg az alhálózatok darabszámát: ");
            AlhalozatokBeker(alhalozatokSzama, kiinduloCim);
            HalozatokKiir();
            CreateFile();
        }

        static void CreateFile()
        {
            StreamWriter sw = new StreamWriter("outputResult.csv", false, Encoding.UTF8);
            sw.WriteLine("Hálózat neve;Hálózati cím;Alhálóhazati maszkja;Szórási címe");
            alhalozatok.ForEach(x => sw.WriteLine($"{x.Nev};{x.Cim};{x.Maszk};{x.SzorasCim}"));
            sw.Close();
        }

        static void HalozatokKiir()
        {
            Console.WriteLine("\nAz alhálózatok a következők: ");
            Console.WriteLine("{0,20} {1,20} {2,20} {3,20}", "Hálózat neve", "Hálózati cím", "Alhálóhazati maszkja", "Szórási címe");
            foreach (var halozat in alhalozatok)
            {
                Console.WriteLine("{0, 20} {1, 20} {2, 20} {3,20}", halozat.Nev, halozat.Cim,halozat.Maszk,halozat.SzorasCim);
            }
        }

        static void AlhalozatokBeker(int alhDB, string kiInduloCim)
        {
            for (int i = 0; i < alhDB; i++)
            {
                Console.Write($"Adja meg az {i+1}. alhálózat nevét: ");
                string alhNev = Console.ReadLine();
                int kioszthatoDB = GetDbBeker($"Adja meg az {i + 1}.alhálózatban szükséges kiosztható címek számát: ");
                alhalozatok.Add(new Halozat(alhNev, kioszthatoDB));
            }
            alhalozatok = alhalozatok.OrderByDescending(x => x.HostDB).ToList();
            for (int i = 0; i < alhalozatok.Count; i++)
            {
                alhalozatok[i].Calc(i == 0 ? kiInduloCim : alhalozatok[i-1].SzorasCim, i == 0 ? true : false);
            }
        }

        static int GetDbBeker(string uzenet)
        {
            bool ok = false;
            int szam = 0;
            string beker = "";
            do
            {
                Console.Write(uzenet);
                beker = Console.ReadLine();
                if(!int.TryParse(beker, out szam) || szam <= 0)
                {
                    Console.WriteLine("Csak pozitív egész szám adható meg!");
                }
                else
                {
                    ok = true;
                }
            } while (!ok);
            return szam;
        }

        static string HalozatCimBeker()
        {
            bool ok = false;
            string bekertCim = "";
            string[] reszek = null;
            do
            {
                Console.Write("Adja meg a címet és a maszkot: ");
                bekertCim = Console.ReadLine();
                if (bekertCim.Contains("/"))
                {
                    reszek = bekertCim.Split("/");
                    if (reszek.Length == 2 && CheckIfIP(reszek[0]) && int.TryParse(reszek[1], out int szam) && (szam >= 0 && szam <= 255))
                    {
                        bekertCim = reszek[0] + " " + Halozat.PrefixToMask(int.Parse(reszek[1]));
                        ok = true;
                    }
                }
                else if(bekertCim.Contains(" "))
                {
                    reszek = bekertCim.Split(); 
                    if(reszek.Length == 2 && CheckIfIP(reszek[0]) && CheckIfIP(reszek[1]))
                    {
                        ok = true;
                    }
                   
                }
                else
                {
                    continue;
                }
            } while (!ok);
            return bekertCim;
        }

       

        static bool CheckIfIP(string ip)
        {
            string[] oktettek = ip.Split('.');
            if (oktettek.Length == 4)
            {
                for (int i = 0; i < oktettek.Length; i++)
                {
                    if (!int.TryParse(oktettek[i], out int szam) || (szam < 0 || szam > 255))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

    }
}
