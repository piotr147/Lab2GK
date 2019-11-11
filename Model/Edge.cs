using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Lab2GK.Model
{
    public class Edge : ObservableObject
    {
        public (int x, int y) V1 { get; set; }
        public (int x, int y) V2 { get; set; }

        public int Ymax
        {
            get { return V1.y > V2.y ? V1.y : V2.y; }
        }

        public int GetX(int y)
        {
            return ((y - V1.y) * (V2.x - V1.x) / (V2.y - V1.y) + V1.x);
        }
    }
}
