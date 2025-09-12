
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> Lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = Lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("OPS", ex.Message, "OK");
        }
    }

    private async Task CarregarProdutos(string filtro = "")
    {
        Lista.Clear();

        List<Produto> tmp = string.IsNullOrWhiteSpace(filtro)
            ? await App.Db.GetAll()
            : await App.Db.Search(filtro);

        tmp.ForEach(i => Lista.Add(i));
    }

    // Botão "Adicionar"
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlert("OPS", ex.Message, "OK");
        }
    }

    // Botão "Relatório" -> Relatório por Categoria
    private async void ToolbarItem_Relatorio_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.RelatorioCategoria(Lista.ToList()));
        }
        catch (Exception ex)
        {
            await DisplayAlert("OPS", ex.Message, "OK");
        }
    }

    // Filtro de pesquisa
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;
            lst_produtos.IsRefreshing = true;

            await CarregarProdutos(q);
        }
        catch (Exception ex)
        {
            await DisplayAlert("OPS", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    // Botão "Somar"
    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = Lista.Sum(i => i.Total);
            string msg = $"O total é {soma:C}";
            await DisplayAlert("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("OPS", ex.Message, "OK");
        }
    }

    // Remover produto
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is MenuItem selecionado && selecionado.BindingContext is Produto p)
            {
                bool confirm = await DisplayAlert(
                    "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

                if (confirm)
                {
                    await App.Db.Delete(p.Id);
                    Lista.Remove(p);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Editar produto
    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem is Produto produto)
            {
                await Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = produto,
                });
                lst_produtos.SelectedItem = null; // limpa seleção
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Atualizar lista (pull-to-refresh)
    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("OPS", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}
