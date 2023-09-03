using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using Cysharp.Threading.Tasks;

namespace Toybox {

    /// <summary>
    /// おもちゃを受け取るインターフェイス
    /// </summary>
    public interface IAcceptToyProvider
    {
        public IReadOnlyReactiveProperty<Collision2D> IAcceptToy { get; }
        public UniTask CompleteAcceptToyEventSetting { get; }

        public void InitializeAcceptToyEvent();
    }
}