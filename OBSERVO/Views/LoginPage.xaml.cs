namespace OBSERVO.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	public LoginPage(string companyName)
	{
        InitializeComponent();
        LogoEmpresa.Source = ImageSource.FromUri(new Uri(companyName));
    }
}