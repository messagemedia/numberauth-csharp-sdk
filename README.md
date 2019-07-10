# MessageMedia Number Authorisation C# SDK
[![Pull Requests Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat)](http://makeapullrequest.com)
[![maven](https://img.shields.io/maven-metadata/v/http/central.maven.org/maven2/com/messagemedia/sdk/messages/maven-metadata.xml.svg)](https://mvnrepository.com/artifact/com.messagemedia.sdk/messages)

The number authorisation API allows you to manage your blacklists. MessageMedia automatically adds numbers to your blacklist if people send one of the opt out keywords in response to one of your messages.

This is a legal requirement. If you decide to handle the legal compliance yourself, calls to this endpoint will not affect your messages.

![Isometric](https://i.imgur.com/jJeHwf5.png)

## Table of Contents
* [Authentication](#closed_lock_with_key-authentication)
* [Errors](#interrobang-errors)
* [Information](#newspaper-information)
  * [Slack and Mailing List](#slack-and-mailing-list)
  * [Bug Reports](#bug-reports)
  * [Contributing](#contributing)
* [Installation](#star-installation)
* [Get Started](#clapper-get-started)
* [API Documentation](#closed_book-api-documentation)
* [Need help?](#confused-need-help)
* [License](#page_with_curl-license)

## :closed_lock_with_key: Authentication

Authentication is done via API keys. Sign up at https://developers.messagemedia.com/register/ to get your API keys.

Requests are authenticated using HTTP Basic Auth or HMAC. Provide your API key as the auth_user_name and API secret as the auth_password.

## :interrobang: Errors

Our API returns standard HTTP success or error status codes. For errors, we will also include extra information about what went wrong encoded in the response as JSON. The most common status codes are listed below.

#### HTTP Status Codes

| Code      | Title       | Description |
|-----------|-------------|-------------|
| 400 | Invalid Request | The request was invalid |
| 401 | Unauthorized | Your API credentials are invalid |
| 403 | Disabled feature | Feature not enabled |
| 404 | Not Found |	The resource does not exist |
| 50X | Internal Server Error | An error occurred with our API |

## :newspaper: Information

#### Slack and Mailing List

If you have any questions, comments, or concerns, please join our Slack channel:
https://developers.messagemedia.com/collaborate/slack/

Alternatively you can email us at:
developers@messagemedia.com

#### Bug reports

If you discover a problem with the SDK, we would like to know about it. You can raise an [issue](https://github.com/messagemedia/signingkeys-nodejs-sdk/issues) or send an email to: developers@messagemedia.com

#### Contributing

We welcome your thoughts on how we could best provide you with SDKs that would simplify how you consume our services in your application. You can fork and create pull requests for any features you would like to see or raise an [issue](https://github.com/messagemedia/signingkeys-nodejs-sdk/issues)

## :star: Installation
At present the jars are available from a public maven repository.

Use the following dependency in your project to grab via Maven:
```
<dependency>
  <groupId>com.messagemedia.sdk</groupId>
  <artifactId>messages</artifactId>
  <version>2.0.0</version>
</dependency>

```
## :clapper: Get Started
It's easy to get started. Simply enter the API Key and secret you obtained from the [MessageMedia Developers Portal](https://developers.messagemedia.com) into the code snippet below and a mobile number you wish to send to.

### List all blocked numbers
This endpoint returns a list of 100 numbers that are on the blacklist. There is a pagination token to retrieve the next 100 numbers.
```csharp
using System;
using System.Collections.Generic;
using NumberAuthorisation.Standard;
using NumberAuthorisation.Standard.Controllers;
using NumberAuthorisation.Standard.Exceptions;

namespace NumberAuthTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configuration parameters and credentials
            string authUserName = "API_KEY"; // The username to use with basic/HMAC authentication
            string authPassword = "API_SECRET"; // The password to use with basic/HMAC authentication
            Boolean useHmacAuth = false; // Change to true if you are using HMAC keys

            NumberAuthorisationClient client = new NumberAuthorisationClient(authUserName, authPassword, useHmacAuth);

            APIController controller = client.Client;

            try
            {
                dynamic result = controller.ListBlockedNumbersAsync().Result;

                Console.WriteLine(result);
            }
            catch (APIException e)
            {
                Console.WriteLine(e.Message + e.ResponseCode + e.HttpContext.ToString());
            };

        }
    }
}
```

### Add one or more numbers to your blacklist
This endpoint allows you to add one or more numbers to your blacklist. You can add up to 10 numbers in one request.
* Numbers should be in the [E.164](http://en.wikipedia.org/wiki/E.164) format. For example, `+61491570156`.
```csharp
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
            string authUserName = "API_KEY"; // The username to use with basic/HMAC authentication
            string authPassword = "API_SECRET"; // The password to use with basic/HMAC authentication
            Boolean useHmacAuth = false; // Change to true if you are using HMAC keys

            NumberAuthorisationClient client = new NumberAuthorisationClient(authUserName, authPassword, useHmacAuth);

            APIController controller = client.Client;

            try
            {
                List<string> mylist = new List<string>(new string[] { "+61466412529" });
                Numbersarray numbers = new Numbersarray();
                numbers.Numbers = mylist;

                dynamic result = controller.CreateBlacklistNumbersAsync(numbers);
                Console.WriteLine("result: ", result);
            }
            catch (APIException e)
            {
                Console.WriteLine(e.Message + e.ResponseCode + e.HttpContext.ToString());
            };

        }
    }
```

### Remove a number from the blacklist
You can get a messsage ID from a sent message by looking at the `message_id` from the response of the above example.
```java
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
            string authUserName = "API_KEY"; // The username to use with basic/HMAC authentication
            string authPassword = "API_SECRET"; // The password to use with basic/HMAC authentication
            Boolean useHmacAuth = false; // Change to true if you are using HMAC keys

            NumberAuthorisationClient client = new NumberAuthorisationClient(authUserName, authPassword, useHmacAuth);

            APIController controller = client.Client;

            try
            {
                dynamic result = controller.RemoveNumberAsync("+61<number>").Result;
                Console.WriteLine("result: ", result);
            }
            catch (APIException e)
            {
                Console.WriteLine(e.Message + e.ResponseCode + e.HttpContext.ToString());
            };

        }
    }
}
```

### Check the number authorisation blacklist
This endpoints lists for each requested number if you are authorised (which means the number is not blacklisted) to send to this number.

NOTE: We do this call for you internally no matter what. Use this endpoint only if you want to have some indication upfront. If you send a message which is on the blacklist, we issue a delivery receipt with the appropriate status code.
```csharp
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
```

## :closed_book: API Reference Documentation
Check out the [full API documentation](https://developers.messagemedia.com/code/messages-api-documentation/) for more detailed information.

## :confused: Need help?
Please contact developer support at developers@messagemedia.com or check out the developer portal at [developers.messagemedia.com](https://developers.messagemedia.com/)

## :page_with_curl: License
Apache License. See the [LICENSE](LICENSE) file.


