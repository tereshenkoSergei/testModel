using System.Collections.Generic;
using TestModel.Code.Transacts;

namespace TestModel.Code
{
    public class Result
    {
        public int[,] ResultMatrix { get; }
        private List<Student> StudentList { get; }
        private List<Task> TaskList { get; }
        
        public Result(List<Student> studentList, List<Task> taskList) {
            StudentList = studentList;
            TaskList = taskList;
            ResultMatrix = new int[studentList.Count, taskList.Count];
        }
        public void SetAnswerTo(int x, int y, int value) {
            ResultMatrix[x,y] = value;
            ResultMatrix[x,y] = value;
        }
    }
}