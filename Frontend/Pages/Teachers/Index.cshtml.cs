using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementSystem.StudentService.Data;
using StudentService.Models;

namespace Frontend.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Teacher>? Teachers { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TeacherAPI");
        }

        public async Task OnGetAsync()
        {
            Teachers = await _httpClient.GetFromJsonAsync<List<Teacher>>("/api/teachers/GetTeachers");
        }
    }
}
