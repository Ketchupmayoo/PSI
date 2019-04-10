using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI2
{
    class Program
    {
        static public byte[] Convertir_Int_To_Endian(int val)
        {
            byte[] intBytes = BitConverter.GetBytes(val);
            for (int i=0;i<intBytes.Length;i++)
            {
                Console.Write(intBytes[i] + " ");
            }
            return intBytes;
        }
        static int Convertir_Endian_To_Int(byte[] tab)
        {
            string[] data1 = new string[tab.Length]; // Tableau de string pour travailler avec les hexadecimale
            for (int index = 0; index < tab.Length; index++)
            {
                data1[index] = tab[index].ToString("X"); // Conversion du tableau de decimal dans un tableau d'hexadecimale
                Console.WriteLine(data1[index]);
            }
            Array.Reverse(data1); // Inverser les valeurs d'un tableau : La valeur en index 0 va en index data.Length-1
            string data2 = string.Join("", data1); // Convertit le tableau d'hexadecimale sur 4 octet en hexadecimale sur 1 octet
            Console.WriteLine("In Hex: " + data2);
            int result = Convert.ToInt32(data2, 16); // Convertit l'hexadecimal en int
            return result;
        }
        static void Main(string[] args)
        {
            //Console.WriteLine("Saisir un fichier: ");
            //string fichier = Console.ReadLine();
            //Image Image = new MyImage(fichier);
            byte[] tableau = { 230, 4, 0, 0 };
            int val = 1254;
            Console.WriteLine(Convertir_Int_To_Endian(val));
            Console.ReadKey();
        }
    }
}
