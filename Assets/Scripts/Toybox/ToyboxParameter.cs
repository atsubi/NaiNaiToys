using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

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

        private readonly ToyRepository _toyRepository;

        public ToyboxParameter(ToyRepository toyRepository)
        {
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
            if (tmp_score > 100) {
                _score.Value = 100;
            } else {
                _score.Value = tmp_score;
            }
        }
    }
}