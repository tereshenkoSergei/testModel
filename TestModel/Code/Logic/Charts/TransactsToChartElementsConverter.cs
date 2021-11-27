using System;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using TestModel.Annotations;
using TestModel.Code.Transacts;

namespace TestModel.Code.Logic.Charts
{
    public static class TransactsToChartElementsConverter
    {
        public static ChartValues<int> GetStudentDistribution(List<Student> studentList, int pockets)
        {
            List<double> doubles = new List<double>();
            double minValue = 4;
            double maxValue = -4;

            foreach (var stud in studentList)
            {
                doubles.Add(stud.Level);
                minValue = minValue > stud.Level ? stud.Level : minValue;
                maxValue = maxValue < stud.Level ? stud.Level : maxValue;
            }

            doubles.Sort();
            Double sizeOfPocket = Math.Abs((maxValue - minValue) / pockets);

            int[] distribution = new int[pockets];
            double currentMin = minValue;

            for (int i = 0; i < distribution.Length; i++)
            {
                for (int j = 0; j < doubles.Count; j++)
                {
                    if (doubles[j] > currentMin && doubles[j] < currentMin + sizeOfPocket)
                    {
                        distribution[i]++;
                    }
                }

                currentMin += sizeOfPocket;
            }

            List<int> distributionBackup = new List<int>(distribution);
            while (distributionBackup.Contains(0)) distributionBackup.Remove(0);


            distribution = distributionBackup.ToArray();
            return new ChartValues<int>(distribution);
        }
        public static string[] GetLabelsForStudentDistribution(List<Student> studentList, int pockets)
        {
            string[] labels = new string[pockets];
            Double minValue = 4;
            Double maxValue = -4;

            foreach (var stud in studentList)
            {
                minValue = minValue > stud.Level ? stud.Level : minValue;
                maxValue = maxValue < stud.Level ? stud.Level : maxValue;
            }

            Double current = minValue;
            Double step = (maxValue - minValue) / pockets;
            for (int i = 0; i < pockets; i++)
            {
                labels[i] = Math.Round(current, 2).ToString();
                current+=step;
                labels[i] += "-" + Math.Round(current, 2).ToString();
            }

            return labels;
        }
    }
}