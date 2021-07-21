using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ASMinesweeperGame.ViewModel.ValueConverter {
    [ValueConversion(typeof(GameTheme), typeof(Uri))]
    public class GameThemeToBlockCover : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            switch ((GameTheme)value) {
                case GameTheme.AS:
                    return new Uri("/ASMinesweeperGame;component/Resources/Images/BlockCover_ASTheme.png", UriKind.Relative);
                case GameTheme.Ava:
                    return new Uri("/ASMinesweeperGame;component/Resources/Images/BlockCover_AvaTheme.png", UriKind.Relative);
                case GameTheme.Bella:
                    return new Uri("/ASMinesweeperGame;component/Resources/Images/BlockCover_BellaTheme.png", UriKind.Relative);
                case GameTheme.Carol:
                    return new Uri("/ASMinesweeperGame;component/Resources/Images/BlockCover_CarolTheme.png", UriKind.Relative);
                case GameTheme.Diana:
                    return new Uri("/ASMinesweeperGame;component/Resources/Images/BlockCover_DianaTheme.png", UriKind.Relative);
                case GameTheme.Eileen:
                    return new Uri("/ASMinesweeperGame;component/Resources/Images/BlockCover_EileenTheme.png", UriKind.Relative);
                default:
                    return new Uri("/ASMinesweeperGame;component/Resources/Images/BlockCover_ASTheme.png", UriKind.Relative);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
