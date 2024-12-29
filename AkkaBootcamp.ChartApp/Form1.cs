using Akka.Actor;
using Akka.Util.Internal;
using AkkaBootcamp.ChartApp.Actors;

namespace AkkaBootcamp.ChartApp
{
    public partial class Form1 : Form
    {
        private IActorRef _chartActor;
        private readonly AtomicCounter _seriesCounter = new AtomicCounter(1);

        public Form1()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            _chartActor = Program.ChartActors.ActorOf(
                Props.Create(() => new ChartingActor(sysChart)), "charting");

            var series = ChartDataHelper.RandomSeries("FakeSeries" + _seriesCounter.GetAndIncrement());
            _chartActor.Tell(new ChartingActor.InitializeChart(new Dictionary<string, System.Windows.Forms.DataVisualization.Charting.Series>()
            {
                {series.Name, series }
            }));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            _chartActor.Tell(PoisonPill.Instance);

            Program.ChartActors.Terminate();
        }
    }
}
