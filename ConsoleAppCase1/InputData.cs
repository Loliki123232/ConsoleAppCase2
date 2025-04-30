using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCase1
{
    internal class InputData
    {
        public class InputResources
        {
            public double Ore { get; set; }
            public double Nikel { get; set; }
            public double Chrome { get; set; }
            public double Marganec { get; set; }
            public double TimeDP { get; set; }
            public double TimeConverter { get; set; }
            public double TimePS { get; set; }


            //Задаем значение полям т.е. кол-во ресурсов в целом
            public InputResources(double[] arr)
            {
                Ore = arr[0];
                Nikel = arr[1];
                Chrome = arr[2];
                Marganec = arr[3];
                TimeDP = arr[4];
                TimeConverter = arr[5];
                TimePS = arr[6];
            }
        }
    }
}
