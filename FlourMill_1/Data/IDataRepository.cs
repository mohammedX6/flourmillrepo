using FlourMill_1.Models;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IDataRepository
    {
        Task<Administrator> AdminLogin(string username, string password);
        Task<Administrator> AdminLoginFacebook(string username);

        Task<Bakery> BakeryLogin(string username, string password);

        Task<TruckDriver> TruckDriverLogin(string username, string password);
        Task<TruckDriver> TruckDriverLoginFacebook(string username);

        Task<SuperVisor> SuperVisorLogin(string username, string password);

        Task<bool> AdminExists(string username);

        Task<bool> BakeryExists(string username);

        Task<bool> TruckDriverExists(string username);

        Task<bool> SuperVisorExists(string username);

        Task<Administrator> AdminReg(Administrator administrator, string password);
        Task<Administrator> AdminRegFacebook(Administrator administrator);

        Task<Bakery> BakeryReg(Bakery bakery, string password);

        Task<TruckDriver> TruckDriverReg(TruckDriver truck, string password);
        Task<TruckDriver> TruckDriverRegFacebook(TruckDriver truck);

        Task<SuperVisor> SuperVisorReg(SuperVisor superVisor, string password);

        Task<bool> ProductExist(int myid);

        Task<int> DeleteProduct(int myid);

        Task UpdateProduct(Product productDto);

        Task<Product> AddNewProduct(Product createProduct);

        Task<Report> AddReport(Report report);
    }
}