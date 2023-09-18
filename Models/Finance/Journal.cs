using System;
using System.Collections.Generic;

namespace OSIET_Finance.Models.Finance;

public partial class Journal
{
    public Guid JournalId { get; set; }

    public DateTime? Date { get; set; }

    public string? PayerPar { get; set; }

    public Guid ImputationId { get; set; }

    public string? RefPiece { get; set; }

    public string? LocationFichier { get; set; }

    public string? Libelle { get; set; }

    public string Concernee { get; set; } = null!;

    public Guid CompteId { get; set; }

    public Guid? IdSoldeAvant { get; set; }

    public Guid? IdSoldeApres { get; set; }

    public double Montant { get; set; }

    public string Type { get; set; } 
}
