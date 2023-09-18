using Microsoft.EntityFrameworkCore;
using OSIET_Finance.Models.Administration;

namespace OSIET_Finance.Controller.Autorisation
{
    public class AutorisationService : IAutorisationService
    {
        readonly UTILISATEURContext context;
        public AutorisationService(UTILISATEURContext _context)
        {
            context = _context;
        }

        public async Task<bool> CreateRoleAsync(Urole role)
        {
            await context.Uroles.AddAsync(role);
            int i = context.SaveChanges();
            return i > 0;
        }

        public async Task<int> CreateUserAsync(Utilisateur user)
        {
            try
            {
                await context.Utilisateurs.AddAsync(user);
                context.SaveChanges();
                return user.Utilisateurid;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        public async Task<bool> CreateUserRoleAsync(AvoirRole userRole)
        {
            await context.AvoirRoles.AddAsync(userRole);
            int i = context.SaveChanges();
            return i > 0;
        }

        public async Task<List<Urole>> GetAllRolesAsync(int userID)
        {
            List<Urole> cont;
            cont = await context.Uroles.ToListAsync();
            context.SaveChanges();
            return cont;
        }

        public async Task<List<Utilisateur>> GetAllUsersAsync()
        {
            List<Utilisateur> users = new();
            users = await context.Utilisateurs.ToListAsync();
            context.SaveChanges();
            return users;
        }

        public async Task<Utilisateur?> GetUserByIDAsync(int userID)
        {
            Utilisateur? us;
            us = await context.Utilisateurs.Where(u => u.Utilisateurid == userID).FirstOrDefaultAsync();
            context.SaveChanges();
            return us;
        }

        public async Task<Utilisateur?> GetUserByEmailAsync(string email)
        {
            Utilisateur? us;
            try
            {
                us = await context.Utilisateurs.Where(u => u.Email == email.Trim()).FirstAsync();
                //context.SaveChanges();
                return us;
            }
            catch (InvalidOperationException)
            {
                return us = null;
            }
        }

        public async Task<string?> GetUserRoleAsync(string email)
        {
            Utilisateur? us;
            AvoirRole? uRl = new();
            Urole? rl = new();
            us = await context.Utilisateurs.Where(u => u.Email == email).FirstOrDefaultAsync();
            context.SaveChanges();
            if (us != null)
                uRl = await context.AvoirRoles.Where(u => u.Utilisateurid == us.Utilisateurid).FirstOrDefaultAsync();
            context.SaveChanges();
            if (uRl != null)
                rl = await context.Uroles.Where(u => u.Roleid == uRl.Roleid).FirstOrDefaultAsync();
            context.SaveChanges();
            if (rl != null)
                return rl.Rolename;
            else return null;
        }

        public async Task<Utilisateur?> GetValidUserCredentialAsync(string email, string password)
        {
            Utilisateur? user;
            user = await GetUserByEmailAsync(email);

            if (user != null)
            {
                string _passwrd = "";
                if (password == _passwrd)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public async Task<List<AvoirRole>> GetUserRoleListByUserAsync(int idUser)
        {
            List<AvoirRole> usRole;
            usRole = await context.AvoirRoles.Where(u => u.Utilisateurid == idUser).ToListAsync();
            context.SaveChanges();
            return usRole;
        }

        public async Task<bool> SupprimerUser(Utilisateur user)
        {
            context.Utilisateurs.Remove(user);
            int i = await context.SaveChangesAsync();
            return i > 0;
        }
    }
}
