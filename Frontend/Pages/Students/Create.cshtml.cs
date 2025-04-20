using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentService.Models;
using System.Net.Http.Json;

namespace Frontend.Pages.Students;

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
                Text = l.iMaLop.ToString()
            }).ToList();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(); // cần gọi lại để điền lại dropdown nếu có lỗi
            return Page();
        }

        var response = await _httpClient.PostAsJsonAsync("/api/students/AddStudent", HocSinh);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("Index");
        }

        ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thêm học sinh.");
        await OnGetAsync(); // gọi lại để không mất dropdown
        return Page();
    }
}
