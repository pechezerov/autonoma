using Autonoma.Core.Util;
using System;
using System.Collections.Generic;

namespace Autonoma.Core
{
    public static class Globals
    {
        static Globals()
        {
            DataPointTypeSizes.Add(TypeCode.Boolean, 1);
            DataPointTypeSizes.Add(TypeCode.SByte, 1);
            DataPointTypeSizes.Add(TypeCode.Byte, 1);
            DataPointTypeSizes.Add(TypeCode.Int16, 2);
            DataPointTypeSizes.Add(TypeCode.UInt16, 2);
            DataPointTypeSizes.Add(TypeCode.Single, 2);
            DataPointTypeSizes.Add(TypeCode.Int32, 4);
            DataPointTypeSizes.Add(TypeCode.UInt32, 4);
            DataPointTypeSizes.Add(TypeCode.Double, 4);

            foreach (var typeInfo in DataPointTypeSizes)
            {
                DataPointTypes.Add(typeInfo.Key);
            }
        }

        public readonly static HashSet<TypeCode> DataPointTypes = new HashSet<TypeCode>();
        public readonly static Dictionary<TypeCode, ushort> DataPointTypeSizes = new Dictionary<TypeCode, ushort>();
        public const int IdleAdapterTypeId  = 1;
        public const int IdleAdapterId  = 1;
        public const int TestAdapterTypeId  = 2;
        public const int TestAdapterId = 2;

        public static ushort GetTypeSize(TypeCode valueType)
        {
            return DataPointTypeSizes[valueType];
        }
    }
}