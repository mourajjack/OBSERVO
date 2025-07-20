using System.Text.Json;

namespace OBSERVO.Views;

public partial class SelectCompany : ContentPage
{
	public SelectCompany()
	{
		InitializeComponent();
	}

    public async Task<string> GetCompanyByName(string name)
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
                    var logo = json.RootElement.GetProperty("logo").GetString();
                    return logo;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Resposta vazia da API");
            }

        }
        catch (Exception)
        {
            throw new Exception("INTERNET ERRO");
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
        var logo = await GetCompanyByName("CETSEG");
        MinhaImagem.Source = ImageSource.FromUri(new Uri(logo));
    }
}