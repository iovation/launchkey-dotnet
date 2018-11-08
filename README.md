# LaunchKey .NET SDK

## Summary
This project contains the source code for the LaunchKey .NET SDK. For quick examples, see the [Examples Project](src/iovation.LaunchKey.Sdk.ExampleCli). For API introduction and requirements, read on.
## Requirements
This SDK is targeted toward:

- Windows 2008 R2 and above
- Windows 7 and above
- Linux
- OSX
- .NET Framework 4.0+
- .NET Standard 2.0 (.NET 4.6.1+, .NET Core 2.0+, Mono 5.4, etc.) See .NET Standard compatibility here: https://docs.microsoft.com/en-us/dotnet/standard/net-standard


## Building
The solution file is for Visual Studio 2017, and the projects are multi-targeting .NET 4.0 and .NET Standard 2.0. The project should build out of the box. Simply clone, open in VS, compile.

## Tests
The test projects use multi-targeting as well. The best way to run these tests is using the command line:

```
cd iovation.LaunchKey.Sdk.Tests
dotnet test
```

This method results in both the .NET 4.0 and .NET Core runtimes being used for test execution. Currently, VS 2017's IDE-based test runner does *not* properly run all targets.

## Example & Usage
The primary entry point to the SDK is a class named `FactoryFactoryBuilder`. This class, located in the `iovation.LaunchKey.Sdk` namespace provides factories for instantiating API clients which can be used to consume the LaunchKey platform API. The `FactoryFactoryBuilder` is designed to simplify creating the rather complex (and highly customizable!) object model that drives the LaunchKey .NET SDK.

To be able to use the SDK, you will need a few pieces of key information:

* **API Credentials**: in the form of an RSA private key and the unique ID of the Organization, Directory or Service that issued the key. These are issued at the Organization, Directory or Service level. An Organization-level credential may access all services and directories within itself. A Directory-level credential may access itself and its child services. A Service-level credential may only access the service for which it is issued. Generally speaking, you may issue these credentials from the [LaunchKey Admin Center](https://admin.launchkey.com)
* **Service ID, Directory ID or Organization ID**: This is a UUID/GUID for referencing the Organization, Directory or Service you wish to interact with programmatically.

Once you have obtained the above information, you have everything you need to create a `FactoryFactoryBuilder`, and begin consuming the API. 

### A note about credentials and factories
Once you have created a `FactoryFactoryBuilder` and use it to produce a `FactoryFactory` you will have a decision to make. If you are using organization-level credentials, you need to call `MakeOrganizationFactory` and use it to access the API. If you are using directory-level credentials, call `MakeDirectoryFactory` and if you are using service-level credentials call `MakeServiceFactory`.

For example, to perform a basic user ("myusername") authorization against a non-directory service (service ID 6FFC9464-4B29-422A-9D70-87D22CB09A61) using service-level credentials (an RSA private key located in "my-service-key.txt"):

### Step 1: Create and configure FactoryFactoryBuilder

```c#
// create our FactoryFactoryBuilder
var factoryFactoryBuilder = new FactoryFactoryBuilder();

// at this point, you can use FactoryFactoryBuilder to customize how the SDK functions:
// change timeouts, service dependencies, caching and caching rules, etc.

// apply all our settings and create a FactoryFactory
var factoryFactory = factoryFactoryBuilder.Build();
```

### Step 2: Create a Factory based on the type of credential being used
```C#
// read our RSA private key from a file.
var serviceKeyContents = File.ReadAllText("my-service-key.txt");

// Creates a factory that expects service-level credentials and gives access to the API clients which are valid for that level of credential
var serviceFactory = factoryFactory.MakeServiceFactory("6FFC9464-4B29-422A-9D70-87D22CB09A61", serviceKeyContents);
```

### Step 3: Create an API client using the credential-based factory just created
```C#
// Finally, create a ServiceClient which can be used to communicate with the /v3/services endpoints
var serviceClient = serviceFactory.MakeServiceClient();
```

### Step 4: Make the appropriate call
```c#
serviceClient.CreateAuthorizationRequest("myusername");
```

### All Together: Nice and Compact, Polling Authorization Example

```c#
var serviceKeyContents = File.ReadAllText("my-service-key.txt");
var factory = new FactoryFactoryBuilder().Build();
var serviceFactory = factory.MakeServiceFactory("6FFC9464-4B29-422A-9D70-87D22CB09A61", serviceKeyContents);
var serviceClient = serviceFactory.MakeServiceClient();
var authorizationRequest = serviceClient.CreateAuthorizationRequest("myusername");
while (true)
{
	var response = serviceClient.GetAuthorizationResponse(authorizationRequest.Id);
	if (response != null)
	{
		if (response.Authorized)
			Console.WriteLine("Login successful!");
		else
			Console.WriteLine("User denied us!");
		break;
	}
	else
	{
		Console.WriteLine("Waiting for the user to respond ...");
		Thread.Sleep(1000);
	}
}
```

This example is incomplete, and a full implementation would involve additional error handling and/or configuring Webhooks on the service and handling those. For examples of this in action (and dozens of others), see:

- [Polling Authorization Example](src/iovation.LaunchKey.Sdk.ExampleCli/ServiceExamples.cs#L117)
- [Webhook Authorization Example](src/iovation.LaunchKey.Sdk.ExampleCli/ServiceExamples.cs#L83)

## Demo Application
There is an elborate example application as a part of the source code. The demo application is documented and located [here](src/iovation.LaunchKey.Sdk.ExampleCli).

## Further Reading
As always, the best place to get information is the [LaunchKey documentation site](https://docs.launchkey.com).

