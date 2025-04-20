using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.StudentService.Data;
using StudentService.Models;

namespace StudentManagementSystem.StudentService.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("GetStudents")]
    public async Task<ActionResult<List<Student>>> GetStudents()
    {
        return await _context.tblHocSinh.ToListAsync();
    }


    [HttpPost]
    [Route("AddStudent")]
    public async Task<ActionResult<Student>> AddStudent(Student student)
    {
        try
        {
            if (student == null)
            {
                return BadRequest("Student data is null");
            }

            _context.tblHocSinh.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudents), new { id = student.iMaHS.ToString() }, student);
        }
        catch (Exception ex)
        {
            // Trả lại chi tiết lỗi trong response để bạn debug
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



    [HttpGet]
    [Route("GetClass")]
    public async Task<ActionResult<List<Class>>> GetClass()
    {
        return await _context.tblLopHoc.ToListAsync();
    }


}