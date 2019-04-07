using BlangenOA.QuartzNet;
using Quartz;
using Quartz.Impl;
using System;

namespace BlangenOA.Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            IScheduler sched;
            ISchedulerFactory sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();
            JobDetail job = new JobDetail("job1", "group1", typeof(IndexJob));//IndexJob为实现了IJob接口的类
            //5秒后开始第一次运行
            DateTime ts = TriggerUtils.GetNextGivenSecondDate(null, 5);
            //每隔3秒钟执行一次
            TimeSpan interval = TimeSpan.FromSeconds(3);
            //每若干小时运行一次，小时间隔由appsettings中的IndexIntervalHour参数指定
            Trigger trigger = new SimpleTrigger("trigger1", "group1", "job1", "group1", ts, null,
                                                    SimpleTrigger.RepeatIndefinitely, interval);

            sched.AddJob(job, true);
            sched.ScheduleJob(trigger);
            sched.Start();
        }
    }
}
