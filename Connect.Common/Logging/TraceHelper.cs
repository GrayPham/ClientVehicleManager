using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Connect.Common.Logging
{
    public class TraceHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            try
            {
                var st = new StackTrace();
                var sf = st.GetFrame(2);
                return sf.GetMethod().Name;
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
