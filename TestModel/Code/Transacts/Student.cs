using System;

namespace TestModel.Code.Transacts
{
    public class Student : IComparable
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

        public int CompareTo(object obj)
        {
            if(!(obj is Student)) throw  new ArgumentException();
            Student compareStudent = (Student) obj;
            return (int) ((this._level - compareStudent._level) * 1000);
        }
    }
}