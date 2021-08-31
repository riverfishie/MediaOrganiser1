using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MediaOrganiser1.Models;
using PagedList;

namespace MediaOrganiser1.Controllers
{
    public class FileClassesController : Controller
    {
        private FileUploadDb db = new FileUploadDb();

        // FileClasses
        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name" : "";
            ViewBag.SortingContentType = String.IsNullOrEmpty(Sorting_Order) ? "ContentType" : "";
            ViewBag.SortingGenre = String.IsNullOrEmpty(Sorting_Order) ? "Genre" : "";
            //Don't think this line is needed - test
            //ViewBag.SortingCategory = Boolean.(Sorting_Order ? "Category" : "");

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }



            ViewBag.FilterValue = Search_Data;


            var fileClasses = from c in db.FileClasses select c;
            //

            switch (Sorting_Order)
            {
                case "Name":
                    fileClasses = fileClasses.OrderBy(c => c.Name);
                    break;
                case "ContentType":
                    fileClasses = fileClasses.OrderBy(c => c.ContentType);
                    break;
                // Don't want FileClassId to be displayed - removing ordering by order uploaded
                //case "Uploaded":
                //    fileClasses = fileClasses.OrderBy(c => c.FileClassId);
                //    break;
                case "Genre":
                    fileClasses = fileClasses.OrderBy(c => c.Genre);
                    break;
                //case "Category":
                //    fileClasses = fileClasses.OrderBy(c => c.Category);
                //    break;
                default:
                    fileClasses = fileClasses.OrderBy(c => c.Name);
                    break;
            }

            if (!string.IsNullOrEmpty(Search_Data))
            {
                fileClasses = fileClasses.Where(classes => classes.Name.ToUpper().Contains(Search_Data.ToUpper())
                    || classes.Genre.ToUpper().Contains(Search_Data.ToUpper()));
            }


            int Size_Of_Page = 17;
            int No_oF_Page = (Page_No ?? 1);
            return View(fileClasses.ToPagedList(No_oF_Page, Size_Of_Page));
        }
        
        // Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileClass fileClass = db.FileClasses.Find(id);
            if (fileClass == null)
            {
                return HttpNotFound();
            }
            return View(fileClass);
        }

        // Create
        public ActionResult Create()
        {
            return View();
        }

        // Data from upload / create sent to database with post method here.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileClassId,Name,File,ContentType,Category,Genre")] FileClass fileClass, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
   
            { 
                if (fileClass.Category == "Photo" )
                {
                    fileClass.Genre = null;
                }
                else if (fileClass.Category == "Video")
                {
                    fileClass.Genre = null;
                }

                if (upload != null)
                {
                    int fileLength = upload.ContentLength;
                    byte[] MyFile = new byte[fileLength];
                    upload.InputStream.Read(MyFile,0, fileLength);
                    fileClass.File = MyFile;

                    fileClass.ContentType = upload.ContentType;
                    fileClass.Name = upload.FileName;

                    db.FileClasses.Add(fileClass);


                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(fileClass);
        }

        // Edit.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileClass fileClass = db.FileClasses.Find(id);
            if (fileClass == null)
            {
                return HttpNotFound();
            }
            return View(fileClass);
        }

        // Edits made to item sends changed data to database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileClassId,Name,File,ContentType,Category")] FileClass fileClass)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(fileClass).State = EntityState.Modified;
                db.FileClasses.Attach(fileClass);
                db.Entry(fileClass).Property("Name").IsModified = true;
                db.Entry(fileClass).Property("Category").IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fileClass);
        }

        // Delete.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileClass fileClass = db.FileClasses.Find(id);
            if (fileClass == null)
            {
                return HttpNotFound();
            }
            return View(fileClass);
        }

        // Removes deleted item from database.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FileClass fileClass = db.FileClasses.Find(id);
            db.FileClasses.Remove(fileClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
