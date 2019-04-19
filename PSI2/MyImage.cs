using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


//using Serial.IO;
// Kevin LIM et Thomas NGO TD G
namespace PSI2
{
    public class MyImage
    {
        
        #region Attributs
        private string filename;
        private string imageType;
        private int tailleFichier;
        private int tailleOffset;
        private int largeur;
        private int hauteur;
        private int nbBitsCouleur;
        private Pixel[,] image;
        #endregion

        #region Constructeur
        public MyImage(string fileName)
        {
            byte[] myfile = null;
            //Process.Start(fileName); // Affiche le fichier
            if (fileName[fileName.Length - 1] == 'v')
            {
                myfile = TraitementCSV(fileName);
            }
            else
            {
                myfile = File.ReadAllBytes(fileName);
            }
            // ● type d’image (BM par exemple),  
            if (myfile[0] == 66 && myfile[1] == 77)
            {
                this.imageType = "bmp";
            }

            // ● taille du fichier (int)
            byte[] tab = new byte[4];
            for (int index = 0; index < 4; index++)
            {
                tab[index] = myfile[index + 2];
            }
            this.tailleFichier = Convertir_Endian_To_Int(tab);
            
            // ● taille Offset (int)
            for (int index = 0; index < 4; index++)
            {
                tab[index] = myfile[index + 10];
            }
            this.tailleOffset = Convertir_Endian_To_Int(tab);
            
            // ● largeur et hauteur de l’image (int) 
            for (int index = 0; index < 4; index++)
            {
                tab[index] = myfile[index + 18];
            }
            this.largeur = Convertir_Endian_To_Int(tab);
            for (int index = 0; index < 4; index++)
            {
                tab[index] = myfile[index + 22];
            }
            this.hauteur = Convertir_Endian_To_Int(tab);
            
            // ● nombre de bits par couleur(int)  
            for (int index = 0; index < 2; index++)
            {
                tab[index] = myfile[index + 28];
            }
            this.nbBitsCouleur = Convertir_Endian_To_Int(tab);
            
            // ● l’image par elle-même sur laquelle vous ferez les traitements proposés ensuite. (matrice de RGB) 
            int indexColonne = 0;
            int indexLigne = 0;

            image = new Pixel[hauteur, largeur]; // Affectation de la taille de la matrice
            for (int index = 54; index <= (tailleFichier - 3); index += 3) // Lecture des pixels ( 3 bytes )
            {

                if (indexColonne == (largeur))
                {
                    indexColonne = 0;
                    indexLigne++;
                }
                Pixel data = new Pixel(myfile[index], myfile[index + 1], myfile[index + 2]);
                image[indexLigne, indexColonne] = data;

                indexColonne++;
            }
        }
        public MyImage(string filename, string type)
        {
            this.filename = filename;
            Mandelbrot();
        } // Pour fractale
        #endregion

        #region Propriétés
        Pixel[,] Image
        {
            get { return image; }
            set { image = value; }
        }

        #endregion

        #region Méthodes

        #region TD1
        /// <summary>
        /// Affiche les données d'une image dans la console
        /// </summary>
        void AfficherTableauPixel(Pixel[,] image)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    Console.Write(image[i, j].ToString() + " ; ");
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Affiche les données d'un tableau de byte dans la console
        /// </summary>
        void AfficherTableauByte(byte[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                Console.Write(tab[i] + " ");
            }
        }
        /// <summary>
        /// Affiche les données d'un tableau de string dans la console
        /// </summary>
        void AfficherTableauString(string[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                Console.Write(tab[i] + " ");
            }
        }

