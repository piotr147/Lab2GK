using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab2GK.Model
{
    public class MyVector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public MyVector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
