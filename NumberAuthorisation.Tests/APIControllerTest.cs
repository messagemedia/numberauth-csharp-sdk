using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using NumberAuthorisation.Standard;
using NumberAuthorisation.Standard.Utilities; 
using NumberAuthorisation.Standard.Http.Client;
using NumberAuthorisation.Standard.Http.Response;
using NumberAuthorisation.Tests.Helpers;
using NUnit.Framework;
using NumberAuthorisation.Standard;
using NumberAuthorisation.Standard.Controllers;
using NumberAuthorisation.Standard.Exceptions;

namespace NumberAuthorisation.Tests
{
    [TestFixture]
    public class APIControllerTest : ControllerTestBase
    {
        /// <summary>
        /// Controller instance (for all tests)
        /// </summary>
        private static APIController controller;

        /// <summary>
        /// Setup test class
        /// </summary>
        [SetUp]
        public static void SetUpClass()
        {
            controller = GetClient().Client;
        }

        /// <summary>
        /// This endpoint allows you to remove a number from the blacklist.  Only one number can be deleted per request.
        ///
        ///In the example +61491570157 will be removed from the blacklist.
        ///
        ///NOTE:  numbers need to be in international format and therefore start with a + 
        /// </summary>
        [Test]
        public async Task TestRemoveANumberFromTheBlacklist1() 
        {

            // Perform API call
            Stream result = null;

            try
            {
                result = await controller.RemoveANumberFromTheBlacklistAsync();
            }
            catch(APIException) {};

            // Test response code
            Assert.AreEqual(200, httpCallBackHandler.Response.StatusCode,
                    "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);

            Assert.IsTrue(TestHelper.AreHeadersProperSubsetOf (
                    headers, httpCallBackHandler.Response.Headers),
                    "Headers should match");

        }

    }
}