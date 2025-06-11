namespace api_minimal.models.DTOs
{
    public class HorarioDto
    {
        public int DiaSemana { get; set; }
        public string HorarioAtendimento { get; set; } = string.Empty;
        public int IdMonitor { get; set; } 
    }

    public class HorarioComIdDto : HorarioDto
    {
        public int IdHorario { get; set; }
    }
}