using System;

namespace TestModel.Code.Transacts
{
    public class Student
    {
        public long Id { get; set; }
        public double Level { get; set; }
        private static long currentId;

        public Student(double level)
        {
            currentId++;
            Id = currentId;
            Level = level;
        } 

        public string ToString()
        {
            return '[' + Id + "] " + Math.Round(Level, 3);
        }
    }
}