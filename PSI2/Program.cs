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
            //string filename = "test.bmp";
            //Console.WriteLine("Lecture");
            //MyImage image1 = new MyImage(filename);
            //image1.ToString(filename);
            MyImage coco= new MyImage("test.bmp");
            coco.Histograme();
            int hauteur = 100;
            int largeur = 100;
            //MyImage image = new MyImage("test.bmp",largeur,hauteur);
            //image.Histograme();
            //image.Mandelbrot();
            Console.ReadKey();
            
        }
    }
}
