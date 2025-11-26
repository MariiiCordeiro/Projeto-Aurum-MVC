namespace AurumLab.Models
{ //* Criacao de novos atributos que nao ficarao no banco e irao mostrar na tela
    public class DashboardViewModel
    {
        public int TotalDispositivos { get; set; }

        public int TotalAtivos { get; set; }

        public int TotalEmManutencao { get; set; }

        public int TotalInoperantes { get; set; }

        //* Usuario
        public string NomeUsuario { get; set; }

        public string FotoUsuario { get; set; }

        //* Lista de agrupamento
        public List<ItemAgrupado> DispositivosPorTipo { get; set; }
        public List<LocalDispositivo> Locais { get; set; }


    }
}