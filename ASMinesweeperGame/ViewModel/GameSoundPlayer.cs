using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace ASMinesweeperGame.ViewModel {
    public class GameSoundPlayer {
        public static GameSoundPlayer Instance { get; } = new GameSoundPlayer();
        public string GameSoundDirectory { get; set; } = "ASSounds";

        public void LoadSounds() {
            if (!Directory.Exists(GameSoundDirectory)) {
                return;
            }
            foreach (var file in Directory.GetFiles(GameSoundDirectory)) {
                string fileName = Path.GetFileName(file).ToUpper();
                Uri soundUri = new Uri($@"{GameSoundDirectory}\{fileName}", UriKind.Relative);
                // 跳过非MP3
                if (!(Path.GetExtension(fileName) == ".MP3")) {
                    continue;
                }
                if (fileName.StartsWith("OPEN")) {
                    _openFXSound.Add(soundUri);
                }
                else if (fileName.StartsWith("QOPEN")) {
                    _quickOpenFXSound.Add(soundUri);
                }
                else if (fileName.StartsWith("FLAG")) {
                    _flagFXSound.Add(soundUri);
                }
                else if (fileName.StartsWith("MINE")) {
                    _mineFXSound.Add(soundUri);
                }
                else if (fileName.StartsWith("MUSIC")) {
                    _gameMusic.Add(soundUri);
                }
                //else if (fileName.StartsWith("COMPLETED")) {
                //    _flagFXSound.Add(soundUri);
                //}
                //else if (fileName.StartsWith("#AVA")) {
                //    _avaSkillSounds.Add(soundUri);
                //}
                //else if (fileName.StartsWith("#BELLA")) {
                //    _bellaSkillSounds.Add(soundUri);
                //}
                //else if (fileName.StartsWith("#CAROL")) {
                //    _carolSkillSounds.Add(soundUri);
                //}
                //else if (fileName.StartsWith("#DIANA")) {
                //    _dianaSkillSounds.Add(soundUri);
                //}
                //else if (fileName.StartsWith("#EILEEN")) {
                //    _eileenSkillSounds.Add(soundUri);
                //}
            }
        }
        public void PlayOpenFXSound() {
            RandomPlay(_openSoundPlayer, _openFXSound);
        }
        public void PlayQuickOpenFXSound() {
            RandomPlay(_quickOpenSoundPlayer, _quickOpenFXSound);
        }
        public void PlayFlagFXSound() {
            RandomPlay(_flagSoundPlayer, _flagFXSound);
        }
        public void PlayMineFXSound() {
            RandomPlay(_mineSoundPlayer, _mineFXSound);
        }
        public void PlayMusic() {
            RandomPlay(_musicSoundPlayer, _gameMusic);
        }

        private GameSoundPlayer() {
            // 音效播放器初始化
            _musicSoundPlayer.MediaEnded += MusicSoundPlayer_MediaEnded; // 背景音乐重复播放
        }
        private readonly Random _rnd = new Random();
        private readonly List<Uri> _openFXSound = new List<Uri>(); // 点击音效
        private readonly List<Uri> _quickOpenFXSound = new List<Uri>(); // 双击音效
        private readonly List<Uri> _flagFXSound = new List<Uri>(); // 标旗音效
        private readonly List<Uri> _mineFXSound = new List<Uri>(); // 寄音效
        private readonly List<Uri> _gameMusic = new List<Uri>(); // 背景音乐
        private readonly MediaPlayer _openSoundPlayer = new MediaPlayer(); // 点击技能音播放器
        private readonly MediaPlayer _quickOpenSoundPlayer = new MediaPlayer(); // 双击音效播放器
        private readonly MediaPlayer _flagSoundPlayer = new MediaPlayer(); // 标旗音效播放器
        private readonly MediaPlayer _mineSoundPlayer = new MediaPlayer(); // 寄音效播放器
        private readonly MediaPlayer _musicSoundPlayer = new MediaPlayer(); // 背景音乐播放器
        private void RandomPlay(MediaPlayer player, List<Uri> resources) {
            // 音频列表资源为0则跳过播放
            if (resources.Count <= 0) {
                return;
            }
            // 否则随机打开一个并播放
            player.Open(resources[_rnd.Next(0, resources.Count)]);
            player.Play();
        }
        private void MusicSoundPlayer_MediaEnded(object? sender, EventArgs e) {
            PlayMusic();
        }

        //private readonly List<Uri> _avaSkillSounds; // 向晚技能音效
        //private readonly List<Uri> _bellaSkillSounds; // 贝拉技能音效
        //private readonly List<Uri> _carolSkillSounds; // 珈乐技能音效
        //private readonly List<Uri> _dianaSkillSounds; // 嘉然技能音效
        //private readonly List<Uri> _eileenSkillSounds; // 乃琳技能音效
        //private readonly MediaPlayer _gameCompletedSoundPlayer; // 结算音播放器
        //private readonly MediaPlayer _skillActivedSoundPlayer; // 技能启动音播放器
        //public void PlayGameCompletedSound() {
        //    RandomPlay(_gameCompletedSoundPlayer, _flagFXSound);
        //}
        //public void PlaySkillActivedSound(LLKSkill skill) {
        //    switch (skill) {
        //        case LLKSkill.None:
        //            break;
        //        case LLKSkill.AvaPower:
        //            RandomPlay(_skillActivedSoundPlayer, _avaSkillSounds);
        //            break;
        //        case LLKSkill.BellaPower:
        //            RandomPlay(_skillActivedSoundPlayer, _bellaSkillSounds);
        //            break;
        //        case LLKSkill.CarolPower:
        //            RandomPlay(_skillActivedSoundPlayer, _carolSkillSounds);
        //            break;
        //        case LLKSkill.DianaPower:
        //            RandomPlay(_skillActivedSoundPlayer, _dianaSkillSounds);
        //            break;
        //        case LLKSkill.EileenPower:
        //            RandomPlay(_skillActivedSoundPlayer, _eileenSkillSounds);
        //            break;
        //        default:
        //            break;
        //    }
        //    _skillActivedSoundPlayer.Play();
        //}
    }
}
