using System.Collections.Generic;
using TestModel.Code.Transacts;

namespace TestModel.Code.Logic
{
    public abstract class AbstractTestModeler
    {
        public abstract Result RunTest(List<Student> studentList, List<Task> taskList);
    }
}