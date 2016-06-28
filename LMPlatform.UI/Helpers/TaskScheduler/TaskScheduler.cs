using LMPlatform.UI.Helpers.TaskScheduler.Tasks;
using Quartz;
using Quartz.Impl;

namespace LMPlatform.UI.Helpers.TaskScheduler
{
    public class TaskScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail task = JobBuilder.Create<SearchTask>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .Build();

            scheduler.ScheduleJob(task, trigger);
        }
    }
}