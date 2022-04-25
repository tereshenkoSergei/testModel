using System;
using System.Collections.Generic;
using TestModel.Code.Transacts;

namespace TestModel.Code
{
    public class Result
    {
        public int[][] ResultMatrix { get; }
        private List<Student> StudentList { get; }
        private List<Task> TaskList { get; }

        public Result(List<Student> studentList, List<Task> taskList)
        {
            StudentList = studentList;
            TaskList = taskList;
            ResultMatrix = new int[studentList.Count][];
            for (int i = 0; i < ResultMatrix.Length; i++) ResultMatrix[i] = new int[taskList.Count];
        }

        public void SetAnswerTo(int student, int task, int value)
        {
            ResultMatrix[student][task] = value;
            ResultMatrix[student][task] = value;
        }

        public Dictionary<int, KeyValuePair<double, double>> GetResultDictionary()
        {
            int id = 0;
            Dictionary<int, KeyValuePair<double, double>> resultDictionary = new Dictionary<int, KeyValuePair<double, double>>();

            for (int i = 0; i < ResultMatrix.Length; i++)
            {
                Double resultInLogit = 0;
                Double maxResult = 0;
                Double intResult = 0;
                for (int j = 0; j < ResultMatrix[i].Length; j++)
                {
                    maxResult++;
                    intResult += ResultMatrix[i][j];
                }

                resultInLogit = 8 * (intResult / maxResult) - 4;
                resultDictionary.Add(id, new KeyValuePair<double, double>(StudentList[i].Level, resultInLogit));
                id++;
            }

            return resultDictionary;
        }
    }
}