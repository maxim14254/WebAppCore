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
        public ActionResult Index(int page = 1)
        {
            int pageSize = 12; // количество объектов на страницу
            List<Film> films = dbContext.Films.Skip((page - 1) * pageSize).Take(pageSize).ToList<Film>();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = dbContext.Films.Count() };
            IndexViewModel model = new IndexViewModel { PageInfo = pageInfo, Films = films };
            return View(model);
        }
    }
}