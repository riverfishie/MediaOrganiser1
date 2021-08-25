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

        // GET: FileClasses
        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name" : "";
            ViewBag.SortingDate = String.IsNullOrEmpty(Sorting_Order) ? "Uploaded" : "";
            ViewBag.SortingGenre = String.IsNullOrEmpty(Sorting_Order) ? "Genre" : "";

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

            switch (Sorting_Order)
            {
                case "Name":
                    fileClasses = fileClasses.OrderBy(c => c.Name);
                    break;
                case "Uploaded":
                    fileClasses = fileClasses.OrderBy(c => c.FileClassId);
                    break;
                case "Genre":
                    fileClasses = fileClasses.OrderBy(c => c.Genre);
                    break;
                case "FileName":
                    fileClasses = fileClasses.OrderBy(c => c.File);
                    break;
                default:
                    fileClasses = fileClasses.OrderBy(c => c.Name);
                    break;
            }

            if (Search_Data != null)
            {
                fileClasses = fileClasses.Where(stu => stu.Name.ToUpper().Contains(Search_Data.ToUpper())
                    || stu.Genre.ToUpper().Contains(Search_Data.ToUpper()));
            }

            int Size_Of_Page = 7;
            int No_oF_Page = (Page_No ?? 1);
            return View(fileClasses.ToPagedList(No_oF_Page, Size_Of_Page));
        }
        
        // GET: FileClasses/Details/5
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

        // GET: FileClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FileClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileClassId,Name,File,ContentType,Category,Genre")] FileClass fileClass, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
   
            {
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

        // GET: FileClasses/Edit/5
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

        // POST: FileClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileClassId,Name,File,ContentType,Category")] FileClass fileClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fileClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fileClass);
        }

        // GET: FileClasses/Delete/5
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

        // POST: FileClasses/Delete/5
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
