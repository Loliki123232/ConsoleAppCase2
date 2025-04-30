using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleAppCase1
{
    internal class ReadData
    {
        public double[] ReadText()
        {
            string filepath = "case.txt";
            string[] lines = File.ReadAllLines(filepath);
            
            List<double> res = new List<double>();
            foreach (string line in lines)
            {
                Match match = Regex.Match(line, @"(-?\d+(\.\d+)?)");
                string numberString = match.Value;
                
                if (double.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                {
                    res.Add(num);
                }
            }//достает из файла числа

            return res.ToArray();
        }
    }
}

