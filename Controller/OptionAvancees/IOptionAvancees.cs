using OSIET_Finance.Models.Finance;

namespace OSIET_Finance.Controller.OptionAvancees
{
    public interface IOptionAvancees
    {
        public Task<Compte?> GetCompteByIdAsync(Guid idCompte);
        public Task<Compte?> GetCompteByNameAsync(string name);
        public Task<bool> CreateCompteAsync(Compte compte);
        public Task<Solde> CreateSoldeReturnAsync(Solde _solde);
        public Task<bool> GetHaveSoldeInitial(Guid idCompte);
        public Task<Solde?> GetLastSoldeByCompte(Guid CompteId);
        public Task<Imputation?> GetImputationById(Guid idImputation);
        public Task<bool> CreateSoldeAsync(Solde solde);
        public Task<bool> CreateImputationAsync(Imputation imputation);
        public Task<bool> UpdateImputationAsync(Imputation imputation);
        public Task<List<Compte>?> GetAllComptesAsync();
        public Task<List<Imputation>?> GetAllImputationsAsync();
        public Task<List<Solde>?> GetSoldesInitialAsync();
        public Task<List<Solde>?> GetSoldesLastAsync();
        public Task<bool> SupprimerCompte(Compte compte);
        public Task<bool> SupprimerImputation(Imputation _imputation);
    }
}
