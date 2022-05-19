using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using TestModel.Code.Logic.Charts;
using TestModel.Code.Transacts;

namespace TestModel.Code
{
  
    internal partial class MainViewModel
    {
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
        private int _selectedStudentGenerationMethodIndex = 0;
        private int _selectedTaskGenerationMethodIndex = 0;
        private double _studentMedian = 0;
        private double _studentDeviation = 1D;
  
        private double _taskMedian = 0;
        private double _taskDeviation = 1D;
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

        public double StudentMedian
        {
            get => Math.Round(_studentMedian, 3);
            set
            {
                _studentMedian = value;
                if (_studentMedian > 4) _studentMedian = 4;
                if (_studentMedian < -4) _studentMedian = -4;
                OnPropertyChanged(nameof(StudentMedian));
            }
        }

        public double StudentDeviation
        {
            get => Math.Round(_studentDeviation, 3);
            set
            {
                _studentDeviation = value;
                OnPropertyChanged(nameof(StudentDeviation));
            }
        }
        public double TaskMedian
        {
            get => Math.Round(_taskMedian, 3);
            set
            {
                _taskMedian = value;
                if (_taskMedian> 4) _taskMedian= 4;
                if (_taskMedian < -4) _taskMedian= -4;
                OnPropertyChanged(nameof(TaskMedian));
            }
        }

        public double TaskDeviation
        {
            get => Math.Round(_taskDeviation, 3);
            set
            {
                _taskMedian = value;
                OnPropertyChanged(nameof(TaskDeviation));
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
                                Values = TransactsToChartElementsConverter.GetResultDistribution(_globalResult,
                                    _selectedTaskIdResult, POCKETS),
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

        public Visibility StudentCommonMethodVisibility { get; set; }
        public Visibility StudentNormalMethodVisibility { get; set; }
        public Visibility StudentPocketMethodVisibility { get; set; }
        
        
        public Visibility TaskCommonMethodVisibility { get; set; }
        public Visibility TaskNormalMethodVisibility { get; set; }
        public Visibility TaskPocketMethodVisibility { get; set; }
        
        

        public int SelectedStudentGenerationMethodIndex
        {
            get => _selectedStudentGenerationMethodIndex;
            set
            {
                _selectedStudentGenerationMethodIndex = value;

                StudentCommonMethodVisibility = Visibility.Collapsed;
                StudentNormalMethodVisibility = Visibility.Collapsed;
                StudentPocketMethodVisibility = Visibility.Collapsed;
                switch (_selectedStudentGenerationMethodIndex)
                {
                    case 0:
                        StudentCommonMethodVisibility = Visibility.Visible;
                        break;
                    case 1:
                        StudentNormalMethodVisibility = Visibility.Visible;
                        break;
                    case 2:
                        StudentPocketMethodVisibility = Visibility.Visible;
                        break;
                }

                OnPropertyChanged(nameof(StudentCommonMethodVisibility));
                OnPropertyChanged(nameof(StudentNormalMethodVisibility));
                OnPropertyChanged(nameof(StudentPocketMethodVisibility));
                OnPropertyChanged(nameof(SelectedStudentGenerationMethodIndex));
            }
        }
        
        public int SelectedTaskGenerationMethodIndex
        {
            get => _selectedTaskGenerationMethodIndex ;
            set
            {
                _selectedTaskGenerationMethodIndex  = value;

                TaskCommonMethodVisibility = Visibility.Collapsed;
                TaskNormalMethodVisibility = Visibility.Collapsed;
                TaskPocketMethodVisibility = Visibility.Collapsed;
                switch (_selectedTaskGenerationMethodIndex )
                {
                    case 0:
                        TaskCommonMethodVisibility = Visibility.Visible;
                        break;
                    case 1:
                        TaskNormalMethodVisibility = Visibility.Visible;
                        break;
                    case 2:
                        TaskPocketMethodVisibility = Visibility.Visible;
                        break;
                }

                OnPropertyChanged(nameof(TaskCommonMethodVisibility));
                OnPropertyChanged(nameof(TaskNormalMethodVisibility));
                OnPropertyChanged(nameof(TaskPocketMethodVisibility));
                OnPropertyChanged(nameof(SelectedStudentGenerationMethodIndex));
            }
        }
        
        
    }

}