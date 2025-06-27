using System;

namespace Util
{
    public class Stat<T>
    {
        private T _value;
        public event Action Onchanged;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Onchanged?.Invoke();
            }
        }
        public Stat(T initValue)
        {
            _value = initValue;
        }
    }
}