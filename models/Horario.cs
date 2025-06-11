using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_minimal
{
    public class Horario
    {
        [Key]
        public int IdHorario { get; set; }

        [Required]
        [Range(1, 7)] // Dias da semana
        public int DiaSemana { get; set; }

        [Required]
        [StringLength(20)]
        public string HorarioAtendimento { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Monitor")]
        public int IdMonitor { get; set; }

        // Navigation property
        public virtual Monitor Monitor { get; set; } = null!;
    }
}