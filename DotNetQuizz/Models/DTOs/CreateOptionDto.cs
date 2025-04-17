using System.ComponentModel.DataAnnotations;

namespace DotNetQuizz.Models.DTOs
{
    public class CreateOptionDto
    {
        [Required]
        public string Text { get; set; }
        public bool Correct { get; set; }

        [Required]
        public int QuestionId { get; set; }  
    }
}
