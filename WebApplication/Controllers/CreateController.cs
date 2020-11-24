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
            return View(dbContext.Films.Where(f => f.Id == id).FirstOrDefault());
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
                System.IO.File.Delete($"wwwroot{film.UrlImage}");
                dbContext.Remove(film);
                dbContext.SaveChanges();
                return RedirectToAction("MyFilms", "Create");
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
        public ActionResult MyFilms(int page=1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            int countFilm = 12;
            FilmsPaginationModel model = new FilmsPaginationModel();
            List<Film> films = dbContext.Films.Where(f => f.UserName == User.Identity.Name).Skip((page - 1) * countFilm).Take(countFilm).ToList<Film>();
            model.PageInfo = new PageInfo { PageNumber = page, PageSize = countFilm, TotalItems = films.Count };
            return View(model);
        }

    }
}
