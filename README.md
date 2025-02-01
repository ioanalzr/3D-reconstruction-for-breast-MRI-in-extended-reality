# 3D-reconstruction-for-breast-MRI-in-extended-reality - in development

Windows Application for building a 3D breast MRI hologram from DICOM files and visualising it in mixed reality using Microsoft Hololens.

Developed in Unity, with the Mixed Reality toolkit.

Unity Editor Version used: 2021.3.16f1. Other versions might create errors.

Volume rendering from 2D MRI scans is based on the opensource Unit Volume Rendering Project- https://github.com/mlavik1/UnityVolumeRendering .

There are 3 Rendering Modes, we suggest using the Maximum Intensity Projection.

The app is intended to be used on Windows, so that the datasets is accessed and the 3D Rendering is computed on the computer.  
At the moment, this is done by using the app in Unity in PlayMode. This is set up as presented here: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/preview-and-debug-your-app?tabs=openxr . The app starts on the MainMenu Scene, and then opens the Test Scene which integrates the MRTK environment. The RunTime Graphic User Interface can be used from the computer, and the hologram is displayed in Mixed Reality once the object is created. 
The livestreaming presents lagging at times. 


The app is intended to be used as a standalone app which communicates with the Hololens, however this is still in development. https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/use-pc-resources

Possible Errors at build:

The Mixed Reality Toolkit might have to be installed and imported to the project again from the Mixed Reality Feature Tool.https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/welcome-to-mr-feature-tool

The WSAT Test might have to be deleted from Edit>Project Settings>Player.
The initial parameters of the hologram can be changed in VolumeObjectFactory, meshcontainer.transform.


In case the master branch presents additional errors, branch Ioana should have a final functional version.



