using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TestModel.Annotations;
using LiveCharts;
using LiveCharts.Charts;
using LiveCharts.Wpf;
using TestModel.Code.Additions;
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
        private double _minGuessingProbability = 0;
        private double _maxGuessingProbability = 0;
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
                OnPropertyChanged(nameof(MaxLevel));
            }
        }

        public double MaxLevel
        {
            get => Math.Round(_maxLevel, 3);
            set
            {
                _maxLevel = value;
                OnPropertyChanged(nameof(MaxLevel));
                OnPropertyChanged(nameof(MinLevel));
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
                OnPropertyChanged(nameof(MaxComplexity));
            }
        }

        public double MinGuessingProbability
        {
            get => Math.Round(_minGuessingProbability, 0);
            set
            {
                _minGuessingProbability = value;
                OnPropertyChanged(nameof(MinGuessingProbability));
                OnPropertyChanged(nameof(MaxGuessingProbability));
            } 
        }

        public double MaxGuessingProbability
        {
            get => Math.Round(_maxGuessingProbability, 0);
            set
            {
                _maxGuessingProbability = value;
                OnPropertyChanged(nameof(MaxGuessingProbability));
                OnPropertyChanged(nameof(MinGuessingProbability));
            } 
        }

        public double MaxComplexity
        {
            get => Math.Round(_maxComplexity, 3);
            set
            {
                _maxComplexity = value;
                OnPropertyChanged(nameof(MaxComplexity));
                OnPropertyChanged(nameof(MinComplexity));
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
        public RelayCommand GenerateTransactsCommand { get; set; }
        public RelayCommand IncrementStudentAmountCommand { get; set; }
        public RelayCommand DecrementStudentAmountCommand { get; set; }
        public RelayCommand IncrementTaskAmountCommand { get; set; }
        public RelayCommand DecrementTaskAmountCommand { get; set; }

        #endregion

        public SeriesCollection StudentSeriesCollection { get; set; }
        public SeriesCollection TaskSeriesCollection { get; set; }
        public SeriesCollection ResultSeriesCollection { get; set; }
        public string[] StudentLabels { get; set; }
        public string[] TaskLabels { get; set; }
        public Func<int, string> Formatter { get; set; }

        public CartesianChart MainCartesianChart { get; set; }
        public const int POCKETS = 12;
        
        
        

        public MainViewModel()
        {
            GenerateTransactsCommand = new RelayCommand(GenerateTransacts, CanGenerateTransacts);
            IncrementStudentAmountCommand = new RelayCommand(IncrementStudentAmount, CanIncrementStudentAmount);
            DecrementStudentAmountCommand = new RelayCommand(DecrementStudentAmount, CanDecrementStudentAmount);
            IncrementTaskAmountCommand = new RelayCommand(IncrementTaskAmount, CanIncrementTaskAmount);
            DecrementTaskAmountCommand = new RelayCommand(DecrementTaskAmount, CanDecrementTaskAmount);
            
            MainCartesianChart = new CartesianChart();
            MainCartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 1, 1, 2, 3 ,5 }
                }
            };
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void IncrementStudentAmount(object param)
        {
            StudentAmount++;
            OnPropertyChanged(nameof(StudentAmount));
        }

        public bool CanIncrementStudentAmount(object param)
        {
            return true;
        }

        public void DecrementStudentAmount(object param)
        {
            StudentAmount--;
            OnPropertyChanged(nameof(StudentAmount));
        }

        public bool CanDecrementStudentAmount(object param)
        {
            return StudentAmount > 0;
        }

        public void IncrementTaskAmount(object param)
        {
            TaskAmount++;
            OnPropertyChanged(nameof(TaskAmount));
        }

        public bool CanIncrementTaskAmount(object param)
        {
            return true;
        }

        public void DecrementTaskAmount(object param)
        {
            TaskAmount--;
            OnPropertyChanged(nameof(TaskAmount));
        }

        public bool CanDecrementTaskAmount(object param)
        {
            return TaskAmount > 0;
        }


        public void GenerateTransacts(object param)
        {
            StudentList = StudentCreator.GenerateStudents(StudentAmount, MinLevel, MaxLevel);
            //StudentList = StudentCreator.NormalStudentDistribution(StudentAmount, 0, 1);
            //StudentList = StudentCreator.EquidistantStudentDistribution(10, 20, -4, 4);
            //StudentList = StudentCreator.NormalStudentDistribution(1000, 0, 1);
            TaskList = TaskCreator.GenerateTasks(TaskAmount, MinComplexity, MaxComplexity, MinGuessingProbability/100, MaxGuessingProbability/100);


            StudentSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = TransactsToChartElementsConverter.GetStudentDistribution(StudentList, POCKETS)
                }
            };
            StudentLabels = TransactsToChartElementsConverter.GetLabelsForStudentDistribution(StudentList, POCKETS);
            Formatter = value => value.ToString("N");

            TaskSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = TransactsToChartElementsConverter.GetTaskDistribution(TaskList, POCKETS)
                }
            };
            TaskLabels = TransactsToChartElementsConverter.GetLabelsForTaskDistribution(TaskList, POCKETS);
            Formatter = value => value.ToString("N");

            Dictionary<Double, double> result = new StandardTesterModeler().RunTest(StudentList, TaskList)
                .GetResultDictionary();

            List<Double> studentLevels = new List<double>();
            List<Double> resultsOfStudents = new List<double>();
            List<Double> plug = new List<double>();

            foreach (var res in result)
            {
                studentLevels.Add(res.Key);
                resultsOfStudents.Add(res.Value);
                plug.Add(0);
            }
            



            ResultSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<Double>(studentLevels)
                },
                new ColumnSeries
                {
                    Values = new ChartValues<Double>(resultsOfStudents)
                },
                new ColumnSeries
                {
                    Values = new ChartValues<Double>(plug)
                }
            };

            
            MainCartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Results",
                    Values = new ChartValues<double>(resultsOfStudents)
                },
                new LineSeries
                {
                    Title = "Levels",
                    Values = new ChartValues<double>(studentLevels)
                }
            };
            
            OnPropertyChanged(nameof(ResultSeriesCollection));
            OnPropertyChanged(nameof(StudentSeriesCollection));
            OnPropertyChanged(nameof(TaskSeriesCollection));
            OnPropertyChanged(nameof(MainCartesianChart));
        }

        public bool CanGenerateTransacts(object param)
        {
            return true;
        }
    }
}