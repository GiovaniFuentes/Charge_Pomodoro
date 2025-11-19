using System;
using System.Collections.Generic;

[Serializable]
public class SubtaskData
{
    public string text;
    public bool done;
}

[Serializable]
public class MainTaskData
{
    public string title;
    public List<SubtaskData> subtasks = new List<SubtaskData>();
}

[Serializable]
public class AllTasksData
{
    public List<MainTaskData> mainTasks = new List<MainTaskData>();
}
