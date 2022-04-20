using System;

namespace TestModel.Code.Transacts
{
    public class Task
    {
        private double _complexity;
        private static long currentId = 0;
        public long Id { get; set; }

        public double Complexity
        {
            get => _complexity;
            set
            {
                _complexity = value;
                if (_complexity < -4) _complexity = -4;
                if (_complexity > 4) _complexity = 4;
            }
        }

        public double GuessingProbability { get; set; }

        public int Weight { get; set; }
        //Boolean isUsed (should be used?)

        public const double Sensitiveness = 1.71;
        private static long _currentId = 0L;

        public Task(double complexity, double guessingProbability, int weight)
        {
            Id = currentId;
            currentId++;
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