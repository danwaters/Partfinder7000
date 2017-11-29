# Part Finder 7000
Part Finder 7000 is an example iOS app that finds parts using Azure Cognitive Services (Custom Vision), Azure Search, Azure Blob Storage and Xamarin.Forms.

This is a visual search app. The user uses the camera to automatically identify an object ("a part"), and is then redirected to a list of relevant information about that part. The toggle switch, when off, returns a Bing search (see 1st gif). When the toggle switch is on, it uses Azure Search to return data from documents in Blob Storage with text matching the part number (see 2nd gif).

<img align="left" src="https://user-images.githubusercontent.com/780735/33389087-65fb60a6-d4f7-11e7-99fd-99ec574cd4ab.gif" width="200"/>
<img src="https://user-images.githubusercontent.com/780735/33389101-6f05392e-d4f7-11e7-86cd-0f4df2e421fc.gif" width="200"/>

<br/><br/>

## Simple Architecture
The simplest form of this app performs the following tasks:

1. Captures an image from the device as a byte array.
2. POSTs the byte array to the Custom Vision API, which returns a list of predicted "tags" from the Custom Vision project.
3. Searches Bing for the top tag, which is the part number.

![Simple Architecture Bing Search for Identified Part](https://user-images.githubusercontent.com/780735/33388067-81ff7164-d4f4-11e7-9b6b-cd8a83f77b0d.png)

## Advanced Architecture
The next architecture builds upon the last and adds two components: Azure Search and Azure Blob Storage.

The blob storage holds documents (.docx, .pdf) and Azure Search indexes these blobs based on their content and title. This enables documents in the blob storage to be searchable and retrievable based on the tag retrieved from the custom vision service.

![Enterprise Visual Search Architecture](https://user-images.githubusercontent.com/780735/33388068-820d2cc8-d4f4-11e7-8959-ab0a3eaef0ea.png)
