
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


namespace UnityVolumeRendering
{


    /// <summary>
    /// This is a basic runtime GUI, that can be used during play mode.
    /// You can import datasets, and edit them.
    /// Add this component to an empty GameObject in your scene (it's already in the test scene) and click play to see the GUI.
    /// </summary>
    public class RuntimeGUI : MonoBehaviour
    {
        public string savedPath;

        //private void Start()

        /*GameObject[] objects =
        UnityEngine.Object.FindObjectsOfType<GameObject>(); // object that you want to save (get object with 3D model Tag
        if (objects[0].tag == "3Dmodel" && objects[0].activeInHierarchy)
        {
            prefab = objects[0];
        }
    }*/

        private void OnGUI()
        {
            GUILayout.BeginVertical();


            // Show dataset import buttons


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

            // Show button for opening the slicing plane editor (for changing the orientation and position)
            if (GameObject.FindObjectOfType<SlicingPlane>() != null && GUILayout.Button("Edit slicing plane"))
            {
                EditSliceGUI.ShowWindow(GameObject.FindObjectOfType<SlicingPlane>());
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
                
                IFormatter formatter = new BinaryFormatter();
               //Path.Combine(Application.persistentDataPathPath.GetFileName)
                string finalPath = Application.persistentDataPath + @"\" + Path.GetFileName(savedPath) + ".txt";
                //using (FileStream fs = File.Create(finalPath))
                //  {
                //formatter.Serialize(fs, data);
                //} 
             string json = JsonUtility.ToJson(data);
             File.WriteAllText(finalPath, json);
            Debug.Log(finalPath);
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

                            VolumeRenderedObject obj = VolumeObjectFactory.CreateObject(dataset,"new");


                            obj.transform.position = new Vector3(numVolumesCreated, 0, 0);
                            numVolumesCreated++;
                        }
                    }
                }
            }
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

