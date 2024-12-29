using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AkkaBootcamp.ChartApp.Actors;

public class ChartingActor : UntypedActor
{
    #region Messages

    public class InitializeChart
    {
        public InitializeChart(Dictionary<string, Series> initialSeries)
        {
            InitialSeries = initialSeries;
        }

        public Dictionary<string, Series> InitialSeries { get; }
    }

    #endregion

    private readonly Chart _chart;
    private Dictionary<string, Series> _seriesIndex;

    public ChartingActor(Chart chart): this(chart, new Dictionary<string, Series>())
    {
        
    }

    public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex)
    {
        _chart = chart;
        _seriesIndex = seriesIndex;
    }

    protected override void OnReceive(object message)
    {
        if(message is InitializeChart)
        {
            var ic = message as InitializeChart;
            HandleInitialize(ic);
        }
    }

    private void HandleInitialize(InitializeChart? ic)
    {
        if(ic?.InitialSeries is not null)
        {
            _seriesIndex = ic.InitialSeries;
        }

        _chart.Series.Clear();

        if (!_seriesIndex.Any()) return;

        foreach(var series in _seriesIndex)
        {
            series.Value.Name = series.Key;
            _chart.Series.Add(series.Value);
        }
    }
}
