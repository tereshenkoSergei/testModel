using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    internal partial class MainViewModel : FrameworkElement, INotifyPropertyChanged
    {
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
            SelectedStudentGenerationMethodIndex = 0;
            SelectedTaskGenerationMethodIndex = 0;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public void GenerateTransacts(object param)
        {
            GenerateStudents();
            GenerateTasks();
            //StudentList = StudentCreator.NormalStudentDistribution(StudentAmount, 0, 1);
            //StudentList = StudentCreator.EquidistantStudentDistribution(10, 20, -4, 4);
            //StudentList = StudentCreator.NormalStudentDistribution(1000, 0, 1);


            FormatCharts();

            OnPropertyChanged(nameof(ResultSeriesCollection));
            OnPropertyChanged(nameof(StudentSeriesCollection));
            OnPropertyChanged(nameof(TaskSeriesCollection));
            OnPropertyChanged(nameof(MainCartesianChart));
        }

        private void GenerateStudents()
        {
            switch (SelectedStudentGenerationMethodIndex)
            {
                case 0:
                    StudentList =
                        new ObservableCollection<Student>(
                            StudentCreator.GenerateStudents(StudentAmount, MinLevel, MaxLevel));
                    break;
                case 1:
                    StudentList =
                        new ObservableCollection<Student>(
                            StudentCreator.NormalStudentDistribution(StudentAmount, StudentMedian, StudentDeviation));
                    break;
                case 2:
                    break;
            }
        }

        private void GenerateTasks()
        {
            switch (SelectedTaskGenerationMethodIndex)
            {
                case 0:
                    TaskList = new ObservableCollection<Task>(
                        TaskCreator.GenerateTasks(TaskAmount,
                            MinComplexity,
                            MaxComplexity,
                            MinGuessingProbability / 100,
                            MaxGuessingProbability / 100));
                    break;
                case 1:
                    TaskList = new ObservableCollection<Task>(
                        
                        TaskCreator.NormalTaskDistribution(
                            TaskAmount,
                            TaskMedian,
                            TaskDeviation,
                            MinGuessingProbability,
                            MaxGuessingProbability
                            ));
                    break;
                case 2:
                    break;
            }
        }

        private void FormatCharts()
        {
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


        public void Plug()
        {
        }
    }
}