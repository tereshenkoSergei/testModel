using System.Collections.ObjectModel;
using TestModel.Code.Transacts;

namespace TestModel.Code
{

    internal partial class MainViewModel
    {
        public bool CanRunTests(object param)
        {
            return (StudentList?.Count > 0 && TaskList?.Count > 0);
        }

        public bool CanGenerateTransacts(object param)
        {
            return true;
        }

        public bool CanClearTransacts(object param)
        {
            return true;
        }

        public bool CanAddTransacts(object param)
        {
            return true;
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

        public void ClearTransacts(object param)
        {
            StudentList = new ObservableCollection<Student>();
            TaskList = new ObservableCollection<Task>();
            OnPropertyChanged(nameof(StudentList));
            OnPropertyChanged(nameof(TaskList));
        }
    }
}