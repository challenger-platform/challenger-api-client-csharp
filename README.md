Challenger platform API class and examples for C#
===

In example below:

 - `your.challenger.domain` - is the domain of your Challenger implementation
 - `secret_key` - a unique key provided by Challenger to encrypt data exchange
 - `owner_id` - a unique identifier provided by Challenger (optional)
 - `client_id` - the identifier of the client performing action
 - `event_id` - the identifier of the corresponding event in Challenger platform.
 - `multiple` - for quantifiable challenges (ex. get 1 point for every 1 euro spent). Provide value to multiple points with.

## Event tracking example

This code prepares a call to Challenger server on event happened to a client identified by {client_id}:

```C#
using ChallengerPlatform;

// ... your code ...

Challenger.setServer('{your.challenger.domain}');
Challenger.setOwnerId('{owner_id}'); // Optional
Challenger.setClientId('{client_id}');
Challenger.setKey('{secret_key}');
Challenger.addParam('multiple', '{multiple}'); // Optional
bool resp = Challenger.trackEvent('{event}');
```

## Delete client example

This code prepares a call to Challenger to delete particular client {client_id}:

```C#
using ChallengerPlatform;

// ... your code ...

Challenger.setServer('{your.challenger.domain}');
Challenger.setClientId('{client_id}');
Challenger.setKey('{secret_key}');
bool resp = Challenger.deleteClient();
```

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
   - `birthday` (in format 0000-00-00)
 - `value1`, `value2`,  ... - values of optional parameters.

## Web version

Using the C# helper functions provided with Challenger to get widget HTML is as easy as that:

```C#
using ChallengerPlatform;

// ... your code ...

Challenger.setServer('{your.challenger.domain}');
Challenger.setClientId('{client_id}');
Challenger.setKey('{secret_key}');
Challenger.addParam('expiration', '0000-00-00 00:00:00'); // Required
Challenger.addParam('name', 'John'); // Optional
Challenger.addParam('surname', 'Smith'); // Optional
Challenger.addParam('{param1}', '{value1}'); // Optional
Challenger.addParam('{param2}', '{value2}'); // Optional

try{
   string resp = Challenger.getWidgetHtml(); // Return HTML snippet
}catch(Exception ex){
   // Error happened.
}
```

## Mobile app version

This code creates an encrypted URL for mobile ready widget. It should be passed to mobile app and opened in WebView.

```C#
using ChallengerPlatform;

// ... your code ...

Challenger.setServer('{your.challenger.domain}');
Challenger.setClientId('{client_id}');
Challenger.setKey('{secret_key}');
Challenger.addParam('expiration', '0000-00-00 00:00:00'); // Required
Challenger.addParam('{param1}', '{value1}');
Challenger.addParam('{param2}', '{value2}');
Challenger.addParam('mobile', true); // Pass it to get mobile version of the widget

try{
   String widgetUrl = Challenger.getWidgetUrl(); // Return HTML snippet
}catch(Exception ex){
   // Error happened.
}
```
