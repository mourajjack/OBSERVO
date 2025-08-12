using System.Text.RegularExpressions;

namespace OBSERVO.Views;

public partial class Email : ContentPage
{
	public Email()
	{
		InitializeComponent();
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

    private void onSaveClicked(object sender, EventArgs e)
    {

    }
}