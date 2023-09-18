using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSIET_Finance;
using OSIET_Finance.Models;
using OSIET_Finance.Models.Finance;

namespace LYRA.Controllers.ControllerFichiste
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly FINANCEContext _Context;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public FileController(IWebHostEnvironment env, FINANCEContext Context)
        {
            _env = env;
            _Context = Context;
        }

        public async Task<IActionResult> DownloadJournalInTwoDate(DataSend Data)
        {
            DateTime? d01 = new();
            DateTime? d02 = new();

            if (Data.d1 != null && Data.d2 != null)
            {
                d01 = DateTime.Parse(Data.d1);
                d02 = DateTime.Parse(Data.d2);
            }
            List<Journal> journals = await _Context.Journals.Where(f => f.Date.Value.Date >= d01 && f.Date.Value.Date <= d02).ToListAsync();
            List<Journals> journalss = new List<Journals>();
            foreach (var item in journals)
            {
                Journals j = new();
                Compte? c = new();
                Imputation? i = new();

                c = await this.GetCompteByIdAsync(item.CompteId);
                i = await this.GetImputationById(item.ImputationId);
                j.Date = item.Date.Value;
                j.CompteId = item.CompteId;
                j.CompteNom = c.NomCompte;
                j.Montant = item.Montant;
                j.Type = item.Type;
                j.PayerPar = item.PayerPar;
                j.ImputationId = item.ImputationId;
                j.ImputationNom = i.Nom;
                j.Concernee = item.Concernee;
                j.Libelle = item.Libelle;
                j.RefPiece = item.RefPiece;
                j.LocationFichier = item.LocationFichier;
                journalss.Add(j);
            }
            byte[] reportBytes;
            using (var package = Utils.DownloadJournalInTwoDate(journalss, d01.Value, d02.Value))
            {
                reportBytes = package.GetAsByteArray();
            }

            return File(reportBytes, XlsxContentType, $"MyReport{DateTime.Now:dd-MM-yyyy}.xlsx");
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> downloadFile(string fileName)
        {
            var path = Path.Combine(_env.ContentRootPath, "wwwroot", "Pieces", fileName);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octets-stream", Path.GetFileName(path));
        }

        public async Task<IActionResult> DownloadJournalAnnuel(DataSend Data)
        {
            int annee = 0;
            double lastMontant = 0;
            if (Data != null)
            {
                annee = Convert.ToInt32(Data.annee);
            }
            List<Journal> journals = await _Context.Journals.Where(f => f.Date.Value.Date.Year == annee).ToListAsync();
            List<Compte> comptes = new List<Compte>();
            List<Solde> soldes = new List<Solde>();
            comptes = await _Context.Comptes.ToListAsync();
            foreach (Compte compte in comptes)
            {
                try
                {
                    Solde solde = new();
                    solde = await _Context.Soldes.Where(x => x.Date.Value.Date.Year == (annee - 1) && x.CompteId == compte.CompteId).OrderBy(x => x.SoldeId).LastAsync();
                    soldes.Add(solde);
                }
                catch (InvalidOperationException)
                {

                }
            }
            foreach (Solde solde in soldes)
            {
                lastMontant += solde.Montant.Value;
            }

            List<Journals> journalss = new List<Journals>();
            foreach (var item in journals)
            {
                Journals j = new();
                Compte? c = new();
                Imputation? i = new();

                c = await this.GetCompteByIdAsync(item.CompteId);
                i = await this.GetImputationById(item.ImputationId);
                j.Date = item.Date.Value;
                j.CompteId = item.CompteId;
                j.CompteNom = c.NomCompte;
                j.Montant = item.Montant;
                j.Type = item.Type;
                j.PayerPar = item.PayerPar;
                j.ImputationId = item.ImputationId;
                j.ImputationNom = i.Nom;
                j.Concernee = item.Concernee;
                j.Libelle = item.Libelle;
                j.RefPiece = item.RefPiece;
                j.LocationFichier = item.LocationFichier;
                journalss.Add(j);
            }
            byte[] reportBytes;
            using (var package = Utils.DownloadJournalAnnuel(journalss, annee, lastMontant))
            {
                reportBytes = package.GetAsByteArray();
            }

            return File(reportBytes, XlsxContentType, $"MyReport{DateTime.Now:dd-MM-yyyy}.xlsx");
        }

        public async Task<Compte?> GetCompteByIdAsync(Guid idCompte)
        {
            Compte compte = new();
            try
            {
                compte = await _Context.Comptes.Where(c => c.CompteId == idCompte).FirstAsync<Compte>();
                await _Context.SaveChangesAsync();
                return compte;
            }
            catch (InvalidOperationException)
            {
                compte = new();
                return compte;
            }
        }

        public async Task<Imputation?> GetImputationById(Guid idImputation)
        {
            Imputation imputation = new();
            try
            {
                imputation = await _Context.Imputations.Where(c => c.ImputationId == idImputation).FirstAsync<Imputation>();
                await _Context.SaveChangesAsync();
                return imputation;
            }
            catch (InvalidOperationException)
            {
                imputation = new();
                return imputation;
            }
        }

    }




}
