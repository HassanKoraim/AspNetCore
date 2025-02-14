using Microsoft.AspNetCore.Mvc;
using ServiceConstracts;

namespace CRUDExample.Controllers
{
    [Route("[Controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countiesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countiesService = countriesService;
        }
            [Route("[action]")]
        [HttpGet]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }
            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' is expected";
                return View();
            }
            int NCountriesInserted = await _countiesService.UploadCountriesFromExcelFile(excelFile);
            ViewBag.Message = $"{NCountriesInserted} Countries Uploaded";
            return View();
        }
    }
}
