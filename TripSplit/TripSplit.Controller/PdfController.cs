using Microsoft.AspNetCore.Mvc;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfWriterService pdfWriterService;

        public PdfController(IPdfWriterService pdfWriterService)
        {
            this.pdfWriterService = pdfWriterService ?? throw new ArgumentNullException(nameof(pdfWriterService));
        }

        [HttpGet("download-report")]
        public async Task<IActionResult> DownloadPdfReport(int tripId)
        {
            var filePath = await pdfWriterService.WritePdf(tripId);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("The report could not be generated.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/pdf", Path.GetFileName(filePath));
        }
    }
}
