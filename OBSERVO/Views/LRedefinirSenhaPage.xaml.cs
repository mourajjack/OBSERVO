using OBSERVO.Models;
using System.Text;
using System.Text.Json;

namespace OBSERVO.Views;

public partial class RedefinirSenhaPage : ContentPage
{
    private string _senha;
	public RedefinirSenhaPage()
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
                _senha = colaborador.Senha;
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

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SenhaAtualEntry.Text) || string.IsNullOrEmpty(SenhaNovaEntry.Text) || string.IsNullOrEmpty(ConfirmacaoSenhaEntry.Text))
            return;

        //pode ser que dê erro nessa linha, se tiver valor null or empty:
        if (ConfirmacaoSenhaEntry.Text.Length < 4)
        {
            LblErroConfirmacao.IsVisible = true;
            LblErroConfirmacao.Text = "Mínimo de 4 dígitos";
            btnOnSaveClicked.IsEnabled = false;
            btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
            return;
        }
        //pode ser que dê erro nessa linha, se tiver valor null or empty:
        if (SenhaNovaEntry.Text.Length < 4)
        {
            LblErroSenhaNova.IsVisible = true;
            LblErroSenhaNova.Text = "Mínimo de 4 dígitos";
            btnOnSaveClicked.IsEnabled = false;
            btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
            return;
        }

        if ((_senha == SenhaAtualEntry.Text) && (SenhaNovaEntry.Text == ConfirmacaoSenhaEntry.Text))
        {
            //Comment here
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            btnOnSaveClicked.IsEnabled = false;
            SenhaAtualEntry.IsEnabled = false;
            SenhaNovaEntry.IsEnabled = false;
            ConfirmacaoSenhaEntry.IsEnabled = false;

            try
            {
                var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
                if (colaborador != null)
                {
                    //dados
                    var dados = new SenhaModel
                    {
                        Opcao = 1,
                        aba = colaborador.AbaSheets,
                        CPF = colaborador.Cpf,
                        Senha = SenhaNovaEntry.Text
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
                        await DisplayAlert("✅ " + "Sucesso", "Senha atualizada com sucesso", "OK");
                        //Atualizar localDB
                        //voltar para a página anterior...
                        int result = await App.SQLiteDB.AtualizarSenhaAsync(colaborador.Cpf, SenhaNovaEntry.Text);
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
            SenhaAtualEntry.IsEnabled = true;
            SenhaNovaEntry.IsEnabled = true;
            ConfirmacaoSenhaEntry.IsEnabled = true;

        }
        else
        {
            if(_senha != SenhaAtualEntry.Text)
            LblErroSenhaAtual.IsVisible = true;

            if(SenhaNovaEntry.Text != ConfirmacaoSenhaEntry.Text)
            {
                LblErroConfirmacao.IsVisible = true;
                LblErroConfirmacao.Text = "As senhas devem ser iguais";
            }

            await DisplayAlert("❌ Erro", "Senha incorreta", "OK");
        }
    }

    private void ToggleConfirmacaoSenha(object sender, EventArgs e)
    {
        if (ConfirmacaoSenhaEntry.IsPassword)
        {
            ConfirmacaoSenhaEntry.IsPassword = false;
            ConfirmacaoSenhaEntryEYE.Source = "eyeicon.png";
            return;
        }
        else if (!ConfirmacaoSenhaEntry.IsPassword)
        {
            ConfirmacaoSenhaEntry.IsPassword = true;
            ConfirmacaoSenhaEntryEYE.Source = "eye.png";
            return;
        }
    }

    private void ToggleSenhaNova(object sender, EventArgs e)
    {
        if (SenhaNovaEntry.IsPassword)
        {
            SenhaNovaEntry.IsPassword = false;
            SenhaNovaEntryEYE.Source = "eyeicon.png";
            return;
        }
        else if (!SenhaNovaEntry.IsPassword)
        {
            SenhaNovaEntry.IsPassword = true;
            SenhaNovaEntryEYE.Source = "eye.png";
            return;
        }
    }

    private void ToggleSenhaAtual(object sender, EventArgs e)
    {
        if (SenhaAtualEntry.IsPassword)
        {
            SenhaAtualEntry.IsPassword = false;
            SenhaAtualEntryEYE.Source = "eyeicon.png";
            return;
        }
        else if (!SenhaAtualEntry.IsPassword)
        {
            SenhaAtualEntry.IsPassword = true;
            SenhaAtualEntryEYE.Source = "eye.png";
            return;
        }
    }

    private async void OnBackClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void SenhaAtualTextChanged(object sender, TextChangedEventArgs e)
    {
        // Texto antes da mudança
        string textoAnterior = e.OldTextValue;

        // Texto atual (após a mudança)
        string textoNovo = e.NewTextValue;

        // Detecta se foi apagado um caractere
        if (!string.IsNullOrEmpty(textoAnterior) &&
            textoAnterior.Length > textoNovo.Length)
        {
            if (SenhaAtualEntry.Text.Length >= _senha.Length)
            {
                if (!(SenhaAtualEntry.Text == _senha))
                {
                    LblErroSenhaAtual.IsVisible = true;
                    btnOnSaveClicked.IsEnabled = false;
                    btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
                }
                else
                {
                    LblErroSenhaAtual.IsVisible = false;
                    if (!(string.IsNullOrEmpty(SenhaNovaEntry.Text)) && !(string.IsNullOrEmpty(ConfirmacaoSenhaEntry.Text))) 
                    {
                        if (SenhaNovaEntry.Text.Length >=4 && ConfirmacaoSenhaEntry.Text.Length >= 4)
                        {
                            btnOnSaveClicked.IsEnabled = true;
                            btnOnSaveClicked.BackgroundColor = Color.FromArgb("#740862");
                        }
                    }
                }
            }
            else
            {
                btnOnSaveClicked.IsEnabled = false;
                btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
            }
            return;
        }

        if (SenhaAtualEntry.Text.Length >= _senha.Length)
        {
            if(!(SenhaAtualEntry.Text == _senha))
            {
                LblErroSenhaAtual.IsVisible = true;
                btnOnSaveClicked.IsEnabled = false;
                btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
            }
            else
            {
                LblErroSenhaAtual.IsVisible = false;
                if (!(string.IsNullOrEmpty(SenhaNovaEntry.Text)) && !(string.IsNullOrEmpty(ConfirmacaoSenhaEntry.Text)))
                {
                    if (SenhaNovaEntry.Text.Length >= 4 && ConfirmacaoSenhaEntry.Text.Length >= 4)
                    {
                        btnOnSaveClicked.IsEnabled = true;
                        btnOnSaveClicked.BackgroundColor = Color.FromArgb("#740862");
                    }
                }
            }
        }
    }

    private void SenhaNovaTextChanged(object sender, TextChangedEventArgs e)
    {

        // Texto antes da mudança
        string textoAnterior = e.OldTextValue;

        // Texto atual (após a mudança)
        string textoNovo = e.NewTextValue;

        // Detecta se foi apagado um caractere
        if (!string.IsNullOrEmpty(textoAnterior) &&
            textoAnterior.Length > textoNovo.Length)
        {
            if (string.IsNullOrEmpty(SenhaNovaEntry.Text))
            {
                LblErroSenhaNova.Text = "Obrigatório";
                btnOnSaveClicked.IsEnabled = false;
                btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
                return;
            }
        }

        if(SenhaNovaEntry.Text.Length < 4)
        {
            LblErroSenhaNova.IsVisible = true;
            LblErroSenhaNova.Text = "Mínimo de 4 dígitos";
            btnOnSaveClicked.IsEnabled = false;
            btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
        }
        else
        {
            LblErroSenhaNova.IsVisible = false;
            if (SenhaNovaEntry.Text == ConfirmacaoSenhaEntry.Text) 
            {
                LblErroConfirmacao.IsVisible = false;
                LblErroSenhaNova.IsVisible = false;
                //testa para habilitar SALVAR
                if (!(string.IsNullOrEmpty(SenhaAtualEntry.Text)) && !(string.IsNullOrEmpty(SenhaNovaEntry.Text)) && !(string.IsNullOrEmpty(ConfirmacaoSenhaEntry.Text)))
                {
                    if(SenhaAtualEntry.Text == _senha) 
                    {
                        btnOnSaveClicked.IsEnabled = true;
                        btnOnSaveClicked.BackgroundColor = Color.FromArgb("#740862");
                    }
                }
            }
            else
            {
                LblErroConfirmacao.IsVisible = true;
                LblErroConfirmacao.Text = "As senhas devem ser iguais";
            }
        }
    }

    private void ConfirmacaoSenhaTextChanged(object sender, TextChangedEventArgs e)
    {
        // Texto antes da mudança
        string textoAnterior = e.OldTextValue;

        // Texto atual (após a mudança)
        string textoNovo = e.NewTextValue;

        // Detecta se foi apagado um caractere
        if (!string.IsNullOrEmpty(textoAnterior) &&
            textoAnterior.Length > textoNovo.Length)
        {
            if (string.IsNullOrEmpty(ConfirmacaoSenhaEntry.Text))
            {
                LblErroConfirmacao.IsVisible = true;
                LblErroConfirmacao.Text = "Obrigatório";
                btnOnSaveClicked.IsEnabled = false;
                btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
                return;
            }
        }

        if (ConfirmacaoSenhaEntry.Text.Length < 4)
        {
            LblErroConfirmacao.IsVisible = true;
            LblErroConfirmacao.Text = "Mínimo de 4 dígitos";
            btnOnSaveClicked.IsEnabled = false;
            btnOnSaveClicked.BackgroundColor = Color.FromArgb("#ccc");
            return;
        }

        if (SenhaNovaEntry.Text != ConfirmacaoSenhaEntry.Text)
        {
            LblErroConfirmacao.IsVisible = true;
            LblErroConfirmacao.Text = "As senhas devem ser iguais";
        }
        else
        {
            if (SenhaNovaEntry.Text == ConfirmacaoSenhaEntry.Text)
            {
                LblErroConfirmacao.IsVisible = false;
                LblErroSenhaNova.IsVisible = false;
                //testa para habilitar SALVAR
                if (!(string.IsNullOrEmpty(SenhaAtualEntry.Text)) && !(string.IsNullOrEmpty(SenhaNovaEntry.Text)) && !(string.IsNullOrEmpty(ConfirmacaoSenhaEntry.Text)))
                {
                    if (SenhaAtualEntry.Text == _senha)
                    {
                        btnOnSaveClicked.IsEnabled = true;
                        btnOnSaveClicked.BackgroundColor = Color.FromArgb("#740862");
                    }
                }
            }
        }

    }
}