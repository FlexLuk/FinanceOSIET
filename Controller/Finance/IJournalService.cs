using OSIET_Finance.Models.Finance;

namespace OSIET_Finance.Controller.Finance
{
    public interface IJournalService
    {
        public Task<bool> CreateJournal(Journal _journal);
        public Task<List<Journal>> GetAllJournals();
        public Task<Journal> GetJournalById(Guid idJournal);
        public Task<Journal?> GetJournalByRef(string reference);
        public Task<bool> DeleteJournal(Journal _journal);
        public Task<bool> EditJournal(Journal _journal);
        public Task<List<Journal>?> ListeJournalEntreDeuxDate(DateTime? d1, DateTime? d2);

    }
}
