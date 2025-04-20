namespace StudentService.Models
{
    public class Student
    {
        public int iMaHS { get; set; }

        public string sHoTen { get; set; } = null!;

        public bool bGioiTinh { get; set; }

        public DateTime dNgaySinh { get; set; }

        public string sDiaChi { get; set; } = null!;

        public string sSoDienThoai { get; set; } = null!;

        public int iMaLop { get; set; }

        public bool bTrangThai { get; set; }
    }
}
