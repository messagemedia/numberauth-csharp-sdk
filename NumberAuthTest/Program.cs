using System;
using System.Collections.Generic;
using NumberAuthorisation.Standard;
using NumberAuthorisation.Standard.Controllers;
using NumberAuthorisation.Standard.Exceptions;
using NumberAuthorisation.Standard.Models;

namespace NumberAuthTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configuration parameters and credentials
            string authUserName = "ZOUA31Dh4hZEAZm1xdVN"; // The username to use with basic/HMAC authentication
            string authPassword = "6mKCzhjOidQvtHx5WTjKatRGxYgR3p"; // The password to use with basic/HMAC authentication
            Boolean useHmacAuth = false; // Change to true if you are using HMAC keys

            NumberAuthorisationClient client = new NumberAuthorisationClient(authUserName, authPassword, useHmacAuth);

            APIController controller = client.Client;

            try
            {
                dynamic result = controller.GetCheckNumberAsync("+61466412529").Result;
                Console.WriteLine(result);
            }
            catch (APIException e)
            {
                Console.WriteLine(e.Message + e.ResponseCode + e.HttpContext.ToString());
            };

        }
    }
}
