using System;

namespace OBSERVO.Views;

public partial class IUserAccountSettingsPage : ContentPage
{
	public IUserAccountSettingsPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //inicializar colaborador, buscar colaborador no localDB
        try
        {
            var colaborador = await App.SQLiteDB.ColaboradorGetAsync(0);
            if (colaborador != null)
            {
                InitialsLabel.Text = ObterIniciais(colaborador.Nome);
                NomeColaborador.Text = colaborador.Nome;
                Funcao.Text = colaborador.Funcao;
                Cpf.Text = colaborador.Cpf;
                Telefone.Text = colaborador.Telefone;
                Email.Text = colaborador.Email;
                Escala.Text = colaborador.Escala;
                Matricula.Text = colaborador.Matricula;
                Posto.Text = colaborador.Posto;
                Admissao.Text = colaborador.DataAdmissao;
                Empresa.Text = colaborador.Empresa;
                Cnpj.Text = colaborador.Cnpj;

            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }

    public static string ObterIniciais(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            return string.Empty;

        var partes = nomeCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (partes.Length == 0)
            return string.Empty;

        string primeiraLetra = partes[0].Substring(0, 1).ToUpper();
        string ultimaLetra = partes[^1].Substring(0, 1).ToUpper(); // ^1 = última posição (C# 8+)

        return primeiraLetra + ultimaLetra;
    }

    private void OnChangePhotoClicked(object sender, EventArgs e)
    {

    }
}