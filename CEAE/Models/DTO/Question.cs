using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CEAE.Utils;

namespace CEAE.Models.DTO
{
    public class Question
    {
        private const string Placeholder = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        private const string ContentPath = "~/Content/Images/";

        [RequiredT]
        [HiddenInput]
        public int QuestionID { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [DisplayNameT]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        [DisplayNameT]
        [DataType(DataType.Text)]
        public string Text { get; set; }


        [DisplayNameT]
        public int QuestionOrder { get; set; }

        public string ImageUrl => Text != null ? ContentPathUrl : Placeholder;

        private string ContentPathUrl => ContentPath.Replace("~/", "/") + Text;

        public void ImageSave(HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            if (file == null) return;

            var filename = ImagePath(server, file.FileName);
            Image = file;
            if (Text != null)
                ImageDelete(ImagePath(server));
            Text = file.FileName;
            file.SaveAs(filename);
        }

        public string ImagePath(HttpServerUtilityBase server, string filePath = null)
        {
            filePath = filePath ?? (Text ?? "");
            return server.MapPath(ContentPath + filePath);
        }

        public void ImageDelete(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch (Exception)
            {
                // ignored, usually filenotfound / directory not found exceptions
            }
            finally
            {
                Text = null;
            }
        }
    }
}