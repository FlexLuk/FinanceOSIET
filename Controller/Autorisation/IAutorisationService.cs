using OSIET_Finance.Models.Administration;

namespace OSIET_Finance.Controller.Autorisation
{
    public interface IAutorisationService
    {
        public Task<Utilisateur?> GetValidUserCredentialAsync(string email, string password);
        public Task<string?> GetUserRoleAsync(string email);
        public Task<List<AvoirRole>> GetUserRoleListByUserAsync(int idUser);
        public Task<int> CreateUserAsync(Utilisateur user);
        public Task<bool> CreateRoleAsync(Urole role);
        public Task<bool> CreateUserRoleAsync(AvoirRole userRole);
        public Task<Utilisateur?> GetUserByIDAsync(int userID);
        public Task<Utilisateur?> GetUserByEmailAsync(string email);
        public Task<List<Urole>> GetAllRolesAsync(int userID);
        public Task<List<Utilisateur>> GetAllUsersAsync();
        public Task<bool> SupprimerUser(Utilisateur user);
    }
}
