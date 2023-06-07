using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Upload
{
    public class UploadedDocumentList
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
    public class ExplorerModel
    {
        public List<DirModel> dirModelList;
        public List<FileModel> fileModelList;

        public ExplorerModel(List<DirModel> _dirModelList, List<FileModel> _fileModelList)
        {
            dirModelList = _dirModelList;
            fileModelList = _fileModelList;
        }
    }
    public class DirModel
    {
        public string DirPath { get; set; }
        public string DirName { get; set; }
        public DateTime DirAccessed { get; set; }
    }
    public class FileModel
    {
        public string FileName { get; set; }
        public string FileSizeText { get; set; }
        public DateTime FileAccessed { get; set; }
    }
}
