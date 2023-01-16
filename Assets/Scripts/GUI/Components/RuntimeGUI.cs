
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.MixedReality.OpenXR.RemotingSample;
namespace UnityVolumeRendering
{


    /// <summary>
    /// This is a basic runtime GUI, that can be used during play mode.
    /// You can import a DICOM dataset, and edit it.
    /// </summary>
    public class RuntimeGUI : MonoBehaviour
    {
        public string savedPath; // when a folder will be selected for rendering, the path will be saved and modified for saving
        AppRemoting newobj = new AppRemoting();
        private void OnGUI()
        {
            GUILayout.BeginVertical();


            // for using PC resources to power the app with Holographic Remoting remote app
            /*
            if (GUILayout.Button("Connect to HoloLens"))
            {
                newobj.ConnectToRemote();
            }
            */
            if (GUILayout.Button("Start New Model"))
            {
                RuntimeFileBrowser.ShowOpenDirectoryDialog(OnOpenDICOMDatasetResult);

            }
            if (GUILayout.Button("Load existing Model"))
            {
                RuntimeFileBrowser.ShowOpenDirectoryDialog(LoadOnOpenDICOMDatasetResult);
            }


            if (GUILayout.Button("Back to Main Menu"))
            {
                DespawnAllDatasets();
                LoadScene("MainMenu");
            }

            // Show button for opening the dataset editor (for changing the visualisation)
            if (GameObject.FindObjectOfType<VolumeRenderedObject>() != null && GUILayout.Button("Edit imported dataset"))
            {
                EditVolumeGUI.ShowWindow(GameObject.FindObjectOfType<VolumeRenderedObject>());
            }


            if (GUILayout.Button("Show distance measure tool"))
            {
                DistanceMeasureTool.ShowWindow();
            }
            if (GUILayout.Button("Save Model"))
                Save();

            GUILayout.EndVertical();
        }




        public void Save()
        {


            GameObject objectToSave = new GameObject();
            GameObject[] objects =
            UnityEngine.Object.FindObjectsOfType<GameObject>(); // object that you want to save (get object with 3D model Tag
            foreach (GameObject go in objects)
            {
                if (go.tag == "3Dmodel" && go.activeInHierarchy)
                {
                    objectToSave = go;
                    break;
                }

            }


            savedData data = new savedData();
            data.position = objectToSave.transform.localPosition;
            data.rotation = objectToSave.transform.localRotation;
            data.scale = objectToSave.transform.localScale;


            string finalPath = Application.persistentDataPath + @"\" + Path.GetFileName(savedPath) + ".txt";
            string serData = JsonUtility.ToJson(data);
            File.WriteAllText(finalPath, serData);
            //Debug.Log(finalPath);
        }


        public void LoadScene(string SceneToLoad)
        {
            SceneManager.LoadScene(SceneToLoad);
        }

        private void OnOpenDICOMDatasetResult(RuntimeFileBrowser.DialogResult result)
        {
            if (!result.cancelled)
            {
                savedPath = result.path;
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

                        VolumeRenderedObject obj = VolumeObjectFactory.CreateObject(dataset, "new");


                        obj.transform.position = new Vector3(numVolumesCreated, 0, 0);
                        numVolumesCreated++;
                    }
                }
            }
        }
        // 
        private void LoadOnOpenDICOMDatasetResult(RuntimeFileBrowser.DialogResult result)
        {
            if (!result.cancelled)
            {
                savedPath = result.path;
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
                        VolumeRenderedObject obj = VolumeObjectFactory.CreateObject(dataset, "old", result.path);
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

