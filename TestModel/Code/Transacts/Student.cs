using System;

namespace TestModel.Code.Transacts
{
    public class Student
    {
        private double _level;
        private static long currentId = 0;

        public long Id { get; set; }

        public double Level
        {
            get => _level;
            set
            {
                _level = value;
                if (_level < -4) _level = -4;
                if (_level > 4) _level = 4;
            }
        }


        public Student(double level)
        {
            Id = currentId;
            currentId++;
            Level = level;
        } 

        public string ToString()
        {
            return '[' + Id + "] " + Math.Round(Level, 3);
        }
    }
}