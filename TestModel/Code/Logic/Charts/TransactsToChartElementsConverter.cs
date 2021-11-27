using System;
using System.Collections.Generic;
using System.Windows.Documents;
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
            foreach (var s in studentList) doubles.Add(s.Level);
            return DoubleListToChartValues(doubles, pockets);
        }

        public static ChartValues<int> GetTaskDistribution(List<Task> taskList, int pockets)
        {
            List<double> doubles = new List<double>();
            foreach (var s in taskList) doubles.Add(s.Complexity);
            return DoubleListToChartValues(doubles, pockets);
        }

        public static string[] GetLabelsForStudentDistribution(List<Student> studentList, int pockets)
        {
            List<Double> doubles = new List<double>();
            foreach (var s in studentList) doubles.Add(s.Level);
            return GetLablesForDoubles(doubles, pockets);
        }

        public static string[] GetLabelsForTaskDistribution(List<Task> taskList, int pockets)
        {
            List<Double> doubles = new List<double>();
            foreach (var s in taskList) doubles.Add(s.Complexity);
            return GetLablesForDoubles(doubles, pockets);
        }

        private static string[] GetLablesForDoubles(List<Double> doubles, int pockets)
        {
            doubles.Sort();
            string[] labels = new string[pockets];
            Double minValue = 4;
            Double maxValue = -4;

            foreach (var d in doubles)
            {
                minValue = minValue > d? d: minValue;
                maxValue = maxValue < d? d: maxValue;
            }

            Double current = minValue;
            Double step = (maxValue - minValue) / pockets;
            for (int i = 0; i < pockets; i++)
            {
                labels[i] = Math.Round(current, 2).ToString();
                current += step;
                labels[i] += "-" + Math.Round(current, 2).ToString();
            }

            return labels;
        }
        private static ChartValues<int> DoubleListToChartValues(List<Double> doubles, int pockets)
        {
            
            doubles.Sort();
            double minValue = doubles[0];
            double maxValue = doubles[doubles.Count-1];
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
    }
}