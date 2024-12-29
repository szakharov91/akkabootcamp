using Akka.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AkkaBootcamp.ChartApp;

public static class ChartDataHelper
{
    public static Series RandomSeries(string seriesname, SeriesChartType type = SeriesChartType.Line, int points = 10)
    {
        var series = new Series(seriesname) { ChartType = type };
        foreach(var i in Enumerable.Range(0, points))
        {
            var rng = ThreadLocalRandom.Current.NextDouble();
            series.Points.Add(new DataPoint(i, 2.0 * Math.Sin(rng) + Math.Sin(rng / 4.5)));
        }

        series.BorderWidth = 3;
        return series;
    }
}
