using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toys {

    /// <summary>
    /// おもちゃを生成するファクトリークラス
    /// </summary>
    public class ToyCreater
    {
        System.Func<int, GameObject> createToy;

        public ToyCreater(System.Func<int, GameObject> func)
        {
            createToy = func;
        }

        public GameObject Create(int id) => createToy(id);
    }
}