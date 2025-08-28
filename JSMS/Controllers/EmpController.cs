using JSMS.EfContext;
using JSMS.Models.DataBaseModels;
using JSMS.Models.VMModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JSMS.Controllers
{
    public class EmpController : Controller
    {
        private readonly JSMSEFContext _context;
        private readonly IWebHostEnvironment _env;

        public EmpController(JSMSEFContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Employee em)
        {
            if (ModelState.IsValid)
            {
                var data = _context.Employees.FirstOrDefault(x => x.EmpName == em.EmpName && x.EmpEmail == em.EmpEmail);
                if (data != null)
                {
                    HttpContext.Session.SetInt32("EmpId", data.EmpId);
                    return RedirectToAction("Profile", "Emp");
                }
            }
            ViewBag.ErrMsg = "Login Detail Not Proper";

            return View(em);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Employee model)
        {
            if (ModelState.IsValid)
            {
                if(model.EmpResume != null && model.EmpResume.Length > 0)
                {
                    //check extesnion for upload
                    var FileExtension = Path.GetExtension(model.EmpResume.FileName).ToLower();
                    if(FileExtension != ".pdf")
                    {
                        ModelState.AddModelError("", "Pdf Only Allow");
                        return View(model);
                    }
                    // check the upload folder is availbale or not ? if not available then make
                    var UploadFolder = Path.Combine(_env.WebRootPath, "Resumes");
                    if (!Directory.Exists(UploadFolder))
                    {
                        Directory.CreateDirectory(UploadFolder);
                    }

                    var FileSavePath = Path.Combine(UploadFolder, model.EmpName.Trim().ToLower().Replace(" ", "_") + ".pdf");

                    using (var str = new FileStream(FileSavePath, FileMode.Create))
                    {
                        await model.EmpResume.CopyToAsync(str);
                    }

                    model.EmpResumeUrl = "/Resumes/" + model.EmpName!.Trim().ToLower().Replace(" ", "_") + ".pdf";

                    _context.Employees.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Login");

                }
            }
             return View(model);
        }


        public IActionResult Profile()
        {
            int? empId = HttpContext.Session.GetInt32("EmpId");
            if (empId == null)
            {
                return RedirectToAction("Login", "Emp"); // Not logged in
            }

            var emp = _context.Employees.FirstOrDefault(e => e.EmpId == empId);

            if (emp == null)
            {
                return NotFound("Employee not found");
            }

            return View(emp); // single Employee model

        }

        public IActionResult FileDownload(int id)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.EmpId == id);
            if(emp == null || string.IsNullOrEmpty(emp.EmpResumeUrl))
            {
                return NotFound("Resume not found !");
            }

            var filepath = Path.Combine(_env.WebRootPath, emp.EmpResumeUrl.TrimStart('/'));
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound("File Not Found In The Server !");
            }
            var filebyrtes = System.IO.File.ReadAllBytes(filepath);
            return File(filebyrtes,"application/pdf",emp.EmpName + "_Resulme.pdf");
        }


        public IActionResult JobList()
        {
            int? empId = HttpContext.Session.GetInt32("EmpId");
            if (empId == null)
            {
                return RedirectToAction("Login", "Emp"); // Not logged in
            }
            var data = (from p in _context.Posts
                        join f in _context.Founders
                        on p.FdId equals f.FdId
                        select new PostList
                        {
                            Post_Name = p.Post_Name,
                            Post_Id = p.Post_Id,
                            Post_Count = p.Post_Count,
                            Company_Name = f.Fd_Company_Name,
                            Post_Description = p.Post_Description,
                            FdId = f.FdId
                        }).ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Joblist(int Post_id,int FdId) 
        {
            int? empId = HttpContext.Session.GetInt32("EmpId");
            var apply = new Apply
            {
                EmpId = Convert.ToInt32(empId),
                Post_Id = Post_id,
                Status = false,
                FdId = FdId,
                remark = "Action Pending !"
            };

            _context.Applies.Add(apply);
            _context.SaveChanges();
            return RedirectToAction("JobList");
        }

        public IActionResult MyApplies()
        {
            int? empId = HttpContext.Session.GetInt32("EmpId");
            if (empId == null)
            {
                return RedirectToAction("Login", "Emp"); // Not logged in
            }

            var data = (
                from a in _context.Applies
                join p in _context.Posts
                    on a.Post_Id equals p.Post_Id
                join f in _context.Founders
                    on p.FdId equals f.FdId
                where a.EmpId == empId
                select new VMEmpApplication
                {
                    PostName = p.Post_Name,
                    CompanyName = f.Fd_Company_Name,
                    Remark = a.remark,
                    Status = a.Status,
                }).ToList();
            return View(data);
        }

    }
}
