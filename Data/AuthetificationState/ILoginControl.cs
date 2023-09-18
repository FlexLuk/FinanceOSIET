using OSIET_Finance.Models.Administration;

namespace OSIET_Finance.Data.AuthetificationState
{
    public interface ILoginControl
    {
        public Task<Utilisateur?> VerificationUtilisateur(string email, string passswordHash);
        public Task<List<int>> GetRoleIdUSer(Utilisateur utilisateur);
        public Task<List<Urole>> GetRoleUserAuthentified(Utilisateur utilisateur);
    }
}