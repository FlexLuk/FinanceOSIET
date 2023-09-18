using Microsoft.EntityFrameworkCore;
using OSIET_Finance.Models.Finance;
using System.Linq.Dynamic.Core;

namespace OSIET_Finance.Controller.OptionAvancees
{
    public class OptionAvancees : IOptionAvancees
    {
        FINANCEContext Context;

        public OptionAvancees(FINANCEContext _context)
        {
            Context = _context;
        }

        public async Task<bool> CreateCompteAsync(Compte _compte)
        {
            Compte compte = _compte;
            await Context.Comptes.AddAsync(compte);
            int i = Context.SaveChanges();
            return i > 0;
        }

        public async Task<bool> CreateImputationAsync(Imputation _imputation)
        {
            Imputation imputation = _imputation;
            await Context.Imputations.AddAsync(imputation);
            int i = Context.SaveChanges();
            return i > 0;
        }

        public async Task<bool> CreateSoldeAsync(Solde _solde)
        {
            Solde solde = _solde;
            await Context.Soldes.AddAsync(solde);
            int i = Context.SaveChanges();
            return i > 0;
        }

        public async Task<Solde> CreateSoldeReturnAsync(Solde _solde)
        {
            Solde solde = _solde;
            await Context.Soldes.AddAsync(solde);
            Context.SaveChanges();
            return solde;
        }

        public async Task<List<Compte>> GetAllComptesAsync()
        {
            List<Compte> comptes = new();
            comptes = await Context.Comptes.ToListAsync<Compte>();
            Context.SaveChanges();
            return comptes;
        }

        public async Task<List<Imputation>?> GetAllImputationsAsync()
        {
            List<Imputation> imputations = new();
            imputations = await Context.Imputations.ToListAsync<Imputation>();
            Context.SaveChanges();
            return imputations;
        }

        public async Task<Compte?> GetCompteByIdAsync(Guid idCompte)
        {
            Compte compte = new();
            try
            {
                compte = await Context.Comptes.Where(c => c.CompteId == idCompte).FirstAsync<Compte>();
                await Context.SaveChangesAsync();
                return compte;
            }
            catch (InvalidOperationException)
            {
                compte = new();
                return compte;
            }
        }

        public async Task<Compte?> GetCompteByNameAsync(string name)
        {
            Compte compte = new();
            try
            {
                compte = await Context.Comptes.Where(c => c.NomCompte == name).FirstAsync<Compte>();
                await Context.SaveChangesAsync();
                return compte;
            }
            catch (InvalidOperationException)
            {
                compte = new();
                return compte;
            }
        }

        public async Task<bool> GetHaveSoldeInitial(Guid idCompte)
        {
            Solde compte;
            try
            {
                compte = await Context.Soldes.Where(x => x.CompteId == idCompte).FirstAsync();
                await Context.SaveChangesAsync();
                if (compte != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }

        }

        public async Task<Imputation?> GetImputationById(Guid idImputation)
        {
            Imputation imputation = new();
            try
            {
                imputation = await Context.Imputations.Where(c => c.ImputationId == idImputation).FirstAsync<Imputation>();
                await Context.SaveChangesAsync();
                return imputation;
            }
            catch (InvalidOperationException)
            {
                imputation = new();
                return imputation;
            }
        }

        public async Task<Solde?> GetLastSoldeByCompte(Guid CompteId)
        {
            List<Solde> solde = new();
            try
            {
                solde = await Context.Soldes.Where(c => c.CompteId == CompteId).ToListAsync();
                return solde[solde.Count - 1];
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<List<Solde>?> GetSoldesInitialAsync()
        {
            List<Solde> soldes = new();
            List<Compte> comptes = new();
            comptes = await Context.Comptes.ToListAsync<Compte>();
            Context.SaveChanges();
            foreach (Compte compte in comptes)
            {
                Solde solde = new();
                try
                {
                    solde = await Context.Soldes.Where(c => c.CompteId == compte.CompteId).FirstAsync<Solde>();
                    await Context.SaveChangesAsync();
                    soldes.Add(solde);
                }
                catch (InvalidOperationException)
                {
                    solde = null;
                }

            }
            return soldes;
        }

        public async Task<List<Solde>?> GetSoldesLastAsync()
        {
            List<Solde>? soldes = new();
            List<Compte> comptes = new();
            comptes = await Context.Comptes.ToListAsync<Compte>();
            foreach (Compte compte in comptes)
            {
                List<Solde> solde = new();
                try
                {
                    solde = await Context.Soldes.Where(c => c.CompteId == compte.CompteId).ToListAsync();
                    if (solde.Count() > 0)
                        soldes.Add(solde[solde.Count -1]);
                }
                catch (InvalidOperationException)
                {
                    
                }

            }
            return soldes;
        }

        public Task<bool> SupprimerCompte(Compte compte)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SupprimerImputation(Imputation _imputation)
        {
            Context.Imputations.Remove(_imputation);
            int i = await Context.SaveChangesAsync();
            return i > 0;
        }

        public async Task<bool> UpdateImputationAsync(Imputation imputation)
        {
            Context.Entry(imputation).State = EntityState.Modified;
            int i = await Context.SaveChangesAsync();
            return i > 0;
        }
    }
}
