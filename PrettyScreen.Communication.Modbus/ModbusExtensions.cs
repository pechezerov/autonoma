using System;
using System.Collections.Generic;
using System.Text;

namespace PrettyScreen.Communication.Modbus
{
    public static class ModbusExtensions
    {
        public static bool GetBit(this ushort b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

    }
}
