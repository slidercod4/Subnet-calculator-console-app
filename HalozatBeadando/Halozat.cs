using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HalozatBeadando
{
    class Halozat
    {
        public string Nev { get;private set; }
        public string Cim { get;private set; }
        public string Maszk { get;private set; }
        public string SzorasCim { get;private set; }
        public int HostDB { get;private set; }
        public int HaloMeret { get;private set; }

        public Halozat(string nev, int hostDb)
        {
            Nev = nev;
            HostDB = hostDb;
        }

        public void Calc(string haloIP, bool isFirst)
        {
            haloIP = isFirst ? haloIP : IPAddOne(haloIP);
            Maszk = CalcMask();
            Cim = isFirst ? BinaryToDec(CheckIfNetwork(haloIP, Maszk)) : haloIP;
            SzorasCim = IPCalc(Cim.Split('.')); 
        }

        private string IPAddOne(string haloIP)
        {
            string[] IP = haloIP.Split('.');
            string eredmeny = "";
            bool VoltNovel = false;
            for (int i = IP.Length-1; i >= 0; i--)
            {
                int oktettErtek = int.Parse(IP[i]);
                if (oktettErtek < 255)
                {
                    if (!VoltNovel)
                    {
                        oktettErtek++;
                        VoltNovel = true;
                    }
                }
                else
                {
                    oktettErtek = 0;
                }
                eredmeny = eredmeny.Insert(0, oktettErtek.ToString()+".");
            }
            return eredmeny.Remove(eredmeny.Length-1);
        }
     
        private string IPCalc(string[] korrigaltIP)
        {
           
            string[] wildCard = wildCardCalc().Split('.');
            string broadcast = "";
            for (int i = 0; i < korrigaltIP.Length; i++)
            {  
                broadcast += (int.Parse(korrigaltIP[i]) + int.Parse(wildCard[i])) + ".";
            }
            return broadcast.Remove(broadcast.Length-1);

        }
        private string wildCardCalc()
        {
            string wildCard = "";
            string[] maszkOktettek = Maszk.Split('.');
            for (int i = maszkOktettek.Length-1; i >= 0; i--)
            {
                 wildCard = wildCard.Insert(0, (255 - int.Parse(maszkOktettek[i])).ToString()+".");
            }
            return wildCard.Remove(wildCard.Length-1);
        }

        public static string CheckIfNetwork(string IP, string Mask)
        {
            string binaryIP = DecToBinary(IP).Replace(".", String.Empty);
            string binaryMask = DecToBinary(Mask).Replace(".", String.Empty);
            int i = binaryMask.Length-1;
            while (binaryMask[i] == '0')
            {
                if (binaryIP[i] != '0')
                {
                    binaryIP = binaryIP.Remove(i, 1).Insert(i, "0");
                }
                i--;
            }
            i = 0;
            for (int j = 0; j < binaryIP.Length; j++)
            {
                if (j % 8 == 0 && j != 0)
                {
                    binaryIP = binaryIP.Insert(j + i, ".");
                    i++;
                }
            }
            binaryIP = binaryIP.Remove(binaryIP.Length - 1, 1);
            return binaryIP;
        }



        private string CalcMask()
        {
            int halMeret = 0;
            int hatvany = 0;
            for (halMeret = 1; halMeret < HostDB+2; halMeret *= 2)
            {
                hatvany++;
            }
            HaloMeret = halMeret;
            return  PrefixToMask(32 - hatvany);
            
        }

        public static string PrefixToMask(int prefix)
        {
            string binaris = "";
            int db = 0;
            for (int i = 0; i < 32; i++)
            {
                if (i < prefix)
                {
                    binaris += "1";
                }
                else
                {
                    binaris += "0";
                }
                if (i % 8 == 0 && i != 0)
                {
                    binaris = binaris.Insert(i + db, ".");
                    db++;
                }
            }
            return BinaryToDec(binaris);
        }

        public static string DecToBinary(string IP)
        {
            string binaris = "";
            string[] oktetek = IP.Split('.');
            foreach (var oktet in oktetek)
            {
                binaris += Convert.ToString(int.Parse(oktet), 2).PadLeft(8, '0') + ".";

            }
            binaris = binaris.Remove(binaris.Length - 1);
            return binaris;
        }

        public static string BinaryToDec(string binaris)
        {
            string[] darabok = binaris.Split('.');
            string eredmeny = "";
            int db = 0;
            for (int i = 0; i < darabok.Length; i++)
            {
                double tizesSzam = 0;
                db = 0;
                for (int j = darabok[i].Length - 1; j >= 0; j--)
                {
                    if (darabok[i][j] == '1')
                        tizesSzam += Math.Pow(2, db);
                    db++;
                }
                eredmeny += tizesSzam + ".";
            }
            eredmeny = eredmeny.Remove(eredmeny.Length - 1);
            return eredmeny;
        }


    }
}
