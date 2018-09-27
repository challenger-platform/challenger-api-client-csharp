Challenger platform API class and examples for C#
===

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
## Web version

Using the Java helper functions provided with Challenger to get widget HTML is as easy as that:

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
