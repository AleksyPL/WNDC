using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool CheckIfGameObjectFitsTheLayerMask(GameObject objectToCheck, LayerMask layerToCheck)
    {
        string layerMaskBinary = System.Convert.ToString(layerToCheck, 2).PadLeft(32, '0');
        string objectBinary = System.Convert.ToString(objectToCheck.layer, 2).PadLeft(32, '0');
        Debug.Log(layerMaskBinary);
        Debug.Log(objectBinary);
        Debug.Log(objectToCheck);
        //for (int i = 31; i > 7; i--)
        //{
        //    if (layerLayerMaskBinary[i] == '1' && objectToCheck.layer == i)
        //        return true;
        //}
        return false;
    }
}
