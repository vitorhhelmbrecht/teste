using Questao5.Domain.Entidades.Comum;

namespace Questao5.Domain.Entidades
{
    public class IdEmPotencia : EntidadeBase
    {
        public string Id { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }

        public IdEmPotencia() {
            Id = string.Empty;
            Requisicao = string.Empty;
            Resultado = string.Empty;
            ErrosDeValidacao = new();
        }
    }
}
