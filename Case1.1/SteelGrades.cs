using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case1._1
{
    public class SteelGradeParameters
    {
        public string Name { get; set; }
        public double OrePerTon { get; set; }       // руда (тонн/тонну стали)
        public double NickelPerTon { get; set; }    // никель (кг/тонну стали)
        public double ChromePerTon { get; set; }    // хром (кг/тонну стали)
        public double ManganesePerTon { get; set; }  // марганец (кг/тонну стали)
        public double FurnaceTimePerTon { get; set; } // время печи (часы/тонну)
        public double ConverterTimePerTon { get; set; } // время конвертера (часы/тонну)
        public double RollingMachineTimePerTon { get; set; } // время стана (часы/тонну)
        public decimal PricePerTon { get; set; }     // цена (руб/тонну)
        public int TargetVolume { get; set; }       // целевой объем (тонн)
    }

    public static class SteelGrades
    {
        public static readonly SteelGradeParameters GradeA = new SteelGradeParameters
        {
            Name = "Марка A",
            OrePerTon = 2,
            NickelPerTon = 0.5,
            ChromePerTon = 0,
            ManganesePerTon = 0,
            FurnaceTimePerTon = 0.5,
            ConverterTimePerTon = 0.2,
            RollingMachineTimePerTon = 0.3,
            PricePerTon = 10000,
            TargetVolume = 10
        };

        public static readonly SteelGradeParameters GradeB = new SteelGradeParameters
        {
            Name = "Марка B",
            OrePerTon = 1.5,
            NickelPerTon = 0,
            ChromePerTon = 1,
            ManganesePerTon = 2,
            FurnaceTimePerTon = 0.4,
            ConverterTimePerTon = 0.3,
            RollingMachineTimePerTon = 0.4,
            PricePerTon = 12000,
            TargetVolume = 15
        };

        public static readonly SteelGradeParameters GradeC = new SteelGradeParameters
        {
            Name = "Марка C",
            OrePerTon = 1,
            NickelPerTon = 0,
            ChromePerTon = 0,
            ManganesePerTon = 0,
            FurnaceTimePerTon = 0.3,
            ConverterTimePerTon = 0.2,
            RollingMachineTimePerTon = 0.2,
            PricePerTon = 8000,
            TargetVolume = 20
        };
    }
}
