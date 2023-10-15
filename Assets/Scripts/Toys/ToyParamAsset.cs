using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toys {

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ToyParameter/CreateToyParameter")]
    public class ToyParamAsset : ScriptableObject
    {
        public ToyParamList ToyParamData;
    }

    [System.Serializable]
    public class ToyParamList
    {
        public List<ToyParam> ToyParams;
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ToyParam
    {
        [SerializeField]
        public int id;

        [SerializeField]
        public string name;

        [SerializeField]
        public float weight;

        [SerializeField]
        public int point;

        [SerializeField]
        public float angerCareRate;

        [SerializeField]
        public float appearanceRate;
    }

}
