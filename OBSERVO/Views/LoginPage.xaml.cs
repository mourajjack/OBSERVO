namespace OBSERVO.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	public LoginPage(string[] logoAndName)
	{
        InitializeComponent();
        LogoEmpresa.Source = ImageSource.FromUri(new Uri(logoAndName[0]));
		//DisplayAlert("> ", logoAndName[1], "OK");
    }
}