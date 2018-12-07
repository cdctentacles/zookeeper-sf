using System;

namespace zookeeper_sf
{
    class ConditionalValue<T>
    {
        bool hasValue;
        T value;

        public ConditionalValue()
        {
            this.hasValue = false;
        }

        public ConditionalValue(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public T GetValue()
        {
            if (this.hasValue)
            {
                return this.value;
            }
            throw new Exception("Value is not present.");
        }

        public bool HasValue()
        {
            return this.hasValue;
        }
    }
}