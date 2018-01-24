# LaunchKey.NET CLI Example

This example application demonstrates most, if not all, functions of the LaunchKey.NET SDK. This example is meant to serve as a useful test tool and a repository of example usages of the SDK itself. Verifying your own code against this example is a good troubleshooting step.

## The Command Line Interface
The application itself is a fleshed command line application with numerous useful actions. There are too many to list here, but the help screen is a great place to start:

```
  service-auth                       Authorize a user against a service using a polling method

  service-auth-webhook               Authorize a user against a service using the Webhook method.

  service-auth-policy                Authorize a user against a service with an auth policy

  service-session-start              Start a session for a user

  service-session-end                End a session for a user

  directory-device-link              Link a device for a directory user

  directory-device-unlink            Unlink a device for a directory user

  directory-device-list              List all devices linked to a directory user

  directory-session-list             List all active sessions for a given user within a directory

  directory-session-purge            End all sessions across all child services for a directory user

  directory-service-auth             Authenticate a directory user against a directory service

  directory-service-session-start    Start a session for a directory user against a directory service

  directory-service-session-end      End a session for a directory user against a directory service

  org-directory-device-list          (Using Organization Credentials) List all devices linked to a directory user

  org-service-auth                   (Using Organization Credentials) Authenticate a directory user against a service

```

For example, if I wanted to test authorizing a user against a service, I would need 3 pieces of information:

- The Service I wish to authorize against (**Service ID**, a GUID)
- An RSA private key issued from the LaunchKey Admin Center
- The username I wish to authorize

In action:

```
example service-auth -u johnhargrove -p key-service.txt -s f8329e0e-dd7a-11e7-98b4-469158467b1a
```

This reads as "authenticate user johnhargrove against the service identified by f8329e0e-dd7a-11e7-98b4-469158467b1a. Use the RSA private key stored in `key-service.txt` when authenticating to the API


## The Code Itself
Examples are separated into 3 files:

- `ServiceExamples.cs`: Examples that use the 'Service' endpoints, authenticating using Service credentials.
- `DirectoryExamples.cs`: Examples that use the 'Service' and 'Directory' endpoints, authenticating using Directory credentials. Service examples contained here are all related to LaunchKey Services which are *children* of LaunchKey directories.
- `OrgExamples.cs`: A small number of examples that demonstrate using Organization-level credentials to perform operations on the 'Service' and 'Directory' endpoints. Using Organization-level credentials allows the SDK to interface with all services, directories and directory services within an organization.

## Building
The source code for this example is a part of the LaunchKey.NET SDK's main source base. To build the example, simply clone the launchkey-dotnet repo and build the entire solution.