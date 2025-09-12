using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Por favor, preencha a descrição");

                _descricao = value;
            }
        }

        public double Quantidade { get; set; }

        public double Preco { get; set; }

        // Propriedade calculada
        public double Total => Quantidade * Preco;

        // Novo campo: Data de cadastro/compra
        public DateTime DataCadastro { get; set; }
    }
}