using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AkkaBootcamp.ChartApp.Actors.ChartingActor;

namespace AkkaBootcamp.ChartApp.Actors;

/// <summary>
/// Actor responsible for monitoring a specific <see cref="PerformanceCounter"/>
/// </summary>
public class PerformanceCounterActor : UntypedActor
{
    private readonly string seriesName;
    private readonly Func<PerformanceCounter> performanceCounterGenerator;
    private PerformanceCounter counter;

    private readonly HashSet<IActorRef> subscriptions;
    private readonly ICancelable cancelPublishing;

    public PerformanceCounterActor(string seriesName, Func<PerformanceCounter> performanceCounterGenerator)
    {
        this.seriesName = seriesName;
        this.performanceCounterGenerator = performanceCounterGenerator;
        subscriptions = new HashSet<IActorRef>();
        cancelPublishing = new Cancelable(Context.System.Scheduler);
    }

    protected override void PreStart()
    {
        //create a new instance of the performance counter
        counter = performanceCounterGenerator();
        Context.System.Scheduler.ScheduleTellRepeatedly(
            TimeSpan.FromMilliseconds(250),
            TimeSpan.FromMilliseconds(250),
            Self,
            new ChartingActor.GatherMetrics(),
            Self,
            cancelPublishing);
    }

    protected override void PostStop()
    {
        try
        {
            cancelPublishing.Cancel(false);
            counter.Dispose();
        }
        finally
        {
            base.PostStop();
        }
        
    }

    protected override void OnReceive(object message)
    {
        if(message is ChartingActor.GatherMetrics)
        {
            var metric = new ChartingActor.Metric(seriesName, counter.NextValue());
            foreach (var sub in subscriptions)
            {
                sub.Tell(metric);
            }
        }
        else if (message is ChartingActor.SubscribeCounter)
        {
            var sc = message as ChartingActor.SubscribeCounter;
            subscriptions.Add(sc.Subscriber);
        }
        else if (message is UnsubscribeCounter)
        {
            // remove a subscription from this counter
            var uc = message as UnsubscribeCounter;
            subscriptions.Remove(uc.Subscriber);
        }
    }
}
