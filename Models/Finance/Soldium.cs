using System;
using System.Collections.Generic;

namespace OSIET_Finance.Models.Finance;

public partial class Soldium
{
    public Guid SoldeId { get; set; }

    public DateTime Date { get; set; }

    public Guid CompteId { get; set; }

    public string? NomCompte { get; set; }

    public double? Montant { get; set; }
}
