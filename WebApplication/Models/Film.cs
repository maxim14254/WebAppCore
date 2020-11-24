using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Film
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Producer { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UrlImage { get; set; }

    }
}
