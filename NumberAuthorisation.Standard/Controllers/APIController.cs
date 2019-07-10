using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using NumberAuthorisation.Standard;
using NumberAuthorisation.Standard.Utilities;
using NumberAuthorisation.Standard.Http.Request;
using NumberAuthorisation.Standard.Http.Response;
using NumberAuthorisation.Standard.Http.Client;
using NumberAuthorisation.Standard.Exceptions;

namespace NumberAuthorisation.Standard.Controllers
{
    public partial class APIController: BaseController
    {
        #region Singleton Pattern

        //private static variables for the singleton pattern
        private static object syncObject = new object();
        private static APIController instance = null;

        /// <summary>
        /// Singleton pattern implementation
        /// </summary>
        internal static APIController Instance
        {
            get
            {
                lock (syncObject)
                {
                    if (null == instance)
                    {
                        instance = new APIController();
                    }
                }
                return instance;
            }
        }

        #endregion Singleton Pattern

        /// <summary>
        /// This endpoints lists for each requested number if you are authorised (which means the number is not blacklisted) to send to this number.
        /// In the example given +61491570157 is on the blacklist.
        /// NOTE: We do this call for you internally no matter what. Use this endpoint only if you want to have some indication upfront. If you send a message which is on the blacklist, we issue a delivery receipt with the appropriate status code.
        /// </summary>
        /// <param name="numbers">Required parameter: Example: </param>
        /// <return>Returns the dynamic response from the API call</return>
        public dynamic GetCheckNumber(string numbers)
        {
            Task<dynamic> t = GetCheckNumberAsync(numbers);
            APIHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// This endpoints lists for each requested number if you are authorised (which means the number is not blacklisted) to send to this number.
        /// In the example given +61491570157 is on the blacklist.
        /// NOTE: We do this call for you internally no matter what. Use this endpoint only if you want to have some indication upfront. If you send a message which is on the blacklist, we issue a delivery receipt with the appropriate status code.
        /// </summary>
        /// <param name="numbers">Required parameter: Example: </param>
        /// <return>Returns the dynamic response from the API call</return>
        public async Task<dynamic> GetCheckNumberAsync(string numbers)
        {
            //the base uri for api requests
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/number_authorisation/is_authorised/{numbers}");

            //process optional template parameters
            APIHelper.AppendUrlWithTemplateParameters(_queryBuilder, new Dictionary<string, object>()
            {
                { "numbers", numbers }
            });


            //validate and preprocess url
            string _queryUrl = APIHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string,string>()
            {
                { "user-agent", "APIMATIC 2.0" },
                { "accept", "application/json" }
            };

            //append authentication headers
            AuthManager.Instance.GetAuthHeaders(_queryUrl,_baseUri).ToList().ForEach(x => _headers.Add(x.Key, x.Value));

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.Get(_queryUrl,_headers);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request).ConfigureAwait(false);
            HttpContext _context = new HttpContext(_request,_response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            try
            {
                return APIHelper.JsonDeserialize<dynamic>(_response.Body);
            }
            catch (Exception _ex)
            {
                throw new APIException("Failed to parse the response: " + _ex.Message, _context);
            }
        }

        /// <summary>
        /// This endpoint allows you to remove a number from the blacklist.  Only one number can be deleted per request.
        /// In the example +61491570157 will be removed from the blacklist.
        /// NOTE:  numbers need to be in international format and therefore start with a +
        /// </summary>
        /// <param name="number">Required parameter: the number to remove</param>
        /// <return>Returns the dynamic response from the API call</return>
        public dynamic RemoveNumber(string number)
        {
            Task<dynamic> t = RemoveNumberAsync(number);
            APIHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// This endpoint allows you to remove a number from the blacklist.  Only one number can be deleted per request.
        /// In the example +61491570157 will be removed from the blacklist.
        /// NOTE:  numbers need to be in international format and therefore start with a +
        /// </summary>
        /// <param name="number">Required parameter: the number to remove</param>
        /// <return>Returns the dynamic response from the API call</return>
        public async Task<dynamic> RemoveNumberAsync(string number)
        {
            //the base uri for api requests
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/number_authorisation/mt/blacklist/{number}");

            //process optional template parameters
            APIHelper.AppendUrlWithTemplateParameters(_queryBuilder, new Dictionary<string, object>()
            {
                { "number", number }
            });


            //validate and preprocess url
            string _queryUrl = APIHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string,string>()
            {
                { "user-agent", "APIMATIC 2.0" },
                { "accept", "application/json" }
            };

            //append authentication headers
            AuthManager.Instance.GetAuthHeaders(_queryUrl,_baseUri).ToList().ForEach(x => _headers.Add(x.Key, x.Value));

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.Delete(_queryUrl, _headers, null);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request).ConfigureAwait(false);
            HttpContext _context = new HttpContext(_request,_response);

            //Error handling using HTTP status codes
            if (_response.StatusCode == 404)
                throw new APIException(@"The number doesn't exist on the blacklist.", _context);

            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            try
            {
                return APIHelper.JsonDeserialize<dynamic>(_response.Body);
            }
            catch (Exception _ex)
            {
                throw new APIException("Failed to parse the response: " + _ex.Message, _context);
            }
        }

        /// <summary>
        /// This endpoint allows you to add one or more numbers to your blacklist. You can add up to 10 numbers in one request.
        /// NOTE: numbers need to be in international format and therefore start with a +
        /// </summary>
        /// <param name="numbers">Required parameter: Example: </param>
        /// <return>Returns the dynamic response from the API call</return>
        public dynamic CreateBlacklistNumbers(Models.Numbers numbers)
        {
            Task<dynamic> t = CreateBlacklistNumbersAsync(numbers);
            APIHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// This endpoint allows you to add one or more numbers to your blacklist. You can add up to 10 numbers in one request.
        /// NOTE: numbers need to be in international format and therefore start with a +
        /// </summary>
        /// <param name="numbers">Required parameter: Example: </param>
        /// <return>Returns the dynamic response from the API call</return>
        public async Task<dynamic> CreateBlacklistNumbersAsync(Models.Numbers numbers)
        {
            //the base uri for api requests
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/number_authorisation/mt/blacklist");


            //validate and preprocess url
            string _queryUrl = APIHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string,string>()
            {
                { "user-agent", "APIMATIC 2.0" },
                { "accept", "application/json" },
                { "Content-Type", "application/json" }
            };

            //append body params
            var _body = APIHelper.JsonSerialize(numbers);

            //append authentication headers
            AuthManager.Instance.GetAuthHeaders(_queryUrl,_baseUri,_body).ToList().ForEach(x => _headers.Add(x.Key, x.Value));

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request).ConfigureAwait(false);
            HttpContext _context = new HttpContext(_request,_response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            try
            {
                return APIHelper.JsonDeserialize<dynamic>(_response.Body);
            }
            catch (Exception _ex)
            {
                throw new APIException("Failed to parse the response: " + _ex.Message, _context);
            }
        }

        /// <summary>
        /// This endpoint returns a list of 100 numbers that are on the blacklist.  There is a pagination token to retrieve the next 100 numbers
        /// In the example response the numbers +61491570156 and +61491570157 are on the blacklist and therefore will never receive any messages from you.
        /// </summary>
        /// <return>Returns the dynamic response from the API call</return>
        public dynamic ListBlockedNumbers()
        {
            Task<dynamic> t = ListBlockedNumbersAsync();
            APIHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// This endpoint returns a list of 100 numbers that are on the blacklist.  There is a pagination token to retrieve the next 100 numbers
        /// In the example response the numbers +61491570156 and +61491570157 are on the blacklist and therefore will never receive any messages from you.
        /// </summary>
        /// <return>Returns the dynamic response from the API call</return>
        public async Task<dynamic> ListBlockedNumbersAsync()
        {
            //the base uri for api requests
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/number_authorisation/mt/blacklist");


            //validate and preprocess url
            string _queryUrl = APIHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string,string>()
            {
                { "user-agent", "APIMATIC 2.0" },
                { "accept", "application/json" }
            };

            //append authentication headers
            AuthManager.Instance.GetAuthHeaders(_queryUrl,_baseUri).ToList().ForEach(x => _headers.Add(x.Key, x.Value));

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.Get(_queryUrl,_headers);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request).ConfigureAwait(false);
            HttpContext _context = new HttpContext(_request,_response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            try
            {
                return APIHelper.JsonDeserialize<dynamic>(_response.Body);
            }
            catch (Exception _ex)
            {
                throw new APIException("Failed to parse the response: " + _ex.Message, _context);
            }
        }

    }
} 