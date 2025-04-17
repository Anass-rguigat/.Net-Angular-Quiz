using System.ComponentModel.DataAnnotations;

namespace DotNetQuizz.Models.DTOs
{
    public class CreateQuestionDto
    {
        [Required]
        public string QuestionText { get; set; }
        public string Lesson { get; set; }
        public string Explanation { get; set; }
    }
}
