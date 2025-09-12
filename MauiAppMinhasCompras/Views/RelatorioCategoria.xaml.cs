using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    public RelatorioCategoria(List<Produto> produtos)
    {
        InitializeComponent();
        GerarRelatorio(produtos);
    }

    private void GerarRelatorio(List<Produto> produtos)
    {
        var relatorio = produtos
            .GroupBy(p => string.IsNullOrEmpty(p.Categoria) ? "Sem categoria" : p.Categoria)
            .Select(g => new
            {
                Categoria = g.Key,
                Total = g.Sum(p => p.Total)
            })
            .OrderBy(r => r.Categoria)
            .ToList();

        RelatorioCollectionView.ItemsSource = relatorio;
    }
}
