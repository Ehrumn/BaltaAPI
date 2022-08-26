﻿using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage ="O nome é obrigatório")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caractéres")]
    public string Name { get; set; }

    [Required(ErrorMessage ="O nome é obrigatório")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caractéres")]
    public string  Slug { get; set; }
}
