using System.Collections.Generic;
using UnityEngine;


public class Crystal : MonoBehaviour
{
    public List<GameObject> CrystalModels = new List<GameObject>();
    public GameObject ActiveModel;
    
    void Update()
    {
        ActiveModel.transform.Rotate(Vector3.up * Time.deltaTime * 50);
    }

    public void SetModel()
    {
        int Index = Random.Range(0, CrystalModels.Count);
        CrystalModels[Index].SetActive(true);
        ActiveModel = CrystalModels[Index];
    }
   
}
