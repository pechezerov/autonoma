using System;

namespace PrettyScreen.Core.Util
{
    public static class GuidHelper
    {
        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}
