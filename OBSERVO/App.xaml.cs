//using Java.Nio.Channels.Spi;
using OBSERVO.Views;
using OBSERVO.Services;

namespace OBSERVO
{
    public partial class App : Application
    {
        static LocalDBServices localDB;
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

        public static LocalDBServices SQLiteDB
        {
            get
            {
                if (localDB == null)
                {
                    localDB = new LocalDBServices(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GSLocal.db3"));
                }
                return localDB;
            }
        }

        /*
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new SelectCompany());
        }
        */
    }
}