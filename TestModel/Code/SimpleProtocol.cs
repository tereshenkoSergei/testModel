using System;
using System.Collections.Generic;
using TestModel.Code.Transacts;

namespace TestModel.Code
{
    public class SimpleProtocol
    {
        public Student Student { get; }
        public String TestingMethod { get; }
        public String EvaluationMethod { get; }
        public Dictionary<Task, bool> CompletedTasks { get; }
        public Result Result { get; }
    }
}