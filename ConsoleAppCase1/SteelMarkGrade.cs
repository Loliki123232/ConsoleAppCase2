using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleAppCase1.InputData;

namespace ConsoleAppCase1
{
    internal class SteelMarkGrade
    {
        public double Ore { get; set; }
        public double Nikel { get; set; }
        public double Chrome { get; set; }
        public double Marganec { get; set; }
        public double TimeFurnace { get; set; }
        public double TimeConverter { get; set; }
        public double TimeRollingMachine { get; set; }
        public double Price { get; set; }
        public double WorkVolume { get; set; }
        public double TotalProduced { get; set; } = 0;
        public double TotalWorkTimeFurnace { get; set; } = 0;
        public double TotalWorkTimeConverter { get; set; } = 0;
        public double TotalWorkTimeRollingMachine { get; set; } = 0;
        public double TotalPrice { get; set; } = 0;


        //Задаем полям значение т.е. кол-во ресуры на производство одной тонны стали
        public SteelMarkGrade(double[] arr)
        {
            Ore = arr[0];
            Nikel = arr[1];
            Chrome = arr[2];
            Marganec = arr[3];
            TimeFurnace = arr[4];
            TimeConverter = arr[5];
            TimeRollingMachine = arr[6];
            Price = arr[7];
            WorkVolume = arr[8];
        }


        //Загружаем ресуры пока их хватает на производста стали
        public bool UploadResources(InputResources inputResources)
        {
            return inputResources.Ore >= Ore &&
                   inputResources.Nikel >= Nikel &&
                   inputResources.Chrome >= Chrome &&
                   inputResources.Marganec >= Marganec;
        }


        //Печь производящая сталь
        public void Furnace(InputResources inputResources)
        {
            while (TotalProduced < WorkVolume && UploadResources(inputResources))
            {

                inputResources.Ore -= Ore;
                inputResources.Nikel -= Nikel;
                inputResources.Chrome -= Chrome;
                inputResources.Marganec -= Marganec;


                TotalProduced += 1;
                TotalWorkTimeFurnace += TimeFurnace;
                TotalPrice += Price;
            }

        }


        //Конвертер
        public void Converter()
        {
            for (int i = 0; i < TotalProduced; i++)
            {
                TotalWorkTimeConverter += TimeConverter;
            }
        }


        //Прокатный станок
        public void RollingMachine()
        {
            for (int i = 0; i < TotalProduced; i++)
            {
                TotalWorkTimeRollingMachine += TimeRollingMachine;
            }
        }
    }
}

