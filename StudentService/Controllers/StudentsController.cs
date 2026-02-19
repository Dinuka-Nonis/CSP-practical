using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.DTOs;
using StudentService.Models;

namespace StudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAll()
    {
        var students = await _context.Students.ToListAsync();
        return Ok(students.Select(MapToDto));
    }

    // GET: api/students/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentResponseDto>> GetById(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null)
            return NotFound(new { message = $"Student with ID {id} not found." });

        return Ok(MapToDto(student));
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<StudentResponseDto>> Create([FromBody] CreateStudentDto dto)
    {
        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Course = dto.Course,
            Year = dto.Year,
            CreatedAt = DateTime.UtcNow
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = student.Id }, MapToDto(student));
    }

    // PUT: api/students/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<StudentResponseDto>> Update(int id, [FromBody] UpdateStudentDto dto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null)
            return NotFound(new { message = $"Student with ID {id} not found." });

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;
        student.Course = dto.Course;
        student.Year = dto.Year;

        await _context.SaveChangesAsync();

        return Ok(MapToDto(student));
    }

    // DELETE: api/students/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null)
            return NotFound(new { message = $"Student with ID {id} not found." });

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static StudentResponseDto MapToDto(Student student) => new()
    {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        Email = student.Email,
        Course = student.Course,
        Year = student.Year,
        CreatedAt = student.CreatedAt
    };
}
