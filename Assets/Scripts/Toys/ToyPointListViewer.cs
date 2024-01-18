using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

using UniRx;
using TMPro;

namespace Toys {

    public class ToyPointListViewer : MonoBehaviour
    {
        [SerializeField]
        private GameObject ToyPointlListPanel;

        [SerializeField]
        private GameObject PointUI;

        private List<GameObject> _pointUIList = new List<GameObject>();

        private ToySpriteLoader _toySpriteLoader;
        private ToyRepository _toyRepository;

        [Inject]
        void Construct(ToySpriteLoader toySpriteLoader, ToyRepository toyRepository)
        {
            _toySpriteLoader = toySpriteLoader;
            _toyRepository = toyRepository;
        }
        
        async void Start()
        {
            // おもちゃのスプライトを読み込むまで待機
            await _toySpriteLoader.CompleteToySpriteLoading;

            // おもちゃ毎のポイントリストを更新
            int numberOfToyType = _toyRepository.GetNumOfToyType();
            for (int i=0; i<numberOfToyType; i++) {
                GameObject obj = Instantiate(PointUI, Vector3.zero, Quaternion.identity, ToyPointlListPanel.transform);

                Image objImage = obj.transform.GetChild(0).GetComponent<Image>();
                objImage.sprite = _toySpriteLoader.GetToySprite(i);

                TextMeshProUGUI objText = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                objText.text = "+" + _toyRepository.GetToyPoint(i).ToString();
                _pointUIList.Add(obj);
            }
        }

    }
}