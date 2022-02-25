using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ASMinesweeperGame.ViewModel.ValueConverter {
    [ValueConversion(typeof(GameTheme), typeof(Uri))]
    public class GameThemeToFlagMark : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            GameTheme gameTheme;

            try {
                gameTheme = (GameTheme)value;
            }
            catch {
                gameTheme = GameTheme.AS;
            }

            return _resources[gameTheme][_rnd.Next(0, _resources[gameTheme].Length)];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new InvalidOperationException();
        }

        static GameThemeToFlagMark() {
            int emotionsPerMemeber = 25;
            foreach (GameTheme gameTheme in Enum.GetValues(typeof(GameTheme))) {
                _resources[gameTheme] = new Uri[emotionsPerMemeber];
                string baseFolder = gameTheme switch {
                    GameTheme.Ava => "A",
                    GameTheme.Bella => "B",
                    GameTheme.Carol => "C",
                    GameTheme.Diana => "D",
                    GameTheme.Eileen => "E",
                    GameTheme.AS => "A",
                    _ => "A"
                };
                for (int i = 0; i < emotionsPerMemeber; i++) {
                    _resources[gameTheme][i] = new Uri($"/ASMinesweeperGame;component/Resources/Images/FlagMarks/{baseFolder}/{i}.png", UriKind.Relative);
                }

            }
        }
        private readonly Random _rnd = new Random();
        private static readonly Dictionary<GameTheme, Uri[]> _resources = new Dictionary<GameTheme, Uri[]>();
    }
}
