using JSMS.EfContext;
using JSMS.Models.DataBaseModels;
using JSMS.Models.VMModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using OfficeOpenXml;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace JSMS.Controllers
{
    public class FounderController : Controller
    {
        private readonly JSMSEFContext _context;
        private readonly IServiceProvider _serviceProvider;

        public FounderController(JSMSEFContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Founder fdr)
        {
            var data = _context.Founders.FirstOrDefault(x => x.Fd_Company_Name == fdr.Fd_Company_Name && x.Fd_Email == fdr.Fd_Email);
            if(data != null)
            {
                HttpContext.Session.SetInt32("FounderId", data.FdId);
                return RedirectToAction("Profile", "Founder");
            }
            ViewBag.MsgErr = "Login Data Not Match !";
            return View(fdr);
        }

        public IActionResult Registration() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Founder fdr)
        {
            if (ModelState.IsValid)
            {
                await _context.Founders.AddAsync(fdr);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(fdr);
        }


        public IActionResult Profile()
        {
            var FounderId = HttpContext.Session.GetInt32("FounderId");

            var data = _context.Founders.FirstOrDefault(x => x.FdId == FounderId);
            if (data == null) {
                return RedirectToAction("Login", "Founder");
            }

            return View(data);
        }


        public IActionResult Post()
        {
            if (HttpContext.Session.GetInt32("FounderId") == null)
            {
                return RedirectToAction("Login", "Founder");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Post(Post pt)
        {
            if (ModelState.IsValid)
            {
                var fdid =  HttpContext.Session.GetInt32("FounderId");
                pt.FdId = Convert.ToInt32(fdid);
                _context.Posts.Add(pt);
                _context.SaveChanges();
                return RedirectToAction("PostList", "Founder");
            }
            return View();
        }


        public IActionResult PostList()
        {
            if (HttpContext.Session.GetInt32("FounderId") == null)
            {
                return RedirectToAction("Login", "Founder");
            }
            var FounderId = HttpContext.Session.GetInt32("FounderId");

            //var data = _context.Posts.ToList();

            var data = _context.Posts
                        .Include(x => x.Founder)
                        .Where(x=>x.FdId == FounderId)
                        .ToList();
            return View(data);
        }


        public IActionResult PostApplies()
        {
            var fid = HttpContext.Session.GetInt32("FounderId");
            if (fid == null)
            {
                return RedirectToAction("Login", "Founder");
            }
            var data = (
                from e in _context.Employees
                join a in _context.Applies
                    on e.EmpId equals a.EmpId
                join p in _context.Posts
                    on a.Post_Id equals p.Post_Id
                where p.FdId == fid
                select new VMApplies
                {
                    EmpName = e.EmpName,
                    EmpEmail = e.EmpEmail,
                    EmpResumeUrl = e.EmpResumeUrl,
                    Post_name = p.Post_Name,
                    Remark = a.remark,
                    Apply_id = a.ApplyId,
                    Status = false
                }).ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult ExportExel()
        {
            var fid = HttpContext.Session.GetInt32("FounderId");
            var data = (
                from e in _context.Employees
                join a in _context.Applies
                    on e.EmpId equals a.EmpId
                join p in _context.Posts
                    on a.Post_Id equals p.Post_Id
                where p.FdId == fid
                select new VMApplies
                {
                    EmpName = e.EmpName,
                    EmpEmail = e.EmpEmail,
                    EmpResumeUrl = e.EmpResumeUrl,
                    Post_name = p.Post_Name,
                    Apply_id = a.ApplyId,
                    Remark = a.remark
                }).ToList();

            using (var pkg = new ExcelPackage()) {

                var workbook = pkg.Workbook.Worksheets.Add("Candidate_Applies");

                workbook.Cells[1, 1].Value = "No";
                workbook.Cells[1, 2].Value = "Employee_name";
                workbook.Cells[1, 3].Value = "Email";
                workbook.Cells[1, 4].Value = "Post_name";
                workbook.Cells[1, 5].Value = "Remark";

                int row = 2;
                foreach(var dt in data)
                {
                    workbook.Cells[row, 1].Value = dt.Apply_id;
                    workbook.Cells[row, 2].Value = dt.EmpName;
                    workbook.Cells[row, 3].Value = dt.EmpEmail;
                    workbook.Cells[row, 4].Value = dt.Post_name;
                    workbook.Cells[row, 5].Value = dt.Remark;
                    row++;
                }

                var strem = new MemoryStream();
                pkg.SaveAs(strem);
                strem.Position = 0;
                string Exelname = $"Employee_Repost_{DateTime.Now: dd - MM - yyyy}.xlsx";
                return File(strem, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Exelname);
            }
        }



        [HttpGet("Founder/EditApply/{Apply_id}")]
        public IActionResult EditApply(int Apply_id)
        {
            var data = _context.Applies.Find(Apply_id);
            if (data == null) return NotFound(); // ✅ null check પહેલાં જ

            var vm = (
                from e in _context.Employees
                join a in _context.Applies on e.EmpId equals a.EmpId
                join p in _context.Posts on a.Post_Id equals p.Post_Id
                where p.FdId == data.FdId && a.ApplyId == Apply_id
                select new VMApplies
                {
                    EmpName = e.EmpName,
                    EmpEmail = e.EmpEmail,
                    EmpResumeUrl = e.EmpResumeUrl,
                    Post_name = p.Post_Name,
                    Remark = a.remark,
                    Apply_id = a.ApplyId,
                    Status = data.Status
                }
            ).FirstOrDefault(); // <-- LIST નહિ, SINGLE RECORD

            if (vm == null) return NotFound();

            return View(vm);
        }


        [HttpPost]
        public IActionResult EditApply(VMApplies v)
        {
            var apply = _context.Applies.FirstOrDefault(X => X.ApplyId == v.Apply_id);
            if(apply != null)
            {
                apply.remark = v.Remark;
                apply.Status = v.Status;

                _context.SaveChanges();
            }

            return RedirectToAction("PostApplies");
        }


        public IActionResult Appproved()
        {
            var fid = HttpContext.Session.GetInt32("FounderId");

            var data = (
                from e in _context.Employees
                join a in _context.Applies on e.EmpId equals a.EmpId
                join p in _context.Posts on a.Post_Id equals p.Post_Id
                where p.FdId == fid && a.Status == true
                select new VMApplies
                {
                    Apply_id =a.ApplyId,
                    EmpName = e.EmpName,
                    EmpEmail = e.EmpEmail,
                    EmpResumeUrl = e.EmpResumeUrl,
                    Post_name = p.Post_Name,
                    Remark = a.remark,
                    Status = a.Status
                }).ToList();
            return View(data);

           
        }

    }
}
