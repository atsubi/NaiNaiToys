using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using VContainer;
using VContainer.Unity;

using UniRx;
using Hold;

namespace Players
{
    /// <summary>
    /// プレイヤーの掴みアクション時の動作
    /// </summary>
    public class PlayerToyHolder : MonoBehaviour
    {
        // プレイヤーの手の長さ
        [Header("手の長さ"), SerializeField]
        private float _playerHandLength;

        // 掴み判定がある半径
        [Header("判定半径"), SerializeField]
        private float _holdableAreaRadius;

        /// <summary>
        /// 掴み時の判定円の原点
        /// </summary>
        private Vector3 _holdableAreaOriginalPoint;

        // おもちゃのレイヤー(掴み時におもちゃのみを確認する)
        [SerializeField]
        private int _ignorePlayerMask;

        /// <summary>
        /// おもちゃを掴み中かの判定
        /// </summary>
        public IReadOnlyAsyncReactiveProperty<bool> HoldToy;
        private BoolReactiveProperty _holdToy = new BoolReactiveProperty(false);

        /// <summary>
        /// 現在掴んでいるおもちゃのオブジェクト
        /// </summary>
        private GameObject _holdingToy;
        
        /// <summary>
        /// 掴み中のおもちゃを表示する位置
        /// </summary>
        [SerializeField]
        private Transform _head;

        // 現在の向いてる方向を得るために必要
        private PlayerAnimation _playerAnimation;

        [Inject]
        public void Construct(PlayerAnimation playerAnimation)
        {
            _playerAnimation = playerAnimation;
            _ignorePlayerMask = ~(1 << LayerMask.NameToLayer("Player"));
        }

        public void HoldAction(bool action)
        {

            // 掴む
            if (action == true) {
                
                // 掴み判定
                Vector3 playerDirection = new Vector3(_playerAnimation.DirectionX, _playerAnimation.DirectionY, 0.0f).normalized;
                _holdableAreaOriginalPoint = playerDirection * _playerHandLength + transform.position;
                
                var list = Physics2D.CircleCastAll(_holdableAreaOriginalPoint, _playerHandLength, Vector3.zero, .1f, _ignorePlayerMask);
                var holdableToyList = new List<RaycastHit2D>();
                holdableToyList.AddRange(list);
                holdableToyList.Sort((a,b) => (int)Vector3.Distance(a.transform.position, transform.position) - (int)Vector3.Distance(b.transform.position, transform.position));

                bool hit = false;
                int holdedToyNum = -1;
                for (int i=0; i<holdableToyList.Count; i++) {

                    if (holdableToyList[i].collider.transform.GetComponent<IHoldable>() != null) {
                        holdedToyNum = i;
                        hit = true;
                        break;
                    }
                }

                if ( !hit ) return;
                
                // 掴みアクション
                IHoldable holdable = holdableToyList[holdedToyNum].collider.transform.GetComponent<IHoldable>();
                bool result = holdable.TryHold();
                if ( !result ) return;

                // プレイヤー頭上におもちゃを移動
                holdableToyList[holdedToyNum].collider.transform.parent = _head;
                holdableToyList[holdedToyNum].collider.GetComponent<CircleCollider2D>().enabled = false;
                holdableToyList[holdedToyNum].collider.transform.localPosition = Vector3.zero;
                
                // プロパティ設定
                _holdingToy = holdableToyList[holdedToyNum].collider.transform.gameObject;
                _holdToy.Value = true;
            }

            // 離す
            else {

                if (!_holdToy.Value) return;

                // 離すアクション
                IHoldable holdable = _holdingToy.transform.GetComponent<IHoldable>();
                holdable.UnHold();
                _holdingToy.transform.parent = null;
                _holdingToy.transform.GetComponent<CircleCollider2D>().enabled = true;
                _holdingToy.transform.position = transform.position + new Vector3(_playerAnimation.DirectionX, _playerAnimation.DirectionY, 0.0f);
                _holdingToy = null;
                _holdToy.Value = false;
            }
        }


        void OnDrawGizmos()
        {
            GizmosUtility.DrawWireCircle(_holdableAreaOriginalPoint, _playerHandLength);
        }
    }

}