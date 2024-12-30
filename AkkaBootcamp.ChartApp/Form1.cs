using Akka.Actor;
using Akka.Util.Internal;
using AkkaBootcamp.ChartApp.Actors;
using static AkkaBootcamp.ChartApp.Actors.ChartingActor;

namespace AkkaBootcamp.ChartApp
{
    public partial class Form1 : Form
    {
        private IActorRef _chartActor;
        private readonly AtomicCounter _seriesCounter = new AtomicCounter(1);
        private IActorRef _coordinatorActor;
        private Dictionary<CounterType, IActorRef> _toggleActors = new Dictionary<CounterType,
            IActorRef>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            _chartActor = Program.ChartActors.ActorOf(Props.Create(() => new ChartingActor(sysChart, btnPauseResume)), "charting");
            _chartActor.Tell(new ChartingActor.InitializeChart(null));

            _coordinatorActor = Program.ChartActors.ActorOf(Props.Create(() =>
            new PerformanceCounterCoordinatorActor(_chartActor)), "counters");

            _toggleActors[CounterType.Cpu] = Program.ChartActors.ActorOf(
                Props.Create(() => new ButtonToggleActor(_coordinatorActor, btnCpu, CounterType.Cpu, false))
                .WithDispatcher("akka.actor.synchronized-dispatcher"));

            _toggleActors[CounterType.Memory] = Program.ChartActors.ActorOf(
                Props.Create(() => new ButtonToggleActor(_coordinatorActor, btnMemory, CounterType.Memory, false))
                .WithDispatcher("akka.actor.synchronized-dispatcher"));

            _toggleActors[CounterType.Disk] = Program.ChartActors.ActorOf(
                Props.Create(() => new ButtonToggleActor(_coordinatorActor, btnDisk, CounterType.Disk, false))
                .WithDispatcher("akka.actor.synchronized-dispatcher"));

            // Set the CPU toggle to ON so we start getting some data
            _toggleActors[CounterType.Cpu].Tell(new ButtonToggleActor.Toggle());
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            _chartActor.Tell(PoisonPill.Instance);

            Program.ChartActors.Terminate();
        }

        private void btnCpu_Click(object sender, EventArgs e)
        {
            _toggleActors[CounterType.Cpu].Tell(new ButtonToggleActor.Toggle());
        }

        private void btnMemory_Click(object sender, EventArgs e)
        {
            _toggleActors[CounterType.Memory].Tell(new ButtonToggleActor.Toggle());
        }

        private void btnDisk_Click(object sender, EventArgs e)
        {
            _toggleActors[CounterType.Disk].Tell(new ButtonToggleActor.Toggle());
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            _chartActor.Tell(new ChartingActor.TogglePause());
        }
    }
}
