using System;
using BepInEx;
using UnityEngine;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using System.Threading;
using On;
using IL;

namespace cruxtruderCountdown
{
    [BepInPlugin(MOD_ID, "cruxtruder-countdown", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        private const string MOD_ID = "derpyjacob903.cruxtrudercountdown";
        private const string RainRequestStopSongError = "RainRequestStopSong() called! /n We ain't doing that! >:3";
        private bool countdown = false;
        private bool forceCountdown = false;
        private bool eepy = false;

        // Add hooks
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);
            countdown = false;
            // Put your custom hooks here!
            //On.RainWorldGame.InClosingShelter += RainWorldGame_InClosingShelter;
            On.RainWorldGame.GoToDeathScreen += RainWorldGame_GoToDeathScreen;
            On.RainCycle.Update += RainCycle_Update;
            On.Music.MusicPlayer.RainRequestStopSong += MusicPlayer_RainRequestStopSong;
            On.Player.SleepUpdate += Player_SleepUpdate;
            On.Menu.ModdingMenu.Init += ModdingMenu_Init;
            On.Player.Update += Player_Update;
        }

        private void MusicPlayer_RainRequestStopSong(On.Music.MusicPlayer.orig_RainRequestStopSong orig, Music.MusicPlayer self)
        {
            // using this to stop non-countdown music >:3
            if (self.song.name != "TGP_03 - cruxtruder countdown") { orig(self); }
            else {
                //do nothing :3
                Logger.LogMessage(RainRequestStopSongError); 
            } 
        }

        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            if (self.stillInStartShelter && countdown && !forceCountdown)
            {
                countdown = false;
            }
            //else
                //if (forceCountdown) { countdown = true; }
            
            orig(self, eu);
        }

        private void ModdingMenu_Init(On.Menu.ModdingMenu.orig_Init orig, Menu.ModdingMenu self)
        {
            orig(self);
        }

        private void RainWorldGame_GoToDeathScreen(On.RainWorldGame.orig_GoToDeathScreen orig, RainWorldGame self)
        {
            orig(self);
            countdown = false;
            StopMusicEvent stopMusicEvent = new StopMusicEvent();
            stopMusicEvent.type = StopMusicEvent.Type.SpecificSong;
            stopMusicEvent.songName = "TGP_03 - cruxtruder countdown";
            stopMusicEvent.prio = 10f;
            self.world.game.manager.musicPlayer.GameRequestsSongStop(stopMusicEvent);
        }

        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {

        }

        private void Player_SleepUpdate(On.Player.orig_SleepUpdate orig, Player self)
        {
            orig(self);
            //if(self.Sleeping) { eepy = true; }
            //else { eepy = false; }
        }

    //private void MusicPlayer_NewCycleEvent(On.Music.MusicPlayer.orig_NewCycleEvent orig, Music.MusicPlayer self)

        //private bool RainWorldGame_InClosingShelter(On.RainWorldGame.orig_InClosingShelter orig, RainWorldGame self)
        //{
            //eepy = orig(self);
            //orig(self);
        //}

        private void RainCycle_Update(On.RainCycle.orig_Update orig, RainCycle self)
        {
            orig(self);
            //if (self.TimeUntilRain > (int)(167f * 40f) && countdown == true) 
            //{
            //countdown = false;
            //eepy = false;
            //}
            if (self.TimeUntilRain <= (int)(120f * 40f) && !countdown)
            {
                forceCountdown = true;
            }
            if (self.TimeUntilRain <= (int)(167f * 40f) && !countdown || forceCountdown)
            {
                if ((self.world.game.manager.musicPlayer != null && (self.world.game.manager.musicPlayer.song != null || self.world.game.manager.musicPlayer.nextSong != null)) && self.world.game.manager.musicPlayer.song.name != "TGP_03 - cruxtruder countdown")
                {
                    self.world.game.manager.musicPlayer.RainRequestStopSong();
                }
                //StopMusicEvent stopMusicEvent = new StopMusicEvent();
                //stopMusicEvent.type = StopMusicEvent.Type.AllButSpecific;
                //stopMusicEvent.songName = "TGP_03 - cruxtruder countdown";
                //stopMusicEvent.prio = 999f;
                //stopMusicEvent.fadeOutTime = 1f;
                //self.world.game.manager.musicPlayer.GameRequestsSongStop(stopMusicEvent);

                //eepy = true;

                //if (eepy == true) {
                //adampted from the part that plays 
                MusicEvent musicEvent = new MusicEvent();
                //musicEvent.cyclesRest = -1;
                musicEvent.prio = 9f;
                musicEvent.stopAtDeath = false;
                musicEvent.stopAtGate = false;
                musicEvent.volume = 0.5f;
                musicEvent.songName = "TGP_03 - cruxtruder countdown";
                //musicEvent.fadeOutAtThreat = false;
                self.world.game.manager.musicPlayer.GameRequestsSong(musicEvent);
                countdown = true;
                //}
            }
            //if (eepy == true && countdown == true) // In a Closing Shelter
            //{
                //StopMusicEvent stopMusicEvent = new StopMusicEvent();
                //stopMusicEvent.type = StopMusicEvent.Type.SpecificSong;
                //stopMusicEvent.songName = "TGP_03 - cruxtruder countdown";
                //stopMusicEvent.prio = 3f;
                //self.world.game.manager.musicPlayer.GameRequestsSongStop(stopMusicEvent);

                //eepy = false;
            //}
            //eepy = false;
            //else

        }

    }
}