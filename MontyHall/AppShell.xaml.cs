namespace MontyHall
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("GamePlay", typeof(GamePlay));
        }
    }
}
