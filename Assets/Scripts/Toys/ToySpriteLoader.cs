using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

using VContainer;
using VContainer.Unity;

using UniRx;

using Cysharp.Threading.Tasks;


namespace Toys {

    public class ToySpriteLoader
    {
        // おもちゃのスプライトリスト
        private IList<Sprite> _toyListSprites;

        // おもちゃのイメージのロード完了通知
        public UniTask CompleteToySpriteLoading => _uniTaskCompletionSource.Task;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        /// <summary>
        /// おもちゃのスプライトを読み込む
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async void LoadToySprite(CancellationToken cancellation)
        {
            _toyListSprites = await Addressables.LoadAssetAsync<IList<Sprite>>("ToyList").WithCancellation(cancellation);
            _uniTaskCompletionSource.TrySetResult();
        }

        /// <summary>
        /// 指定したおもちゃidのスプライトを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sprite GetToySprite(int id)
        {
            return _toyListSprites[id];
        }
    }
}