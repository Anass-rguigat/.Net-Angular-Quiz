using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetQuizz.Models.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public string Lesson { get; set; }

        public string Explanation { get; set; }

        [JsonIgnore]
        public List<Option> Options { get; set; } = new List<Option>();
    }
}
