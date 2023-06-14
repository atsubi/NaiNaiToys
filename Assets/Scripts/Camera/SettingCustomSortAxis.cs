using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MyCamera {

    [ExecuteInEditMode]
    public class SettingCustomSortAxis : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var camera = GetComponent<Camera>();
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = new Vector3(0.0f, 1.0f, -0.49f);

            #if UNITY_EDITOR
            foreach (SceneView sv in SceneView.sceneViews)
            {
                sv.camera.transparencySortMode = TransparencySortMode.CustomAxis;
                sv.camera.transparencySortAxis = new Vector3(0.0F, 1.0F, -0.49F);
            }
            #endif
            
        }
    }
}