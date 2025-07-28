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
        var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
        await App.SQLiteDB.ColaboradorDeleteItemAsync(colaborador);
    }
}