using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility {

    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        /// <summary>
        /// オブジェクトの実態
        /// </summary>
        private static T _instance;


        /// <summary>
        /// 外部からのオブジェクトの呼び出し口
        /// </summary>
        /// <value></value>        
        public static T Instance
        {
            get 
            {
                if (_instance != null) return _instance;                
                _instance = (T)FindObjectOfType(typeof(T));
                return _instance;
            }
        }


        // 起動時に多重起動を確認する
        protected virtual void Awake()
        {
            // インスタンスが存在しない場合は自身を代入
            if (_instance == null) 
            {
                _instance = this as T;
                return;
            }

            // 自身の場合は何もしない
            if (Instance == this)
            {
                return;
            }

            Destroy(this);
        }            
    }
}