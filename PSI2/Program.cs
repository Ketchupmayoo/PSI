using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Kevin LIM et Thomas NGO TD G
namespace PSI2
{
    class Program
    {

        static void Main(string[] args)
        {

            //Console.WriteLine("Entrer le nom d'un fichier : ");
            string nomfichier = "coco.bmp";
            
            //string nomfichier = Console.ReadLine();
            MyImage Image = new MyImage(nomfichier);
            Image.Convolution("coco.bmp", "flou");
            //Image.Agrandir(nomfichier);
            Console.ReadKey();
            
        }
    }
}
