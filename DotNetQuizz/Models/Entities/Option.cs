using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DotNetQuizz.Models.Entities
{
    public class Option
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public bool Correct { get; set; }

        [ForeignKey(nameof(Question))]
        [JsonIgnore]
        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
