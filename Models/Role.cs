using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace auth.Models
{
    public class Role : IdentityRole
    {


        
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Role Name")]
       
        public string ? NameUser { get; set; } 
    }
}