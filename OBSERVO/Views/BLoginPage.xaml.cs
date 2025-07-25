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

    public async Task<string> BuscarFuncionarioAsync(string nomeEmpresa, string usuario, string senha)
    {
        try
        {
            // Sua URL do script
            var baseUrl = API_DB_CONN.URI_Colaboradores;

            // Monta a URL com os parâmetros
            var url = $"{baseUrl}?nome={Uri.EscapeDataString(nomeEmpresa)}&usuario={Uri.EscapeDataString(usuario)}&senha={Uri.EscapeDataString(senha)}";

            // Faz a requisição
            var response = await _httpClient.GetAsync(url);

            // Garante que deu sucesso, se o programa passar dessa linha deu bom.
            response.EnsureSuccessStatusCode();
            //só pra garantir... nem precisava.
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("📶 ERRO de REDE", "Verifique sua Conexão com a Internet", "OK");
                return string.Empty;
            }

            // Lê o conteúdo como string: ✔ Ele só lê o conteúdo da resposta que já foi baixado.
            var jsonString = await response.Content.ReadAsStringAsync();
            return jsonString;
            //await DisplayAlert("Resposta:", jsonString, "OK");

            // Se quiser, você pode desserializar para objeto:
            // var resultado = JsonSerializer.Deserialize<SeuModelo>(jsonString);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao buscar os dados", ex.Message, "OK");
            return string.Empty;
        }
    }

    private async void onENTRARClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(UsuarioEmpty.Text) || string.IsNullOrEmpty(SenhaEntry.Text))
            return;

        //Comment here
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        BtnENTRAR.IsEnabled = false;

        string usuario = UsuarioEmpty.Text.Replace(".", "").Replace("-", "").Replace(" ", "");


        //BuscarFuncionarioAsync, busca as info
        //xxxxxxxxxxxxxxxxxxxxxx, grava na local db
        //se retornar true, posso chamar:  App.Current.MainPage = new MainFlyoutPage();

        string funcionario = await BuscarFuncionarioAsync(nomeDaEmpresa, usuario, SenhaEntry.Text);

        if (!string.IsNullOrEmpty(funcionario))
        {
            //vamos gravar na local db

        }

        App.Current.MainPage = new MainFlyoutPage();

        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;
        BtnENTRAR.IsEnabled = true;
    }
}