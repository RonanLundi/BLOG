﻿using System.ComponentModel.DataAnnotations;

namespace BlogAPI.ViewModels.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O Email é inválido")]
        public string Email { get; set; }

    }
}
