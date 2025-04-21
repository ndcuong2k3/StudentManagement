using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentService.Models;
using System.Net.Http;

namespace Frontend.Pages.Students
{
    public class UpdateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<SelectListItem> LopHocList { get; set; } = new();

        public UpdateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("StudentAPI");
        }

        [BindProperty]
        public Student HocSinh { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int id { get; set; } // hoặc iMaHS nếu bạn dùng asp-route-iMaHS
        public async Task OnGetAsync()
        {
            //var lopHocResponse = await _httpClient.GetFromJsonAsync<List<Class>>("/api/students/GetClass");
            //var hocSinhResponse = await _httpClient.GetFromJsonAsync<Student>($"/api/students/GetStudentByID?iMaHS={id}");

            //if (hocSinhResponse != null)
            //{
            //    HocSinh = hocSinhResponse;
            //}

            //if (lopHocResponse != null)
            //{
            //    LopHocList = lopHocResponse.Select(l => new SelectListItem
            //    {
            //        Value = l.iMaLop.ToString(),
            //        Text = l.sTenLop.ToString()
            //    }).ToList();
            //}
         
                // Gọi API lấy danh sách lớp học
                var lopHocResponse = await _httpClient.GetAsync("/api/students/GetClass");
                if (lopHocResponse.IsSuccessStatusCode)
                {
                    var classList = await lopHocResponse.Content.ReadFromJsonAsync<List<Class>>();
                    if (classList != null)
                    {
                        LopHocList = classList.Select(l => new SelectListItem
                        {
                            Value = l.iMaLop.ToString(),
                            Text = l.sTenLop
                        }).ToList();
                    }
                }

                // Gọi API lấy học sinh theo ID
                var hocSinhResponse = await _httpClient.GetAsync($"/api/students/GetStudentByID?iMaHS={id}");
                if (hocSinhResponse.IsSuccessStatusCode)
                {
                    var student = await hocSinhResponse.Content.ReadFromJsonAsync<Student>();
                    if (student != null)
                    {
                        HocSinh = student;
                    }
                }
                else
                {
                    // Xử lý nếu học sinh không tồn tại, ví dụ redirect về Index
                    RedirectToPage("Index");
                }
            

        }


        public async Task<IActionResult> OnPostSendUpdateStudentRequestAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // gọi lại để điền lại dropdown nếu có lỗi
                return Page();
            }

            
            var response = await _httpClient.PostAsJsonAsync("/api/students/UpdateStudent ", HocSinh);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thêm học sinh.");
            await OnGetAsync(); // gọi lại để không mất dropdown
            return Page();
        }
    }
}
