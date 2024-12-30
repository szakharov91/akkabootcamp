using Akka.Actor;
using Akka.Configuration.Hocon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AkkaBootcamp.ChartApp.Actors;

public class ChartingActor : ReceiveActor
{
    #region Reporting

    /// <summary>
    /// Signal used to indicate that it's time to sample all counters
    /// </summary>
    public class GatherMetrics { }

    /// <summary>
    /// Metric data at the time of sample
    /// </summary>
    public class Metric
    {
        public Metric(string series, float counterValue)
        {
            Series = series;
            CounterValue = counterValue;
        }

        public string Series { get; }
        public float CounterValue { get; }
    }

    /// <summary>
    /// Remove an existing <see cref="Series"/> from the chart
    /// </summary>
    public class RemoveSeries
    {
        public RemoveSeries(string seriesName)
        {
            SeriesName = seriesName;
        }

        public string SeriesName { get; private set; }
    }

    #region Performance Counter Management

    /// <summary>
    /// All types of counters supported by this example
    /// </summary>
    public enum CounterType
    {
        Cpu,
        Memory,
        Disk
    }

    /// <summary>
    /// Enables a counter and begins publishing values to <see cref="Subscriber"/>.
    /// </summary>
    public class SubscribeCounter
    {
        public SubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            Subscriber = subscriber;
            Counter = counter;
        }

        public CounterType Counter { get; private set; }

        public IActorRef Subscriber { get; private set; }
    }

    /// <summary>
    /// Unsubscribes <see cref="Subscriber"/> from receiving updates 
    /// for a given counter
    /// </summary>
    public class UnsubscribeCounter
    {
        public UnsubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            Subscriber = subscriber;
            Counter = counter;
        }

        public CounterType Counter { get; private set; }

        public IActorRef Subscriber { get; private set; }
    }

    #endregion

    #endregion

    #region Messages

    /// <summary>
    /// Toggles the pausing between charts
    /// </summary>
    public class TogglePause { }

    public class InitializeChart
    {
        public InitializeChart(Dictionary<string, Series> initialSeries)
        {
            InitialSeries = initialSeries;
        }

        public Dictionary<string, Series> InitialSeries { get; }
    }

    /// <summary>
    /// Add a new <see cref="Series"/> to the chart
    /// </summary>
    public class AddSeries
    {
        public AddSeries(Series series)
        {
            Series = series;
        }

        public Series Series { get; private set; }
    }

    #endregion

    private readonly Button _pauseButton;
    private readonly Chart _chart;
    private Dictionary<string, Series> _seriesIndex;

    /// <summary>
    /// Maximum number of points we will allow in a series
    /// </summary>
    public const int MaxPoints = 250;

    /// <summary>
    /// Incrementing counter we use to plot along the X-axis
    /// </summary>
    private int xPosCounter = 0;
    public ChartingActor(Chart chart, Button pauseButton) : this(chart, new Dictionary<string, Series>(), pauseButton)
    {
        
    }

    public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex, Button pauseButton)
    {
        _chart = chart;
        _seriesIndex = seriesIndex;
        _pauseButton = pauseButton;

        Charting();
    }

    private void Charting()
    {
        Receive<InitializeChart>(ic => HandleInitialize(ic));
        Receive<AddSeries>(addSeries => HandleAddSeries(addSeries));
        Receive<RemoveSeries>(removeSeries => HandleRemoveSeries(removeSeries));
        Receive<Metric>(metric => HandleMetrics(metric));

        //new receive handler for the TogglePause message type
        Receive<TogglePause>(pause =>
        {
            SetPauseButtonText(true);
            BecomeStacked(Paused);
        });
    }

    // Actors/ChartingActor.cs - add to the very bottom of the ChartingActor class
    private void SetPauseButtonText(bool paused)
    {
        _pauseButton.Text = string.Format("{0}", !paused ? "PAUSE ||" : "RESUME ->");
    }

    // Actors/ChartingActor.cs - just after the Charting method
    private void Paused()
    {
        Receive<Metric>(metric => HandleMetricsPaused(metric));
        Receive<TogglePause>(pause =>
        {
            SetPauseButtonText(false);
            UnbecomeStacked();
        });
    }

    // Actors/ChartingActor.cs - inside Individual Message Type Handlers region
    private void HandleMetricsPaused(Metric metric)
    {
        if (!string.IsNullOrEmpty(metric.Series)
            && _seriesIndex.ContainsKey(metric.Series))
        {
            var series = _seriesIndex[metric.Series];
            // set the Y value to zero when we're paused
            series.Points.AddXY(xPosCounter++, 0.0d);
            while (series.Points.Count > MaxPoints) series.Points.RemoveAt(0);
            SetChartBoundaries();
        }
    }

    private void SetChartBoundaries()
    {
        double maxAxisX, maxAxisY, minAxisX, minAxisY = 0.0d;
        var allPoints = _seriesIndex.Values.SelectMany(series => series.Points).ToList();
        var yValues = allPoints.SelectMany(point => point.YValues).ToList();
        maxAxisX = xPosCounter;
        minAxisX = xPosCounter - MaxPoints;
        maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
        minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0.0d;
        if (allPoints.Count > 2)
        {
            var area = _chart.ChartAreas[0];
            area.AxisX.Minimum = minAxisX;
            area.AxisX.Maximum = maxAxisX;
            area.AxisY.Minimum = minAxisY;
            area.AxisY.Maximum = maxAxisY;
        }
    }

    private void HandleAddSeries(AddSeries series)
    {
        if (!string.IsNullOrEmpty(series.Series.Name) &&
        !_seriesIndex.ContainsKey(series.Series.Name))
        {
            _seriesIndex.Add(series.Series.Name, series.Series);
            _chart.Series.Add(series.Series);
            SetChartBoundaries();
        }
    }

    private void HandleInitialize(InitializeChart? ic)
    {
        if (ic.InitialSeries != null)
        {
            // swap the two series out
            _seriesIndex = ic.InitialSeries;
        }

        // delete any existing series
        _chart.Series.Clear();

        // set the axes up
        var area = _chart.ChartAreas[0];
        area.AxisX.IntervalType = DateTimeIntervalType.Number;
        area.AxisY.IntervalType = DateTimeIntervalType.Number;

        SetChartBoundaries();

        // attempt to render the initial chart
        if (_seriesIndex.Any())
        {
            foreach (var series in _seriesIndex)
            {
                // force both the chart and the internal index to use the same names
                series.Value.Name = series.Key;
                _chart.Series.Add(series.Value);
            }
        }

        SetChartBoundaries();
    }

    private void HandleRemoveSeries(RemoveSeries series)
    {
        if (!string.IsNullOrEmpty(series.SeriesName) &&
            _seriesIndex.ContainsKey(series.SeriesName))
        {
            var seriesToRemove = _seriesIndex[series.SeriesName];
            _seriesIndex.Remove(series.SeriesName);
            _chart.Series.Remove(seriesToRemove);
            SetChartBoundaries();
        }
    }

    private void HandleMetrics(Metric metric)
    {
        if (!string.IsNullOrEmpty(metric.Series) &&
            _seriesIndex.ContainsKey(metric.Series))
        {
            var series = _seriesIndex[metric.Series];
            series.Points.AddXY(xPosCounter++, metric.CounterValue);
            while (series.Points.Count > MaxPoints) series.Points.RemoveAt(0);
            SetChartBoundaries();
        }
    }
}
