    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using StudentManagementSystem.StudentService.Data;
    using StudentService.Models;
    using System.Net.Http;

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
    [Route("validateStudentInformation")]
    public ActionResult validateStudentInformation(Student student)
    {
        if (student == null)
        {
            return BadRequest("Chưa điền đầy đủ thông tin");
        }

        // Kiểm tra các trường bắt buộc
        if (string.IsNullOrEmpty(student.sHoTen))
        {
            return BadRequest("Chưa điền đầy đủ thông tin");
        }

        if (string.IsNullOrEmpty(student.sDiaChi))
        {
            return BadRequest("Chưa điền đầy đủ thông tin");
        }

        if (string.IsNullOrEmpty(student.sSoDienThoai))
        {
            return BadRequest("Chưa điền đầy đủ thông tin");
        }

        // Thêm các kiểm tra khác tùy theo yêu cầu

        return Ok("Dữ liệu hợp lệ");
    }

    [HttpPost]
    [Route("checkDuplicateStudentID")]
    public async Task<ActionResult> CheckDuplicateStudentID( Student student)
    {
        try
        {
            // Kiểm tra xem mã học sinh có tồn tại trong cơ sở dữ liệu không
            var studentExists = await _context.tblHocSinh
                                              .AnyAsync(s => s.iMaHS == student.iMaHS).ConfigureAwait(false); 

            if (studentExists)
            {
                // Nếu có, trả về thông báo rằng mã học sinh đã tồn tại
                return BadRequest("Mã học sinh đã tồn tại.");
            }

            // Nếu không có, trả về thông báo mã học sinh hợp lệ
            return Ok("Không trùng mã học sinh");
        }
        catch (Exception ex)
        {
            // Xử lý lỗi và trả về lỗi server nếu có vấn đề trong quá trình truy vấn
            return StatusCode(500, $"Lỗi server: {ex.Message}");
        }
    }


    [HttpPost]
        [Route("SaveStudentInformation")]
        public async Task<ActionResult<Student>> SaveStudentInformation(Student student)
        {
            try
            {
                _context.tblHocSinh.Add(student);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStudents), new { id = student.iMaHS.ToString() }, student);
            }
            catch (Exception ex)
            {
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