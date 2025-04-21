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

    [HttpGet]
    [Route("GetStudentByID")]
    public async Task<ActionResult<List<Student>>> GetStudentByID(int iMaHS)
    {
        var student = await _context.tblHocSinh
                               .FirstOrDefaultAsync(s => s.iMaHS == iMaHS)
                               .ConfigureAwait(false);
        return Ok(student);
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
    public async Task<ActionResult> CheckDuplicateStudentID(Student student)
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

    [HttpPost]
    [Route("UpdateStudent")]
    public async Task<ActionResult<Student>> UpdateStudent(Student student)
    {
        try
        {
            var studentExits = await _context.tblHocSinh.FirstOrDefaultAsync(hs => hs.iMaHS == student.iMaHS);
            studentExits.sHoTen = student.sHoTen;
            studentExits.sDiaChi = student.sDiaChi;
            studentExits.dNgaySinh = student.dNgaySinh;
            studentExits.bGioiTinh = student.bGioiTinh;
            studentExits.bTrangThai = student.bTrangThai;
            studentExits.sSoDienThoai = student.sSoDienThoai;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật học sinh thành công", data = studentExits });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete]
    [Route("DeleteStudent/{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        try
        {
            var student = await _context.tblHocSinh.FindAsync(id);

            if (student == null)
            {
                return NotFound($"Không tìm thấy học sinh với mã: {id}");
            }

            _context.tblHocSinh.Remove(student);
            await _context.SaveChangesAsync();

            return Ok($"Đã xóa học sinh có mã: {id}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi server: {ex.Message}");
        }
    }



    [HttpGet]
    [Route("GetClass")]
    public async Task<ActionResult<List<Class>>> GetClass()
    {
        return await _context.tblLopHoc.ToListAsync();
    }


}