        /// <summary>
        /// Permet de traiter créer le tableau de byte à partir d'un CSV
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] TraitementCSV(string fileName)
        {
            byte[] bytepixel = null;
            try
            {
                StreamReader lecture = new StreamReader(fileName);
                string ligne = "";
                string chaine_pixel = "";
                int lignefichier = 0; //Permet de savoir la ligne lue dans le fichier
                while (lecture.EndOfStream == false)
                {
                    lignefichier++;
                    ligne = lecture.ReadLine();

                    if (lignefichier == 1) //Lecture de la première ligne (header1)
                    {
                        string[] temp = ligne.Split(';');
                        for (int j = 0; j < 14; j++) { chaine_pixel += temp[j] + ';'; }
                    }
                    else
                    {
                        if (lignefichier == 2) //Lectur de la deuxiemeligne (header2)
                        {
                            string[] temp = ligne.Split(';');
                            for (int j = 0; j < 40; j++) { chaine_pixel += temp[j] + ';'; }
                        }
                        else
                        {
                            image = new Pixel[hauteur, largeur];
                            string[] temp = ligne.Split(';');
                            for (int i = 0; i < temp.Length; i++)
                            {
                                if (temp[i] != "")
                                {
                                    chaine_pixel += temp[i] + ';';
                                }
                            }
                        }
                    }

                }
                lecture.Close();
                string[] tabpixel = chaine_pixel.Split(';');
                bytepixel = new byte[tabpixel.Length];
                for (int i = 0; i < tabpixel.Length - 1; i++)
                {
                    byte temp = Convert.ToByte(tabpixel[i]);
                    bytepixel[i] = Convert.ToByte(tabpixel[i]);
                }
                //AfficherTableauByte(bytepixel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return bytepixel;
        }

        /// <summary>
        /// Affiche le header, le header info et l'image sur la console
        /// </summary>
        /// <param name="myfile">l'image à afficher</param>
        public void AffichageFichier(string fileName)
        {
            Console.WriteLine("HEADER");
            Console.WriteLine();
            Console.WriteLine();
            byte[] myfile = File.ReadAllBytes(fileName);
            for (int i = 0; i < 14; i++)
            {
                Console.Write(myfile[i] + " ");
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

        /// <summary>
        /// Convertit une séquence d’octet au format little endian en entier
        /// </summary>
        /// <param name="tab">Séquence d'octet</param>
        /// <returns>L'entier correspondant à la séquence d'octet</returns>
        static public int Convertir_Endian_To_Int(byte[] tab)
        {
            string[] data1 = new string[tab.Length]; // Tableau de string pour travailler avec les hexadecimale
            for (int index = 0; index < tab.Length; index++)
            {
                data1[index] = tab[index].ToString("X"); // Conversion du tableau de decimal dans un tableau d'hexadecimale
            }
            Array.Reverse(data1); // Inverser les valeurs d'un tableau : La valeur en index 0 va en index data.Length-1
            string data2 = string.Join("", data1); // Convertit le tableau d'hexadecimale sur 4 octet en hexadecimale sur 1 octet
            int result = Convert.ToInt32(data2, 16); // Convertit l'hexadecimal en int
            return result;
        }

        static public byte[] Convertir_Int_To_Endian(int val)
        {
            byte[] intBytes = BitConverter.GetBytes(val);

            return intBytes;
        }

        /// <summary>
        /// prend une instance de MyImage et la transforme en fichier binaire respectant la structure du fichier.bmp
        /// </summary>
        /// <param name="fileName">nom du fichier traité</param>
        public void From_Image_To_File(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string ImageToByte = "ImageToByte.bmp";
            byte[] data = new byte[myfile.Length];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne++)
                {

                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(ImageToByte, data);

            Process.Start(ImageToByte);
        }
        #endregion

        #region TD2
        /// <summary>
        /// Création d'une image en noir et blanc à partir d'une image donnée en paramètre
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        public void Noir_Blanc(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string Noir_Blanc = "Noir_Blanc.bmp";
            byte[] data = new byte[myfile.Length];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne++)
                {
                    byte datatmp = Convert.ToByte((image[indexLigne, indexColonne].Red + image[indexLigne, indexColonne].Green + image[indexLigne, indexColonne].Blue) / 3);
                    data[compteur] = datatmp;
                    data[compteur + 1] = datatmp;
                    data[compteur + 2] = datatmp;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(Noir_Blanc, data);

            Process.Start(Noir_Blanc);
        }
        /// <summary>
        /// Création d'une image avec effet miroir à partir d'une image donnée en paramètre
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        public void Effet_Miroir(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string Effet_Miroir = "Effet_Miroir.bmp";
            byte[] data = new byte[myfile.Length];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;

            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne++) //Parcours la matrice du coin haut gauche [0,0]
            {
                for (int indexColonne = (image.GetLength(1) - 1); indexColonne >= 0; indexColonne--)
                {

                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 3;
                }
            }


            File.WriteAllBytes(Effet_Miroir, data);
            Process.Start(Effet_Miroir);

        }
        /// <summary>
        /// Création d'une image avec rotation à 90° à partir d'une image donnée en paramètre
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        public void Rotation_90(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string Rotation = "Rotation.bmp";
            byte[] data = new byte[myfile.Length];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(largeur);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;

            for (int indexColonne = (image.GetLength(1) - 1); indexColonne >= 0; indexColonne--)
            {
                for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne++)
                {
                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 3;

                }
            }

            File.WriteAllBytes(Rotation, data);
            Process.Start(Rotation);

        }
        /// <summary>
        /// Création d'une image avec rotation à 180° à partir d'une image donnée en paramètre
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        public void Rotation_180(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string Rotation_180 = "Rotation.bmp";
            byte[] data = new byte[myfile.Length];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;

            for (int indexLigne = (image.GetLength(0) - 1); indexLigne >= 0; indexLigne--)
            {
                for (int indexColonne = (image.GetLength(1) - 1); indexColonne >= 0; indexColonne--)
                {
                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 3;
                }
            }


            File.WriteAllBytes(Rotation_180, data);
            Process.Start(Rotation_180);
        }
        /// <summary>
        /// Création d'une image avec rotation à 270° à partir d'une image donnée en paramètre
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        public void Rotation_270(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string Rotation = "Rotation.bmp";
            byte[] data = new byte[myfile.Length];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(largeur);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;

            for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne++)
            {
                for (int indexLigne = (image.GetLength(0) - 1); indexLigne >= 0; indexLigne--)
                {
                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 3;
                }
            }


            File.WriteAllBytes(Rotation, data);
            Process.Start(Rotation);
        }
        /// <summary>
        /// Création d'une image aggrandie à partir d'une image donnée en paramètre
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        public void Agrandir(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string Agrandir = "Agrandir.bmp";
            byte[] data = new byte[tailleOffset + (2 * largeur * 2 * hauteur) * 3];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleOffset + (largeur * hauteur) * 3);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(largeur * 2);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteur * 2);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne++)
                {

                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;

                    data[compteur + 3] = image[indexLigne, indexColonne].Red;
                    data[compteur + 4] = image[indexLigne, indexColonne].Green;
                    data[compteur + 5] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 6;
                }
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne++)
                {

                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;

                    data[compteur + 3] = image[indexLigne, indexColonne].Red;
                    data[compteur + 4] = image[indexLigne, indexColonne].Green;
                    data[compteur + 5] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 6;
                }
            }

            File.WriteAllBytes(Agrandir, data);

            Process.Start(Agrandir);
        }
        /// <summary>
        /// Création d'une image rétrécie à partir d'une image donnée en paramètre
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        public void Retrecir(string fileName)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string Retrecir = "Retrecir.bmp";
            byte[] data = new byte[tailleOffset + (largeur / 2 * hauteur / 2) * 3];

            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(Convert.ToInt32(tailleOffset + ((largeur / 2) * (hauteur / 2)) * 3));

            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(Convert.ToInt32(largeur / 2));
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(Convert.ToInt32(hauteur / 2));
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;
            /*
            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne = indexLigne+2)
            {
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne = indexColonne + 2)
                {
                    byte pixel1 = Convert.ToByte((image[indexLigne, indexColonne].Red + image[indexLigne, indexColonne].Green + image[indexLigne, indexColonne].Blue) / 3);
                    byte pixel2 = Convert.ToByte((image[indexLigne, indexColonne + 1].Red + image[indexLigne, indexColonne + 1].Green + image[indexLigne, indexColonne + 1].Blue) / 3);
                    byte pixel3 = Convert.ToByte((image[indexLigne + 1, indexColonne].Red + image[indexLigne + 1, indexColonne].Green + image[indexLigne + 1, indexColonne].Blue) / 3);
                    byte pixel4 = Convert.ToByte((image[indexLigne + 1, indexColonne + 1].Red + image[indexLigne + 1, indexColonne + 1].Green + image[indexLigne + 1, indexColonne + 1].Blue) / 3);

                    byte max = pixel1;
                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;

                    if (pixel2 > max)
                    {
                        max = pixel2;
                        data[compteur] = image[indexLigne, indexColonne+1].Red;
                        data[compteur + 1] = image[indexLigne, indexColonne+1].Green;
                        data[compteur + 2] = image[indexLigne, indexColonne+1].Blue;
                    }

                    if (pixel3 > max)
                    {
                        max = pixel3;
                        data[compteur] = image[indexLigne+1, indexColonne].Red;
                        data[compteur + 1] = image[indexLigne+1, indexColonne].Green;
                        data[compteur + 2] = image[indexLigne+1, indexColonne].Blue;
                    }

                    if (pixel4 > max)
                    {
                        data[compteur] = image[indexLigne+1, indexColonne + 1].Red;
                        data[compteur + 1] = image[indexLigne+1, indexColonne + 1].Green;
                        data[compteur + 2] = image[indexLigne+1, indexColonne + 1].Blue;
                    }

                    compteur = compteur+3;
                }
 
            }
            */

            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne = indexLigne + 2)
            {
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne = indexColonne + 2)
                {

                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(Retrecir, data);

            Process.Start(Retrecir);
        }
        #endregion TD2

        #region TD3
        /// <summary>
        /// Création d'une image avec un effet choisi dans les paramètres
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        /// <param name="effet">nom de l'effet (detection, renforcement, flou, repoussage)</param>
        public void Convolution(string fileName, string effet)
        {
            byte[] myfile = File.ReadAllBytes(fileName);
            string convolution = "Convolution.bmp";
            byte[] data = new byte[myfile.Length];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }
            
            //Image
            int[,] effetConvolution = null;
            if (effet == "detection")
            {
                int[,] detection = new int[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                effetConvolution = detection;
            }
            if (effet == "renforcement")
            {
                int[,] renforcement = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
                effetConvolution = renforcement;
            }
            if (effet == "flou")
            {
                //int[,] flou = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                int[,] flou = new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
                effetConvolution = flou;
            }
            if (effet == "repoussage")
            {
                int[,] repoussage = new int[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                effetConvolution = repoussage;
            }

            int compteur = 54;
            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne++)
                {
                    if (indexLigne == 0 || indexColonne == 0 || indexLigne == (image.GetLength(0) - 1) || indexColonne == (image.GetLength(1) - 1))
                    {
                        data[compteur] = image[indexLigne, indexColonne].Red;
                        data[compteur + 1] = image[indexLigne, indexColonne].Green;
                        data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                        compteur = compteur + 3;
                    }
                    else
                    {
                        Pixel pixelTemp = ApplicationConvolution(image, effetConvolution, indexLigne, indexColonne);
                        data[compteur] = pixelTemp.Red;
                        data[compteur + 1] = pixelTemp.Green;
                        data[compteur + 2] = pixelTemp.Blue;
                        compteur = compteur + 3;
                    }

                }
            }

            File.WriteAllBytes(convolution, data);

            Process.Start(convolution);

        }
        /// <summary>
        /// Calculer la valeur d'un pixel après application d'une matrice de convolution sur l'image
        /// </summary>
        /// <param name="image">matrice de pixel contenant l'image</param>
        /// <param name="matrice_convolution">la matrice qui correspond à un effet choisi</param>
        /// <param name="x">une coordonnée du pixel de sorti </param>
        /// <param name="y">une coordonnée du pixel de sorti</param>
        /// <returns></returns>
        Pixel ApplicationConvolution(Pixel[,] image, int[,] matrice_convolution, int x, int y)
        {
            byte red = 0;
            byte green = 0;
            byte blue = 0;
            int dataRed = 0;
            int dataGreen = 0;
            int dataBlue = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    dataRed += red + (image[(x - 1) + i, (y - 1) + j].Red) * matrice_convolution[i, j];
                    dataGreen += green + (image[(x - 1) + i, (y - 1) + j].Green) * matrice_convolution[i, j];
                    dataBlue += blue + (image[(x - 1) + i, (y - 1) + j].Blue) * matrice_convolution[i, j];

                }
            }
            if (dataRed > 255)
            {
                red = 255;
            }
            else if (dataRed < 0)
            {
                red = 0;
            }
            else
            {
                red = Convert.ToByte(dataRed);
            }

            if (dataGreen > 255)
            {
                green = 255;
            }
            else if (dataGreen < 0)
            {
                green = 0;
            }
            else
            {
                green = Convert.ToByte(dataGreen);
            }

            if (dataBlue > 255)
            {
                blue = 255;
            }
            else if (dataBlue < 0)
            {
                blue = 0;
            }
            else
            {
                blue = Convert.ToByte(dataBlue);
            }


            Pixel pixelTemp = new Pixel(red, green, blue);

            return pixelTemp;
        }
        #endregion

        #region TD4
        /// <summary>
        /// Remplir une matrice de pixel de pixel blanc
        /// </summary>
        private void RemplirBlanc(Pixel[,] image)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j] = new Pixel(255, 255, 255);
                }
            }
        }
        /// <summary>
        /// Création de 3 images représentant les histogrammes de rouge, de vert, et de bleu sur l'image
        /// </summary>
        public void Histogramme()
        {
            int[] histogram_r = new int[256];
            int[] histogram_g = new int[256];
            int[] histogram_b = new int[256];
            for (int i = 0; i < image.GetLength(0); i++) // Remplissage 
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    histogram_r[image[i, j].Red]++;
                    histogram_g[image[i, j].Green]++;
                    histogram_b[image[i, j].Blue]++;
                }
            }
            /*
            AfficherTableauInt(histogram_r);
            AfficherTableauInt(histogram_g);
            AfficherTableauInt(histogram_g);
            */
            //Prend la plus grande valeur dans les tableaux
            int hauteurMax = 0;
            int somme = 0;
            for (int i = 0; i < 256; i++)
            {
                somme += histogram_r[i];
                if (hauteurMax < histogram_r[i])
                {
                    hauteurMax = histogram_r[i];
                }
                if (hauteurMax < histogram_b[i])
                {
                    hauteurMax = histogram_b[i];
                }
                if (hauteurMax < histogram_g[i])
                {
                    hauteurMax = histogram_g[i];
                }
            }
            Pixel[,] histo_r = new Pixel[hauteurMax, 256];
            RemplirBlanc(histo_r);
            for (int indexColonne = 0; indexColonne < 255; indexColonne++)
            {
                for (int indexLigne = 0; indexLigne < histogram_r[indexColonne]; indexLigne++)
                {
                    histo_r[indexLigne, indexColonne] = new Pixel(0, 0, 255);
                }
            }

            Pixel[,] histo_g = new Pixel[hauteurMax, 256];
            RemplirBlanc(histo_g);
            for (int indexColonne = 0; indexColonne < 255; indexColonne++)
            {
                for (int indexLigne = 0; indexLigne < histogram_g[indexColonne]; indexLigne++)
                {
                    histo_g[indexLigne, indexColonne] = new Pixel(0, 255, 0);
                }
            }
            Pixel[,] histo_b = new Pixel[hauteurMax, 256];
            RemplirBlanc(histo_b);
            for (int indexColonne = 0; indexColonne < 255; indexColonne++)
            {
                for (int indexLigne = 0; indexLigne < histogram_b[indexColonne]; indexLigne++)
                {
                    histo_b[indexLigne, indexColonne] = new Pixel(255, 0, 0);
                }
            }

            string rouge = "rouge.bmp";
            string vert = "vert.bmp";
            string bleu = "bleu.bmp";

            byte[] data = new byte[hauteurMax * 256 * 3 + 54];
            byte[] tempdata = new byte[4];

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(hauteurMax * 256 * 3 + 54);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(54);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(histo_r.GetLength(1));
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteurMax);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }


            //Histo rouge
            int compteur = 54;
            //AfficherTableauPixel(histo_r);
            for (int indexLigne = 0; indexLigne < histo_r.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < histo_r.GetLength(1); indexColonne++)
                {
                    //Console.WriteLine("flag");
                    //Console.WriteLine("red: " + histo_r[indexLigne, indexColonne].Red);
                    data[compteur] = histo_r[indexLigne, indexColonne].Red;
                    data[compteur + 1] = histo_r[indexLigne, indexColonne].Green;
                    data[compteur + 2] = histo_r[indexLigne, indexColonne].Blue;
                    compteur += 3;
                }
            }
            File.WriteAllBytes(rouge, data);
            Process.Start(rouge);
            // Histo vert
            compteur = 54;
            //AfficherTableauPixel(histo_r);
            for (int indexLigne = 0; indexLigne < histo_g.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < histo_g.GetLength(1); indexColonne++)
                {
                    //Console.WriteLine("flag");
                    //Console.WriteLine("red: " + histo_r[indexLigne, indexColonne].Red);
                    data[compteur] = histo_g[indexLigne, indexColonne].Red;
                    data[compteur + 1] = histo_g[indexLigne, indexColonne].Green;
                    data[compteur + 2] = histo_g[indexLigne, indexColonne].Blue;
                    compteur += 3;
                }
            }
            File.WriteAllBytes(vert, data);
            Process.Start(vert);
            // Histo bleu
            compteur = 54;
            //AfficherTableauPixel(histo_r);
            for (int indexLigne = 0; indexLigne < histo_b.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < histo_b.GetLength(1); indexColonne++)
                {
                    //Console.WriteLine("flag");
                    //Console.WriteLine("red: " + histo_r[indexLigne, indexColonne].Red);
                    data[compteur] = histo_b[indexLigne, indexColonne].Red;
                    data[compteur + 1] = histo_b[indexLigne, indexColonne].Green;
                    data[compteur + 2] = histo_b[indexLigne, indexColonne].Blue;
                    compteur += 3;
                }
            }
            File.WriteAllBytes(bleu, data);
            Process.Start(bleu);

        }
        /// <summary>
        /// Création d'une fractale
        /// </summary>
        public void Mandelbrot()
        {
            double maxr = 0.6;
            double maxi = 1.2;
            double minr = -2.1;
            double mini = -1.2;

            double zx = 0;
            double zy = 0;
            double cx = 0;
            double cy = 0;

            int zoom = 1000;
            double tempzx = 0;
            int loopmax = 1000;
            int loopgo = 0;

            int hauteur = Convert.ToInt32((maxr - minr) * zoom);
            int largeur = Convert.ToInt32((maxi - mini) * zoom);


            double xjump = ((maxr - minr) / Convert.ToDouble(largeur));
            double yjump = ((maxi - mini) / Convert.ToDouble(hauteur));

            Pixel[,] image = new Pixel[hauteur, largeur];
            RemplirNoir(image);
            byte[] data = CreerBMP(hauteur, largeur);

            for (int x = 0; x < hauteur; x++)
            {
                cx = (xjump * x) - Math.Abs(minr);
                for (int y = 0; y < largeur; y++)
                {
                    zx = 0;
                    zy = 0;
                    cy = (yjump * y) - Math.Abs(mini);
                    loopgo = 0;
                    while (zx * zx + zy * zy <= 4 && loopgo < loopmax)
                    {
                        loopgo++;
                        tempzx = zx;
                        zx = (zx * zx) - (zy * zy) + cx;
                        zy = (2 * tempzx * zy) + cy;
                    }
                    if (loopgo != loopmax)
                    {
                        image[x, y] = new Pixel(255, 255, 255);
                    }

                    else
                    {
                        image[x, y] = new Pixel(0, 0, 0);
                    }

                }
            }
            //AfficherTableauPixel(image);
            byte[] imagebyte = ConvertirMatricePixel(image);
            //Console.WriteLine(imgbyte.Length);
            for (int i = 54; i < data.Length; i++)
            {
                data[i] = imagebyte[i - 54];
            }
            //AfficherTableauByte(data);
            File.WriteAllBytes(filename, data);
            Process.Start(filename);
        }
        /// <summary>
        /// Remplir une matrice de pixel de pixel noir
        /// </summary>
        private void RemplirNoir(Pixel[,] image)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j] = new Pixel(0, 0, 0);
                }
            }
        }
        /// <summary>
        /// Créer un fichier bmp en ayant les informations de hauteur et de largeur
        /// </summary>
        /// <param name="hauteur"></param>
        /// <param name="largeur"></param>
        /// <returns></returns>
        public byte[] CreerBMP(int hauteur, int largeur)
        {
            tailleFichier = largeur * hauteur * 3 + 54;
            byte[] data = new byte[tailleFichier];
            byte[] tempdata = new byte[4];
            data[0] = 66;
            data[1] = 77;

            //Définition de la taille du fichier
            tempdata = Convertir_Int_To_Endian(tailleFichier);
            for (int index = 2; index <= 5; index++)
            {
                data[index] = tempdata[index - 2];
                //Console.Write(data[index] + " ");
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            //Définiton de la taille de l'offset
            this.tailleOffset = 54;
            tempdata = Convertir_Int_To_Endian(tailleOffset);
            for (int index = 10; index <= 13; index++)
            {
                data[index] = tempdata[index - 10];
                //Console.Write(data[index] + " ");
            }

            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            //Définition de la largeur
            tempdata = Convertir_Int_To_Endian(largeur);
            for (int index = 18; index <= 21; index++)
            {
                data[index] = tempdata[index - 18];
                //Console.Write(data[index] + " ");
            }
            tempdata = Convertir_Int_To_Endian(hauteur);
            for (int index = 22; index <= 25; index++)
            {
                data[index] = tempdata[index - 22];
                //Console.Write(data[index] + " ");
            }

            data[26] = 0;
            data[27] = 0;

            //Définition Nb de bits couleur
            this.nbBitsCouleur = 24;
            tempdata = Convertir_Int_To_Endian(this.nbBitsCouleur);
            for (int index = 28; index <= 30; index++)
            {
                data[index] = tempdata[index - 28];
                //Console.Write(data[index] + " ");
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            int indexColonne = 0;
            int indexLigne = 0;

            image = new Pixel[hauteur, largeur]; // Affectation de la taille de la matrice
            for (int index = 54; index <= (tailleFichier - 3); index += 3) // Lecture des pixels ( 3 bytes )
            {

                if (indexColonne == (largeur))
                {
                    indexColonne = 0;
                    indexLigne++;
                }
                Pixel tempixel = new Pixel(data[index], data[index + 1], data[index + 2]);
                image[indexLigne, indexColonne] = tempixel;

                //Console.WriteLine("Index: " + index + "; " + myfile[index] + " " + myfile[index + 1] + " " + myfile[index + 2]);

                indexColonne++;
            }


            File.WriteAllBytes(this.filename, data);
            //AfficherTableauByte(data);
            //Process.Start(this.filename);
            return data;
        }
        /// <summary>
        /// Conversion d'une matrice de pixel en tableau de byte
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] ConvertirMatricePixel(Pixel[,] image)
        {
            int cmp = 0;
            byte[] data = new byte[image.GetLength(0) * image.GetLength(1) * 3];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    data[cmp] = image[i, j].Red;
                    data[cmp + 1] = image[i, j].Green;
                    data[cmp + 2] = image[i, j].Blue;
                    cmp += 3;
                }
            }
            //Console.WriteLine("ConvertirMatricePixel");
            //AfficherTableauByte(data);
            return data;
        }

        #endregion

        #region TD5
        /// <summary>
        /// Superposer une image avec une autre image donnée en paramètre
        /// </summary>
        public void CoderImage(MyImage Image2)
        {
            byte[] tempdata = new byte[4];
            int hauteurF = Image2.hauteur;
            int largeurF = Image2.largeur;
            int tailleFichierF = Image2.tailleFichier;
            int tailleOffsetF = Image2.tailleOffset;
            if (hauteurF < hauteur)
            {
                hauteurF = hauteur;
            }
            if (largeurF < largeur)
            {
                largeurF = largeur;
            }
            if (tailleFichierF < tailleFichier)
            {
                tailleFichierF = tailleFichier;
            }

            byte[] data = new byte[tailleFichierF]; // Creation tableau de byte

            //Header
            data[0] = 66;
            data[1] = 77;

            tempdata = Convertir_Int_To_Endian(tailleFichierF);
            for (int i = 2; i <= 5; i++)
            {
                data[i] = tempdata[i - 2];
            }

            tempdata = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                data[i] = tempdata[i - 6];
            }

            tempdata = Convertir_Int_To_Endian(tailleOffsetF);
            for (int i = 10; i <= 13; i++)
            {
                data[i] = tempdata[i - 10];
            }

            //HeaderInfo
            tempdata = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                data[i] = tempdata[i - 14];
            }

            tempdata = Convertir_Int_To_Endian(largeurF);
            for (int i = 18; i <= 21; i++)
            {
                data[i] = tempdata[i - 18];
            }

            tempdata = Convertir_Int_To_Endian(hauteurF);
            for (int i = 22; i <= 25; i++)
            {
                data[i] = tempdata[i - 22];
            }

            data[26] = 0;
            data[27] = 0;

            tempdata = Convertir_Int_To_Endian(nbBitsCouleur);
            for (int i = 28; i <= 29; i++)
            {
                data[i] = tempdata[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                data[i] = 0;
            }

            //Image
            int compteur = 54;

            for (int indexLigne = 0; indexLigne < hauteurF; indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < largeurF; indexColonne++)
                {
                    int flag = 0;
                    if (hauteur - 1 < indexLigne)
                    {
                        data[compteur] = TwoByteToOneByte(0, Image2.image[indexLigne, indexColonne].Red);
                        data[compteur + 1] = TwoByteToOneByte(0, Image2.image[indexLigne, indexColonne].Green);
                        data[compteur + 2] = TwoByteToOneByte(0, Image2.image[indexLigne, indexColonne].Blue);
                        flag++;
                    }
                    if (largeur - 1 < indexColonne)
                    {
                        data[compteur] = TwoByteToOneByte(0, Image2.image[indexLigne, indexColonne].Red);
                        data[compteur + 1] = TwoByteToOneByte(0, Image2.image[indexLigne, indexColonne].Green);
                        data[compteur + 2] = TwoByteToOneByte(0, Image2.image[indexLigne, indexColonne].Blue);
                        flag++;
                    }
                    if (Image2.hauteur - 1 < indexLigne)
                    {
                        data[compteur] = TwoByteToOneByte(image[indexLigne, indexColonne].Red, 0);
                        data[compteur + 1] = TwoByteToOneByte(image[indexLigne, indexColonne].Green, 0);
                        data[compteur + 2] = TwoByteToOneByte(image[indexLigne, indexColonne].Blue, 0);
                        flag++;
                    }
                    if (Image2.largeur - 1 < indexColonne)
                    {
                        data[compteur] = TwoByteToOneByte(image[indexLigne, indexColonne].Red, 0);
                        data[compteur + 1] = TwoByteToOneByte(image[indexLigne, indexColonne].Green, 0);
                        data[compteur + 2] = TwoByteToOneByte(image[indexLigne, indexColonne].Blue, 0);
                        flag++;
                    }

                    if (flag == 0)
                    {
                        data[compteur] = TwoByteToOneByte(image[indexLigne, indexColonne].Red, Image2.image[indexLigne, indexColonne].Red);
                        data[compteur + 1] = TwoByteToOneByte(image[indexLigne, indexColonne].Green, Image2.image[indexLigne, indexColonne].Green);
                        data[compteur + 2] = TwoByteToOneByte(image[indexLigne, indexColonne].Blue, Image2.image[indexLigne, indexColonne].Blue);
                    }

                    compteur = compteur + 3;
                }
            }


            /*
            // Copie toute l'image1 dans tableau de byte data
            for (int indexLigne = 0; indexLigne < image.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < image.GetLength(1); indexColonne++)
                {
                    data[compteur] = image[indexLigne, indexColonne].Red;
                    data[compteur + 1] = image[indexLigne, indexColonne].Green;
                    data[compteur + 2] = image[indexLigne, indexColonne].Blue;
                    compteur = compteur + 3;
                }
            }
            compteur = 54; // Reset compteur à 54
            Console.WriteLine(tailleFichierF);
            Console.ReadKey();
            // Copie de l'image 2 dans tableau de byte sous certaine condition pour superposition
            for (int indexLigne = 0; indexLigne < Image2.image.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Image2.image.GetLength(1); indexColonne++)
                {
                    if (indexColonne % 2 == 1) // Si impair colle son byte dans matrice supersotition
                    {
                        data[compteur] = Image2.image[indexLigne, indexColonne].Red;
                        data[compteur + 1] = Image2.image[indexLigne, indexColonne].Green;
                        data[compteur + 2] = Image2.image[indexLigne, indexColonne].Blue;
                    }

                    if (indexLigne >= image.GetLength(0) || indexColonne >= image.GetLength(1)) // image2 + grande que image1
                    {
                        data[compteur] = Image2.image[indexLigne, indexColonne].Red;
                        data[compteur + 1] = Image2.image[indexLigne, indexColonne].Green;
                        data[compteur + 2] = Image2.image[indexLigne, indexColonne].Blue;
                    }
                    Console.WriteLine(compteur);
                    compteur = compteur + 3;
                }
            }
            */
            string superposition = "Superposition.bmp";
            File.WriteAllBytes(superposition, data);
            Process.Start(superposition);

        }
        /// <summary>
        /// Creation d'un byte à partir des informations de deux bytes
        /// </summary>
        /// <returns></returns>
        public byte TwoByteToOneByte(byte byte1, byte byte2)
        {
            string numero1 = Convert.ToString(byte1, 2).PadLeft(8, '0'); // Convertir byte1 en base 2 avec 8 chiffres
            string numero2 = Convert.ToString(byte2, 2).PadLeft(8, '0');
            //Substring(LettreDépart, nbDeLettre) enlever des lettres
            //numero1.Substring(0, 4); // 4 premiers chiffres
            string somme = numero1.Substring(0, 4) + numero2.Substring(0, 4);
            byte final = Convert.ToByte(somme, 2);
            return final;
        }

        #endregion

        #region Innovation
        /// <summary>
        /// Rajouter du texte sur une image
        /// </summary>
        /// <param name="fileName">nom complet de l'image</param>
        /// <param name="text">texte à rajouter</param>
        public void TextOnImage(string fileName, string text)
        {
            System.Drawing.Image Innovation = System.Drawing.Bitmap.FromFile(fileName);
            System.Drawing.Graphics Graphics = System.Drawing.Graphics.FromImage(Innovation);
            Graphics.DrawString(text, System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.White, new System.Drawing.PointF(this.hauteur/2, this.largeur/2));
            Graphics.Dispose();
            Innovation.Save("innovation.bmp");
            Process.Start("innovation.bmp");
        }
        #endregion

        #endregion
    }
}
