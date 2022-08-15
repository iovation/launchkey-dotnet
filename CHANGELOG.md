Change Log
----------
4.1.1
=====
* Updated Newtonsoft.Json to remediate CWE-755: [handle improper handling of exceptional conditions](https://github.com/advisories/GHSA-5crp-9r3c-p9vr)

4.1.0
=====
* Added PublicKey Key Types for differentiating between keys that can encrypt, sign or perform both actions.
* Added the capability for separate Encryption and Signature keys for entities as an enabler for FIPS 140-3
* Added totp functionality

4.0.0
=====
* Removed support for .NET 4.0 and bumped minimum to 4.5.0
* Added SDK version tracking user agent

3.7.1
=====
* Fixed bug where cache-control headers were not properly compared due to limitation in System.Web.HttpWebResponse

3.7.0
=====
* Added DeviceID Parsing to Authorization Request

3.6.0
=====
* Added new Policy objects
    * MethodAmountPolicy
    * FactorsPolicy
    * LegacyPolicy
    * ConditionalGeofencePolicy
* Added new Fence objects:
    * TerritoryFence
    * GeoCircleFence
* Added new client methods:
    * SetAdvancedServicePolicy
    * GetAdvancedServicePolicy
    * GetAdvancedAuthorizationResponse
* Added new Webhook method:
    * GetAdvancedWebhook
* Bugfix concerning LaunchKey API key rotation
* Deprecated:
    * ServicePolicy
    * SetServicePolicy
    * GetServicePolicy
    * GetWebhook
    * Location

3.5.1
=====
* Added DenialContextInquiryEnabled to Directory management
    * Allows setting whether user can provide a reason for denying the Authentication Request

3.5.0
=====
* Added Auth Method Insight
    * Returns auth method state at the time of authorization
    * Returns the auth policy at the time of authorization
    
* Added Device Link Completion Webhook
    * Added webhookUrl to UpdateDirectory method
    * Added HandleWebhook to directory client
    * Added DeviceLinkCompletionResponse
    
* Added device integration testing   

* Refactored example CLI app to handle webhooks

* Deprecated HandleWebhook method with two params in directory and service clients

* Added TraceListener for runtime warnings

3.4.0
=====

* Added dynamic linking code TTL functionality

* Added authorization request cancel functionality to the Service client

3.3.0
=====

* Added dynamic auth TTL and title functionality

* Added dynamic auth push message body and title functionality

* Added auth busy signal error handling

* Added new auth response format

* Added auth denial context functionality
