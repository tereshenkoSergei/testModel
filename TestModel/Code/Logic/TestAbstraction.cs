using System;
using TestModel.Code.Transacts;
using Task = TestModel.Code.Transacts.Task;

namespace TestModel.Code.Logic
{
    public abstract class TestAbstraction
    {
        private Task _task;
        private Student _student;

        public Task Task => _task;
        public Student Student => _student;

        public double GetSuccessProbability()
        {
            double successProbability;
            successProbability = (
                Task.GuessingProbability +
                (1 - Task.GuessingProbability) *
                Math.Exp(Task.Sensitiveness *
                         (Student.Level - Task.Complexity))
                /
                (1 + Math.Exp(Task.Sensitiveness *
                              (Student.Level - Task.Complexity)))
            );
            successProbability = Math.Round(successProbability, 8);
            return successProbability;
        }
    }
}