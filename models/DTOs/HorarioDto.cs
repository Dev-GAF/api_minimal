namespace api_minimal.models.DTOs
{
    public class MonitorSimplesDto
    {
        public int IdMonitor { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
    }
    public class HorarioDto
    {
        public int DiaSemana { get; set; }
        public string HorarioAtendimento { get; set; } = string.Empty;
        public int IdMonitor { get; set; }

        public MonitorSimplesDto? Monitor { get; set; }
    }

    public class HorarioComIdDto : HorarioDto
    {
        public int IdHorario { get; set; }
    }
}