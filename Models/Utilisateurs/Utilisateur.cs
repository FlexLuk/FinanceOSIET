using System.ComponentModel.DataAnnotations;

namespace OSIET_Finance.Models.Administration
{
    public partial class Utilisateur
    {
        public short Utilisateurid { get; set; }

        [Required(ErrorMessage = "Veuillez entrer l'adresse mail")]
        [EmailAddress(ErrorMessage = "Adresse email n'est pas valide")]
        public string Email { get; set; } = null!;
        public string? Passwordhash { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Fonction { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }
}
