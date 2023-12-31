using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        private Vector3 _offset = new Vector3(0.8f, 0.8f, 0.0f);

        // ストレングスゲージを表示するパネル
        [SerializeField]
        private Image _StrengthUIPanel;


        // ストレングスゲージの残量イメージ
        [SerializeField]
        private Image _forgageImage;

        // 残量イメージに表示するスプライトリスト
        private List<Sprite> _forgageSpriteList;


        /// <summary>
        /// ストレングスゲージの位置
        /// </summary>
        private RectTransform _strengthViewTransform;

        
        [Inject]
        async void Construct()
        {
            // ストレングスゲージに設定するスプライトのリストを読み込む
            await loadgageImages();

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
        private async UniTask loadgageImages(CancellationToken cancellationToken = default)
        {
            var tmpSpriteList = await Addressables.LoadAssetAsync<IList<Sprite>>("Gage").WithCancellation(cancellationToken);
            _forgageSpriteList = tmpSpriteList.OrderBy(sprite => sprite.name).ToList();
        }


        /// <summary>
        /// ストレングの残量に応じて、残量ゲージの表示量を調整
        /// </summary>
        /// <param name="strength"></param>
        public void AdjustForgage(float strength)
        {
            // 残量表示を更新 
            _forgageImage.fillAmount = strength / 100.0f;
        }


        /// <summary>
        /// ストレングスの残量に応じて、残量ゲージの色を調整
        /// </summary>
        /// <param name="strength"></param>
        public void UpdateForgageImageSprite(float strength)
        {
            if (_forgageSpriteList == null) return;

            if (strength > 70.0f) {
                _forgageImage.sprite = _forgageSpriteList[3];
                return;
            } else if(strength <= 70.0f && strength > 30.0f) {
                _forgageImage.sprite = _forgageSpriteList[2];
                return;
            } else {
               _forgageImage.sprite = _forgageSpriteList[1];
                return;
            }
        }

        /// <summary>
        /// ストレングスゲージのパネルをgrayに変更
        /// </summary>
        public void GrayUIPanel()
        {
            Color color = Color.gray;
            color.a = 0.4f;
            _StrengthUIPanel.color = color;
        }


        /// <summary>
        /// ストレングスゲージのパネルをwhiteに変更
        /// </summary>
        public void WhiteUIPanel()
        {
            Color color = Color.white;
            color.a = 0.4f;
            _StrengthUIPanel.color = color;
        }
    }
}