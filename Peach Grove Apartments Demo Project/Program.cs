using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Peach_Grove_Apartments_Demo_Project
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Run();
        //}



        //public static IWebHost CreateHostBuilder(string[] args)
        //{

        //    return WebHost.CreateDefaultBuilder(args)
        //       .ConfigureAppConfiguration((ctx, builder) =>
        //       {
        //           var keyVaultEndpoint = GetdKeyVaultEndpoint();
        //           if (!string.IsNullOrEmpty(keyVaultEndpoint))
        //           {
        //               var azureServiceTokenProvider = new AzureServiceTokenProvider();
        //               var keyVaultClient = new KeyVaultClient(
        //                   new KeyVaultClient.AuthenticationCallback(
        //                       azureServiceTokenProvider.KeyVaultTokenCallback));
        //               builder.AddAzureKeyVault(
        //                   keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        //           }
        //       }
        //    ).UseStartup<Startup>()
        //     .Build();
        //}

        //private static string GetdKeyVaultEndpoint() => "https://peachgrovekeyvault.vault.azure.net/"; 

        ////ConfigureWebHostDefaults(webBuilder =>
        ////        {
        ////            webBuilder.UseStartup<Startup>();
        ////        });
    }

}
