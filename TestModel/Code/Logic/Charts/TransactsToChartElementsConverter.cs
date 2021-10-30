using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using TestModel.Code.Transacts;

namespace TestModel.Code.Logic.Charts
{
    public static class TransactsToChartElementsConverter
    {
        public static LineSeries ConvertStudentsToSeriesCollection(List<Student> studentList)
        {
            List<double> doubles = new List<double>();
            foreach (var stud in studentList)
            {
                doubles.Add(stud.Level);
            }

            doubles.Sort();
            return new LineSeries {Values = new ChartValues<double>(doubles)};
        }

        public static LineSeries GetStudentDistribution(List<Student> studentList)
        {
            int pockets = 160;
            List<double> doubles = new List<double>();
            double minValue = 4;
            double maxValue = -4;
            double step = 0.05;
            foreach (var stud in studentList)
            {
                doubles.Add(stud.Level);
                minValue = minValue > stud.Level ? stud.Level : minValue;
                maxValue = maxValue < stud.Level ? stud.Level : maxValue;
            }

            doubles.Sort();
            int[] distribution = new int[pockets];

            for (int i = 0; i < doubles.Count; i++)
            {
                double currentMin = -4;
                for (int j = 0; j < pockets; j++)
                {
                    if (doubles[i] > currentMin && doubles[i] < currentMin + step)
                    {
                        distribution[j]++;
                    }

                    currentMin += step;
                }
            }

            ChartValues<int> values = new ChartValues<int>(distribution);
            return new LineSeries {Values = values};
        }


        public static LineSeries GetStudentDistributionExperemental(List<Student> studentList)
        {
            List<double> doubles = new List<double>();
            double minValue = 4;
            double maxValue = -4;
            double step = 0.01;
            foreach (var stud in studentList)
            {
                doubles.Add(stud.Level);
                minValue = minValue > stud.Level ? stud.Level : minValue;
                maxValue = maxValue < stud.Level ? stud.Level : maxValue;
            }

            doubles.Sort();
            int[] distribution = new int[studentList.Count];

            for (int i = 0; i < doubles.Count; i++)
            {
                double currentMin = -4;
                for (int j = 0; j < studentList.Count; j++)
                {
                    if (doubles[i] > currentMin && doubles[i] < currentMin + step)
                    {
                        distribution[j]++;
                    }

                    currentMin += step;
                }
            }

            List<int> distributionBackup = new List<int>(distribution);
           while (distributionBackup.Contains(0)) distributionBackup.Remove(0);


            distribution = distributionBackup.ToArray();
            ChartValues<int> values = new ChartValues<int>(distribution);
            return new LineSeries {Values = values};
        }
    }
}