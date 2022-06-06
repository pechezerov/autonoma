using Autonoma.Domain.Electric;
using Autonoma.UI.Presentation.Controls.Electric;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Autonoma.UI.Presentation.Converters
{
    public class VoltageColorConverter : IMultiValueConverter
    {
        public readonly static VoltageColorConverter Instance = new();

        private Dictionary<VoltageClass, Brush> VoltageToBrush = new Dictionary<VoltageClass, Brush>();

        public VoltageColorConverter()
        {
            VoltageToBrush.Add(VoltageClass.kV1150, ElectricColors.FSK_1150Brush);
            VoltageToBrush.Add(VoltageClass.kV750, ElectricColors.FSK_1150Brush);
            VoltageToBrush.Add(VoltageClass.kV500, ElectricColors.FSK_1150Brush);
            VoltageToBrush.Add(VoltageClass.kV400, ElectricColors.FSK_400Brush);
            VoltageToBrush.Add(VoltageClass.kV330, ElectricColors.FSK_330Brush);
            VoltageToBrush.Add(VoltageClass.kV220, ElectricColors.FSK_220Brush);
            VoltageToBrush.Add(VoltageClass.kV150, ElectricColors.FSK_150Brush);
            VoltageToBrush.Add(VoltageClass.kV110, ElectricColors.FSK_110Brush);

            VoltageToBrush.Add(VoltageClass.kV35, ElectricColors.FSK_35Brush);
            VoltageToBrush.Add(VoltageClass.kV20, ElectricColors.FSK_20Brush);
            VoltageToBrush.Add(VoltageClass.kV10, ElectricColors.FSK_10Brush);
            VoltageToBrush.Add(VoltageClass.kV6, ElectricColors.FSK_6Brush);
            VoltageToBrush.Add(VoltageClass.kV04, ElectricColors.FSK_04Brush);

            VoltageToBrush.Add(VoltageClass.kV21_18_G, ElectricColors.FSK_G21_18Brush);
            VoltageToBrush.Add(VoltageClass.kV16_14_G, ElectricColors.FSK_G16_14Brush);
            VoltageToBrush.Add(VoltageClass.kV10G, ElectricColors.FSK_G10d5Brush);
        }

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count < 3) return ElectricColors.ExceptionColorBrush;
            var state = (values[0] != null ? (values[0] as VoltageState?) : null) ?? default(VoltageState);
            var ground = (values[1] != null ? (values[1] as GroundState?) : null) ?? default(GroundState);
            var nom = (values[2] != null ? (values[2] as VoltageClass?) : null) ?? default(VoltageClass);

            if (ground == GroundState.Unknown)
                return ElectricColors.FSK_ErrorBrush;
            else if ((state == VoltageState.Nominal) && (ground == GroundState.Grounded))
                return ElectricColors.FSK_ErrorBrush;

            //особый случай для Switch
            if (values.Count == 4)
            {
                var pos = (values[3] != null ? (values[3] as SwitchPosition?) : null) ?? default(SwitchPosition);
                if (pos == SwitchPosition.Error)
                    return ElectricColors.FSK_ErrorBrush;
            }

            if (ground == GroundState.Grounded)
                return ElectricColors.FSK_GroundBrush;
            if (state == VoltageState.Nominal)
                return VoltageToBrush[nom];
            else if (state == VoltageState.Empty)
                return ElectricColors.FSK_NoVoltageBrush;

            return ElectricColors.ExceptionColorBrush;
        }
    }
}
