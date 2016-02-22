# InfinitespaceStudios.Pipeline

This repo contains the Public Extensions for the MonoGame Content Pipeline developed by Infinitespace Studios. 
All code is under the MS-PL licence. 

## RemoteEffectProcessor

This processor is designed to allow Mac OS and Linux developers compile shaders on their platforms. It does this by 
using a remote service on a Windows machine to compile the shader. There are currently three options for using this 
Processor

1) Use the default service at https://pipeline.infinfitespace-studios.co.uk. This is currrently free, but hosting is not free so if this option is popular a donation might be requested.
2) Take the Azure service code and host your own :)
3) User the RemoteEffectServier to host the service on a local windows box.

Options 2) and 3) will require the *RemoteAddress* and *RemotePort* properties of the Processor to be set in order to contact the service.
