# Bitrate calculation exercise

Imagine the vendor Arista had a video transcoder known as X-Video. For each network interface included in X-
Video, the API returns the number of transmitted and received octets at a determined moment. Our customer

Fox is using several of that equipment and asked Skyline to add the respective bitrates, Rx and Tx, to the current
driver.

Exercise: parse the following JSON and implement a method to calculate the requested bitrates for both Rx
and Tx considering a polling rate of 2Hz.

Use the programming language at your preference. C# would be an added-value.

``` json
{
"Device": "Arista",
"Model": "X-Video",
"NIC": [{
"Description": "Linksys ABR",
"MAC": "14:91:82:3C:D6:7D",
"Timestamp": "2020-03-23T18:25:43.511Z",
"Rx": "3698574500",
"Tx": "122558800"
}]
}
```

# Solution
## Features
- .Net 8.0
- Dynamic Data Source
- Connector which can variably poll the Data Source, based on a specified frequency
- Connector results can be polled independently of device polling.
- Nic and Device `bits per second` calculation.
- Outputs to Console for simplicity—(Logging should be added via DI)
