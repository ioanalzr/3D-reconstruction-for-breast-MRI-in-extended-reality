# 3D-reconstruction-for-breast-MRI-in-extended-reality - in development

Windows Application for building a 3D breast MRI hologram from DICOM files and visualising it in mixed reality using Microsoft Hololens.

Developed in Unity, making use of the Mixed Reality toolkit.

Unity Editor Version used: 2021.3.16f1. Other versions might create errors.

Volume rendering from 2D MRI scans is based on the opensource Unit Volume Rendering Project- https://github.com/mlavik1/UnityVolumeRendering [Referenced at the end]


The app is intended to be used on Windows, so that the datasets can be accessed and the 3D Rendering is computed on the computer.  
At the moment, this is done by using the app in Unity in PlayMode. This is set up as presented here: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/preview-and-debug-your-app?tabs=openxr . The app starts on the MainMenu Scene, and then opens the Test Scene which integrates the MRTK environment. The RunTime Graphic User Interface can be used from the computer, and the hologram is displayed in Mixed Reality once the object is created. 
The livestreaming presents lagging at time. 


The app is intended to be used as a standalone app which communicates with the Hololens, however this is still in development. https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/use-pc-resources

Possible Errors at build:

The Mixed Reality Toolkit might have to be installed and imported to the project again from the Mixed Reality Feature Tool.https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/welcome-to-mr-feature-tool

The WSAT Test might have to be deleted from Edit>Project Settings>Player.

MIT License

Copyright (c) [year] [fullname]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.





