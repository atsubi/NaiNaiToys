using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using UniRx.Triggers;

using VContainer;
using VContainer.Unity;


namespace Strength {

    public class StrengthViewer : MonoBehaviour
    {
        /// <summary>
        /// 追従するオブジェクトのTransform
        /// </summary>
        [SerializeField]
        private Transform _targetTransform;

        [SerializeField]
        private Vector3 _offset = new Vector3(1.0f, 1.0f, 0.0f);

        // ストレングスゲージの残量イメージ
        [SerializeField]
        private Image _foregageImage;


        /// <summary>
        /// ストレングスゲージの位置
        /// </summary>
        private RectTransform _strengthViewTransform;

        
        [Inject]
        void Construct()
        {
            // ストレングスゲージの位置をTargetに追従する
            _strengthViewTransform = GetComponent<RectTransform>();
            this.UpdateAsObservable()
                .Subscribe( _ => {
                    _strengthViewTransform.position
                        = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetTransform.position + _offset);
                });
        }


        /// <summary>
        /// ストレングの残量に応じて、残量ゲージの色と表示量を調整
        /// </summary>
        /// <param name="Strength"></param>
        void AdjustForegage(float strength)
        {
            // 残量表示を更新 
            _foregageImage.fillAmount = strength / 100.0f;
   
        }
    }
}