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
            bool stop = false;
            while (!stop)
            {
                Console.WriteLine("Quel fichier voulez traiter? ");
                Console.WriteLine();
                Console.WriteLine("1 / coco.bmp " +
                    "\n2 / lac_en_montagne.bmp" +
                    "\n3 / Test.bmp" +
                    "\n4 / Image1.csv");

                int choixfichier = Convert.ToInt32(Console.ReadLine());
                string fichier = "";
                switch (choixfichier)
                {
                    case 1:
                        fichier = "coco.bmp";
                        break;
                    case 2:
                        fichier = "lac_en_montagne.bmp";

                        break;
                    case 3:
                        fichier = "Test.bmp";
                        break;
                    case 4:
                        fichier = "Image1.csv";
                        break;
                    case 5:
                        fichier = "Surprise.bmp";
                        break;
                }
                Console.Clear();

                MyImage Image = new MyImage(fichier);

                Console.WriteLine("Que voulez-vous faire? ");
                Console.WriteLine();
                Console.WriteLine("1  / Nuance de Gris " +
                    "\n2  / Agrandir" +
                    "\n3  / Retrecir" +
                    "\n4  / Rotation(90, 180, 270)" +
                    "\n5  / Effet Miroir" +
                    "\n6  / Appliquer un filtre" +
                    "\n7  / Fractale" +
                    "\n8  / Histogramme" +
                    "\n9  / Superposition" +
                    "\n10 / Rajouter du texte sur l'image");

                int choixTraitement = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (choixTraitement)
                {

                    case 1:
                        Console.WriteLine("Nuance de Gris");
                        Image.Noir_Blanc(fichier);
                        break;
                    case 2:
                        Console.WriteLine("Agrandir");
                        Image.Agrandir(fichier);
                        break;
                    case 3:
                        Console.WriteLine("Rétrécir");
                        Image.Retrecir(fichier);
                        break;
                    case 4:
                        Console.WriteLine("Quel degré de rotation?" +
                            "\n" +
                            "\n1 / 90" +
                            "\n2 / 180" +
                            "\n3 / 270");
                        int degre = Convert.ToInt32(Console.ReadLine());
                        switch (degre)
                        {
                            case 1:
                                Console.WriteLine("Rotation 90");
                                Image.Rotation_90(fichier);
                                break;
                            case 2:
                                Console.WriteLine("Rotation 180");
                                Image.Rotation_180(fichier);
                                break;
                            case 3:
                                Console.WriteLine("Rotation 270");
                                Image.Rotation_270(fichier);
                                break;
                        }
                        break;
                    case 5:
                        Console.WriteLine("Effet miroir");
                        Image.Effet_Miroir(fichier);
                        break;
                    case 6:
                        Console.WriteLine("Quel filtre voulez-vous appliquer?" +
                            "\n" +
                            "\n1 / Détection de contour" +
                            "\n2 / Renforcement" +
                            "\n3 / Flou" +
                            "\n4 / Repoussage");
                        int choixeffet = Convert.ToInt32(Console.ReadLine());
                        string effet = "";
                        switch (choixeffet)
                        {
                            case 1:
                                effet = "detection";
                                break;
                            case 2:
                                effet = "renforcement";
                                break;
                            case 3:
                                effet = "flou";
                                break;
                            case 4:
                                effet = "repoussage";
                                break;
                        }
                        Image.Convolution(fichier, effet);
                        break;
                    case 7:
                        Console.WriteLine("Fractale");
                        Console.WriteLine("En cours de chargement...");
                        MyImage ImageFractale = new MyImage("fractale.bmp", "fractale");
                        
                        break;
                    case 8:
                        Console.WriteLine("Histogramme");
                        Image.Histogramme();
                        break;
                    case 9:
                        Console.WriteLine("Avec quel fichier souhaitez-vous superposer l'image?" +
                            "\n1 / coco.bmp " +
                            "\n2 / lac_en_montagne.bmp" +
                            "\n3 / Test.bmp" +
                            "\n4 / Image1.csv");
                        int choixfichier2 = Convert.ToInt32(Console.ReadLine());
                        string nomdufichier2 = "";
                        switch (choixfichier2)
                        {
                            case 1:
                                nomdufichier2 = "coco.bmp";
                                break;
                            case 2:
                                nomdufichier2 = "lac_en_montagne.bmp";
                                break;
                            case 3:
                                nomdufichier2 = "Test.bmp";
                                break;
                            case 4:
                                nomdufichier2 = "Image1.csv";
                                break;
                        }
                        MyImage Image2 = new MyImage(nomdufichier2);
                        Image.CoderImage(Image2);
                        break;
                    case 10:
                        Console.WriteLine("Entrez du texte:");
                        string texte = Console.ReadLine();
                        Image.TextOnImage(fichier, texte);
                        break;
                    case 11:
                        Image.TextOnImage(fichier, "BARBECUE");
                        break;
                }
                Console.Clear();
                Console.WriteLine("Souhaitez-vous continuer?" +
                    "\n" +
                    "\n1 Oui" +
                    "\n2 Non");
                int choixContinuer = Convert.ToInt32(Console.ReadLine());
                switch (choixContinuer)
                {
                    case 1:
                       stop = false;
                        Console.Clear();
                       break;
                    case 2:
                        stop = true;
                        break;
                }
                
            }
            

        }
    }
}
