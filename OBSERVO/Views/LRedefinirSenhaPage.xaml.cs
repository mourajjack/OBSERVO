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

    private void OnSalvarClicked(object sender, EventArgs e)
    {

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
                    btnOnSaveClicked.IsEnabled = true;
                    btnOnSaveClicked.BackgroundColor = Color.FromArgb("#740862");
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
                btnOnSaveClicked.IsEnabled = true;
                btnOnSaveClicked.BackgroundColor = Color.FromArgb("#740862");
            }
        }
    }
}