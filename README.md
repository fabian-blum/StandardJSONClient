# JSONClient
To access a WebAPI behindet a Token security system

# How to use
```C#
// Init JSONClient
JsonClient.Client.ApiBaseUrl = Properties.Resources.ApiBaseUrl;
JsonClient.Client.ApiPassword = Properties.Resources.ApiPassword;
JsonClient.Client.ApiProjectPath = Properties.Resources.ApiProjectPath;
JsonClient.Client.ApiTokenPath = Properties.Resources.ApiTokenPath;
JsonClient.Client.ApiUserName = Properties.Resources.ApiUserName;
```