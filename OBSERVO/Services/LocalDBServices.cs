using OBSERVO.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSERVO.Services
{
    public class LocalDBServices
    {
        SQLiteAsyncConnection localDB;

        public LocalDBServices(string dbPath)
        {
            localDB = new SQLiteAsyncConnection(dbPath);
            localDB.CreateTableAsync<Colaboradores>().Wait();
        }

        //Insert and Update new record DB Local
        public Task<int> ColaboradorSaveAndUpdateInLocalDBAsync(Colaboradores colaboradores)
        {
            if (colaboradores.Id == 0)
            {
                return localDB.InsertAsync(colaboradores);
            }
            else
            {
                return localDB.UpdateAsync(colaboradores);
            }
        }

        public async Task CriaTabelaColaboradores()
        {
             localDB.CreateTableAsync<Colaboradores>().Wait();
        }

        public Task<Colaboradores> ColaboradorGetAsync(int colaboradorId)
        {
            return localDB.Table<Colaboradores>().Where(i => i.Id == colaboradorId).FirstOrDefaultAsync();
        }

        public Task<int> ColaboradorDeleteItemAsync(Colaboradores colaboradores)
        {
            return localDB.DeleteAsync(colaboradores);
        }

        public async Task<bool> DeletarTabelaColaboradoresAsync()
        {
            try
            {
                await localDB.ExecuteAsync("DROP TABLE IF EXISTS Colaboradores");
                return true; // sucesso
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar tabela: {ex.Message}");
                return false; // falha
            }
        }

    }
}
