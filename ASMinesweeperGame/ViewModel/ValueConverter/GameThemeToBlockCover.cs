using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ASMinesweeperGame.ViewModel.ValueConverter {
    [ValueConversion(typeof(GameTheme), typeof(Uri))]
    public class GameThemeToBlockCover : IValueConverter {
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
            [GameTheme.AS] = new Uri($"/ASMinesweeperGame;component/Resources/Images/BlockCovers/ASTheme.png", UriKind.Relative),
            [GameTheme.Ava] = new Uri($"/ASMinesweeperGame;component/Resources/Images/BlockCovers/AvaTheme.png", UriKind.Relative),
            [GameTheme.Bella] = new Uri($"/ASMinesweeperGame;component/Resources/Images/BlockCovers/BellaTheme.png", UriKind.Relative),
            [GameTheme.Carol] = new Uri($"/ASMinesweeperGame;component/Resources/Images/BlockCovers/CarolTheme.png", UriKind.Relative),
            [GameTheme.Diana] = new Uri($"/ASMinesweeperGame;component/Resources/Images/BlockCovers/DianaTheme.png", UriKind.Relative),
            [GameTheme.Eileen] = new Uri($"/ASMinesweeperGame;component/Resources/Images/BlockCovers/EileenTheme.png", UriKind.Relative),
        };
    }
}
