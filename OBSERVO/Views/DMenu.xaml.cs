using System.Threading.Tasks;

namespace OBSERVO.Views;

public partial class Menu : ContentPage
{
	public Menu()
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
                iniciaisNome.Text = ObterIniciais(colaborador.Nome);
                nomeColaborador.Text = CortarComReticencias(colaborador.Nome);
                cpf.Text = MascaraCPF(colaborador.Cpf);
            }
            else
            {
                //deleta tabela e volta pro início...
                //Futuramente trate caso não consiga deletar a tabela.
                if (await App.SQLiteDB.DeletarTabelaColaboradoresAsync())
                {
                    //Volta pro inicio
                    var pagina = new NavigationPage(
                    new SelectCompany()
                    );

                    pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
                    pagina.BarTextColor = Color.FromRgb(219, 219, 219);

                    App.Current.MainPage = pagina;
                }
            }
        }
        catch (Exception)
        {
            //deleta tabela e volta pro início
            //Futuramente trate caso não consiga deletar a tabela.
            if (await App.SQLiteDB.DeletarTabelaColaboradoresAsync())
            {
                //Volta pro inicio
                var pagina = new NavigationPage(
                new SelectCompany()
                );

                pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
                pagina.BarTextColor = Color.FromRgb(219, 219, 219);

                App.Current.MainPage = pagina;
            }
        }
    }

    public static string MascaraCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return string.Empty;

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
            return cpf; // ou lançar uma exceção, se preferir

        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }


    public static string CortarComReticencias(string texto, int limite = 23)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        if (texto.Length <= limite)
            return texto + "...";

        return texto.Substring(0, limite).TrimEnd() + "...";
    }

    public static string ObterIniciais(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            return string.Empty;

        var partes = nomeCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (partes.Length == 0)
            return string.Empty;

        string primeiraLetra = partes[0].Substring(0, 1).ToUpper();
        string ultimaLetra = partes[^1].Substring(0, 1).ToUpper(); // ^1 = última posição (C# 8+)

        return primeiraLetra + ultimaLetra;
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

    private async void OnFecharMenuTapped(object sender, TappedEventArgs e)
    {
        ((FlyoutPage)Application.Current.MainPage).IsPresented = false;
    }
}