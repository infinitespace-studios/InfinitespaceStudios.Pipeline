# InfinitespaceStudios.Pipeline

This repo contains the Public Extensions for the MonoGame Content Pipeline developed by Infinitespace Studios. 
All code is under the MIT licence. 

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

1. Open your project and find the Packages Folder. Right click and select Add Packages.

    ![](https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki/images/AddPackage.png)

2. This will open the Nuget search Dialog. Search for "InfinitespaceStudios.Pipeline" and add the Package.

    ![](https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki/images/Add.png)

3. Once the package has been added. Open the Content.mgcb file in the Pipeline Editor.

    ![](https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki/images/OpenContent.png)

4. Select the "Content" node and then find the References property in the property grid. Double click the References property to bring up the Add References Dialog.

    ![](https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki/images/AddPipeline.png)

5. Search for the "InfinitespaceStudios.Pipeline.dll" and Add it by clicking on the "Add" button. Note this should be located in the "packages\InfinitespaceStudios.Pipeline.X.X.X\Tools" folder. Once that is done, Save the Content.mgcb. Close it an re open it (there is a bug in the Pipeline Tool). The select the .fx file you want to change. 

    ![](https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki/images/ChangeProcessor.png)

6. Select the Processor property and in the drop down you should see "Remote Effect Processor - Infinitespace Studios". Select this Item.

    ![](https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki/images/RemoteEffect.png)

7. If you are using the defaults just Save the Content.mcgb. Close the Pipeline tool and Build and Run you app. It should compile without any issues. If there is a problem with the .fx file the error will be reported in the build log.

    ![](https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki/images/BuildAndRun.png)

If you are using a Custom Azure site or the Local Service on a Windows box you can use the *RemoteAddress* , *RemotePort* and *Protocol* properties to change the location of the server. Valid *Protocol* values are "http" and "https" if you have a secured service. The *RemoteAddress* can be a CNAME or IP address. 
