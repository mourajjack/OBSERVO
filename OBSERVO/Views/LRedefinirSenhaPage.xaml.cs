namespace OBSERVO.Views;

public partial class RedefinirSenhaPage : ContentPage
{
	public RedefinirSenhaPage()
	{
		InitializeComponent();
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

    private void OnBackClicked(object sender, TappedEventArgs e)
    {

    }
}