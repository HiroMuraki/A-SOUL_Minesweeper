using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ASMinesweeperGame.ViewModel.ValueConverter {
    [ValueConversion(typeof(GameTheme), typeof(Uri))]
    public class GameThemeToBackground : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                return _resources[(GameTheme)value];
            }
            catch {
                return _resources[GameTheme.AS];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new InvalidOperationException();
        }

        private static readonly Dictionary<GameTheme, Uri> _resources = new Dictionary<GameTheme, Uri> {
            [GameTheme.AS] = new Uri($"/ASMinesweeperGame;component/Resources/Images/Backgrounds/ASTheme.jpg", UriKind.Relative),
            [GameTheme.Ava] = new Uri($"/ASMinesweeperGame;component/Resources/Images/Backgrounds/AvaTheme.jpg", UriKind.Relative),
            [GameTheme.Bella] = new Uri($"/ASMinesweeperGame;component/Resources/Images/Backgrounds/BellaTheme.jpg", UriKind.Relative),
            [GameTheme.Carol] = new Uri($"/ASMinesweeperGame;component/Resources/Images/Backgrounds/CarolTheme.jpg", UriKind.Relative),
            [GameTheme.Diana] = new Uri($"/ASMinesweeperGame;component/Resources/Images/Backgrounds/DianaTheme.jpg", UriKind.Relative),
            [GameTheme.Eileen] = new Uri($"/ASMinesweeperGame;component/Resources/Images/Backgrounds/EileenTheme.jpg", UriKind.Relative),
        };
    }
}
