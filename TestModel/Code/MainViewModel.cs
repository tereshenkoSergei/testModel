using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using TestModel.Annotations;
using LiveCharts;
using LiveCharts.Wpf;
using TestModel.Code.Logic;
using TestModel.Code.Transacts;

namespace TestModel.Code
{
    internal class MainViewModel : FrameworkElement, INotifyPropertyChanged
    {
        #region Vars

        private int _studentAmount;
        private int _taskAmount;
        private double _minComplexity;
        private double _maxComplexity;
        private List<Student> _studentList;
        private List<Task> _taskList;


        public int StudentAmount
        {
            get => _studentAmount;
            set
            {
                _studentAmount = value;
                OnPropertyChanged(nameof(StudentAmount));
            }
        }

        public int TaskAmount
        {
            get => _taskAmount;
            set
            {
                _taskAmount = value;
                OnPropertyChanged(nameof(TaskAmount));
            }
        }

        public double MinComplexity
        {
            get => Math.Round(_minComplexity, 3);
            set
            {
                _minComplexity = value;
                OnPropertyChanged(nameof(MinComplexity));
            }
        }

        public double MaxComplexity
        {
            get => Math.Round(_maxComplexity, 3);
            set
            {
                _maxComplexity = value;
                OnPropertyChanged(nameof(MaxComplexity));
            }
        }

        public List<Student> StudentList
        {
            get => _studentList;
            set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentList));
            }
        }

        public List<Task> TaskList
        {
            get => _taskList;
            set
            {
                _taskList = value;
                OnPropertyChanged(nameof(TaskList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<int, string> YFormatter { get; set; }

        private int pockets = 10;

        public MainViewModel()
        {
            StudentList = StudentCreator.GenerateStudents(500, -4, 4);
            //StudentList = StudentCreator.EquidistantStudentDistribution(10, 20, -4, 4);
            //StudentList = StudentCreator.NormalStudentDistribution(1000, 0, 1);
            TaskList = TaskCreator.GenerateTasks(10, -4, 4);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}