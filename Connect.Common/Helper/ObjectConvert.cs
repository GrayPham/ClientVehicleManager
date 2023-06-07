namespace nsConnect.Common.Helper
{
    public class Object<T>
    {
        public T Value { get; set; }

        public Object()
        {
            Value = default(T);
        }

        public Object(T value)
        {
            Value = value;
        }
    }
}
