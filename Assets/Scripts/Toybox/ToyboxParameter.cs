using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using VContainer;
using VContainer.Unity;

using Toys;

namespace Toybox {
    
    /// <summary>
    /// 
    /// </summary>
    public class ToyboxParameter 
    {

        /// <summary>
        /// おもちゃ箱に入っているおもちゃリスト
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <returns></returns>
        private readonly ReactiveCollection<int> _containToys = new ReactiveCollection<int>();

        /// <summary>
        /// おもちゃ箱のスコア
        /// </summary>
        public IReadOnlyReactiveProperty<int> Score => _score;
        private IntReactiveProperty _score = new IntReactiveProperty(0);

        /// <summary>
        /// クリアに必要なスコア
        /// </summary>
        private int _cleardScore;

        /// <summary>
        /// クリアフラグ
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsCleard => _isCleard;
        private BoolReactiveProperty _isCleard = new BoolReactiveProperty(false);

        private readonly ToyRepository _toyRepository;

        [Inject]
        public ToyboxParameter(ToyRepository toyRepository)
        {
            _cleardScore = 5;
            _toyRepository = toyRepository;
        }
        
        /// <summary>
        /// おもちゃ箱におもちゃを入れる
        /// </summary>
        /// <param name="toyId"></param>
        public void AddToy(int toyId)
        {
            _containToys.Add(toyId);
        }

        /// <summary>
        /// おもちゃ箱をリセットする
        /// </summary>
        public void ResetToybox()
        {
            _containToys.Clear();
        }

    
        /// <summary>
        /// おもちゃ箱のリストが更新された時のイベントストリーム
        /// </summary>
        /// <returns></returns>
        public System.IObservable<Unit> ToyboxChangeEvent()
        {
            return Observable.Merge(
					_containToys.ObserveCountChanged().AsUnitObservable(),
					_containToys.ObserveAdd().AsUnitObservable(),
                    _containToys.ObserveRemove().AsUnitObservable(),
					_containToys.ObserveReplace().AsUnitObservable(),
                    _containToys.ObserveMove().AsUnitObservable(),
                    _containToys.ObserveReset().AsUnitObservable())
				.BatchFrame();
        }



        /// <summary>
        /// おもちゃ箱に入っているおもちゃのポイント合計を取得する
        /// 合計は100以下
        /// </summary>
        /// <returns></returns>
        public void UpdateScore()
        {
            int tmp_score = _containToys.Sum(id => _toyRepository.GetToyPoint(id));
            if (tmp_score >= _cleardScore) {
                _score.Value = _cleardScore;
                _isCleard.Value = true;
            } else {
                _score.Value = tmp_score;
                _isCleard.Value = false;
            }
        }
    }
}