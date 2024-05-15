using Core_Prac_01.Models;
using Core_Prac_01.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Core_Prac_01.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly ApplicantDbContext db;
        private readonly IWebHostEnvironment env;
        public ApplicantsController(ApplicantDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        public async Task<IActionResult> Index(int pg = 1)
        {
            var data = await db.Applicants.Include(a => a.Qualifications).ToPagedListAsync(pg, 5);
            return View(data);
        }
        public IActionResult Create()
        {
            var model = new ApplicantInputModel();
            model.Qualifications.Add(new QualificationInputModel());
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicantInputModel model, string act = "")
        {
            if (act == "add")
            {
                model.Qualifications.Add(new QualificationInputModel());
            }
            if (act.StartsWith("remove"))
            {

                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Qualifications.RemoveAt(index);
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    Applicant a = new Applicant
                    {
                        ApplicantName = model.ApplicantName,
                        Gender = model.Gender,
                        AppliedFor = model.AppliedFor,
                        BirthDate = model.BirthDate,
                        IsReadyToWorkAnyWhere = model.IsReadyToWorkAnyWhere
                    };
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fn = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string sp = Path.Combine(env.WebRootPath, "Pictures", fn);
                    FileStream fs = new FileStream(sp, FileMode.Create);
                    await model.Picture.CopyToAsync(fs);
                    fs.Close();
                    a.Picture = fn;
                    foreach (var x in model.Qualifications)
                    {
                        a.Qualifications.Add(new Qualification { Degree = x.Degree, Institute = x.Institute, PassingYear = x.PassingYear, Result = x.Result });
                    }
                    await db.Applicants.AddAsync(a);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var a = await db.Applicants.Include(x => x.Qualifications).FirstOrDefaultAsync(x => x.ApplicantId == id);
            if (a == null) { return NotFound(); }
            var model = new ApplicantEditModel
            {
                ApplicantId = a.ApplicantId,
                ApplicantName = a.ApplicantName,
                AppliedFor = a.AppliedFor,
                BirthDate = a.BirthDate,
                Gender = a.Gender,
                IsReadyToWorkAnyWhere = a.IsReadyToWorkAnyWhere,

            };
            foreach (var x in a.Qualifications)
            {
                model.Qualifications.Add(new QualificationInputModel
                {
                    QualificationId = x.QualificationId,
                    Degree = x.Degree,
                    Institute = x.Institute,
                    PassingYear = x.PassingYear,
                    Result = x.Result
                });
            }
            ViewBag.CurrentPic = a.Picture;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicantEditModel model, string act = "")
        {
            if (act == "add")
            {
                model.Qualifications.Add(new QualificationInputModel());
            }
            if (act.StartsWith("remove"))
            {

                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Qualifications.RemoveAt(index);
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var a = await db.Applicants.Include(x => x.Qualifications).FirstOrDefaultAsync(x => x.ApplicantId == model.ApplicantId);
                    if (a == null) { return NotFound(); }
                    a.ApplicantName = model.ApplicantName;
                    a.AppliedFor = model.AppliedFor;
                    a.BirthDate = model.BirthDate;
                    a.Gender = model.Gender;
                    a.IsReadyToWorkAnyWhere = model.IsReadyToWorkAnyWhere;
                    if (model.Picture != null)
                    {
                        string ext = Path.GetExtension(model.Picture.FileName);
                        string fn = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string sp = Path.Combine(env.WebRootPath, "Pictures", fn);
                        FileStream fs = new FileStream(sp, FileMode.Create);
                        await model.Picture.CopyToAsync(fs);
                        fs.Close();
                        a.Picture = fn;
                    }
                    db.Qualifications.RemoveRange(a.Qualifications.ToList());
                    foreach (var x in model.Qualifications)
                    {
                        a.Qualifications.Add(new Qualification { Degree = x.Degree, Institute = x.Institute, PassingYear = x.PassingYear, Result = x.Result });
                    }

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var a = await db.Applicants.Include(x => x.Qualifications).FirstOrDefaultAsync(x => x.ApplicantId == id);
            return View(a);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DoDelete(int id)
        {
            var a = await db.Applicants.Include(x => x.Qualifications).FirstOrDefaultAsync(x => x.ApplicantId == id);
            if (a == null) { return NotFound(); }
            db.Applicants.Remove(a);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //
        // Qualifactions
        ////////////////////////////////////////////
        //
        public IActionResult CreateQualification(int applicantId)
        {
            ViewBag.Applicants = db.Applicants.Select(x => new { x.ApplicantId, x.ApplicantName }).ToList();
            ViewBag.ApplicantId = applicantId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateQualification(QualificationInputModel model)
        {
            if (ModelState.IsValid)
            {
                await db.Qualifications.AddAsync(new Qualification { Degree = model.Degree, Institute = model.Institute, PassingYear = model.PassingYear, Result = model.Result, ApplicantId = model.ApplicantId });
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Applicants = db.Applicants.Select(x => new { x.ApplicantId, x.ApplicantName }).ToList();
            ViewBag.ApplicantId = model.ApplicantId;
            return View(model);
        }
        public async Task<IActionResult> EditQualification(int id)
        {
            var q = await db.Qualifications.FirstOrDefaultAsync(x => x.QualificationId == id);
            if (q == null) return NotFound();
            ViewBag.Applicants = db.Applicants.Select(x => new { x.ApplicantId, x.ApplicantName }).ToList();

            return View(new QualificationInputModel
            {
                QualificationId = q.QualificationId,
                ApplicantId = q.ApplicantId,
                Degree = q.Degree,
                Institute = q.Institute,
                PassingYear = q.PassingYear,
                Result = q.Result,
            });
        }
        [HttpPost]
        public async Task<IActionResult> EditQualification(QualificationInputModel model)
        {
            var q = await db.Qualifications.FirstOrDefaultAsync(x => x.QualificationId == model.QualificationId);
            if (q == null) return NotFound();
            if (ModelState.IsValid)
            {
                q.ApplicantId = model.ApplicantId;
                q.Degree = model.Degree;
                q.PassingYear = model.PassingYear;
                q.Institute = model.Institute;
                q.Result = model.Result;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Applicants = db.Applicants.Select(x => new { x.ApplicantId, x.ApplicantName }).ToList();

            return View(model);
        }
        public async Task<IActionResult> DeleteQualification(int id)
        {
            var a = await db.Qualifications.Include(x => x.Applicant).FirstOrDefaultAsync(x => x.QualificationId == id);


            return View(a);
        }
        [HttpPost, ActionName("DeleteQualification")]
        public async Task<IActionResult> DoDeleteQualification(int id)
        {
            var a = await db.Qualifications.Include(x => x.Applicant).FirstOrDefaultAsync(x => x.QualificationId == id);
            if (a == null) { return NotFound(); }
            db.Qualifications.Remove(a);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
