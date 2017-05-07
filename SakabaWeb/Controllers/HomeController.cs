using ImageResizer;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SakabaWeb.Controllers
{
    public class HomeController : Controller
    {
        private ILog Logger => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View(new BossFormModel());
        }
        [HttpPost]
        public ActionResult Index(BossFormModel formModel, HttpPostedFileBase file)
        {
            //return View(formModel);
            string physicalPath = Server.MapPath($"~/img/default.png");
            if (file != null)
            {
                var preUploadChecker = new PreUploadChecker(file);
                if (preUploadChecker.HasError)
                {
                    ModelState.AddModelError("file", preUploadChecker.ErrorMessage);
                    return View(formModel);
                }

                string guid = Guid.NewGuid().ToString("N");
                string extension = Path.GetExtension(file.FileName).ToLower();
                string imgPath = $"img/{guid}{extension}";

                physicalPath = Server.MapPath($"~/{imgPath}");
                ImageSave(file, physicalPath, 120);
                Logger.Info(physicalPath);
            }

            string args = ToArgString(new string[] {
                formModel.Name,
                physicalPath,
                formModel.LifePoint.ToString(),
                formModel.EvadeRate.ToString(),
                formModel.VoiceAppear,
                formModel.VoiceDamage,
                formModel.VoiceCounter,
                formModel.VoiceDead,
                formModel.DropItem,
                formModel.Weakness
            });
            Logger.Info(args);

            try
            {
                string exe = Server.MapPath($"~/exe/SakabaConsole.exe");
                System.Diagnostics.Process.Start(exe, args);

                TempData["Success"] = "登録が完了しました。数分待ってもタイムライン上に表示されない場合は管理者までお問い合わせください。";
                Logger.Info("登録完了");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "エラーが発生しました。管理者までお問い合わせください。";
                Logger.Fatal(ex);
            }

            return RedirectToAction(nameof(Index));
        }





        internal class PreUploadChecker
        {
            public string ErrorMessage { get; }
            public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

            public PreUploadChecker(HttpPostedFileBase uploadFile)
            {
                int size = 100; // MB
                string[] allowExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (uploadFile == null || uploadFile.ContentLength == 0)
                {
                    ErrorMessage = "画像ファイルとして認識できません";
                    return;
                }
                string extension = Path.GetExtension(uploadFile.FileName).ToLower();
                if (!uploadFile.ContentType.StartsWith("image") || !allowExtentions.Contains(extension))
                {
                    ErrorMessage = "拡張子が jpg, png, gif の画像ファイルのみ登録できます";
                    return;
                }
                if (uploadFile.ContentLength > size * 1024 * 1024)
                {
                    ErrorMessage = $"サイズ {size} MB までの画像ファイルのみ登録できます";
                    return;
                }
            }
        }

        protected void ImageSave(HttpPostedFileBase uploadFile, string physicalPath, int size, bool padding = false)
        {
            string query = padding ? $"w={size};h={size};autorotate=true;" : $"maxwidth={size};maxheight={size};autorotate=true;";
            var imageJob = new ImageJob(uploadFile, physicalPath, new Instructions(query))
            {
                CreateParentDirectory = true
            };
            imageJob.Build();
        }

        private string ToArgString(string[] args)
        {
            var sb = new StringBuilder();
            foreach (var item in args)
            {
                sb.Append($"\"{item?.Replace("\"", "\\\"").Trim()}\" ");
            }
            return sb.ToString();
        }
    }
}