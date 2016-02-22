# InfinitespaceStudios.Pipeline

This repo contains the Public Extensions for the MonoGame Content Pipeline developed by Infinitespace Studios. 
All code is under the MS-PL licence. 

## RemoteEffectProcessor

This processor is designed to allow Mac OS and Linux developers compile shaders on their platforms. It does this by 
using a remote service on a Windows machine to compile the shader. There are currently three options for using this 
Processor

1. Use the default service at https://pipeline.infinfitespace-studios.co.uk. This is currrently free, but hosting is not free so if this option is popular a donation might be requested.
2. Take the Azure service code and host your own :)
3. User the RemoteEffectServier to host the service on a local windows box.

Options 2) and 3) will require the *RemoteAddress* , *RemotePort* and *Protocol* properties of the Processor to be set in order to contact the service.

### Using the RemoteEffectProcessor

If you want to use the *default* service

1. Add a Nuget Package . InfinitespaceStudios.Pipeline
2. Open your Content.mgcb files in the Pipeline Tool.
3. Click the Content Node and double click on the Refernce Property in the property grid.
4. Find the InfinitespaceStudios.Pipeline.dll  in `packages\InfinitespaceStudios.Pipeline.X.X.X\Tools\InfinitespaceStudios.Pipeleine.dll`
5. Select the Effect .fx file
6. Change the Processor to be  "Remote Effect Processor - Infinitespace Studios"
    (If it does not show up in the list, close and open the Content.mgcb file)
7. Go back to your app and Run.

If you need to change to your own service enter the correct details in the properties for *RemoteAddress* , *RemotePort* and *Protocol*.
For the local Server you will need to use http for the *Protocol* property. If you host your own you will need to use http unless you have your own SSL certificate.
