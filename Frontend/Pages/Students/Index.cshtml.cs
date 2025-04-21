using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentService.Models;

namespace Frontend.Pages.Students;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    public List<Student>? Students { get; set; }

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("StudentAPI");
    }

    public async Task OnGetAsync()
    {
        Students = await _httpClient.GetFromJsonAsync<List<Student>>("/api/students/GetStudents");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/students/DeleteStudent/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage();
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Xóa học sinh thất bại.");
            await OnGetAsync(); 
            return Page();
        }
    }

}