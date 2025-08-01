using OBSERVO.Models;
using System;
using System.Text;
using System.Text.Json;
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
        if (string.IsNullOrEmpty(TelefoneEntry.Text))
            return;

        try
        {
            var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
            if (colaborador != null)
            {
                //dados
                var dados = new Telefones
                {
                    Opcao = 1,
                    aba = colaborador.AbaSheets,
                    CPF = colaborador.Cpf,
                    Telefone = TelefoneEntry.Text
                };

                string json = JsonSerializer.Serialize(dados);

                using var client = new HttpClient();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(API_DB_CONN.URI_Colaboradores, content);
                //var resposta = await response.Content.ReadAsStringAsync();

                // Garante que deu sucesso
                response.EnsureSuccessStatusCode();

                string respostaJson = await response.Content.ReadAsStringAsync();

                await DisplayAlert("RESPOSTA", respostaJson, "OK");

            }
        }
        catch (Exception)
        {

        }
    }

    private async void OnBackClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}