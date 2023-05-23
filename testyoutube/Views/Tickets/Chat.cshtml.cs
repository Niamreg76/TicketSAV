using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class InputModel : PageModel
{

    [BindProperty]
    public InputModel Input { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Le message ne peut pas dépasser 500 caractères.")]
    public string Message { get; set; }
}