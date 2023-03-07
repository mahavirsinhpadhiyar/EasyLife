using EasyLife.Controllers;
using EasyLife.Web.Models.Financial.Investment;
using EasyLife.Web.Models.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace EasyLife.Web.Controllers
{
    public class UploadController : EasyLifeControllerBase
    {
        private IHostingEnvironment Environment;

        public UploadController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult Index()
        {
            if (LoggedInUserRole() == Authorization.Roles.RoleEnums.Roles.User)
            {
                return Redirect("/Error/Index");
            }
            List<UploadedDocumentList> dataList = new List<UploadedDocumentList>();

            string path = Path.Combine(this.Environment.ContentRootPath, "Uploads");
            DirectoryInfo d = new DirectoryInfo(path);

            //foreach (var file in d.GetFiles("*.txt"))
            //{
            //    Directory.Move(file.FullName, filepath + "\\TextFiles\\" + file.Name);
            //}

            if (Directory.Exists(path))
            {
                foreach (var file in d.GetFiles())
                {
                    dataList.Add(new UploadedDocumentList() { FileName = file.Name, FilePath = file.FullName });
                }
            }

            return View(dataList);
        }

        [HttpPost]
        public IActionResult Index(List<IFormFile> postedFiles)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Download(string FileName)
        {
            //return File(this.Environment.ContentRootFileProvider.GetFileInfo(FileName).CreateReadStream(), , enableRangeProcessing: true);
            //https://stackoverflow.com/questions/45727856/how-to-download-a-file-in-asp-net-core use for mime types and file download useful.
            string path = Path.Combine(this.Environment.ContentRootPath, "Uploads/" + FileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/force-download",FileName);
        }

        public IActionResult MutualFund()
        {
            string apiUrl = "https://api.mfapi.in/mf/146127";

            MutualFundHistoryDetails mutualFundHistoryDetails = new MutualFundHistoryDetails();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                mutualFundHistoryDetails = JsonConvert.DeserializeObject<MutualFundHistoryDetails>(response.Content.ReadAsStringAsync().Result);
            }

            return Json(response.Content.ReadAsStringAsync().Result);
        }
    }
}
