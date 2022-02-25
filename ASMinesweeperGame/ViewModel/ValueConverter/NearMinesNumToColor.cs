using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ASMinesweeperGame.ViewModel.ValueConverter {
    [ValueConversion(typeof(int), typeof(Brush))]
    public class NearMinesNumToColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                return _resources[(int)value];
            }
            catch {
                return _resources[0];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new InvalidOperationException();
        }

        private static readonly Dictionary<int, SolidColorBrush> _resources = new Dictionary<int, SolidColorBrush>() {
            [0] = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00)),
            [1] = new SolidColorBrush(Color.FromRgb(0x89, 0x89, 0x89)),
            [2] = new SolidColorBrush(Color.FromRgb(0x3d, 0xad, 0xe6)),
            [3] = new SolidColorBrush(Color.FromRgb(0x00, 0x8d, 0x09)),
            [4] = new SolidColorBrush(Color.FromRgb(0x00, 0xad, 0xb3)),
            [5] = new SolidColorBrush(Color.FromRgb(0x00, 0x41, 0xb3)),
            [6] = new SolidColorBrush(Color.FromRgb(0x5f, 0x00, 0xb3)),
            [7] = new SolidColorBrush(Color.FromRgb(0xb3, 0x00, 0xa1)),
            [8] = new SolidColorBrush(Color.FromRgb(0xb3, 0x00, 0x00)),
            [9] = new SolidColorBrush(Color.FromRgb(0xe6, 0x3d, 0x59)),
        };
    }
}
