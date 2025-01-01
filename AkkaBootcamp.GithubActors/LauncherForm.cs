using Akka.Actor;
using AkkaBootcamp.GithubActors.Actors;

namespace AkkaBootcamp.GithubActors
{
    public partial class LauncherForm : Form
    {
        private IActorRef _mainFormActor;

        public LauncherForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            /* INITIALIZE ACTORS */
            _mainFormActor = Program.GithubActors.ActorOf(Props.Create(() => new MainFormActor(lblIsValid)), ActorPaths.MainFormActor.Name);
            Program.GithubActors.ActorOf(Props.Create(() => new GithubValidatorActor(GithubClientFactory.GetClient())), ActorPaths.GithubValidatorActor.Name);
            Program.GithubActors.ActorOf(Props.Create(() => new GithubCommanderActor()),
                ActorPaths.GithubCommanderActor.Name);
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            _mainFormActor.Tell(new ProcessRepo(tbRepoUrl.Text));
        }

        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
