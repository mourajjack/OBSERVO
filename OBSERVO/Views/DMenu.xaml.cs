using System.Threading.Tasks;

namespace OBSERVO.Views;

public partial class Menu : ContentPage
{
	public Menu()
	{
		InitializeComponent();
	}

    private async void Sair_Button_Clicked(object sender, EventArgs e)
    {
		try
		{
            var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
            if (colaborador != null)
            {
                int resultado = await App.SQLiteDB.ColaboradorDeleteItemAsync(colaborador);

                if (resultado > 0)
                {
                    //Console.WriteLine("Colaborador deletado com sucesso!");
                    //chama SelectCompany
                    var pagina = new NavigationPage(
                    new SelectCompany()
                    );

                    pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
                    pagina.BarTextColor = Color.FromRgb(219, 219, 219);

                    App.Current.MainPage = pagina;
                }
                else
                {
                    //Console.WriteLine("Nenhum registro deletado (ID pode estar errado).");
                }

            }
        }
		catch (Exception)
		{

		}
    }
}