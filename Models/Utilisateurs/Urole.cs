using System.ComponentModel.DataAnnotations;

namespace OSIET_Finance.Models.Administration
{
    public partial class Urole
    {
        public int Roleid { get; set; }

        [Required(ErrorMessage = "Ce champs est obligatoire")]
        public string Rolename { get; set; } = null!;
    }
}
