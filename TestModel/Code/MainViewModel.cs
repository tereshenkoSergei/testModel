using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TestModel.Annotations;
using LiveCharts;
using LiveCharts.Wpf;
using TestModel.Code.Logic;
using TestModel.Code.Logic.Charts;
using TestModel.Code.Transacts;

namespace TestModel.Code
{
    internal class MainViewModel : FrameworkElement, INotifyPropertyChanged
    {
        #region Vars

        private int _studentAmount = 10;
        private int _taskAmount = 10;
        private double _minComplexity = -4;
        private double _maxComplexity = 4;
        private double _minLevel = -4;
        private double _maxLevel = 4;
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
        public double MinLevel
        {
            get => Math.Round(_minLevel, 3);
            set
            {
                _minLevel = value;
                OnPropertyChanged(nameof(MinLevel));
            }
        }
        public double MaxLevel
        {
            get => Math.Round(_maxLevel, 3);
            set
            {
                _maxLevel = value;
                OnPropertyChanged(nameof(MaxLevel));
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
        
        public RelayCommand GenerateStudentsCommand { get; set; }
        #endregion
        
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<int, string> Formatter { get; set; }
        public MainViewModel()
        {
           GenerateStudentsCommand = new RelayCommand(GenerateTransacts, CanGenerateTransacts);

        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
        public void GenerateTransacts(object param)
        {
            StudentList = StudentCreator.NormalStudentDistribution(StudentAmount, 0, 1);
            //StudentList = StudentCreator.EquidistantStudentDistribution(10, 20, -4, 4);
            //StudentList = StudentCreator.NormalStudentDistribution(1000, 0, 1);
            TaskList = TaskCreator.GenerateTasks(TaskAmount, MinComplexity, MaxComplexity);
            
            
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = TransactsToChartElementsConverter.GetStudentDistribution(StudentList, 10)
                }
            };
            Labels = TransactsToChartElementsConverter.GetLabelsForStudentDistribution(StudentList, 10);
            Formatter = value => value.ToString("N");
            
            OnPropertyChanged(nameof(SeriesCollection));
        }

        public bool CanGenerateTransacts(object param)
        {
            return true;
        }
    }
}