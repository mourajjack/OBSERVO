﻿using OBSERVO.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OBSERVO.Views;

public partial class Telefone : ContentPage
{
	public Telefone()
	{
		InitializeComponent();

        // Esconde a barra de navegação apenas nesta página, desnecessario!
        NavigationPage.SetHasNavigationBar(this, false);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
            if (colaborador != null)
            {
                //setar telefone na view
                numeroDeTelefone.Text += colaborador.Telefone;
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

    private async void onSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TelefoneEntry.Text))
            return;

        //Comment here
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        btnOnSaveClicked.IsEnabled = false;

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
                    Telefone = FormatarTelefone(TelefoneEntry.Text)
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
                    await DisplayAlert("✅ " + FormatarTelefone(TelefoneEntry.Text), jsonResp.RootElement.GetProperty("message").GetString(), "OK");
                    //Atualizar localDB
                    //voltar para a página anterior...
                    int result = await App.SQLiteDB.AtualizarTelefoneAsync(colaborador.Cpf, FormatarTelefone(TelefoneEntry.Text));
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
    }

    private async void OnBackClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    public static string FormatarTelefone(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1️⃣ Remove tudo que não é número
        string apenasNumeros = Regex.Replace(input, @"\D", "");

        // 2️⃣ Garante no máximo 11 dígitos
        if (apenasNumeros.Length > 11)
            apenasNumeros = apenasNumeros.Substring(0, 11);

        // 3️⃣ Aplica a máscara
        if (apenasNumeros.Length == 11)
        {
            // Celular com 9 dígitos
            return Regex.Replace(apenasNumeros, @"(\d{2})(\d{5})(\d{4})", "($1) $2-$3");
        }
        else if (apenasNumeros.Length == 10)
        {
            // Telefone fixo
            return Regex.Replace(apenasNumeros, @"(\d{2})(\d{4})(\d{4})", "($1) $2-$3");
        }
        else
        {
            // Se ainda não tiver todos os dígitos, retorna apenas o que tem
            return apenasNumeros;
        }
    }
}