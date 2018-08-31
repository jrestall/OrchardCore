# WebHooks (OrchardCore.WebHooks)

Webhooks provide a way for external services to receive information about certain events as soon as they happen in your Orchard site. They allow you to extend and integrate your site with other applications around the web.

When the specified events happen we’ll send a POST request to each of the URLs you have configured. This allows external sites to be notified in near real-time when events such as a new blog post are published within your site.

# List of Supported Webhook Events
Right now webhooks can be registered for the following events:

| Model          | Event                      | Trigger                                 |
|----------------|----------------------------|-----------------------------------------|
| Content        | content.created            | When any content item is created.       |
|                | content.updated            | When any content item is updated.       |
|                | content.removed            | When any content item is removed.       |
|                | content.published          | When any content item is published.     |
|                | content.unpublished        | When any content item is unpublished.   |
| {Content Type} | {content type}.created     | When any {content type} is created.     |
|                | {content type}.updated     | When any {content type} is updated.     |
|                | {content type}.removed     | When any {content type} is removed.     |
|                | {content type}.published   | When any {content type} is published.   |
|                | {content type}.unpublished | When any {content type} is unpublished. |
| Asset          | asset.created              | When any media asset is created.        |
|                | asset.updated              | When any media asset is updated.        |
|                | asset.removed              | When any media asset is removed.        |