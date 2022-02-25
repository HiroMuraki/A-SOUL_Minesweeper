using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ASMinesweeperGame.ViewModel.ValueConverter {
    [ValueConversion(typeof(GameTheme), typeof(Brush))]
    public class GameThemeToBrush : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                return _resources[(GameTheme)value];
            }
            catch (Exception) {
                return _resources[GameTheme.AS];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new InvalidOperationException();
        }

        private static readonly Dictionary<GameTheme, Brush> _resources = new Dictionary<GameTheme, Brush> {
            [GameTheme.AS] = (Brush)App.ColorDict["AvaTheme"],
            [GameTheme.Ava] = (Brush)App.ColorDict["AvaTheme"],
            [GameTheme.Bella] = (Brush)App.ColorDict["BellaTheme"],
            [GameTheme.Carol] = (Brush)App.ColorDict["CarolTheme"],
            [GameTheme.Diana] = (Brush)App.ColorDict["DianaTheme"],
            [GameTheme.Eileen] = (Brush)App.ColorDict["EileenTheme"],
        };
    }
}
