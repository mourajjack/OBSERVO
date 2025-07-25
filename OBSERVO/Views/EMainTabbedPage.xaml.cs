namespace OBSERVO.Views;

public partial class MainTabbedPage : TabbedPage
{
	public MainTabbedPage()
	{
		InitializeComponent();
    }

    private async void COLABORADORClick(object sender, EventArgs e)
    {
        var pagina = new NavigationPage(
                new IUserAccountSettingsPage()
                    );

        pagina.BarBackgroundColor = Color.FromRgb(116, 8, 116);
        pagina.BarTextColor = Color.FromRgb(219, 219, 219);

        await Navigation.PushAsync(pagina);
    }
}