using System;
using System.Collections.Generic;
using TestModel.Code.Logic;
using TestModel.Code.Transacts;

namespace TestModel.Code.Additions
{
    public class StandardTesterModeler : AbstractTestModeler
    {
        public override Result RunTest(List<Student> studentList, List<Task> taskList) {
            Random random = new Random();
            Result result = new Result(studentList, taskList);
            double successProbability;

            for (int i = 0; i < studentList.Count; i++) {
                for (int j = 0; j < taskList.Count; j++) {

                    successProbability = (
                        taskList[j].GuessingProbability+
                        (1 - taskList[j].GuessingProbability) *
                        Math.Exp(Task.Sensitiveness*
                                 (studentList[i].Level - taskList[j].Complexity))
                        /
                        (1 + Math.Exp(Task.Sensitiveness*
                                      (studentList[i].Level - taskList[j].Complexity)) )
                    );
                    successProbability = Math.Round(successProbability, 8);

                    if (random.NextDouble()<successProbability){
                        result.SetAnswerTo(i, j, taskList[j].Weight);
                    }else {
                        result.SetAnswerTo(i, j, 0);
                    }
                }
            }
            return result;
        }

    }
}