using System.Collections;
using System.Collections.Generic;

using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

using UniRx;
using UniRx.Triggers;

using VContainer;
using VContainer.Unity;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using Cysharp.Threading.Tasks;


namespace Strength {

    public class StrengthViewer : MonoBehaviour
    {
        /// <summary>
        /// 追従するオブジェクトのTransform
        /// </summary>
        [SerializeField]
        private Transform _targetTransform;

        [SerializeField]
        private Vector3 _offset = new Vector3(0.5f, 0.5f, 0.0f);

        // ストレングスゲージの残量イメージ
        [SerializeField]
        private Image _foregageImage;

        // 残量イメージに表示するスプライトリスト
        private IList<Sprite> _forgageSpriteList;


        /// <summary>
        /// ストレングスゲージの位置
        /// </summary>
        private RectTransform _strengthViewTransform;

        
        [Inject]
        void Construct()
        {
            // ストレングスゲージに設定するスプライトのリストを読み込む
            loadgageImages().Forget();

            // ストレングスゲージの位置をTargetに追従する
            _strengthViewTransform = GetComponent<RectTransform>();
            this.UpdateAsObservable()
                .Subscribe( _ => {
                    _strengthViewTransform.position
                        = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetTransform.position + _offset);
                });
        }


        /// <summary>
        /// ストレングスゲージに設定するスプライトのリストを読み込む
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async UniTaskVoid loadgageImages(CancellationToken cancellationToken = default)
        {
            _forgageSpriteList = await Addressables.LoadAssetAsync<IList<Sprite>>("Gage").WithCancellation(cancellationToken);
        }


        /// <summary>
        /// ストレングの残量に応じて、残量ゲージの色と表示量を調整
        /// </summary>
        /// <param name="Strength"></param>
        public void AdjustForegage(float strength)
        {
            // 残量表示を更新 
            _foregageImage.fillAmount = strength / 100.0f;
   
        }


    }
}