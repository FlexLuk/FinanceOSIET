using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OSIET_Finance.Models.Finance;

public partial class Compte
{
    public Guid CompteId { get; set; }

    public string NumCompte { get; set; } = null!;

    public string NomCompte { get; set; } = null!;
}
