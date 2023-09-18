using System;
using System.Collections.Generic;

namespace OSIET_Finance.Models.Finance;

public partial class Imputation
{
    public Guid ImputationId { get; set; }

    public string Nom { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int NumeroCompte { get; set; }

    public string? Description { get; set; }
}
