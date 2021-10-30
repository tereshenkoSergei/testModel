using System;

namespace TestModel.Code.Transacts
{
    public class Task
    {
        public long Id { get; set; }
        public double Complexity { get; set; }
        public double GuessingProbability { get; set; }

        public int Weight { get; set; }
        //Boolean isUsed (should be used?)

        public const double Sensitiveness = 1.71;
        private static long _currentId = 0L;

        public Task(double complexity, double guessingProbability, int weight)
        {
            Complexity = complexity;
            GuessingProbability = guessingProbability;
            Weight = weight;
        }

        public Task(double complexity, double guessingProbability) : this(complexity, guessingProbability, 1)
        {
        }

        public string ToString()
        {
            return "Task{" +
                   "id=" + Id +
                   ", complexity=" + Math.Round(Complexity, 3) +
                   ", guessingProbability=" + Math.Round(GuessingProbability, 3) +
                   ", weight=" + Weight +
                   '}';
        }
    }
}