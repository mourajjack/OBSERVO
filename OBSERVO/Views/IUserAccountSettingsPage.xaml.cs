using System;

namespace OBSERVO.Views;

public partial class IUserAccountSettingsPage : ContentPage
{
	public IUserAccountSettingsPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //inicializar colaborador, buscar colaborador no localDB
        try
        {
            var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
            if (colaborador != null)
            {
                InitialsLabel.Text = ObterIniciais(colaborador.Nome);
                NomeColaborador.Text = colaborador.Nome;
                Funcao.Text = colaborador.Funcao;
                Cpf.Text = MascaraCPF(colaborador.Cpf);
                Telefone.Text = colaborador.Telefone;
                Email.Text = colaborador.Email;
                Escala.Text = colaborador.Escala;
                Matricula.Text = colaborador.Matricula;
                Posto.Text = colaborador.Posto;
                Admissao.Text = colaborador.DataAdmissao;
                Empresa.Text = colaborador.Empresa;
                Cnpj.Text = colaborador.Cnpj;

            }
            else
            {
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
                //else
            }
        }
        catch (Exception ex)
        {
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

    private void OnChangePhotoClicked(object sender, EventArgs e)
    {

    }

    private void OnSenhaTapped(object sender, TappedEventArgs e)
    {

    }

    private void OnEmailTapped(object sender, TappedEventArgs e)
    {

    }

    private async void OnTelefoneTapped(object sender, TappedEventArgs e)
    {
        var pagina = new Telefone();

        await Navigation.PushModalAsync(pagina);
    }
}