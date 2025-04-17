using DotNetQuizz.Data;
using DotNetQuizz.Models.DTOs;
using DotNetQuizz.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetQuizz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public QuestionsController(QuizDbContext context)
        {
            _context = context;
        }

        // ✅ Get all questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionResponseDto>>> GetQuestions()
        {
            var questions = await _context.Questions
                .Include(q => q.Options)
                .AsNoTracking()
                .ToListAsync();

            var response = questions.Select(q => new QuestionResponseDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                Lesson = q.Lesson,
                Explanation = q.Explanation,
                Options = q.Options.Select(o => new OptionResponseDto
                {
                    Id = o.Id,
                    Text = o.Text,
                    Correct = o.Correct
                }).ToList()
            }).ToList();

            return Ok(new { questions = response });
        }

        // ✅ Get question by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionResponseDto>> GetQuestionById(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound(new { message = "Question not found" });
            }

            return Ok(new QuestionResponseDto
            {
                Id = question.Id,
                QuestionText = question.QuestionText,
                Lesson = question.Lesson,
                Explanation = question.Explanation,
                Options = question.Options.Select(o => new OptionResponseDto
                {
                    Id = o.Id,
                    Text = o.Text,
                    Correct = o.Correct
                }).ToList()
            });
        }

        // ✅ Create question
        [HttpPost]
        public async Task<ActionResult<QuestionResponseDto>> PostQuestion(CreateQuestionDto questionDto)
        {
            var question = new Question
            {
                QuestionText = questionDto.QuestionText,
                Lesson = questionDto.Lesson,
                Explanation = questionDto.Explanation
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestionById), new { id = question.Id }, new QuestionResponseDto
            {
                Id = question.Id,
                QuestionText = question.QuestionText,
                Lesson = question.Lesson,
                Explanation = question.Explanation,
                Options = new List<OptionResponseDto>()
            });
        }

        // ✅ Update question
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, CreateQuestionDto questionDto)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound(new { message = "Question not found" });
            }

            question.QuestionText = questionDto.QuestionText;
            question.Lesson = questionDto.Lesson;
            question.Explanation = questionDto.Explanation;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Delete question
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound(new { message = "Question not found" });
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
