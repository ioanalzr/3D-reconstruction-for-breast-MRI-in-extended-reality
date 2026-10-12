# 3D-reconstruction-for-breast-MRI-in-extended-reality (in development)

**Windows Application for building a 3D breast MRI hologram from DICOM slices which you can interact with in Mixed Reality using Microsoft HoloLens 2.**

## Acknowledgements 
- Volume rendering and DICOM import are based on **[UnityVolumeRendering](https://github.com/mlavik1/UnityVolumeRendering)** by Matias Lavik (MIT License, see below).
- Mixed reality interaction uses the **[Mixed Reality Toolkit](https://github.com/microsoft/MixedRealityToolkit-Unity)** and the **Mixed Reality OpenXR Plugin** by Microsoft.
- `ConnectHololens.cs` is adapted from Microsoft's OpenXR remoting sample.

## Capabilties
- **Importing DICOM series.** Load folder containing the `.dcm` / `.dicom` / `.dicm` files which will be stacked into a 3D volume.
- **Volume rendering**, with three modes:
  - Direct Volume Rendering
  - **Maximum Intensity Projection** (recommended)
  - Isosurface rendering
- **Interaction with the hologram** The hologram is an MRTK object, so with HoloLens you can grab, move, rotate and scale it with your hands, as well as take notes.
- **Measuring distances** between two points clicked on the volume.
- **Saving and restoring hologram manipulations.** It stores the hologram's position, rotation and scale in a .json file for each dataset, so you can reopen a case where you left it.


The app is intended to be used on Windows, so that the datasets is accessed and the 3D Rendering is computed on the computer. At the moment, this is done by using the app in Unity in PlayMode (https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/preview-and-debug-your-app?tabs=openxr).

The app starts on the MainMenu Scene, and then opens the Test Scene which integrates the MRTK environment. The RunTime Graphic User Interface can be used from the computer, and the hologram is displayed in Mixed Reality once the object is created. Live streaming presents lagging at times. 


The app is intended to be used as a standalone app which communicates with the Hololens, however this is still in development (https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/use-pc-resources).

## Troubleshooting

The Mixed Reality Toolkit might have to be installed and imported to the project again from the Mixed Reality Feature Tool (https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/welcome-to-mr-feature-tool)

The WSAT Test might have to be deleted from Edit>Project Settings>Player.
The initial parameters of the hologram can be changed in VolumeObjectFactory, meshcontainer.transform.

In case the master branch presents additional errors, branch Ioana should have a final functional version.

<details>
<summary>UnityVolumeRendering licence (MIT)</summary>

```
MIT License

Copyright (c) 2019 Matias Lavik

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
```

</details>



