using OBSERVO.Models;
using System.Net.Http;
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

        nomeDaEmpresa = logoAndName[1];

        ShowLogo(logoAndName);
        //DisplayAlert("> ", logoAndName[1], "OK");
    }

    public async void ShowLogo(string[] logoAndName)
    {
        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(logoAndName[0]);

            // Garante que deu sucesso, se o programa passar dessa linha deu bom.
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                LogoEmpresa.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception)
        {
            LogoEmpresa.Source = "socialentrepreneurship.png";
        }

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
            //vamos testar se o servidor retornou: (success: true)
            //vamos gravar na local db
            var json = System.Text.Json.JsonDocument.Parse(funcionario);
            if (json.RootElement.GetProperty("success").GetBoolean())
            {
                var colaborador = new Colaboradores
                {
                    Id = 0,
                    Matricula = json.RootElement.GetProperty("matricula").GetString(),
                    Nome = json.RootElement.GetProperty("nome").GetString(),
                    Cpf = json.RootElement.GetProperty("cpf").GetString(),
                    Senha = json.RootElement.GetProperty("senha").GetString(),
                    DataNascimento = json.RootElement.GetProperty("dataDeNascimento").GetString(),
                    Email = json.RootElement.GetProperty("email").GetString(),
                    Telefone = json.RootElement.GetProperty("telefone").GetString(),
                    Funcao = json.RootElement.GetProperty("funcao").GetString(),
                    Escala = json.RootElement.GetProperty("escalaDeServico").GetString(),
                    Posto = json.RootElement.GetProperty("postoDeServico").GetString(),
                    DataAdmissao = json.RootElement.GetProperty("dataDaAdmissao").GetString(),
                    Empresa = json.RootElement.GetProperty("empresa").GetString(),
                    Cnpj = json.RootElement.GetProperty("cnpj").GetString()
                };

                //Salvar no db local:
                try
                {
                    int result = await App.SQLiteDB.ColaboradorSaveAndUpdateInLocalDBAsync(colaborador);
                    //int result = await App.SQLiteDB.ColaboradorDeleteItemAsync(colaborador);

                    if (result > 0)
                    {
                        // Sucesso
                        App.Current.MainPage = new MainFlyoutPage();
                    }
                    else
                    {
                        // Se der erro ao salvar ou cair em uma excepton
                        //delete a tabela;
                        if(await App.SQLiteDB.DeletarTabelaColaboradoresAsync())
                        {
                            //await DisplayAlert("result <= 0", "result <= 0", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Se der erro ao salvar ou cair em uma excepton
                    //delete a tabela;
                    // Se der erro ao salvar ou cair em uma excepton
                    //delete a tabela;
                    if (await App.SQLiteDB.DeletarTabelaColaboradoresAsync())
                    {
                        //await DisplayAlert("Delete tabela", "deletado com sucesso", "OK");
                        await DisplayAlert("⚠ Erro ao salvar", ex.Message, "OK");
                    }

                    FecharAplicativo();

                    //Volta pro inicio
                    var pagina = new NavigationPage(
                    new SelectCompany()
                    );

                    pagina.BarBackgroundColor = Color.FromRgb(116, 8, 98);
                    pagina.BarTextColor = Color.FromRgb(219, 219, 219);

                    App.Current.MainPage = pagina;

                }

            }
            else
            {
                //
                await DisplayAlert("🚫 Acesso Negado", "Nome de usuário ou senha inválidos. Por favor, tente novamente.", "OK");
            }
        }
        else
        {
            await DisplayAlert("⚠ ERRO DE REDE", "Erro ao buscar colaborador", "OK");
            //...
        }

        //App.Current.MainPage = new MainFlyoutPage();

        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;
        BtnENTRAR.IsEnabled = true;
    }

    public void FecharAplicativo()
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
    System.Environment.Exit(0);
#elif IOS
    // Não recomendado! Apenas exibir uma mensagem ou retornar à tela inicial.
#endif
    }
}