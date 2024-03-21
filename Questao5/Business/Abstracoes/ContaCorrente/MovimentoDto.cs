namespace Questao5.Business.Abstracoes.ContaCorrente
{
    public class MovimentoDto
    {
        public string IdRequisicao { get; set; }
        public int NumeroContaCorrente { get; set; }
        public double Valor { get; set; }
        public string Tipo { get; set; }
    }
}
