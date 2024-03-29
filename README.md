Challenger platform API class and examples for C#
===

In example below:

 - `your.challenger.domain` - is the domain of your Challenger implementation
 - `secret_key` - a unique key provided by Challenger to encrypt data exchange
 - `owner_id` - a unique identifier provided by Challenger. Normally used by coalitional partners (optional)
 - `client_id` - the identifier of the client performing action
 - `event_id` - the identifier of the corresponding event in Challenger platform.
 - `multiple` - for quantifiable challenges (ex. get 1 point for every 1 euro spent). Provide value to multiple points with.

## Event tracking example

This code prepares a call to Challenger server on event happened to a client identified by {client_id}:

```C#
using ChallengerPlatform;

// ... your code ...

Challenger challenger = new Challenger(DOMAIN)
            {
                ClientId = CLIENT_ID,
                Key = SECRET_KEY,
                UseHTTPS = true,
                //OwnerId = owner_id // Optional
            };
challenger.addParam("param1", "value1"); // Optional
challenger.addParam("param2", "value2"); // Optional
bool resp = Challenger.trackEvent("event");
```

## Delete client example

This code prepares a call to Challenger to delete particular client {client_id}:

```C#
using ChallengerPlatform;

// ... your code ...

Challenger challenger = new Challenger(DOMAIN)
            {
                ClientId = CLIENT_ID,
                Key = SECRET_KEY,
                UseHTTPS = true,
            };
bool resp = Challenger.deleteClient();
```

N.B. This function is not accessible for coalitional partners.

# Performance widgets

In examples below:
 - `your.challenger.domain` - is the domain of your Challenger implementation.
 - `client_id` - the identifier of the client performing action
 - `secret_key` - a unique key provided by Challenger to encrypt data exchange
 - `param1`, `param2`, ... - optional parameters to pass to the widget (For example name of the client). List of parameters Challenger can map:
   - `expiration` (in format 0000-00-00 00:00:00) - required param
   - `name`
   - `surname`
   - `email`
   - `phone`
   - `lang` (2-digit language code. I.e. "en", "es", "lt", "hr")
   - `birthday` (in format 0000-00-00)
 - `value1`, `value2`,  ... - values of optional parameters.

Using the C# helper functions provided with Challenger to get widget HTML is as easy as that:

```C#
using ChallengerPlatform;

// ... your code ...

Challenger challenger = new Challenger(DOMAIN)
            {
                ClientId = CLIENT_ID,
                Key = SECRET_KEY,
                UseHTTPS = true,
                //OwnerId = owner_id // Optional
            };
Challenger.addParam("expiration", "0000-00-00 00:00:00"); // Required
Challenger.addParam("name", "John"); // Optional
Challenger.addParam("surname", "Smith"); // Optional
Challenger.addParam("lang", "en"); // Optional
Challenger.addParam("param1", "value1"); // Optional
Challenger.addParam("param2", "value2"); // Optional

// Option A: Get a widget HTML generated on server
try{
   string resp = Challenger.getWidgetHtml(); // Returns HTML snippet
}catch(Exception ex){
   // Error happened.
}

// Option B: Get an URL of the widget generated on server 
try{
   String widgetUrl = Challenger.getWidgetUrl(); // Returns URL
}catch(Exception ex){
   // Error happened.
}

// Option C: Get and encrypted token to authorize the user and draw the widget on client-side
// For locally drawn widgets `getEncryptedData()` method could be used instead of `getWidgetHtml()`. Please refer:
// https://github.com/challenger-platform/challenger-widget#get-apiwidgetauthenticateuser for more information
string encryptedData = Challenger.getEncryptedData();
```

N.B. This function is not accessible for coalitional partners.
