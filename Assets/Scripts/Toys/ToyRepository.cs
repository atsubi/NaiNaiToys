using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Toys {

    public class ToyRepository
    {
        private ToyParamAsset _toyParamAsset;

        public ToyRepository(ToyParamAsset toyParamAsset)
        {
            _toyParamAsset = toyParamAsset;
        }

        public float GetToyWeight(int id)
        {
            return _toyParamAsset.ToyParamData.ToyParams.Find(toyParam => toyParam.id == id).weight;
        }

        public string GetToyName(int id)
        {
            return _toyParamAsset.ToyParamData.ToyParams.Find(toyParam => toyParam.id == id).name;
        }

        public float GetToyAngerCareRate(int id)
        {
            return _toyParamAsset.ToyParamData.ToyParams.Find(toyParam => toyParam.id == id).angerCareRate;
        }

        public int GetToyPoint(int id)
        {
            return _toyParamAsset.ToyParamData.ToyParams.Find(toyParam => toyParam.id == id).point;
        }


        /// <summary>
        /// 登録されているおもちゃの種類数を返す
        /// </summary>
        /// <returns></returns>
        public int GetNumOfToyType()
        {
            return _toyParamAsset.ToyParamData.ToyParams.Count;
        }


        /// <summary>
        /// 指定された出現頻度に従い、ランダムにおもちゃのIDを生成する
        /// </summary>
        /// <returns></returns>
        public int GetToyId()
        {
            List<ToyParam> toyParams = _toyParamAsset.ToyParamData.ToyParams;

            float appearanceRateSum = toyParams.Sum(toyParam => toyParam.appearanceRate);
            float appearanceRundomValue = UnityEngine.Random.Range(0.0f, appearanceRateSum);

            int resultId = 1;
            float appearanceRateBorder = 0.0f;

            foreach(ToyParam toyParam in toyParams.OrderBy(toyParam => toyParam.id)) {
                appearanceRateBorder += toyParam.appearanceRate;
                if (appearanceRundomValue <= appearanceRateBorder) {
                    resultId = toyParam.id;
                    break;
                }
            }

            return resultId;
        }
        
    }
}