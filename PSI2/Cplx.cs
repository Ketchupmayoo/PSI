using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI2
{
    class Cplx
    {
        public double a; //Nb réel
        public double b; //nb imaginaire

        public Cplx(double a, double b)
        {
            this.a = a;
            this.b = b;

        }
        public void Carre()
        {
            double temp = (a * a) - (b * b);
            b = 2.0 * a * b;
            a = temp;
        }
        public double Distance()
        {
            return Math.Sqrt((a * a) + (b * b));
        }
        public void Ajouter(Cplx c)
        {
            a += c.a;
            b += c.b;
        }
    }
}
