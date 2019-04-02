using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI2
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Saisir un fichier: ");
            //string fichier = Console.ReadLine();
            //Image Image = new MyImage(fichier);
            byte[] tableau = { 230, 4, 0, 0 };
            Console.WriteLine(Convertir_Endian_To_Int(tableau));
            Console.ReadKey();
        }
    }
}
