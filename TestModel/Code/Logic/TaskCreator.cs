using System;
using System.Collections.Generic;
using TestModel.Code.Transacts;

namespace TestModel.Code.Logic
{
    public class TaskCreator
    {
        public static List<Task> GenerateTasks(int amount,
            double minComplexity,
            double maxComplexity,
            double minGuessingProbability,
            double maxGuessingProbability,
            int minWeight,
            int maxWeight
        )
        {
            if (maxComplexity < minComplexity || maxGuessingProbability < minGuessingProbability ||
                maxWeight < minWeight || minWeight < 0)
            {
                throw new ArgumentException("Максимальное не может быть меньше минимального");
            }

            Random random = new Random();
            List<Task> taskList = new List<Task>();


            for (int i = 0; i < amount; i++)
            {
                taskList.Add(new Task(
                    Math.Round(minComplexity + (maxComplexity - minComplexity) * random.NextDouble(), 3),
                    Math.Round(
                        minGuessingProbability +
                        (maxGuessingProbability - minGuessingProbability) * random.NextDouble(), 3),
                    random.Next(maxWeight - minWeight) + minWeight
                ));

                taskList.Sort((t1, t2) => (int) ((t1.Complexity - t2.Complexity) * 10000));
            }

            return taskList;
        }

        public static List<Task> GenerateTasks(int amount,
            double minComplexity,
            double maxComplexity,
            double minGuessingProbability,
            double maxGuessingProbability
        )
        {
            return GenerateTasks(amount, minComplexity, maxComplexity, minGuessingProbability, maxGuessingProbability,
                1, 2);
        }


        public static List<Task> GenerateTasks(int amount,
            double minComplexity,
            double maxComplexity
        )
        {
            return GenerateTasks(amount, minComplexity, maxComplexity, 0.0, 0.0);
        }


        public static List<Task> EquidistantTasksDistribution(int amountPerPocket,
            int pockets,
            double minComplexity,
            double maxComplexity,
            double minGuessingProbability,
            double maxGuessingProbability)
        {
            List<Task> taskList = new List<Task>();
            double step = (maxComplexity - minComplexity) / pockets;


            double currentMinComplexity = minComplexity;
            double currentMaxComplexity = minComplexity + step;
            for (int i = 0; i < pockets; i++)
            {
                taskList.AddRange(GenerateTasks(
                    amountPerPocket,
                    currentMinComplexity,
                    currentMaxComplexity,
                    minGuessingProbability,
                    maxGuessingProbability));

                currentMaxComplexity += step;
                currentMinComplexity += step;
            }

            return taskList;
        }
        
        public static List<Task> NormalStudentDistribution(
            int amount, 
            double median, 
            double deviation, 
            int minGuessingProbability,
            int maxGuessingProbability)
        {
            List<double> dbs = new List<double>();
            Random random = new Random();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < amount; i++)
            {
                double randomDouble = random.NextDouble() * 8 - 4;
                //randomDouble = i * (8/(double)amount) - 4;
                dbs.Add(randomDouble);

                double complexity = 1 / (deviation * Math.Sqrt(2 * Math.PI));

                double temp = (median - randomDouble) / deviation;
                temp = temp * temp;
                temp = -0.5 * temp;
                temp = Math.Pow(Math.E, temp);
                if ((int) (random.NextDouble() * 1000) % 2 == 1) temp *= -1;
                complexity = complexity * temp;

                complexity *= 10;
                complexity = Math.Round(complexity, 3);

                complexity += median;
                double guessingProbability = Math.Round(
                    minGuessingProbability +
                    (maxGuessingProbability - minGuessingProbability) * random.NextDouble(), 3);
                
                tasks.Add(new Task(complexity, guessingProbability));
            }

            return tasks;
        }
    }
}