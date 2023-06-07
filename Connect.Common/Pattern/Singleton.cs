namespace nsFramework.Common.Pattern
{
    public sealed class Singleton<T> where T : class, new()
    {
        // http://csharpindepth.com/articles/general/singleton.aspx
        private Singleton() { /* Private constructor */ }

        public static T Instance { get { return Nested.instance; } }

        private class Nested
        {
            internal static readonly T instance = new T();
            // Explicit static constructor to tell C# compiler
            // not to mark type as before field initialize
            static Nested()
            {
            }
        }
    }
}
