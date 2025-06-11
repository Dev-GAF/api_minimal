using System.ComponentModel.DataAnnotations;

namespace api_minimal
{
    public class Monitor
    {
        [Key]
        public int IdMonitor { get; set; }

        [Required]
        [StringLength(100)]
        public string NOme { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Apelido { get; set; } = string.Empty;

        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
    }
}