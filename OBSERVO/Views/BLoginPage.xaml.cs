namespace OBSERVO.Views;

public partial class LoginPage : ContentPage
{
	static string nomeDaEmpresa = string.Empty;
    private readonly HttpClient _httpClient;

    public LoginPage()
	{
		InitializeComponent();
    }

	public LoginPage(string[] logoAndName)
	{
        InitializeComponent();
        _httpClient = new HttpClient();

        LogoEmpresa.Source = ImageSource.FromUri(new Uri(logoAndName[0]));
		nomeDaEmpresa = logoAndName[1];
		//DisplayAlert("> ", logoAndName[1], "OK");
    }

    public async Task BuscarFuncionarioAsync(string nomeEmpresa, string usuario, string senha)
    {
        try
        {
            // Sua URL do script
            var baseUrl = API_DB_CONN.URI_Colaboradores;

            // Monta a URL com os parâmetros
            var url = $"{baseUrl}?nome={Uri.EscapeDataString(nomeEmpresa)}&usuario={Uri.EscapeDataString(usuario)}&senha={Uri.EscapeDataString(senha)}";

            // Faz a requisição
            var response = await _httpClient.GetAsync(url);

            // Garante que deu sucesso
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("📶 ERRO de REDE", "Verifique sua Conexão com a Internet", "OK");
                return;
            }

            // Lê o conteúdo como string
            var jsonString = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Resposta:", jsonString, "OK");

            // Se quiser, você pode desserializar para objeto:
            // var resultado = JsonSerializer.Deserialize<SeuModelo>(jsonString);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao buscar dados", ex.Message, "OK");
        }
    }

    private async void onENTRARClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(UsuarioEmpty.Text) || string.IsNullOrEmpty(SenhaEntry.Text))
            return;
        //
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        BtnENTRAR.IsEnabled = false;

        string usuario = UsuarioEmpty.Text.Replace(".", "").Replace("-", "").Replace(" ", "");

        await BuscarFuncionarioAsync(nomeDaEmpresa, usuario, SenhaEntry.Text);

        App.Current.MainPage = new MainFlyoutPage();

        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;
        BtnENTRAR.IsEnabled = true;
    }
}