# StandardJSONClient
A .NET Standard 1.4 based Client to access a WebAPI behindet a Token security system.

# Nuget Package
https://www.nuget.org/packages/StandardJSONClient

# How to use
First you must initalize the following attributes via resource file.

![Resource File](https://6vu8qg.bn1303.livefilestore.com/y4mDdeQ_l2yIHNPSUlRWY-t0qdv5-a76Leh7cVfW8-Oj59aZ2VEh59wy6wZgpGOR712U_17TB_U9kUxd225xmfJKrOzPmqxFx55vaf6-VxeR-o-vZTlJ-LA3nzXQyzH8Abm6LHscRaGDwSNfUj0fY4tZRRgbtMQmr-nEdt6IBtMrYgn0IGBsPduUi6qY-ehiq1HrIYpUfMGCdR_OMr_e-gC3g?width=877&height=209&cropmode=none)

```C#
// Init JSONClient
JsonClient.Client.ApiBaseUrl = Properties.Resources.ApiBaseUrl;
JsonClient.Client.ApiPassword = Properties.Resources.ApiPassword;
JsonClient.Client.ApiProjectPath = Properties.Resources.ApiProjectPath;
JsonClient.Client.ApiTokenPath = Properties.Resources.ApiTokenPath;
JsonClient.Client.ApiUserName = Properties.Resources.ApiUserName;
```

```C#
var response = JsonClient.Client.GetRequest<List<Patient>>(
                requestPath: "/api/Patients").Result;

Console.WriteLine(response[0].FirstName);
Console.WriteLine(response[0].LastName);
```
