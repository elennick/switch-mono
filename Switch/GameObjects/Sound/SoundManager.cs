using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Switch.GameObjects.Sound
{
    class SoundManager
    {
        private static SoundManager instance;
        private Dictionary<String, SoundEffect> sounds;
        private Dictionary<String, Song> songs;
        private ContentManager contentManager { get; set; }
        private bool songStarted;
        private bool contentLoaded;
        private bool musicEnabled;
        private String currentSong;
        private bool musicPaused;

        private SoundManager()
        {
            sounds = new Dictionary<String, SoundEffect>();
            songs = new Dictionary<String, Song>();

            songStarted = false;
            contentLoaded = false;
            musicEnabled = !SwitchGame.DEBUG_MODE;
            musicPaused = false;
            currentSong = "";
        }

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }

        public void LoadSoundMedia(ContentManager contentManager)
        {
            if (!contentLoaded)
            {
                this.contentManager = contentManager;

                sounds.Add("flip", contentManager.Load<SoundEffect>("Sound\\Effects\\flip"));
                sounds.Add("explode-tile", contentManager.Load<SoundEffect>("Sound\\Effects\\explode-tile"));
                sounds.Add("laser", contentManager.Load<SoundEffect>("Sound\\Effects\\laser"));
                sounds.Add("nuke-explode", contentManager.Load<SoundEffect>("Sound\\Effects\\nuke-explode"));
                sounds.Add("bullettime", contentManager.Load<SoundEffect>("Sound\\Effects\\bullettime"));
                sounds.Add("levelup", contentManager.Load<SoundEffect>("Sound\\Effects\\levelup"));
                sounds.Add("menu-select", contentManager.Load<SoundEffect>("Sound\\Effects\\menu-select"));
                sounds.Add("menu-select2", contentManager.Load<SoundEffect>("Sound\\Effects\\menu-select2"));
                sounds.Add("wombat-growl", contentManager.Load<SoundEffect>("Sound\\Effects\\wombat-growl"));
                sounds.Add("readySet", contentManager.Load<SoundEffect>("Sound\\Effects\\readySet"));
                sounds.Add("go", contentManager.Load<SoundEffect>("Sound\\Effects\\go"));
                sounds.Add("2p-alarm", contentManager.Load<SoundEffect>("Sound\\Effects\\2p-alarm"));
                sounds.Add("game-over", contentManager.Load<SoundEffect>("Sound\\Effects\\game-over"));
                sounds.Add("player-select", contentManager.Load<SoundEffect>("Sound\\Effects\\player-select"));

                songs.Add("gameplay-song", contentManager.Load<Song>("Sound\\Music\\HouseMoFo"));
                songs.Add("menu-song", contentManager.Load<Song>("Sound\\Music\\Religions"));

                contentLoaded = true;
            }
        }
         
        public void PlaySound(String soundName) 
        {
            if(sounds.ContainsKey(soundName)) {
                //Console.WriteLine("playing sound " + sounds[soundName].Name);
                sounds[soundName].Play();
            }
        }

        public void PlaySong(String songName)
        {
            if ((!songStarted || songName != currentSong) && 
                songs.ContainsKey(songName) && 
                musicEnabled &&
                !musicPaused)
            {
                //Console.WriteLine("playing song " + songName);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.Play(songs[songName]);
                songStarted = true;
                currentSong = songName;
            }
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
            songStarted = false;
        }

        public bool IsSongPlaying()
        {
            return songStarted;
        }

        public bool IsMusicPaused()
        {
            return musicPaused;
        }

        public void SetMusicEnabled(bool musicEnabled)
        {
            this.musicEnabled = musicEnabled;
            if (!this.musicEnabled && IsSongPlaying())
            {
                StopSong();
            }
        }

        public void SetMusicPaused(bool musicPaused)
        {
            this.musicPaused = musicPaused;
            if (this.musicPaused && IsSongPlaying())
            {
                StopSong();
            }
        }
    }
}
