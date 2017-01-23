using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using Switch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Switch.Menus
{
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry rumbleOnEntry;
        MenuEntry musicOnEntry;
        MenuEntry fullScreenEntry;
        MenuEntry controlsEntry;
        MenuEntry howToPlayEntry;
        MenuEntry creditsEntry;
        MenuEntry statsEntry;

        static bool musicOn = true;
        static bool rumbleOn = true;
        static bool fullscreenOn;

        public OptionsMenuScreen()
            : base("Help & Options")
        {
            this.setSubMenuTitleText("Configure Your Junk!");

            fullscreenOn = SwitchGame.Instance.Graphics.IsFullScreen;

            musicOnEntry = new MenuEntry(string.Empty);
            rumbleOnEntry = new MenuEntry(string.Empty);
            fullScreenEntry = new MenuEntry(string.Empty);
            controlsEntry = new MenuEntry("Controls");
            howToPlayEntry = new MenuEntry("How To Play");
            creditsEntry = new MenuEntry("Credits");
            statsEntry = new MenuEntry("Game Stats");

            SetMenuEntryText();

            MenuEntry backMenuEntry = new ExitOrBackMenuEntry("Back To Main Menu...");

            rumbleOnEntry.Selected += RumbleOnMenuEntrySelected;
            musicOnEntry.Selected += MusicOnMenuEntrySelected;
            fullScreenEntry.Selected += FullScreenEntrySelected;
            controlsEntry.Selected += ControlsMenuEntrySelected;
            howToPlayEntry.Selected += HowToPlayMenuEntrySelected;
            creditsEntry.Selected += CreditsMenuEntrySelected;
            statsEntry.Selected += StatsMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(rumbleOnEntry);
            MenuEntries.Add(musicOnEntry);
            MenuEntries.Add(fullScreenEntry);
            MenuEntries.Add(controlsEntry);
            MenuEntries.Add(howToPlayEntry);
            MenuEntries.Add(creditsEntry);
            MenuEntries.Add(statsEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void SetMenuEntryText()
        {
            musicOnEntry.Text = "Music: " + (musicOn ? "ON" : "OFF");
            rumbleOnEntry.Text = "Rumble: " + (rumbleOn ? "ON" : "OFF");
            fullScreenEntry.Text = "Mode: " + (fullscreenOn ? "FULLSCREEN" : "WINDOWED");
        }

        void MusicOnMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicOn = !musicOn;
            SetMenuEntryText();
            SoundManager.Instance.SetMusicEnabled(musicOn);
        }

        void RumbleOnMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            rumbleOn = !rumbleOn;
            SetMenuEntryText();
            VibrationManager.Instance.setVibrationEnabled(rumbleOn);
        }

        void FullScreenEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            fullscreenOn = !fullscreenOn;
            SetMenuEntryText();
            SwitchGame.Instance.Graphics.IsFullScreen = fullscreenOn;
            SwitchGame.Instance.Graphics.ApplyChanges();
        }

        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen(), null);
        }

        void HowToPlayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HowToPlayScreen(), null);
        }

        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen(), null);
        }

        void StatsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new StatsScreen(), null);
        }
    }
}
