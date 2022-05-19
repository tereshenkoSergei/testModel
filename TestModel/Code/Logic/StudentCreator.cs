using System;
using System.Collections.Generic;
using TestModel.Code.Transacts;

namespace TestModel.Code.Logic
{
    public class StudentCreator
    {
        public static List<Student> GenerateStudents(int amount, Double minLevel, Double maxLevel)
        {
            List<Student> studentList = new List<Student>();
            Random random = new Random();
            for (int i = 0; i < amount; i++)
            {
                studentList.Add(new Student(Math.Round(minLevel + (maxLevel - minLevel) * random.NextDouble(), 3)));
            }

            //studentList.Sort((student1, student2) => (int) ((student1.Level - student2.Level) * 10000));
            return studentList;
        }

        public static List<Student> EquidistantStudentDistribution(
            int amountPerPocket,
            int pockets,
            Double minLevel,
            Double maxLevel)
        {
            List<Student> studentList = new List<Student>();
            double step = (maxLevel - minLevel) / pockets;

            double currentMinLevel = minLevel;
            double currentMaxLevel = minLevel + step;

            for (int i = 0; i < pockets; i++)
            {
                studentList.AddRange(GenerateStudents(
                    amountPerPocket,
                    currentMinLevel,
                    currentMaxLevel
                ));
                currentMinLevel += step;
                currentMaxLevel += step;
            }

            return studentList;
        }

        public static List<Student> NormalStudentDistribution(int amount, double median, double deviation)
        {
            List<double> dbs = new List<double>();
            Random random = new Random();
            List<Student> students = new List<Student>();

            for (int i = 0; i < amount; i++)
            {
                double randomDouble = random.NextDouble() * 8 - 4;
               //randomDouble = i * (8/(double)amount) - 4;
                dbs.Add(randomDouble);

                double level = 1 / (deviation * Math.Sqrt(2 * Math.PI));

                double temp = (median - randomDouble) / deviation;
                temp = temp * temp;
                temp = -0.5 * temp;
                temp = Math.Pow(Math.E, temp);
                if ((int) (random.NextDouble() * 1000) % 2 == 1) temp *= -1;
                level = level * temp;

                level *= 10;
                level = Math.Round(level, 3);

                level += median;
                students.Add(new Student(level));
            }

            return students;
        }

        public static List<Student> BoxMethodDistribution(
            int amount,
            int minLevel,
            int maxLevel
        )
        {
            List<Student> students = new List<Student>();

            return students;
        }
    }
}