using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Laboratorio0_1016816.Models;
using WMPLib;

namespace Laboratorio0_1016816.Tools
{
    public class Singleton
    {
        public Dictionary<string, SongModel> Playlist = new Dictionary<string, SongModel>();
        public List<SongModel> specialList = new List<SongModel>();
        public WindowsMediaPlayer SoundPlayer = new WindowsMediaPlayer();
        public int PositionActualSong=-1;
        private static Singleton MainData;
        public static Singleton Data
        {
            get
            {
                if (MainData == null)
                {
                    MainData = new Singleton();
                }
                return MainData;
            }
            set
            {

            }
        }
    }
}