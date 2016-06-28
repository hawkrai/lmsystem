using System;
using System.IO;
using Quartz;

namespace LMPlatform.UI.Helpers.TaskScheduler.Tasks
{
    public class SearchTask : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var indecesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Lucene_indeces");
            if (Directory.Exists(indecesDirectory))
                Directory.Delete(indecesDirectory, true);
        }
    }
}