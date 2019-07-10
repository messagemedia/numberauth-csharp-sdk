using System;
using NumberAuthorisation.Standard.Controllers;
using NumberAuthorisation.Standard.Http.Client;
using NumberAuthorisation.Standard.Utilities;

namespace NumberAuthorisation.Standard
{
    public partial class NumberAuthorisationClient
    {

        /// <summary>
        /// Singleton access to Client controller
        /// </summary>
        public APIController Client
        {
            get
            {
                return APIController.Instance;
            }
        }
        /// <summary>
        /// The shared http client to use for all API calls
        /// </summary>
        public IHttpClient SharedHttpClient
        {
            get
            {
                return BaseController.ClientInstance;
            }
            set
            {
                BaseController.ClientInstance = value;
            }        
        }
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public NumberAuthorisationClient() { }
		/// <summary>
        /// Client initialization constructor
        /// </summary>
        public NumberAuthorisationClient(string UserName, string Password, bool hmacAuthentication = false)
        {
            if (!hmacAuthentication)
            {
                Configuration.BasicAuthUserName = UserName;
                Configuration.BasicAuthPassword = Password;
            }
            else
            {
                Configuration.HmacAuthUserName = UserName;
                Configuration.HmacAuthPassword = Password;
            }
        }
        #endregion
    }
}