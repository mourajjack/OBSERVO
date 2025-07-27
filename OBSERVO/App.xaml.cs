//using Java.Nio.Channels.Spi;
using OBSERVO.Views;

namespace OBSERVO
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var pagina = new NavigationPage(
                new SelectCompany()
                    );

            pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
            pagina.BarTextColor = Color.FromRgb(219, 219, 219);

            MainPage = pagina;
        }

        /*
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new SelectCompany());
        }
        */
    }
}