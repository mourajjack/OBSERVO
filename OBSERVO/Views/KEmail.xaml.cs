using OBSERVO.Models;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OBSERVO.Views;

public partial class Email : ContentPage
{
	public Email()
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
                //setar e-mail na view
                LblEmail.Text += colaborador.Email;
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
            await Navigation.PopModalAsync();
        }
    }

    private async void OnBackClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void EmailEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Pega o texto do campo
        string email = EmailEntry.Text ?? "";

        // Regex para validar e-mail simples
        bool emailValido = Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase
        );

        // Habilita/desabilita o botão
        btnOnSaveClicked.IsEnabled = emailValido;

        // Troca a cor de fundo
        btnOnSaveClicked.BackgroundColor = emailValido
            ? Color.FromArgb("#740862")
            : Color.FromArgb("#ccc");
    }

    private async void onSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EmailEntry.Text))
            return;

        //Comment here
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        btnOnSaveClicked.IsEnabled = false;
        EmailEntry.IsEnabled = false;

        try
        {
            var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
            if (colaborador != null)
            {
                //dados
                var dados = new EmailModel
                {
                    Opcao = 1,
                    aba = colaborador.AbaSheets,
                    CPF = colaborador.Cpf,
                    Email = EmailEntry.Text
                };

                string json = JsonSerializer.Serialize(dados);

                using var client = new HttpClient();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(API_DB_CONN.URI_Colaboradores, content);
                var respostaJson = await response.Content.ReadAsStringAsync();

                // Garante que deu sucesso
                response.EnsureSuccessStatusCode();

                var jsonResp = System.Text.Json.JsonDocument.Parse(respostaJson);
                if (jsonResp.RootElement.GetProperty("success").GetBoolean())
                {
                    await DisplayAlert("✅ " + EmailEntry.Text, jsonResp.RootElement.GetProperty("message").GetString(), "OK");
                    //Atualizar localDB
                    //voltar para a página anterior...
                    int result = await App.SQLiteDB.AtualizarEmailAsync(colaborador.Cpf, EmailEntry.Text);
                    if (result > 0)
                    {
                        //Update Success
                        //await DisplayAlert("✅ " + TelefoneEntry.Text, "LOCAL DB UPDATE SUCCESS", "OK");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("❌  Erro ao salvar", "Verifique sua conexão com a internet e tente novamente", "OK");
                    }

                }
                else
                {
                    await DisplayAlert("❌  Erro de rede", "Verifique sua conexão com a internet e tente novamente", "OK");
                }

            }
        }
        catch (Exception)
        {
            await DisplayAlert("❌  Erro de rede", "Verifique sua conexão com a internet e tente novamente", "OK");
            await Navigation.PopModalAsync();
        }

        //Comment here
        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;
        btnOnSaveClicked.IsEnabled = true;
        EmailEntry.IsEnabled = true;
    }

}