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
    public class OptionsController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public OptionsController(QuizDbContext context)
        {
            _context = context;
        }

        // ✅ Get all options
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OptionResponseDto>>> GetAllOptions()
        {
            var options = await _context.Options.AsNoTracking().ToListAsync();

            var response = options.Select(o => new OptionResponseDto
            {
                Id = o.Id,
                Text = o.Text,
                Correct = o.Correct
            }).ToList();

            return Ok(new { options = response });
        }

        // ✅ Get option by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OptionResponseDto>> GetOptionById(int id)
        {
            var option = await _context.Options.FindAsync(id);
            if (option == null)
            {
                return NotFound(new { message = "Option not found" });
            }

            return Ok(new OptionResponseDto
            {
                Id = option.Id,
                Text = option.Text,
                Correct = option.Correct
            });
        }

        // ✅ Create option
        [HttpPost]
        public async Task<ActionResult<OptionResponseDto>> PostOption(CreateOptionDto optionDto)
        {
            var question = await _context.Questions.FindAsync(optionDto.QuestionId);
            if (question == null)
            {
                return NotFound(new { message = "Question not found" });
            }

            var option = new Option
            {
                Text = optionDto.Text,
                Correct = optionDto.Correct,
                QuestionId = optionDto.QuestionId
            };

            _context.Options.Add(option);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOptionById), new { id = option.Id }, new OptionResponseDto
            {
                Id = option.Id,
                Text = option.Text,
                Correct = option.Correct
            });
        }

        // ✅ Update option
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOption(int id, CreateOptionDto optionDto)
        {
            var option = await _context.Options.FindAsync(id);
            if (option == null)
            {
                return NotFound(new { message = "Option not found" });
            }

            option.Text = optionDto.Text;
            option.Correct = optionDto.Correct;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Delete option
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOption(int id)
        {
            var option = await _context.Options.FindAsync(id);
            if (option == null)
            {
                return NotFound(new { message = "Option not found" });
            }

            _context.Options.Remove(option);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
