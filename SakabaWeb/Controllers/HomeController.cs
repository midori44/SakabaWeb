﻿using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SakabaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomeFormModel());
        }
        [HttpPost]
        public ActionResult Index(HomeFormModel formModel, HttpPostedFileBase file)
        {
            //return View(formModel);
            if (file != null)
            {
                var preUploadChecker = new PreUploadChecker(file);
                if (preUploadChecker.HasError)
                {
                    ModelState.AddModelError("file", preUploadChecker.ErrorMessage);
                    return View(formModel);
                }

                string guid = Guid.NewGuid().ToString("N");
                string extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                string imgPath = $"img/{guid}{extension}";

                string physicalPath = Server.MapPath($"~/{imgPath}");
                ImageSave(file, physicalPath, 120);
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
    }
}