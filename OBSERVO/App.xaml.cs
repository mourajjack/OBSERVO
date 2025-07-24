//using Java.Nio.Channels.Spi;
using OBSERVO.Views;

namespace OBSERVO
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SelectCompany());
        }

        /*
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new SelectCompany());
        }
        */
    }
}