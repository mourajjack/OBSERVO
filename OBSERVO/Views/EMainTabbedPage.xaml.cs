using OBSERVO.Models;

namespace OBSERVO.Views;

public partial class MainTabbedPage : TabbedPage
{
	public MainTabbedPage()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);

        if (colaborador != null)
            NameCOLABORADOR.Text = colaborador.Nome;
        else
            NameCOLABORADOR.Text = "Não encontrado";
    }

    private async void COLABORADORClick(object sender, EventArgs e)
    {
        var pagina = new NavigationPage(
                new IUserAccountSettingsPage()
                    );

        pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
        pagina.BarTextColor = Color.FromRgb(219, 219, 219);

        await Navigation.PushAsync(pagina);
    }
}