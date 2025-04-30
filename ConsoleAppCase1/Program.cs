using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using static ConsoleAppCase1.InputData;
using System.Reflection.PortableExecutable;
using ConsoleAppCase1;

class Program
{
    static void Main(string[] args)
    {
        //Путь к файлу
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string path = Path.Combine(desktopPath, "case.txt");


        //Считываем файл и полученные данные записываем в массивы для дальнейшего использования в классах
        ReadData reader = new ReadData();
        double[] numbers = reader.ReadText();
        double[] arrInput = { numbers[0], numbers[1], numbers[2], numbers[3], numbers[4], numbers[5], numbers[6] };
        double[] arrMarkA = { numbers[7], numbers[8], numbers[9], numbers[10], numbers[11], numbers[12], numbers[13], numbers[14], numbers[15] };
        double[] arrMarkB = { numbers[16], numbers[17], numbers[18], numbers[19], numbers[20], numbers[21], numbers[22], numbers[23], numbers[24] };
        double[] arrMarkC = { numbers[25], numbers[26], numbers[27], numbers[28], numbers[29], numbers[30], numbers[31], numbers[32], numbers[33] };


        //Создаем экземпляры классов
        InputResources inputResources = new InputResources(arrInput);
        SteelMarkGrade steelMarkA = new SteelMarkGrade(arrMarkA);
        SteelMarkGrade steelMarkB = new SteelMarkGrade(arrMarkB);
        SteelMarkGrade steelMarkC = new SteelMarkGrade(arrMarkC);


        //Загружаем в печку материал и производим сталь
        steelMarkA.Furnace(inputResources);
        steelMarkB.Furnace(inputResources);
        steelMarkC.Furnace(inputResources);


        //Отправляем сталь на конвертер
        steelMarkA.Converter();
        steelMarkB.Converter();
        steelMarkC.Converter();


        //Отправляем сталь с конвертера на прокатный станок
        steelMarkA.RollingMachine();
        steelMarkB.RollingMachine();
        steelMarkC.RollingMachine();


        //Высчитываем время работы печи, конвертера и прокатного станка
        double FurnaceWork = steelMarkA.TotalWorkTimeFurnace + steelMarkB.TotalWorkTimeFurnace + steelMarkC.TotalWorkTimeFurnace;
        double ConvertorWork = steelMarkA.TotalWorkTimeConverter + steelMarkB.TotalWorkTimeConverter + steelMarkC.TotalWorkTimeConverter;
        double RollingMachineWork = steelMarkA.TotalWorkTimeRollingMachine + steelMarkB.TotalWorkTimeRollingMachine + steelMarkC.TotalWorkTimeRollingMachine;


        //Вывод
        Console.WriteLine("Оптимальный производственный план:\n");
        Console.WriteLine($"Марка Стали А: {steelMarkA.TotalProduced} т.");
        Console.WriteLine($"Марка Стали B: {steelMarkB.TotalProduced} т.");
        Console.WriteLine($"Марка Стали C: {steelMarkC.TotalProduced} т.\n");
        Console.WriteLine($"Не использованные ресуры:\nРуда:{inputResources.Ore}" +
            $"\nНикель:{inputResources.Nikel}" +
            $"\nХром:{inputResources.Chrome}" +
            $"\nМарганец:{inputResources.Marganec}\n" +
            $"\nВремя работы доменно печи: {FurnaceWork}" +
            $"\nВремя работы конвертора: {ConvertorWork}" +
            $"\nВремя работы прокатного стана: {RollingMachineWork}\n");
        Console.WriteLine($"Общая прибыль: {steelMarkA.TotalPrice + steelMarkB.TotalPrice + steelMarkC.TotalPrice}");


    }
}