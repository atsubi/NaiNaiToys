using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using UnityEditor;

// your custom namespace
using Toys;
// ---------------------

# if UNITY_EDITOR

/// <summary>
/// Google スプレットシートからマスターデータを取得するUnity拡張エディタ
/// </summary>
[CustomEditor(typeof(MasterDataLoader))]
public class GetMasterData : Editor {

    private CancellationTokenSource _cts = new CancellationTokenSource();

    public async override void OnInspectorGUI() {

        var masterDataLoader = target as MasterDataLoader;

        base.OnInspectorGUI();
        serializedObject.Update();

        if (GUILayout.Button("Googleスプレットシートからマスターデータを読み込み"))
        {
            var token = _cts.Token;

            try {
                Debug.Log("読み込み開始");

                string response_text = await DownloadMasterDataAsync(masterDataLoader._spreadSheetURL, token);

                ToyParamList toyParamList = JsonUtility.FromJson<ToyParamList>(response_text);
            
                var toyParamAsset = ScriptableObject.CreateInstance<ToyParamAsset>();
                toyParamAsset.name = "ToyParamAsset";
                toyParamAsset.ToyParamData = toyParamList;

                string path = "Assets/Resources/ToyParamAsset.asset";
                

                var asset = (ToyParamAsset)AssetDatabase.LoadAssetAtPath(path, typeof(ToyParamAsset));
                if (asset == null) {
                    AssetDatabase.CreateAsset(toyParamAsset, path);
                } else {
                    EditorUtility.CopySerialized(toyParamAsset, asset);
                    AssetDatabase.SaveAssets();
                }
            
                AssetDatabase.Refresh();

                Debug.Log("読み込み完了");
            }
            catch (OperationCanceledException e) {
                Debug.Log(e.Message);
            }

            return;
        }

        if (GUILayout.Button("読み込みを中断"))
        {
            _cts?.Cancel();
        }
        
    }

    /// <summary>
    /// Googleスプレットシートに保存されたデータをJSON形式で取得する
    /// </summary>
    /// <returns></returns>
    public async UniTask<string> DownloadMasterDataAsync(string spreadsheetURL, CancellationToken token)
    {
        UnityWebRequest request = UnityWebRequest.Get(spreadsheetURL);

        await request.SendWebRequest().ToUniTask(cancellationToken: token);

        if (request.result == UnityWebRequest.Result.ProtocolError 
            || request.result == UnityWebRequest.Result.ConnectionError
            || request.result == UnityWebRequest.Result.DataProcessingError) throw new Exception(request.error);

        return request.downloadHandler.text;
    }

}

# endif