﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaiteRaju.ServiceMapper;
using RaiteRaju.Web.Models;
using System.IO;
using System.Text.RegularExpressions;
using RaiteRaju.ApplicationUtility;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RaiteRaju.Web.Controllers
{
    public class AddPostController : ErrorController
    {
        //
        // GET: /AddPost/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPost()
        {
            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            HttpCookie KeyCookie = Request.Cookies["_RRPS"];

            if (UserIdCookie != null && KeyCookie!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult AdDetails(int AdId)
        {
            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            HttpCookie KeyCookie = Request.Cookies["_RRPS"];

            Utility en = new Utility();
            if (UserIdCookie != null)
            {
                    AdDetailsModel model = new AdDetailsModel();
                    InformationServiceWrapper objservice = new InformationServiceWrapper();
                    Int32 Userid = Convert.ToInt32(en.Decrypt(UserIdCookie["_RRUID"]));

                    List<Int32> UserAdList = new List<int>();
                    UserAdList = objservice.getAdIdsWithUserid(Userid);

                    if (UserAdList.Contains(AdId))
                    {
                        model = objservice.FetchAdDetails(AdId);
                        ViewBag.AdId = model.AdID;
                        //ViewBag.Title = model.txtAddTitle;
                        ViewBag.Description = model.txtAdDescription;
                        ViewBag.SubCategoryName = model.txtSubCategoryName;
                        ViewBag.Category = model.Category;
                        ViewBag.Price = model.txtPrice;
                        ViewBag.Quantity = model.txtQuantity;
                        ViewBag.Unit = model.SellingUnit;
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("UserAccount", "User");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
        }

        [HttpPost]
        public ActionResult AdDetails(FormCollection fnAd)
        {


            int success = 0;
            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            HttpCookie KeyCookie = Request.Cookies["_RRPS"];

            if (UserIdCookie != null)
            {
                    ManagementServiceWrapper ObjService = new ManagementServiceWrapper();
                    AdDetailsModel obj = new AdDetailsModel();
                    obj.AdID = Convert.ToInt32(fnAd["AdId"]);
                    obj.Category = fnAd["ddlCategoryText"];
                    obj.txtSubCategoryName = fnAd["txtSubCategoryName"];
                    obj.txtAdDescription = fnAd["txtAdDescription"];
                    obj.txtPrice = Convert.ToInt32(fnAd["txtPrice"]);
                    obj.txtQuantity = Convert.ToInt32(fnAd["txtQuantity"]);
                    obj.SellingUnit = fnAd["ddlUnitText"];
                    HttpPostedFileBase file = Request.Files["myImage"];

                    if (obj.Category == "Fruit" || obj.Category == "Handloom" || obj.Category == "Equipment" || obj.Category == "Vegetable" || obj.Category == "Others")
                    {
                        obj.txtSubCategoryName = obj.txtSubCategoryName;
                    }
                    else
                    {
                        obj.txtSubCategoryName = obj.Category;
                    }

                    string s = "[^<>'\"/`%-]";
                    int count = 0;

                    if (System.Text.RegularExpressions.Regex.IsMatch(obj.Category, "^[a-zA-Z0-9 .]"))
                    {
                        count = count + 1;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(obj.txtSubCategoryName, s))
                    {
                        count = count + 1;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(obj.txtAdDescription, s))
                    {
                        count = count + 1;
                    }

                    if (System.Text.RegularExpressions.Regex.IsMatch(obj.SellingUnit, "^[a-zA-Z0-9 .]"))
                    {
                        count = count + 1;
                    }
                    if (Regex.Match(obj.txtPrice.ToString(), "[1-9]").Success)
                    {
                        count = count + 1;
                    }
                    if (Regex.Match(obj.txtQuantity.ToString(), "[1-9]").Success)
                    {
                        count = count + 1;
                    }

                    if (count == 6)
                    {
                        ObjService.UpdateAdDetails(obj);
                        success = obj.AdID;
                        return Json(success, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        success = -99;
                        return Json(success, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
        }

        [HttpPost]
        public ActionResult AdDelete(int adId)
        {
            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            if (UserIdCookie != null)
            {

                ManagementServiceWrapper obj = new ManagementServiceWrapper();
                obj.DeleteUserAd(adId);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetDropDownValues()
        {
            InformationServiceWrapper objservice = new InformationServiceWrapper();
            DropDownWrapperModel ModelObj = new DropDownWrapperModel();
            ModelObj = objservice.GetDropDownValues();

            ViewBag.DistrictLIst = ModelObj.District;
            ViewBag.MandalList = ModelObj.Mandal;
            return Json(1,JsonRequestBehavior.AllowGet);
        }
       // [HttpPost]
        //public ActionResult Index(FormCollection collection)
        //{
        //    string userid = Session["_RRUID"].ToString();
        //    HttpPostedFileBase file = Request.Files["ImageData"];
        //    AdDetailsModel objUserPhotoGalleryMaster = new AdDetailsModel();
        //    objUserPhotoGalleryMaster.UserID = Convert.ToInt32(userid);

        //    objUserPhotoGalleryService.UploadAlbums(objUserPhotoGalleryMaster);
        //    return View(objUserPhotoGalleryService.GetAlbums(Convert.ToInt32(userid)).ToList());
        //}

        //methods to convert byte array into image format  


        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase image, Int32 AdId)
        {
            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            Utility en = new Utility();

            if (UserIdCookie != null)
            {
                string userid = en.Decrypt(UserIdCookie["_RRUID"].ToString());
                HttpPostedFileBase file = Request.Files["ImageData"];

                if (file != null)
                {

                    var LENGTH = (file.ContentLength/1024);
                    var ext = Path.GetExtension(file.FileName);
                    Stream strm = file.InputStream;

                    var ActualImagePath = Path.Combine(Server.MapPath("~/PhotoManagement/Actual/"), Convert.ToString(AdId) + ".jpg");
                    var CompressedImagePath  = Path.Combine(Server.MapPath("~/PhotoManagement/Compressed/"), Convert.ToString(AdId) + ".jpg");

                    var CompressedDelPath = Path.Combine(Server.MapPath("~/PhotoManagement/Deleted/"), Convert.ToString(AdId) + "_Compressed_Deleted.jpg");
                    var ActualDelPath = Path.Combine(Server.MapPath("~/PhotoManagement/Deleted/"), Convert.ToString(AdId) + "_Actual_Deleted.jpg");

                    if (System.IO.File.Exists(CompressedImagePath))
                    {
                        System.IO.File.Delete(CompressedDelPath);
                        System.IO.File.Move(CompressedImagePath, CompressedDelPath);

                        if (System.IO.File.Exists(ActualImagePath))
                        {
                            System.IO.File.Delete(ActualDelPath);
                            System.IO.File.Move(ActualImagePath, ActualDelPath);
                        }

                        ReduceImageSize(strm, CompressedImagePath);
                        if (LENGTH >= 350) { ReduceImageSizeForActualImage(strm, ActualImagePath); }
                        else { file.SaveAs(ActualImagePath); }
                    }
                    else
                    {
                        ReduceImageSize(strm, CompressedImagePath);
                        if (LENGTH >= 350) { ReduceImageSizeForActualImage(strm, ActualImagePath); }
                        else { file.SaveAs(ActualImagePath); }
                    }


                    AdDetailsModel obj = new AdDetailsModel();
                    obj.UserID = Convert.ToInt32(userid);
                    obj.AdID = AdId;
                    obj.Image = ConvertToBytes(file);
                    ManagementServiceWrapper OBJ = new ManagementServiceWrapper();
                    OBJ.UploadImage(obj);
                    return Json(AdId, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(AdId, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(AdId, JsonRequestBehavior.AllowGet);
            }
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }


        private void ReduceImageSize(Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
               // image.RotateFlip(RotateFlipType.Rotate90FlipNone); 
                //var scaleFactor = 0.5;
                var newWidth = 155;// (int)(image.Width * scaleFactor);
                var newHeight = 110;// (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
        private void ReduceImageSizeForActualImage(Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var scaleFactor = 0.5;
                var newHeight = (int)(image.Height * scaleFactor);
                var newWidth =  (int)(image.Width * scaleFactor);
                
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }  
        public ActionResult RetrieveImage(int AdId)
        {
            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            Utility en = new Utility();
            if (UserIdCookie != null)
            {
                AdDetailsModel ModelINPUT = new AdDetailsModel();
                AdDetailsModel ModelOut = new AdDetailsModel();
                ModelINPUT.AdID = AdId;
                ModelINPUT.UserID = Convert.ToInt32(en.Decrypt(UserIdCookie["_RRUID"]));
                InformationServiceWrapper InfoObj = new InformationServiceWrapper();
                ModelOut = InfoObj.GetImage(ModelINPUT);

                if (ModelOut != null)
                {
                    return File(ModelOut.Image, "image/jpg");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        [HttpPost]
        public ActionResult AddPost(FormCollection fnPost)
        {
            int AdId = 0;
            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            HttpCookie PhoneNumberCookie=Request.Cookies["_RRUPn"];
            Utility en = new Utility();

            if (UserIdCookie != null)
            {
                ManagementServiceWrapper ObjService = new ManagementServiceWrapper();
                AdDetailsModel obj = new AdDetailsModel();
                obj.Category = fnPost["ddlCategoryText"];
                obj.txtSubCategoryName = fnPost["txtSubCategoryName"];
                obj.txtAdDescription = fnPost["txtAdDescription"];
                obj.txtPrice = Convert.ToInt32(fnPost["txtPrice"]);
                obj.txtQuantity = Convert.ToInt32(fnPost["txtQuantity"]);
                obj.SellingUnit = fnPost["ddlUnitText"];
                obj.UserID = Convert.ToInt32(en.Decrypt(UserIdCookie["_RRUID"]));
                obj.MobileNuber = Convert.ToInt64(en.Decrypt(PhoneNumberCookie["_RRUPn"]));
                if (obj.Category == "Fruit" || obj.Category == "Handloom" || obj.Category == "Equipment" || obj.Category == "Vegetable" || obj.Category == "Others")
                {
                    obj.txtSubCategoryName = obj.txtSubCategoryName;
                }
                else {
                    obj.txtSubCategoryName = obj.Category;
                }

                string s = "[^<>'\"/`%-]";
                int count = 0;

                if (System.Text.RegularExpressions.Regex.IsMatch(obj.Category, "^[a-zA-Z0-9 .]"))
                {
                    count = count + 1;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(obj.txtSubCategoryName, s))
                {
                    count = count + 1;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(obj.txtAdDescription, s))
                {
                    count = count + 1;
                }
                
                if (System.Text.RegularExpressions.Regex.IsMatch(obj.SellingUnit, "^[a-zA-Z0-9 .]"))
                {
                    count = count + 1;
                }
                if (Regex.Match(obj.txtPrice.ToString(), "[1-9]").Success)
                {
                    count = count + 1;
                }
                if (Regex.Match(obj.txtQuantity.ToString(), "[1-9]").Success)
                {
                    count = count + 1;
                }

                if (count == 6)
                {
                    AdId = ObjService.InsertAddPostDetails(obj);
                    return Json(AdId, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AdId = -1;
                    return Json(AdId, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                AdId = -1;
                return Json(AdId, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult _AdSearchCategory(int Category, int PageNumber)
        {
            int outTotalPageNumber = 0;
            ViewBag.CurrentPageNumber = PageNumber;
            InformationServiceWrapper objservice = new InformationServiceWrapper();
            List<AdDetailsModel> LisModelObj = new List<AdDetailsModel>();
            LisModelObj = objservice.SPRRGetADbyCategory(Category, PageNumber, out outTotalPageNumber);
            ViewBag.TotalPageNumber = outTotalPageNumber;
            ViewBag.Adlist = LisModelObj;

            ViewBag.SearchOption = "CATEGORY";
            ViewBag.SearchData = Category;
            return PartialView("_AdSearch");
        }

        public ActionResult _AdSearch(string State, string District, string DistrictId, string Mandal, string MandalId, string CategoryId, Int32 PageNumber)
        {
            int TotalPageNumber = 0;
            ViewBag.CurrentPageNumber = PageNumber;

            InformationServiceWrapper objservice = new InformationServiceWrapper();

            AdFilterModel Model = new AdFilterModel();
            Model.txtState = State == "" ? null : State; //form["ddlStateText"];
            Model.txtDistrict = District == "" ? null : District; //form["ddlDistrict"];
            Model.txtMandal = Mandal == "" ? null : Mandal; //form["ddlMandal"];
            Model.CategoryId = Convert.ToInt32(CategoryId); //form["ddlCategory"];
            // Model.TxtKeyWord = Keyword == "" ? null : Keyword; //form["txtSearchWord"];
            Model.INTPAGENUMBER = PageNumber;
            Model.INTPAGESIZE = 10;
            List<AdDetailsModel> LisModelObj = new List<AdDetailsModel>();

            LisModelObj = objservice.GetfilteredAds(Model, out TotalPageNumber);
            ViewBag.Adlist = LisModelObj;
            ViewBag.TotalPageNumber = TotalPageNumber;

            @ViewBag.CategoryId = Model.CategoryId;
            @ViewBag.selectedState = Model.txtState;
            @ViewBag.SelectedDistrictId = DistrictId;
            @ViewBag.selectedMandalId = MandalId;

            return PartialView("AdSearch");
        }
        public ActionResult AdSearch(int CategoryId, int PageNumber)
        {
            ViewBag.CategoryId = CategoryId;

            int outTotalPageNumber = 0;
            ViewBag.CurrentPageNumber = PageNumber;
            InformationServiceWrapper objservice = new InformationServiceWrapper();
            List<AdDetailsModel> LisModelObj = new List<AdDetailsModel>();
            LisModelObj = objservice.SPRRGetADbyCategory(CategoryId, PageNumber, out outTotalPageNumber);


            foreach (AdDetailsModel item in LisModelObj)
            {
                if (item.Category == "Fruit" || item.Category == "Handloom" || item.Category == "Equipment" || item.Category == "Vegetable" || item.Category == "Others")
                {
                    item.Category = item.txtSubCategoryName;
                }
               
            }

            ViewBag.TotalPageNumber = outTotalPageNumber;
            ViewBag.Adlist = LisModelObj;
            ViewBag.SearchData = CategoryId;
            return View("AdSearch");
        }

        public ActionResult AdDisplay(int AdId)
        {
            AdDetailsModel model = new AdDetailsModel();
            InformationServiceWrapper objservice = new InformationServiceWrapper();
            model = objservice.SPRRGetAdDisplayDetails(AdId);
            ViewBag.IntAdId = model.AdID;
            ViewBag.AdId = model.AdID+".jpg";
            ViewBag.Title = model.txtAddTitle;
            ViewBag.Description = model.txtAdDescription;
            ViewBag.Category = model.Category;
            ViewBag.SubCategoryName = model.txtSubCategoryName;
            ViewBag.Price = model.txtPrice;
            ViewBag.Quantity = model.txtQuantity;
            ViewBag.Unit = model.SellingUnit;
            ViewBag.Name = model.Name;
            ViewBag.MobileNumber = model.MobileNuber.ToString().Substring(0,3)+"XXXXXX";
            ViewBag.Location = model.Location;
            ViewBag.AdPostedDate = model.PostedDate.ToString();

            if (model.Category == "Fruit" || model.Category == "Handloom" || model.Category == "Equipment" || model.Category == "Vegetable" || model.Category == "Others")
            {
                ViewBag.DefinedTitle = model.txtSubCategoryName + " " + model.txtQuantity + " " + model.SellingUnit + " at Price Rs." + model.txtPrice + " Per " + model.SellingUnit.Substring(0, model.SellingUnit.Length - 1);
            }
            else
            {
                ViewBag.DefinedTitle = model.Category + " " + model.txtQuantity + " " + model.SellingUnit + " at Price Rs." + model.txtPrice + " Per " + model.SellingUnit.Substring(0, model.SellingUnit.Length - 1);
            }
            ViewBag.DefinedUnit = " /" + model.SellingUnit.Substring(0, model.SellingUnit.Length - 1);
            //searchingData
            //ViewBag.SearchOption = "CATEGORY";
            //ViewBag.PageNumber = PageNumber;
            //ViewBag.CategoryID = CategoryID;
               

            return View("AdDisplay");

        }
        public ActionResult AdDisplayWithPhoneNumber(int AdId)
        {

            HttpCookie UserIdCookie = Request.Cookies["_RRUID"];
            HttpCookie PhoneNumberCookie=Request.Cookies["_RRUPn"];
            if (UserIdCookie != null && PhoneNumberCookie != null)
            {
                AdDetailsModel model = new AdDetailsModel();
                InformationServiceWrapper objservice = new InformationServiceWrapper();
                model = objservice.SPRRGetAdDisplayDetails(AdId);
                ViewBag.IntAdId = model.AdID;
                ViewBag.AdId = model.AdID + ".jpg";
                ViewBag.Title = model.txtAddTitle;
                ViewBag.Description = model.txtAdDescription;
                ViewBag.Category = model.Category;
                ViewBag.Price = model.txtPrice;
                ViewBag.Quantity = model.txtQuantity;
                ViewBag.Unit = model.SellingUnit;
                ViewBag.Name = model.Name;
                ViewBag.MobileNumber = model.MobileNuber.ToString();
                ViewBag.Location = model.Location;
                ViewBag.AdPostedDate = model.PostedDate;

                if (model.Category == "Fruit" || model.Category == "Handloom" || model.Category == "Equipment" || model.Category == "Vegetable" || model.Category == "Others")
                {
                    ViewBag.DefinedTitle = model.txtSubCategoryName + " " + model.txtQuantity + " " + model.SellingUnit + " at Price Rs." + model.txtPrice + " Per " + model.SellingUnit.Substring(0, model.SellingUnit.Length - 1);
                }
                else
                {
                    ViewBag.DefinedTitle = model.Category + " " + model.txtQuantity + " " + model.SellingUnit + " at Price Rs." + model.txtPrice + " Per " + model.SellingUnit.Substring(0, model.SellingUnit.Length - 1);
                }
                ViewBag.DefinedUnit = " /" + model.SellingUnit.Substring(0, model.SellingUnit.Length - 1);

                ManagementServiceWrapper ManObj = new ManagementServiceWrapper();
                Utility en = new Utility();
                AdDetailsModel StatisticsObj = new AdDetailsModel();
                StatisticsObj.UserID = Convert.ToInt32(en.Decrypt(UserIdCookie["_RRUID"]));
                StatisticsObj.MobileNuber = Convert.ToInt64(en.Decrypt(PhoneNumberCookie["_RRUPn"]));
                StatisticsObj.AdID = model.AdID; ;
                ManObj.SPInsertAdViewsStatistics(StatisticsObj);
                return View("AdDisplay");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }
        public ActionResult GetfilteredAds(string State,string District,string Mandal,string CategoryId,string Keyword)
        {
            int TotalPageNumber=0;

            InformationServiceWrapper objservice = new InformationServiceWrapper();
            DropDownWrapperModel ModelObj = new DropDownWrapperModel();
            ModelObj = objservice.GetDropDownValues();
            ViewBag.DistrictLIst = ModelObj.District;
            ViewBag.MandalList = ModelObj.Mandal;

            AdFilterModel Model = new AdFilterModel();
            Model.txtState = State==""?null:State;// form["ddlStateText"];
            Model.txtDistrict = District == "" ? null : District; ; //form["ddlDistrict"];
            Model.txtMandal =Mandal== "" ? null :Mandal;// form["ddlMandal"];
            Model.CategoryId =Convert.ToInt32(CategoryId); //Convert.ToInt32(form["ddlCategory"]);
            Model.TxtKeyWord = Keyword== "" ? null :Keyword; //form["txtSearchWord"];

            List<AdDetailsModel> LisModelObj = new List<AdDetailsModel>();

            LisModelObj = objservice.GetfilteredAds(Model,out TotalPageNumber);
            ViewBag.Adlist = LisModelObj;
            ViewBag.TotalPageNumber = TotalPageNumber;

            return View("FilteredAds");
        }
        
    }
}
