using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Kevin LIM et Thomas NGO TD G
namespace PSI2
{
    class Pixel
    {
        #region Attributs
        byte red;
        byte green;
        byte blue;
        #endregion

        #region Constructeur
        public Pixel(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
        #endregion

        #region Propriétés
        public byte Red
        {
            get { return red; }
            set { red = value; }
        }
        public byte Green
        {
            get { return green; }
            set { green = value; }
        }
        public byte Blue
        {
            get { return blue; }
            set { blue = value; }
        }
        #endregion

        #region Méthodes
        public string AffichagePixel()
        {
            return red + " " + green + " " + blue;
        }
        #endregion
    }
}
