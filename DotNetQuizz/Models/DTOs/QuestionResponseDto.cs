using System.Collections.Generic;

namespace DotNetQuizz.Models.DTOs
{
    public class QuestionResponseDto
    {
        public int Id { get; set; } // ✅ Ajout de l'ID
        public string QuestionText { get; set; }
        public string Lesson { get; set; }
        public string Explanation { get; set; }
        public List<OptionResponseDto> Options { get; set; }
    }

    public class OptionResponseDto
    {
        public int Id { get; set; } // ✅ Ajout de l'ID
        public string Text { get; set; }
        public bool Correct { get; set; }
    }

    

    
}
