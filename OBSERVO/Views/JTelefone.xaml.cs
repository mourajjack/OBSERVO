using System.Threading.Tasks;

namespace OBSERVO.Views;

public partial class Telefone : ContentPage
{
	public Telefone()
	{
		InitializeComponent();

        // Esconde a barra de navegação apenas nesta página
        NavigationPage.SetHasNavigationBar(this, false);

    }

    private async void onSaveClicked(object sender, EventArgs e)
    {
        
    }

    private async void OnBackClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}