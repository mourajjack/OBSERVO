using System.Text.Json;

namespace OBSERVO.Views;

public partial class SelectCompany : ContentPage
{
	public SelectCompany()
	{
		InitializeComponent();
	}

    public async Task<string[]> GetCompanyByName(string name)
    {
        try
            {
            HttpClient _httpClient = GetClient();

            var response = await _httpClient.GetAsync(API_DB_CONN.URI_SelectCompany + "?name=" + name);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Wi-Fi erro. Verifique sua conexão com a internet e tente novamente mais tarde!");
            }

            var dataString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(dataString))
            {
                var json = System.Text.Json.JsonDocument.Parse(dataString);
                if (json.RootElement.GetProperty("success").GetBoolean())
                {
                    string[] logo = new string[2];
                    logo[0] = json.RootElement.GetProperty("logo").GetString();
                    logo[1] = json.RootElement.GetProperty("name").GetString();
                    return logo;
                }
                else
                {
                    await DisplayAlert(CompanyName.Text + "❓", "Empresa não encontrada. Verifique se o nome está correto.", "OK");
                    string[] logo = new string[2];
                    return logo;
                }
            }
            else
            {
                await DisplayAlert("📶 ERRO de REDE", "Verifique sua Conexão com a Internet", "OK");
                string[] logo = new string[2];
                return logo;
            }

        }
        catch (Exception)
        {
            await DisplayAlert("📶 ERRO DE REDE", "Verifique sua Conexão com a Internet", "OK");
            string[] logo = new string[2];
            return logo;
        }
    }

    public HttpClient GetClient()
    {
        HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(10);
        client.DefaultRequestHeaders.Add("Accept", "Application/json");
        client.DefaultRequestHeaders.Add("Connection", "close");
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    private async void OnPROSSEGUIR_Clicked(object sender, EventArgs e)
    {
        //verificar se o usuário digitou alguma coisa;
        if (string.IsNullOrEmpty(CompanyName.Text))
            return;

        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        OnPROSSEGUIR.IsEnabled = false;
        //Logo vai na [0],
        //name vai na [1];
        string[] logoAndName = new string[2];
        string companyName = CompanyName.Text.Replace(" ","");

        if (!string.IsNullOrEmpty(companyName))
        {
            logoAndName = await GetCompanyByName(companyName);
        }

        if (!string.IsNullOrEmpty(logoAndName[0]))
        {
            //MinhaImagem.Source = ImageSource.FromUri(new Uri(logo));
            //App.Current.MainPage = new NavigationPage(new LoginPage(logo));

            var pagina = new NavigationPage(
                new LoginPage(logoAndName)
                    );

            pagina.BarBackgroundColor = Color.FromRgb(139, 0, 139);
            pagina.BarTextColor = Color.FromRgb(219, 219, 219);

            await Navigation.PushAsync(pagina);
        }

        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;
        OnPROSSEGUIR.IsEnabled = true;

    }
}