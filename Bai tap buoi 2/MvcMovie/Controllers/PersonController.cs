using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Models.Process;
using System.IO;
using X.PagedList;              // dùng cái này
using X.PagedList.Extensions;   // để có ToPagedList

namespace MvcMovie.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ExcelProcess _excelProcess = new ExcelProcess();

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== INDEX có phân trang – CHỈ CÓ DUY NHẤT ACTION NÀY =====
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize   = 5;

            var model = _context.Person
                                .OrderBy(p => p.PersonId)
                                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var person = await _context.Person.FirstOrDefaultAsync(x => x.PersonId == id);
            if (person == null) return NotFound();

            return View(person);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,FullName,Address")] Person person)
        {
            if (!ModelState.IsValid) return View(person);

            _context.Add(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var person = await _context.Person.FindAsync(id);
            if (person == null) return NotFound();

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,FullName,Address")] Person person)
        {
            if (id != person.PersonId) return NotFound();
            if (!ModelState.IsValid) return View(person);

            try
            {
                _context.Update(person);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(person.PersonId)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var person = await _context.Person.FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null) return NotFound();

            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }

        // ===== UPLOAD EXCEL =====
        [HttpGet]
        public IActionResult Upload()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please choose excel file to upload!");
                return View("Create");
            }

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                ModelState.AddModelError("", "Please choose excel file to upload!");
                return View("Create");
            }

            var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Excels");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath     = Path.Combine(folderPath, fileName);
            var fileLocation = new FileInfo(filePath).ToString();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dt = _excelProcess.ExcelToDataTable(fileLocation);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var id = dt.Rows[i][0]?.ToString();
                if (string.IsNullOrWhiteSpace(id))
                    continue;

                // tránh insert trùng PersonId
                if (_context.Person.Any(p => p.PersonId == id))
                    continue;

                var ps = new Person
                {
                    PersonId = id,
                    FullName = dt.Rows[i][1]?.ToString(),
                    Address  = dt.Rows[i][2]?.ToString()
                };

                _context.Person.Add(ps);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===== DOWNLOAD EXCEL =====
        public IActionResult Download()
        {
            OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("MINH");

            var fileName = "PersonList.xlsx";

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "PersonID";
                worksheet.Cells["B1"].Value = "FullName";
                worksheet.Cells["C1"].Value = "Address";

                var personList = _context.Person.ToList();
                worksheet.Cells["A2"].LoadFromCollection(personList);

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
        }
    }
}
