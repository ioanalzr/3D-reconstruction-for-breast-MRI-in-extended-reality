
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityVolumeRendering
{
    /// <summary>
    /// This is a basic runtime GUI, that can be used during play mode.
    /// You can import datasets, and edit them.
    /// Add this component to an empty GameObject in your scene (it's already in the test scene) and click play to see the GUI.
    /// </summary>
    public class RuntimeGUI : MonoBehaviour
    {
        private void OnGUI()
        {
            GUILayout.BeginVertical();


            // Show dataset import buttons


            if (GUILayout.Button("Import DICOM dataset"))
            {
                RuntimeFileBrowser.ShowOpenDirectoryDialog(OnOpenDICOMDatasetResult);
            }


            if (GUILayout.Button("Back to Main Menu"))
            {
                LoadScene("MainMenu");
            }

            // Show button for opening the dataset editor (for changing the visualisation)
            if (GameObject.FindObjectOfType<VolumeRenderedObject>() != null && GUILayout.Button("Edit imported dataset"))
            {
                EditVolumeGUI.ShowWindow(GameObject.FindObjectOfType<VolumeRenderedObject>());
            }

            // Show button for opening the slicing plane editor (for changing the orientation and position)
            if (GameObject.FindObjectOfType<SlicingPlane>() != null && GUILayout.Button("Edit slicing plane"))
            {
                EditSliceGUI.ShowWindow(GameObject.FindObjectOfType<SlicingPlane>());
            }

            if (GUILayout.Button("Show distance measure tool"))
            {
                DistanceMeasureTool.ShowWindow();
            }

            GUILayout.EndVertical();
        }
        

    public void LoadScene(string SceneToLoad)
        {
            SceneManager.LoadScene(SceneToLoad);
        }

       private void OnOpenDICOMDatasetResult(RuntimeFileBrowser.DialogResult result)
        {
            if (!result.cancelled)
            {
                // We'll only allow one dataset at a time in the runtime GUI (for simplicity)
                DespawnAllDatasets();

                bool recursive = true;

                // Read all files
                IEnumerable<string> fileCandidates = Directory.EnumerateFiles(result.path, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    .Where(p => p.EndsWith(".dcm", StringComparison.InvariantCultureIgnoreCase) || p.EndsWith(".dicom", StringComparison.InvariantCultureIgnoreCase) || p.EndsWith(".dicm", StringComparison.InvariantCultureIgnoreCase));

                // Import the dataset
                IImageSequenceImporter importer = ImporterFactory.CreateImageSequenceImporter(ImageSequenceFormat.DICOM);
                IEnumerable<IImageSequenceSeries> seriesList = importer.LoadSeries(fileCandidates);
                float numVolumesCreated = 0;
                foreach (IImageSequenceSeries series in seriesList)
                {
                    VolumeDataset dataset = importer.ImportSeries(series);
                    // Spawn the object
                    if (dataset != null)
                    {
                        VolumeRenderedObject obj = VolumeObjectFactory.CreateObject(dataset);
                        obj.transform.position = new Vector3(numVolumesCreated, 0, 0);
                        numVolumesCreated++;
                    }
                }
            }
        }

        private void DespawnAllDatasets()
        {
            VolumeRenderedObject[] volobjs = GameObject.FindObjectsOfType<VolumeRenderedObject>();
            foreach (VolumeRenderedObject volobj in volobjs)
            {
                GameObject.Destroy(volobj.gameObject);
            }
        }
    }
}

