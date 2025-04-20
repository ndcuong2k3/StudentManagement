using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentService.Models;
using System.Net.Http.Json;

namespace Frontend.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Student HocSinh { get; set; } = new();

        public List<SelectListItem> LopHocList { get; set; } = new();

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("StudentAPI");
        }

        public async Task OnGetAsync()
        {
            var lopHocResponse = await _httpClient.GetFromJsonAsync<List<Class>>("/api/students/GetClass");

            if (lopHocResponse != null)
            {
                LopHocList = lopHocResponse.Select(l => new SelectListItem
                {
                    Value = l.iMaLop.ToString(),
                    Text = l.sTenLop.ToString()
                }).ToList();
            }
        }

        public async Task<IActionResult> OnPostSendAddStudentRequestAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // gọi lại để điền lại dropdown nếu có lỗi
                return Page();
            }

            // Gọi API validate trước khi thêm học sinh
            var validateResponse = await _httpClient.PostAsJsonAsync("/api/students/validateStudentInformation", HocSinh);

            if (!validateResponse.IsSuccessStatusCode)
            {
                // Nếu validation thất bại, lấy thông báo lỗi và hiển thị
                var validationError = await validateResponse.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, validationError);
                await OnGetAsync(); // gọi lại để không mất dropdown
                return Page();
            }

            // Kiểm tra mã học sinh có trùng không
            var checkDuplicateResponse = await _httpClient.PostAsJsonAsync("/api/students/checkDuplicateStudentID", HocSinh);

            if (!checkDuplicateResponse.IsSuccessStatusCode)
            {
                // Nếu mã học sinh đã tồn tại, lấy thông báo lỗi và hiển thị
                var duplicateError = await checkDuplicateResponse.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, duplicateError);
                await OnGetAsync(); // gọi lại để không mất dropdown
                return Page();
            }

            // Nếu validation thành công, tiếp tục gọi API AddStudent
            var response = await _httpClient.PostAsJsonAsync("/api/students/SaveStudentInformation", HocSinh);

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
