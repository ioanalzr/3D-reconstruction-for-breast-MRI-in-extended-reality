/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveModel : MonoBehaviour
{

    GameObject[] objects = UnityEngine.Object.FindObjectOfType<GameObject>(); // object that you want to save (get object with 3D model Tag
        foreach (GameObject go in objects)
            if(go.tag=="3Dmodel"&&go.activeInHierarchy)
                new GameObject prefab = go;
    public string savePath = @"C:\Users\Razvan\Desktop\git\SavedModels"  ; // define file location
    

    public void Save()
    {
        GameObjectData data = new GameObjectData();
        data.position = prefab.transform.position;
        data.rotation = prefab.transform.rotation;
        data.scale = prefab.transform.localScale;

        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(savePath, json);


    }
    /*
    public void Load()
    {
        // GameObject loadedmodel = object that will be modified
        string json = File.ReadAllText(savePath);
        GameObjectData data = JsonConvert.DeserializeObject(json);
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = data.position;
        newObject.transform.rotation = data.rotation;
        newObject.transform.localScale = data.scale;
    
/*

    

    public class GameObjectData 
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public string notes;

        
    }
}
*/