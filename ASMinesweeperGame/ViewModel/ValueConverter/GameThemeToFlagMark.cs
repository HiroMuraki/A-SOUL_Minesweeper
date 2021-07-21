using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ASMinesweeperGame.ViewModel.ValueConverter {
    [ValueConversion(typeof(GameTheme), typeof(Uri))]
    public class GameThemeToFlagMark : IValueConverter {
        private static Random _rnd = new Random();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return new Uri($"/ASMinesweeperGame;component/Resources/Images/FlagMarks/FlagMark{_rnd.Next(1, 21)}.png", UriKind.Relative);
            //switch ((GameTheme)value) {
            //    case GameTheme.AS:
            //        return new Uri("/ASMinesweeperGame;component/Resources/Images/FlagMark_ASTheme.png", UriKind.Relative);
            //    case GameTheme.Ava:
            //        return new Uri("/ASMinesweeperGame;component/Resources/Images/FlagMark_AvaTheme.png", UriKind.Relative);
            //    case GameTheme.Bella:
            //        return new Uri("/ASMinesweeperGame;component/Resources/Images/FlagMark_BellaTheme.png", UriKind.Relative);
            //    case GameTheme.Carol:
            //        return new Uri("/ASMinesweeperGame;component/Resources/Images/FlagMark_CarolTheme.png", UriKind.Relative);
            //    case GameTheme.Diana:
            //        return new Uri("/ASMinesweeperGame;component/Resources/Images/FlagMark_DianaTheme.png", UriKind.Relative);
            //    case GameTheme.Eileen:
            //        return new Uri("/ASMinesweeperGame;component/Resources/Images/FlagMark_EileenTheme.png", UriKind.Relative);
            //    default:
            //        return new Uri("/ASMinesweeperGame;component/Resources/Images/FlagMark_ASTheme.png", UriKind.Relative);
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
