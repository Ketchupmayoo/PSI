using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace PSI2
{
    class MyImage
    {
        #region Attributs
        private string ImageType;
        private int TailleFichier;
        private int tailleOffset;
        private int largeur;
        private int hauteur;
        private int nbbitsred;
        private int nbbitsgreen;
        private int nbbitsblue;
        private Pixel[] image;
        #endregion

        public MyImage(string myfile)
        {
            Process.Start(myfile);
            byte[] tableau = File.ReadAllBytes(myfile);
        }
        public void AffichageFichier(string myfile)
        {
            Console.WriteLine("HEADER");
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < 14; i++)
            {
                Console.Write(myfile[i] + "");
            }
            Console.WriteLine("\n\nHEADER INFO\n\n");
            for (int i = 14; i < 54; i++)
            {
                Console.Write(myfile[i] + " ");

            }
            Console.WriteLine("\n\nIMAGE\n");
            for (int i = 54; i < myfile.Length; i++)
            {
                Console.Write(myfile[i] + "\t");
            }
        }
        public int Convertir_Endian_To_Int(byte[] tab)
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


        
        /*public byte[] Convertir_Int_To_Endian(int val)
        {
        }*/
    }
}
