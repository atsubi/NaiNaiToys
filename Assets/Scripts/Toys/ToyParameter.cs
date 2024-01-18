using System;

using UniRx;


namespace Toys {
    public class ToyParameter 
    {
        /// <summary>
        /// おもちゃID
        /// </summary>
        public IReadOnlyReactiveProperty<int> Id => _id;
        private ReactiveProperty<int> _id = new ReactiveProperty<int>(0);

        /// <summary>
        /// おもちゃのIDを設定する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SetToyId(int id)
        {
            if (id < 0) {
                return false;
            }

            _id.Value = id;

            return true;
        }

    }
}