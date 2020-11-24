using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        readonly MyDbContext dbContext;
        public HomeController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        [HttpGet]
        public ActionResult Index(int page=1)
        {
            int countFilm = 12;
            FilmsPaginationModel model = new FilmsPaginationModel();
            List<Film> films = dbContext.Films.ToList<Film>();
            model.Films = films.Skip((page - 1) * countFilm).Take(countFilm);
            model.PageInfo = new PageInfo { PageNumber = page, PageSize = countFilm, TotalItems = films.Count };
            return View(model);
        }
    }
}
