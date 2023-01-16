using System;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UnityVolumeRendering
{
    [Serializable]
    public class savedData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public string notes;

    }
    public class VolumeObjectFactory
    {
        public static VolumeRenderedObject CreateObject(VolumeDataset dataset,string state="", string path="")
        {
            
            GameObject outerObject = new GameObject("VolumeRenderedObject_" + dataset.datasetName);
            //outerObject.tag = "3Dmodel";                                        
            VolumeRenderedObject volObj = outerObject.AddComponent<VolumeRenderedObject>();
            //volObj.tag = "3Dmodel";
            GameObject meshContainer = GameObject.Instantiate((GameObject)Resources.Load("VolumeContainer"));
            meshContainer.tag = "3Dmodel";
            
            if (state == "old")
            {
                
                string finalPath = Application.persistentDataPath + @"\" + Path.GetFileName(path) + ".txt";
                savedData datacontainer = getdata(finalPath);
                meshContainer.transform.parent = outerObject.transform;
                meshContainer.transform.localScale = datacontainer.scale;//
                meshContainer.transform.localPosition = datacontainer.position; //
                meshContainer.transform.parent = outerObject.transform;
                outerObject.transform.localRotation = datacontainer.rotation; //
            }
            else
            {
                meshContainer.transform.parent = outerObject.transform;
                meshContainer.transform.localScale = new Vector3(0.7, 0.7, 0.7);
                meshContainer.transform.localPosition = new Vector3(0, 1, 0);
                meshContainer.transform.parent = outerObject.transform;
                outerObject.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            }

            MeshRenderer meshRenderer = meshContainer.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
            volObj.meshRenderer = meshRenderer;
            volObj.dataset = dataset;

            const int noiseDimX = 512;
            const int noiseDimY = 512;
            Texture2D noiseTexture = NoiseTextureGenerator.GenerateNoiseTexture(noiseDimX, noiseDimY);

            TransferFunction tf = TransferFunctionDatabase.CreateTransferFunction();
            Texture2D tfTexture = tf.GetTexture();
            volObj.transferFunction = tf;

            TransferFunction2D tf2D = TransferFunctionDatabase.CreateTransferFunction2D();
            volObj.transferFunction2D = tf2D;

            meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
            meshRenderer.sharedMaterial.SetTexture("_GradientTex", null);
            meshRenderer.sharedMaterial.SetTexture("_NoiseTex", noiseTexture);
            meshRenderer.sharedMaterial.SetTexture("_TFTex", tfTexture);

            meshRenderer.sharedMaterial.EnableKeyword("MODE_DVR");
            meshRenderer.sharedMaterial.DisableKeyword("MODE_MIP"); // tried to set this to enable to make it default render mode - didn't work
            meshRenderer.sharedMaterial.DisableKeyword("MODE_SURF");

            if(dataset.scaleX != 0.0f && dataset.scaleY != 0.0f && dataset.scaleZ != 0.0f)
            {
                float maxScale = Mathf.Max(dataset.scaleX, dataset.scaleY, dataset.scaleZ);
                volObj.transform.localScale = new Vector3(dataset.scaleX / maxScale, dataset.scaleY / maxScale, dataset.scaleZ / maxScale);
            }
            meshContainer.AddComponent<ConstraintManager>();
            meshContainer.AddComponent<ObjectManipulator>();
            meshContainer.AddComponent<BoxCollider>();
            meshContainer.AddComponent<NearInteractionGrabbable>();
            
            return volObj;
        }
        /*
        public static VolumeRenderedObject LoadObject(VolumeDataset dataset, string path)
        {
            string finalPath = Application.persistentDataPath + @"\" + Path.GetFileName(path) + ".txt";
            savedData datacontainer = getdata(finalPath);

            GameObject outerObject = new GameObject("VolumeRenderedObject_" + dataset.datasetName);
            //outerObject.tag = "3Dmodel";
            VolumeRenderedObject volObj = outerObject.AddComponent<VolumeRenderedObject>();
            GameObject meshContainer = GameObject.Instantiate((GameObject)Resources.Load("VolumeContainer"));
            meshContainer.transform.parent = outerObject.transform; 
            meshContainer.transform.localScale = datacontainer.scale;//
            meshContainer.transform.localPosition = datacontainer.position; //
           // meshContainer.transform.parent = outerObject.transform;
            outerObject.transform.localRotation = datacontainer.rotation; //

            MeshRenderer meshRenderer = meshContainer.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
            volObj.meshRenderer = meshRenderer;
            volObj.dataset = dataset;

            const int noiseDimX = 512;
            const int noiseDimY = 512;
            Texture2D noiseTexture = NoiseTextureGenerator.GenerateNoiseTexture(noiseDimX, noiseDimY);

            TransferFunction tf = TransferFunctionDatabase.CreateTransferFunction();
            Texture2D tfTexture = tf.GetTexture();
            volObj.transferFunction = tf;

            TransferFunction2D tf2D = TransferFunctionDatabase.CreateTransferFunction2D();
            volObj.transferFunction2D = tf2D;

            meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
            meshRenderer.sharedMaterial.SetTexture("_GradientTex", null);
            meshRenderer.sharedMaterial.SetTexture("_NoiseTex", noiseTexture);
            meshRenderer.sharedMaterial.SetTexture("_TFTex", tfTexture);

            meshRenderer.sharedMaterial.EnableKeyword("MODE_DVR");
            meshRenderer.sharedMaterial.DisableKeyword("MODE_MIP"); // tired to set this to enable to make it default render mode - didn't work
            meshRenderer.sharedMaterial.DisableKeyword("MODE_SURF");

            if (dataset.scaleX != 0.0f && dataset.scaleY != 0.0f && dataset.scaleZ != 0.0f)
            {
                float maxScale = Mathf.Max(dataset.scaleX, dataset.scaleY, dataset.scaleZ);
                volObj.transform.localScale = new Vector3(dataset.scaleX / maxScale, dataset.scaleY / maxScale, dataset.scaleZ / maxScale);
            }
            meshContainer.AddComponent<ConstraintManager>();
            meshContainer.AddComponent<ObjectManipulator>();
            meshContainer.AddComponent<NearInteractionGrabbable>();
            meshContainer.AddComponent<BoxCollider>();
            return volObj;
        }
        */
        public static savedData getdata(string path)
        {
            string json = File.ReadAllText(path);
            Debug.Log(json);
            savedData data = JsonUtility.FromJson<savedData>(json);
            return data; 
         }



        public static void SpawnCrossSectionPlane(VolumeRenderedObject volobj)
        {
            GameObject quad = GameObject.Instantiate((GameObject)Resources.Load("CrossSectionPlane"));
            quad.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 0.0f);
            CrossSectionPlane csplane = quad.gameObject.GetComponent<CrossSectionPlane>();
            csplane.SetTargetObject(volobj);
            quad.transform.position = volobj.transform.position;

#if UNITY_EDITOR
            UnityEditor.Selection.objects = new UnityEngine.Object[] { quad };
#endif
        }

        public static void SpawnCutoutBox(VolumeRenderedObject volobj)
        {
            GameObject obj = GameObject.Instantiate((GameObject)Resources.Load("CutoutBox"));
            obj.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 0.0f);
            CutoutBox cbox = obj.gameObject.GetComponent<CutoutBox>();
            cbox.SetTargetObject(volobj);
            obj.transform.position = volobj.transform.position;

#if UNITY_EDITOR
            UnityEditor.Selection.objects = new UnityEngine.Object[] { obj };
#endif
        }
    }
}
