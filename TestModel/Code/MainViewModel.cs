using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
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
        private int _selectedStudentId = 0;
        private int _selectedTaskId = 0;
        private int _selectedTaskIdResult = 0;
        private ObservableCollection<Student> _studentList;
        private ObservableCollection<Task> _taskList;
        private Student SelectedStudent { get; set; }
        private Task SelectedTask { get; set; }
        private Task SelectedTaskResult { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Result _globalResult;
        public RelayCommand GenerateTransactsCommand { get; set; }
        public RelayCommand IncrementStudentAmountCommand { get; set; }
        public RelayCommand DecrementStudentAmountCommand { get; set; }
        public RelayCommand IncrementTaskAmountCommand { get; set; }
        public RelayCommand DecrementTaskAmountCommand { get; set; }
        public RelayCommand RunTestsCommand { get; set; }
        public RelayCommand AddTransactsCommand { get; set; }
        public RelayCommand ClearTransactsCommand { get; set; }
        public SeriesCollection StudentSeriesCollection { get; set; }
        public SeriesCollection TaskSeriesCollection { get; set; }
        public SeriesCollection ResultSeriesCollection { get; set; }
        public string[] StudentLabels { get; set; }
        public string[] TaskLabels { get; set; }
        public Func<int, string> Formatter { get; set; }

        public CartesianChart MainCartesianChart { get; set; }
        public CartesianChart TaskCompletionCartesianChart { get; set; }
        public CartesianChart DifferenceCartesianChart { get; set; }
        public const int POCKETS = 12;

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
                if (_minLevel > 4) _minLevel = 4;
                if (_minLevel < -4) _minLevel = -4;
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
                if (_maxLevel > 4) _maxLevel = 4;
                if (_maxLevel < -4) _maxLevel = -4;
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
                if (_minComplexity > 4) _minComplexity = 4;
                if (_minComplexity < -4) _minComplexity = -4;
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
                if (_maxComplexity > 4) _maxComplexity = 4;
                if (_maxComplexity < -4) _maxComplexity = -4;
                OnPropertyChanged(nameof(MaxComplexity));
                OnPropertyChanged(nameof(MinComplexity));
            }
        }

        public ObservableCollection<Student> StudentList
        {
            get => _studentList;
            set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentList));
            }
        }

        public int SelectedStudentId
        {
            get
            {
                if (StudentList.Count <= 0)
                {
                    SelectedStudent = new Student(0);
                }
                else
                {
                    if (_selectedStudentId < 0) _selectedStudentId = 0;

                    SelectedStudent = StudentList[_selectedStudentId];
                }

                return _selectedStudentId;
            }
            set
            {
                _selectedStudentId = value;

                if (StudentList.Count <= 0) return;
                if (_selectedStudentId < 0)
                {
                    _selectedStudentId = 0;
                    SelectedStudent = StudentList[_selectedStudentId];
                }

                OnPropertyChanged(nameof(StudentList));
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        public int SelectedTaskId
        {
            get
            {
                if (TaskList.Count <= 0)
                {
                    SelectedTask = new Task(0, 0);
                }
                else
                {
                    if (_selectedTaskId < 0) _selectedTaskId = 0;

                    SelectedTask = TaskList[_selectedTaskId];
                }

                return _selectedTaskId;
            }
            set
            {
                _selectedTaskId = value;

                if (TaskList.Count <= 0) return;
                if (_selectedTaskId < 0)
                {
                    _selectedTaskId = 0;
                    SelectedTask = TaskList[_selectedTaskId];
                }

                OnPropertyChanged(nameof(TaskList));
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public int SelectedTaskIdResult
        {
            get
            {
                if (TaskList.Count <= 0)
                {
                    SelectedTaskResult = new Task(0, 0);
                }
                else
                {
                    if (_selectedTaskIdResult < 0) _selectedTaskIdResult = 0;

                    SelectedTaskResult = TaskList[_selectedTaskIdResult];
                }

                return _selectedTaskIdResult;
            }
            set
            {
                _selectedTaskIdResult = value;

                if (TaskList.Count <= 0) return;
                if (_selectedTaskIdResult < 0)
                {
                    _selectedTaskIdResult = 0;
                    SelectedTaskResult = TaskList[_selectedTaskIdResult];
                }

                {
                    if (_globalResult != null)
                        TaskCompletionCartesianChart.Series = new SeriesCollection
                        {
                            new LineSeries
                            {
                                Values = TransactsToChartElementsConverter.GetResultDistribution(_globalResult, _selectedTaskIdResult, POCKETS),
                            }
                        };
                }

                OnPropertyChanged(nameof(TaskList));
                OnPropertyChanged(nameof(SelectedTaskResult));
            }
        }

        public ObservableCollection<Task> TaskList
        {
            get => _taskList;
            set
            {
                _taskList = value;
                OnPropertyChanged(nameof(TaskList));
            }
        }

        #endregion

        public MainViewModel()
        {
            GenerateTransactsCommand = new RelayCommand(GenerateTransacts, CanGenerateTransacts);
            IncrementStudentAmountCommand = new RelayCommand(IncrementStudentAmount, CanIncrementStudentAmount);
            DecrementStudentAmountCommand = new RelayCommand(DecrementStudentAmount, CanDecrementStudentAmount);
            IncrementTaskAmountCommand = new RelayCommand(IncrementTaskAmount, CanIncrementTaskAmount);
            DecrementTaskAmountCommand = new RelayCommand(DecrementTaskAmount, CanDecrementTaskAmount);
            RunTestsCommand = new RelayCommand(RunTests, CanRunTests);
            AddTransactsCommand = new RelayCommand(AddTransacts, CanAddTransacts);
            ClearTransactsCommand = new RelayCommand(ClearTransacts, CanClearTransacts);

            MainCartesianChart = new CartesianChart();
            DifferenceCartesianChart = new CartesianChart();
            MainCartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> {0}
                }
            };

            TaskCompletionCartesianChart = new CartesianChart();
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

        public void AddTransacts(object param)
        {
            ObservableCollection<Student> studentsToAdd;
            ObservableCollection<Task> tasksToAdd;

            studentsToAdd = new ObservableCollection<Student>(
                StudentCreator.GenerateStudents(StudentAmount, MinLevel, MaxLevel));

            Thread.Sleep(1);
            tasksToAdd = new ObservableCollection<Task>(
                TaskCreator.GenerateTasks(TaskAmount,
                    MinComplexity,
                    MaxComplexity,
                    MinGuessingProbability / 100,
                    MaxGuessingProbability / 100));

            if (StudentList == null) StudentList = new ObservableCollection<Student>();
            if (TaskList == null) TaskList = new ObservableCollection<Task>();

            foreach (var student in studentsToAdd) StudentList.Add(student);
            foreach (var task in tasksToAdd) TaskList.Add(task);

            OnPropertyChanged(nameof(StudentList));
            OnPropertyChanged(nameof(TaskList));
        }

        public void ClearTransacts(object param)
        {
            StudentList = new ObservableCollection<Student>();
            TaskList = new ObservableCollection<Task>();
            OnPropertyChanged(nameof(StudentList));
            OnPropertyChanged(nameof(TaskList));
        }

        public bool CanClearTransacts(object param)
        {
            return true;
        }

        public bool CanAddTransacts(object param)
        {
            return true;
        }

        public void GenerateTransacts(object param)
        {
            StudentList =
                new ObservableCollection<Student>(StudentCreator.GenerateStudents(StudentAmount, MinLevel, MaxLevel));
            //StudentList = StudentCreator.NormalStudentDistribution(StudentAmount, 0, 1);
            //StudentList = StudentCreator.EquidistantStudentDistribution(10, 20, -4, 4);
            //StudentList = StudentCreator.NormalStudentDistribution(1000, 0, 1);
            TaskList = new ObservableCollection<Task>(
                TaskCreator.GenerateTasks(TaskAmount,
                    MinComplexity,
                    MaxComplexity,
                    MinGuessingProbability / 100,
                    MaxGuessingProbability / 100));


            StudentSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = TransactsToChartElementsConverter.GetStudentDistribution(StudentList.ToList(), POCKETS)
                }
            };
            StudentLabels =
                TransactsToChartElementsConverter.GetLabelsForStudentDistribution(StudentList.ToList(), POCKETS);
            Formatter = value => value.ToString("N");

            TaskSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = TransactsToChartElementsConverter.GetTaskDistribution(TaskList.ToList(), POCKETS)
                }
            };
            TaskLabels = TransactsToChartElementsConverter.GetLabelsForTaskDistribution(TaskList.ToList(), POCKETS);
            Formatter = value => value.ToString("N");


            OnPropertyChanged(nameof(ResultSeriesCollection));
            OnPropertyChanged(nameof(StudentSeriesCollection));
            OnPropertyChanged(nameof(TaskSeriesCollection));
            OnPropertyChanged(nameof(MainCartesianChart));
        }

        public void RunTests(object param)
        {
            Result result = new StandardTesterModeler()
                .RunTest(StudentList.ToList(), TaskList.ToList());


            Dictionary<int, KeyValuePair<double, double>> resultDictionary = result.GetResultDictionary();
            List<Double> studentLevels = new List<double>();
            List<Double> resultsOfStudents = new List<double>();
            List<Double> difference = new List<double>();
            List<double> zeroLine = new List<double>();
            List<double> trustUp = new List<double>();
            List<double> trustDown = new List<double>();


            foreach (var res in resultDictionary)
            {
                studentLevels.Add(((double) 100 / 8) * (res.Value.Key + 4));
                resultsOfStudents.Add(((double) 100 / 8) * (res.Value.Value + 4));
                difference.Add(((double) 100 / 8) * (res.Value.Value - res.Value.Key));
                zeroLine.Add(0);
                trustUp.Add(10);
                trustDown.Add(-10);
            }


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
                },
                new LineSeries
                {
                    Title = "dif",
                    Values = new ChartValues<double>(difference)
                }
            };
            DifferenceCartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "dif",
                    Values = new ChartValues<double>(difference)
                },
                new LineSeries
                {
                    Title = "0",
                    Values = new ChartValues<double>(zeroLine)
                },
                new LineSeries
                {
                    Title = "Доверительный интервал верхний",
                    Values = new ChartValues<double>(trustUp)
                },
                new LineSeries
                {
                    Title = "Доверительный интервал нижний",
                    Values = new ChartValues<double>(trustDown)
                }
            };
            _globalResult = result;
            TaskCompletionCartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = TransactsToChartElementsConverter.GetResultDistribution(result, 0, POCKETS),
                }
            };
        }

        public bool CanRunTests(object param)
        {
            return (StudentList?.Count > 0 && TaskList?.Count > 0);
        }

        public bool CanGenerateTransacts(object param)
        {
            return true;
        }
    }
}