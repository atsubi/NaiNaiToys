using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ToyParameter/CreateToyParameter")]
public class ToyParamAsset : ScriptableObject
{
    public List<ToyParam> ToyParamList = new List<ToyParam>();
}

[System.Serializable]
public class ToyParam
{
    

}
