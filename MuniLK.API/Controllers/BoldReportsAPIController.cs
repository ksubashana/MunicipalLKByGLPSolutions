using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Memory;
using MuniLK.Application.Documents.Interfaces;
using MuniLK.Application.Documents.Queries.GetDocument;
using MuniLK.Application.Reports.Commands.UploadReport;
using MuniLK.Application.Reports.DTOs;
using MuniLK.Application.Reports.Queries;
using System.Reflection;

namespace BlazorReportingTools
{
    [ApiController]
    [Route("api/{controller}/{action}/{id?}")]
    public class BoldReportsAPIController : ControllerBase , IReportController, IReportLogger
    {
        // Report viewer requires a memory cache to store the information of consecutive client requests and
        // the rendered report viewer in the server.
        private readonly IMemoryCache _cache;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMediator _mediator;
        private readonly ILogger<BoldReportsAPIController> _logger;

        // IWebHostEnvironment used with sample to get the application data from wwwroot.
        private IWebHostEnvironment _hostingEnvironment;

        public BoldReportsAPIController(IMemoryCache memoryCache,
            IWebHostEnvironment hostingEnvironment,
        IBlobStorageService blobStorageService,
        IMediator mediator,
        ILogger<BoldReportsAPIController> logger)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            _blobStorageService = blobStorageService;
            _mediator = mediator;
            _logger = logger;
        }

        //Get action for getting resources from the report
        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        // Method will be called from Report Viewer client to get the image src for Image report item.
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }
        [NonAction]
        public async void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            try
            {
                //var reportID = new GetReportQuery(Guid.Parse(reportOption.ReportModel.ReportPath));

                // Here, use CQRS to get the blob path
                //var report = await _mediator.Send(reportID);

                //if (report == null || string.IsNullOrWhiteSpace(report.BlobPath))
                //    throw new FileNotFoundException("Report metadata not found for ReportPath.");

                //var blobResult = await _blobStorageService.DownloadAsync(report.BlobPath, default);
                //if (blobResult == null)
                //    throw new FileNotFoundException("RDL file not found in Blob Storage.");


                //var memoryStream = new MemoryStream();
                //await blobResult.Value.Content.CopyToAsync(memoryStream);
                //memoryStream.Position = 0;

                //reportOption.ReportModel.Stream = memoryStream;

                string basePath = _hostingEnvironment.WebRootPath;
                // Here, we have loaded the sales-order-detail.rdl report from the application folder wwwrootResources. sales-order-detail.rdl should be located in the wwwroot\Resources application folder.
                System.IO.FileStream inputStream = new System.IO.FileStream("C:\\PersonalApplications\\MunicipalLKByGLPSolutions\\MuniLK.WebUI\\wwwroot\\resources\\"  + reportOption.ReportModel.ReportPath + ".rdl", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                MemoryStream reportStream = new MemoryStream();
                inputStream.CopyTo(reportStream);
                reportStream.Position = 0;
                inputStream.Close();
                reportOption.ReportModel.Stream = reportStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load report in OnInitReportOptions.");
                throw;
            }
        }

        // Method will be called when report is loaded internally to start the layout process with ReportHelper.
        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

        // Post action to process the report from the server based on json parameters and send the result back to the client.
        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
        {
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadReportRequest request)
        {
            var id = await _mediator.Send(new UploadReportCommand(request));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var report = await _mediator.Send(new GetReportQuery(id));
            return report != null ? Ok(report) : NotFound();
        }
        [NonAction]
        public void LogError(string message, Exception exception, MethodBase methodType, ErrorType errorType)
        {
            WriteLogs(string.Format("Error Message: {0} \n Stack Trace: {1}", message, exception.StackTrace));
        }
        [NonAction]
        public void LogError(string errorCode, string message, Exception exception, string errorDetail, string methodName, string className)
        {
            WriteLogs(string.Format("Class Name: {0} \n Method Name: {1} \n Error Message: {2} \n Stack Trace: {3}", className, methodName, errorDetail, exception.StackTrace));
        }
        [NonAction]
        internal void WriteLogs(string errorMessage)
        {
            _logger.LogError("Error In BOLD Reports" + errorMessage , errorMessage);

        }
    }

}
