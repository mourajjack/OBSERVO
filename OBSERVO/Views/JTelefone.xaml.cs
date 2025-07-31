using System.Threading.Tasks;

namespace OBSERVO.Views;

public partial class Telefone : ContentPage
{
	public Telefone()
	{
		InitializeComponent();

        // Esconde a barra de navega��o apenas nesta p�gina
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