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

            // Monta a URL com os par�metros
            var url = $"{baseUrl}?nome={Uri.EscapeDataString(nomeEmpresa)}&usuario={Uri.EscapeDataString(usuario)}&senha={Uri.EscapeDataString(senha)}";

            // Faz a requisi��o
            var response = await _httpClient.GetAsync(url);

            // Garante que deu sucesso
            response.EnsureSuccessStatusCode();

            // L� o conte�do como string
            var jsonString = await response.Content.ReadAsStringAsync();

            await DisplayAlert("Resposta:", jsonString, "OK");

            // Se quiser, voc� pode desserializar para objeto:
            // var resultado = JsonSerializer.Deserialize<SeuModelo>(jsonString);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao buscar dados", ex.Message, "OK");
        }
    }

    private void onENTRARClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(UsuarioEmpty.Text) || string.IsNullOrEmpty(SenhaEntry.Text))
            return;

        string usuario = UsuarioEmpty.Text.Replace(".", "").Replace("-", "").Replace(" ", "");

        BuscarFuncionarioAsync(nomeDaEmpresa, usuario, SenhaEntry.Text);
    }
}