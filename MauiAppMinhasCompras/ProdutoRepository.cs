using SQLite;
using MauiAppMinhasCompras.Models;

public class ProdutoRepository
{
    private readonly SQLiteAsyncConnection _db;

    public ProdutoRepository(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<Produto>().Wait();
    }

    // 🔎 Buscar por categoria
    public Task<List<Produto>> GetProdutosByCategoriaAsync(string categoria)
    {
        if (categoria == "Todos")
        {
            return _db.Table<Produto>().ToListAsync(); // retorna tudo
        }

        return _db.Table<Produto>()
                  .Where(p => p.Categoria == categoria)
                  .ToListAsync(); // retorna só daquela categoria
    }
}
