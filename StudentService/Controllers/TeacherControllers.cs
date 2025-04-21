using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.StudentService.Data;
using StudentService.Models;

namespace StudentService.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeacherControllers:ControllerBase
    {
        private readonly AppDbContext _context;

        public TeacherControllers(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetTeachers")]
        public async Task<ActionResult<List<Teacher>>> GetTeachers()
        {
            return await _context.tblGiaoVien.ToListAsync();
        }
    }
}
