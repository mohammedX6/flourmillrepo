using FlourMill_1.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _context;

        public DataRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Administrator> AdminLogin(string username, string password)
        {
            var user = await _context.Administrator.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        public async Task<Bakery> BakeryLogin(string email, string password)
        {
            var user = await _context.Bakery.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        public async Task<TruckDriver> TruckDriverLogin(string email, string password)
        {
            var user = await _context.TruckDriver.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        public async Task<TruckDriver> TruckDriverLoginFacebook(string email)
        {
            var user = await _context.TruckDriver.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<SuperVisor> SuperVisorLogin(string username, string password)
        {
            var user = await _context.SuperVisor.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }

        public async Task<Administrator> AdminReg(Administrator administrator, string password)
        {
            byte[] PasswordHash, PasswordSalt;
            CreatePasswordHash(password, out PasswordHash, out PasswordSalt);
            administrator.PasswordHash = PasswordHash;
            administrator.PasswordSalt = PasswordSalt;
            await _context.Administrator.AddAsync(administrator);
            await _context.SaveChangesAsync();

            return administrator;
        }

        public async Task<Administrator> AdminRegFacebook(Administrator administrator)
        {
            await _context.Administrator.AddAsync(administrator);
            await _context.SaveChangesAsync();

            return administrator;
        }

        public async Task<Administrator> AdminLoginFacebook(string username)
        {
            var user = await _context.Administrator.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<Bakery> BakeryReg(Bakery bakery, string password)
        {
            byte[] PasswordHash, PasswordSalt;
            CreatePasswordHash(password, out PasswordHash, out PasswordSalt);
            bakery.PasswordHash = PasswordHash;
            bakery.PasswordSalt = PasswordSalt;
            await _context.Bakery.AddAsync(bakery);
            await _context.SaveChangesAsync();

            return bakery;
        }

        public async Task<TruckDriver> TruckDriverReg(TruckDriver truck, string password)
        {
            byte[] PasswordHash, PasswordSalt;
            CreatePasswordHash(password, out PasswordHash, out PasswordSalt);
            truck.PasswordHash = PasswordHash;
            truck.PasswordSalt = PasswordSalt;
            await _context.TruckDriver.AddAsync(truck);
            await _context.SaveChangesAsync();

            return truck;
        }

        public async Task<TruckDriver> TruckDriverRegFacebook(TruckDriver truck)
        {
            await _context.TruckDriver.AddAsync(truck);
            await _context.SaveChangesAsync();

            return truck;
        }

        public async Task<SuperVisor> SuperVisorReg(SuperVisor superVisor, string password)
        {
            byte[] PasswordHash, PasswordSalt;
            CreatePasswordHash(password, out PasswordHash, out PasswordSalt);
            superVisor.PasswordHash = PasswordHash;
            superVisor.PasswordSalt = PasswordSalt;
            await _context.SuperVisor.AddAsync(superVisor);
            await _context.SaveChangesAsync();

            return superVisor;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> AdminExists(string username)
        {
            if (await _context.Administrator.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }

        public async Task<bool> BakeryExists(string username)
        {
            if (await _context.Bakery.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }

        public async Task<bool> TruckDriverExists(string username)
        {
            if (await _context.TruckDriver.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }

        public async Task<bool> SuperVisorExists(string username)
        {
            if (await _context.SuperVisor.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }

        public async Task<bool> ProductExist(int myid)
        {
            if (await _context.Product.AnyAsync(x => x.ID == myid))
                return true;

            return false;
        }

        public async Task<int> DeleteProduct(int myID)
        {
            int result = 0;
            var getProduct = await _context.Product.FirstOrDefaultAsync(x => x.ID == myID);
            if (getProduct != null)
            {
                _context.Product.Remove(getProduct);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> AddNewProduct(Product createProduct)
        {
            await _context.Product.AddAsync(createProduct);
            await _context.SaveChangesAsync();
            return createProduct;
        }

        public async Task<Report> AddReport(Report report)
        {
            await _context.Report.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}