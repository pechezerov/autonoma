using Autonoma.Core.Util;
using System;
using System.Collections.Generic;

namespace Autonoma.Core
{
    public static class Globals
    {
        static Globals()
        {
            _dataPointTypeSizes.Add(TypeCode.Boolean, 1);
            _dataPointTypeSizes.Add(TypeCode.SByte, 1);
            _dataPointTypeSizes.Add(TypeCode.Byte, 1);
            _dataPointTypeSizes.Add(TypeCode.Int16, 2);
            _dataPointTypeSizes.Add(TypeCode.UInt16, 2);
            _dataPointTypeSizes.Add(TypeCode.Single, 2);
            _dataPointTypeSizes.Add(TypeCode.Int32, 4);
            _dataPointTypeSizes.Add(TypeCode.UInt32, 4);
            _dataPointTypeSizes.Add(TypeCode.Double, 4);

            foreach (var typeInfo in DataPointTypeSizes)
            {
                _dataPointTypes.Add(typeInfo.Key);
            }
        }

        private static HashSet<TypeCode> _dataPointTypes = new HashSet<TypeCode>();
        private static Dictionary<TypeCode, ushort> _dataPointTypeSizes = new Dictionary<TypeCode, ushort>();
        public static IReadOnlySet<TypeCode> DataPointTypes => _dataPointTypes;
        public static IReadOnlyDictionary<TypeCode, ushort> DataPointTypeSizes => _dataPointTypeSizes;

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