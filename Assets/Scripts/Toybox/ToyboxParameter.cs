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
        private readonly ReactiveCollection<int> _containToys = new ReactiveCollection<int>();

        private readonly ToyRepository _toyRepository;

        public ToyboxParameter(ToyRepository toyRepository)
        {
            _toyRepository = toyRepository;
        }
        
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