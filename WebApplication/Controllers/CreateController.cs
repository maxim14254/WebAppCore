using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CreateController : Controller
    {
        readonly MyDbContext dbContext;
        IWebHostEnvironment webHostEnvironment;
        public CreateController(MyDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Film()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Film(IFormFile uploadedFile, Film film)
        {

            if (dbContext.Films.Where(f => f.Name == film.Name).Any())
            {
                ModelState.AddModelError("", "Фильм с таким названием уже существует");
                return View();
            }
            string wwwroot = webHostEnvironment.WebRootPath;
            DirectoryInfo dirInfo = new DirectoryInfo(wwwroot + "\\Images");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string path = $"\\Images\\{film.Name}_{User.Identity.Name}_{uploadedFile.FileName}";

            film.UrlImage = path;
            film.UserName = User.Identity.Name;
            dbContext.Films.Add(film);
            using (var fileStream = new FileStream(wwwroot + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
                dbContext.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetFilm(int id)
        {
            var film = dbContext.Films.Where(f => f.Id == id).FirstOrDefault();
            if (film == null)
                return RedirectToAction("NotFound", "Create");
            return View(film);
        }

        [HttpGet]
        public ActionResult EditFilm(int id)
        {
            Film film = dbContext.Films.Where(f => f.Id == id).FirstOrDefault();
            if (film != null)
            {
                if (film.UserName == User.Identity.Name)
                {
                    return View(film);
                }
                else
                {
                    return Redirect($"/Create/GetFilm?id={film.Id}");
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Create");
            }
        }
        [HttpGet]
        public ActionResult DeleteFilm(int id)
        {
            Film film = dbContext.Films.Where(f => f.Id == id).FirstOrDefault();
            if (film != null)
            {
                if (film.UserName == User.Identity.Name)
                {
                    System.IO.File.Delete($"wwwroot{film.UrlImage}");
                    dbContext.Remove(film);
                    dbContext.SaveChanges();
                    return RedirectToAction("MyFilms", "Create");
                }
                else { return Redirect($"/Create/GetFilm?id={film.Id}"); }
            }
            else
            {
                return RedirectToAction("NotFound", "Create");
            }

        }

        public new ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditFilm(IFormFile uploadedFile, Film film)
        {

            Film f = dbContext.Films.Where(f => f.Id == film.Id).FirstOrDefault();
            if (uploadedFile != null)
            {
                FileInfo fileInf = new FileInfo("wwwroot" + f.UrlImage);
                fileInf.Delete();

                string path = $"/Images/{film.Name}_{User.Identity.Name}_{uploadedFile.FileName}";
                f.UrlImage = path;
                using (var fileStream = new FileStream("wwwroot" + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            f.Name = film.Name;
            f.Producer = film.Producer;
            f.Year = film.Year;
            f.Content = film.Content;
            dbContext.SaveChanges();
            return RedirectToAction("MyFilms", "Create");

        }


        [HttpGet]
        public ActionResult MyFilms(int page = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 12; // количество объектов на страницу
            List<Film> films1 = dbContext.Films.Where(s => s.UserName == User.Identity.Name).ToList();
            List<Film> films2 = films1.Skip((page - 1) * pageSize).Take(pageSize).ToList<Film>();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = films1.Count() };
            IndexViewModel model = new IndexViewModel { PageInfo = pageInfo, Films = films2 };
            return View(model);
        }

    }
}