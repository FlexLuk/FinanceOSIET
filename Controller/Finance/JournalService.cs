using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using OSIET_Finance.Models.Finance;
using OSIET_Finance.Pages.Finance;
using OSIET_Finance.Pages.OptionAvancees;

namespace OSIET_Finance.Controller.Finance
{
    public class JournalService : IJournalService
    {
        FINANCEContext Context;

        public JournalService(FINANCEContext _context)
        {
            Context = _context;
        }
        public async Task<bool> CreateJournal(Journal _journal)
        {
            Journal journal = _journal;
            await Context.Journals.AddAsync(journal);
            int i = Context.SaveChanges();
            return i > 0;
        }

        public async Task<bool> DeleteJournal(Journal _journal)
        {
            Context.Journals.Remove(_journal);
            int i = await Context.SaveChangesAsync();
            return i > 0;
        }

        public async Task<bool> EditJournal(Journal _journal)
        {
            Context.Entry(_journal).State = EntityState.Modified;
            int i = await Context.SaveChangesAsync();
            return i > 0;
        }

        public async Task<List<Journal>> GetAllJournals()
        {
            List<Journal> journal = new();
            journal = await Context.Journals.ToListAsync<Journal>();
            Context.SaveChanges();
            return journal;
        }

        public async Task<Journal> GetJournalById(Guid idJournal)
        {
            Journal journal = new();
            try
            {
                journal = await Context.Journals.Where(c => c.CompteId == idJournal).FirstAsync<Journal>();
                return journal;
            }
            catch (InvalidOperationException)
            {
                journal = new();
                return journal;
            }
        }

        public async Task<Journal?> GetJournalByRef(string reference)
        {
            Journal? journal = new();
            try
            {
                journal = await Context.Journals.Where(c => c.RefPiece == reference).FirstAsync<Journal>();
                return journal;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<List<Journal>?> ListeJournalEntreDeuxDate(DateTime? d1, DateTime? d2)
        {
            List<Journal>? journal = new();
            try
            {
                journal = await Context.Journals.Where(list => list.Date >= d1 && list.Date <= d2).ToListAsync<Journal>();
                Context.SaveChanges();
                return journal;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
