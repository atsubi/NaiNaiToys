using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace Toybox {
    
    /// <summary>
    /// 
    /// </summary>
    public class ToyboxParameter 
    {
        private readonly ReactiveCollection<int> _containToys = new ReactiveCollection<int>();
        
        public void AddToy(int toyId)
        {
            _containToys.Add(toyId);
        }

        public int GetScore()
        {
            return 0;
        }
    }
}