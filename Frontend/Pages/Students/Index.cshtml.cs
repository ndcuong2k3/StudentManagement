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
}