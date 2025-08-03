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
        try
        {
            var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);

            if (colaborador != null)
            {
                //NameCOLABORADOR.Text = "Olá, " + ObterNomeEPrimeiroSobrenome(colaborador.Nome) + "   ❯";
                NameCOLABORADOR.Text = "Olá, " + ObterNomeEPrimeiroSobrenome(colaborador.Nome);
            }
            else
            {
                //volta pro inicio
                var pagina = new NavigationPage(
                new SelectCompany()
                    );

                pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
                pagina.BarTextColor = Color.FromRgb(219, 219, 219);

                App.Current.MainPage = pagina;
            }
        }
        catch (Exception)
        {
            //volta pro inicio
            var pagina = new NavigationPage(
            new SelectCompany()
                );

            pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
            pagina.BarTextColor = Color.FromRgb(219, 219, 219);

            App.Current.MainPage = pagina;
        }
    }

    public string ObterNomeEPrimeiroSobrenome(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            return string.Empty;

        var partes = nomeCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (partes.Length >= 2)
            return $"{partes[0]} {partes[1]}"; // nome + primeiro sobrenome
        else
            return nomeCompleto.Trim(); // se tiver só o primeiro nome
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