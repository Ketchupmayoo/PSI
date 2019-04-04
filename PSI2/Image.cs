using System;
using System.Collections.Generic;
using System.Text;

namespace PSI
{
    class Image
    {
        //Attributs
        Pixel[,] tabpix;
        int hauteur;
        int largeur;

        public Image (int hauteur, int largeur, Pixel[,] tabpix)
        {
            hauteur = tabpix.GetLength(0);
            largeur = tabpix.GetLength(1);
            this.tabpix = tabpix;
        }
    }
}
