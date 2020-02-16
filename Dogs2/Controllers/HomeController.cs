using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dogs2.Models;
using Microsoft.AspNetCore.Http;
using Dogs2.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Dogs2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DogsDB _dogsDB;

        public HomeController(DogsDB dogsDB, ILogger<HomeController> logger)
        {
            _dogsDB = dogsDB;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sortOrder, string filterString, DateTime filterDate)
        {
            try
            {
                var name = HttpContext.Session.GetString("userId");
                if (name == null)
                { return Redirect("/Account/");}

                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
   
                ViewData["CurrentFilter"] = filterString;
                if (filterDate != DateTime.MinValue)
                { 
                    ViewData["currentFilter2"] = filterDate.ToString("yyyy-MM-dd"); 
                }
                else
                {
                    ViewData["currentFilter2"] = null;
                }

                var queue = from q in _dogsDB.Queues
                            join u in _dogsDB.Userdetails on q.userId equals u.userId
                            where q.queueDateTime > DateTime.Now
                            select new QueueModel { queueId = q.queueId, userId = q.userId, queueDateTime = q.queueDateTime, insertDateTime = q.insertDateTime, users1 = u };

                if (!String.IsNullOrEmpty(filterString))
                {
                    queue = queue.Where(q => q.users1.displayName.Contains(filterString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        queue = queue.OrderByDescending(s => s.users1.displayName);
                        break;
                    case "Date":
                        queue = queue.OrderBy(s => s.queueDateTime);
                        break;
                    case "date_desc":
                        queue = queue.OrderByDescending(s => s.queueDateTime);
                        break;
                    default:
                        queue = queue.OrderBy(s => s.users1.displayName);
                        break;
                }

                
                return View(await queue.AsNoTracking().ToListAsync());
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Error));
            }
        }
        public async Task<ActionResult> DetailsAsync(int Id)
        {
            try
            {
                var queue = await _dogsDB.Queues.FirstOrDefaultAsync(m => m.queueId == Id);
                queue.users1 = await _dogsDB.Userdetails.FirstOrDefaultAsync(m => m.userId == queue.userId);

                //var queue = new QueueModel();
                return PartialView("Details", queue);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Error));
            }
        }

        public async Task<ActionResult> DeleteConfAsync(int Id)
        {
            try
            {
                var queue = await _dogsDB.Queues.FirstOrDefaultAsync(m => m.queueId == Id);
                queue.users1 = await _dogsDB.Userdetails.FirstOrDefaultAsync(m => m.userId == queue.userId);

                //var queue = new QueueModel();
                return PartialView("Delete", queue);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
                [Bind("EnrollmentDate,FirstMidName,LastName")] QueueModel queue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dogsDB.Add(queue);
                    await _dogsDB.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(queue);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var q = _dogsDB.Queues.Where(s => s.queueId == Id).FirstOrDefault();
            if (q == null)
            {
                string userId = HttpContext.Session.GetString("userId");
                q = new QueueModel() {  userId = int.Parse(userId) };
            }
            q.users1 = _dogsDB.Userdetails.Where(u => u.userId == q.userId).FirstOrDefault();

            return View(q);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QueueModel queue, bool? saveChangesError = false)
        {
            QueueModel queueToUpdate;
            try
            {
                queue.insertDateTime = DateTime.Now;
                if (queue.queueId == 0)
                { 
                    queueToUpdate = new QueueModel { };
                    if (await TryUpdateModelAsync<QueueModel>(
                        queueToUpdate,
                        "",
                        s => s.queueId, s => s.userId, s => s.insertDateTime, s => s.queueDateTime))
                    {
                        queueToUpdate.insertDateTime = DateTime.Now;
                        _dogsDB.Add(queueToUpdate);
                        await _dogsDB.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    queueToUpdate = await _dogsDB.Queues.FirstOrDefaultAsync(s => s.queueId == queue.queueId);

                    if (await TryUpdateModelAsync<QueueModel>(
                        queueToUpdate,
                        "",
                        s => s.queueId, s => s.insertDateTime, s => s.queueDateTime))
                    {
                        queueToUpdate.insertDateTime = DateTime.Now;
                        await _dogsDB.SaveChangesAsync();
                        
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Error));
            }
        }
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            try
            {
                if (id == null)
                {
                    return Error();
                }

                var queue = await _dogsDB.Queues
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.queueId == id);
                if (queue == null)
                {
                    return NotFound();
                }

                if (saveChangesError.GetValueOrDefault())
                {
                    ViewData["ErrorMessage"] =
                        "Delete failed. Try again, and if the problem persists " +
                        "see your system administrator.";
                }

                try
                {
                    _dogsDB.Queues.Remove(queue);
                    await _dogsDB.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Error));
            }
        }
       
        public IActionResult Privacy()
        {
            var name = HttpContext.Session.GetString("userId");
            if (name == null)
            { return Redirect("/Account/"); }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}
