using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class FilmsPaginationModel
    {
        public IEnumerable<Film> Films { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
