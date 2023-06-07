using EasyLife.Controllers;
using EasyLife.Web.Models.Financial.Investment;
using EasyLife.Web.Models.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ActionResult FolderList(string path)
        {
            string realPath;
            if (String.IsNullOrEmpty(path))
                realPath = Path.Combine(this.Environment.ContentRootPath, "Uploads");
            else
                realPath = path;
            var directories = Directory.GetDirectories(realPath);
            // or realPath = "FullPath of the folder on server" 

            if (System.IO.File.Exists(realPath))
            {
                //http://stackoverflow.com/questions/1176022/unknown-file-type-mime
                return base.File(realPath, "application/octet-stream");
            }
            else if (System.IO.Directory.Exists(realPath))
            {
                //Uri url = Request.Url;
                ////Every path needs to end with slash
                //if (url.ToString().Last() != '/')
                //{
                //    Response.Redirect("/Uploads/" + path + "/");
                //}

                List<FileModel> fileListModel = new List<FileModel>();

                List<DirModel> dirListModel = new List<DirModel>();

                IEnumerable<string> dirList = Directory.EnumerateDirectories(realPath);
                foreach (string dir in dirList)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);

                    DirModel dirModel = new DirModel();

                    dirModel.DirPath =  dir;
                    dirModel.DirName = Path.GetFileName(dir);
                    dirModel.DirAccessed = d.LastAccessTime;

                    dirListModel.Add(dirModel);
                }

                IEnumerable<string> fileList = Directory.EnumerateFiles(realPath);
                foreach (string file in fileList)
                {
                    FileInfo f = new FileInfo(file);

                    FileModel fileModel = new FileModel();

                    if (f.Extension.ToLower() != "php" && f.Extension.ToLower() != "aspx"
                        && f.Extension.ToLower() != "asp")
                    {
                        fileModel.FileName = Path.GetFileName(file);
                        fileModel.FileAccessed = f.LastAccessTime;
                        fileModel.FileSizeText = (f.Length < 1024) ?
                                 f.Length.ToString() + " B" : f.Length / 1024 + " KB";

                        fileListModel.Add(fileModel);
                    }
                }

                ExplorerModel explorerModel = new ExplorerModel(dirListModel, fileListModel);

                return View(explorerModel);
            }
            else
            {
                return View(new ExplorerModel(new List<DirModel>(), new List<FileModel>()));
                //return Content(path + " is not a valid file or directory.");
            }
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

        public IActionResult Download(string path)
        {
            //return File(this.Environment.ContentRootFileProvider.GetFileInfo(FileName).CreateReadStream(), , enableRangeProcessing: true);
            //https://stackoverflow.com/questions/45727856/how-to-download-a-file-in-asp-net-core use for mime types and file download useful.
            //string path = Path.Combine(this.Environment.ContentRootPath, "Uploads/" + FileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/force-download", Path.GetFileName(path));
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
