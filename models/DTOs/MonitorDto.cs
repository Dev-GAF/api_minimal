namespace api_minimal.models.DTOs
{
    public class MonitorDto
    {
        public string RA { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
    }

    public class MonitorComHorariosDto
    {
        public int IdMonitor { get; set; }
        public string RA { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public List<HorarioDto> Horarios { get; set; } = new();
    }
}