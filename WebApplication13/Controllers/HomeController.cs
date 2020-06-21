using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace WebApplication13.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var CS = new SqlConnectionStringBuilder();
            CS.DataSource = "BI-LAPTOP\\SQLEXPRESS";
            CS.InitialCatalog = "web13.4";
            CS.UserID = "sa";
            CS.IntegratedSecurity = false;
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            var secret1 = await keyVault.GetSecretAsync("https://truongkv1705.vault.azure.net/secrets/DBPassword");
            CS.Password = secret1.Value;
            ViewBag.secret = CS.ToString();

            return View();
        }
        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential("7a1a09cf-fbe7-413a-bc9d-473ec584d1ae", "G57O_xzp8PX19_j_7WyEah__V008sv_gz6");
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the token");

            return result.AccessToken;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}