using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio0_1016816.Models;
using Laboratorio0_1016816.Tools;
using System.IO;
using WMPLib;

namespace Laboratorio0_1016816.Controllers
{
    public class SongController : Controller
    {
        
        // about the general <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<---------------------------------------------------------------------
        // GET: Song
        public ActionResult Index()
        {
            Singleton.Data.PositionActualSong = -1;
            return View(Singleton.Data.Playlist.Values.ToList());
        }

        // GET: Song/Details/5
        public ActionResult Details(string id)
        {
            return View(Singleton.Data.Playlist[id]);
        }

        // GET: Song/Edit/5
        public ActionResult Edit(string id)
        {
            return View(Singleton.Data.Playlist[id]);
        }

        // POST: Song/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Singleton.Data.Playlist.Remove(id);
                Singleton.Data.specialList.RemoveAll(x => x.name == id);
                SongModel NewSong = new SongModel();
                NewSong.name = collection["name"].Split(',')[1];
                NewSong.singer = collection["singer"];
                NewSong.duration = collection["duration"];
                NewSong.durationSeconds = TimeSpan.Parse(NewSong.duration);
                Singleton.Data.Playlist.Add(NewSong.name, NewSong);
                Singleton.Data.specialList.Add(NewSong);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Song/Delete/5
        public ActionResult Delete(string id)
        {
            return View(Singleton.Data.Playlist[id]);
        }

        // POST: Song/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Singleton.Data.Playlist.Remove(id);
                Singleton.Data.specialList.RemoveAll(x => x.name == id);
                Singleton.Data.SoundPlayer.close();
                Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Song/Play/5
        //[HttpPost]
        public ActionResult Play(string id, string view = "Index")
        {
            try
            {
                //// TODO: Add update logic here

                Singleton.Data.SoundPlayer.URL = Singleton.Data.Playlist[id].SoundPath;
            }
            catch
            {
            }
            return RedirectToAction(view);
        }

        public ActionResult RePlay(string view = "Index")
        {
            try
            {
                //// TODO: Add update logic here
                Singleton.Data.SoundPlayer.controls.play();
            }
            catch
            {
            }
            return RedirectToAction(view);
        }


        public ActionResult Pause(string view = "Index")
        {
            try
            {
                //// TODO: Add update logic here
                Singleton.Data.SoundPlayer.controls.pause();
            }
            catch
            {
            }
            return RedirectToAction(view);
        }


        public ActionResult SaveSong(HttpPostedFileBase[] file)
        {
            if (file == null) return RedirectToAction("Index");

            TagLib.File tagFile;
            foreach (var f in file)
            {
                if (f != null)
                {
                    tagFile = TagLib.File.Create(f.FileName);
                    SongModel NewSong = new SongModel();

                    NewSong.name = tagFile.Tag.Title;
                    if (NewSong.name == null)
                    {
                        NewSong.name = f.FileName.Split('\\')[f.FileName.Split('\\').Length - 1].Replace(".mp3", "");
                    }

                    try
                    {
                        if (Singleton.Data.Playlist[NewSong.name] != null)
                        {
                            //of this form, the songs, are upload one.
                        }
                    }
                    catch
                    {
                        string artist = "";
                        for (int i = 0; i < tagFile.Tag.Artists.Length; i++)
                        {
                            artist = " " + artist + " " + tagFile.Tag.Artists[i] + ".";
                        }
                        NewSong.singer = artist.Trim();
                        NewSong.durationSeconds = tagFile.Properties.Duration;
                        NewSong.duration = tagFile.Properties.Duration.ToString();
                        NewSong.SoundPath = f.FileName;

                        Singleton.Data.Playlist.Add(NewSong.name, NewSong);
                    }
                }
            }
           
            return RedirectToAction("Index");
        }

        
        public ActionResult Search()
        {
            return View(Singleton.Data.Playlist.Values.ToList());
        }

        [HttpPost]
        public ActionResult Search(string id)
        {
            List<SongModel> contain = new List<SongModel>();
            try
            {
                contain.Add(Singleton.Data.Playlist[id]);
                return View(contain);
            }
            catch
            {
                return View(contain);
            }
        }

        public ActionResult Back()
        {
            Singleton.Data.SoundPlayer.close();
            Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
            return View("index", Singleton.Data.Playlist.Values.ToList());
        }

        // about the special Playlist <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<---------------------------------------------------------------------

        public ActionResult InitialGoPlaylist()
        {
            Singleton.Data.SoundPlayer.close();
            Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
            return View("GoPlayList",Singleton.Data.specialList);
        }

        public ActionResult GoPlaylist()
        {
            return View(Singleton.Data.specialList);
        }

        public ActionResult SpecialDelete(string id)
        {
            int index = Singleton.Data.specialList.FindIndex(x => x.name == id);
            if(Singleton.Data.SoundPlayer.URL == Singleton.Data.specialList[index].SoundPath)
            {
                Singleton.Data.SoundPlayer.close();
                Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
                Singleton.Data.PositionActualSong = -1;
            }
            Singleton.Data.specialList.RemoveAll(x => x.name == id);
            return RedirectToAction("GoPlaylist");
        }

        public ActionResult AddToSpecialPlaylist(string id)
        {
            if (Singleton.Data.specialList.FindAll(x => x.name==id).Count==0)
            {
                Singleton.Data.specialList.Add(Singleton.Data.Playlist[id]);
            }
            return RedirectToAction("Index");
        }
        // about sorts for specialPlaylist <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<---------------------------------------------------------------------
        public ActionResult AscendingNames()
        {
            Singleton.Data.SoundPlayer.close();
            Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
            Singleton.Data.PositionActualSong = -1;
            Singleton.Data.specialList.Sort((x, y) => x.name.CompareTo(y.name));
            return RedirectToAction("GoPlaylist");
        }

        public ActionResult DescendingNames()
        {
            Singleton.Data.SoundPlayer.close();
            Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
            Singleton.Data.PositionActualSong = -1;
            Singleton.Data.specialList.Sort((x, y) => y.name.CompareTo(x.name));
            return RedirectToAction("GoPlaylist");
        }

        public ActionResult AscendingDuration()
        {
            Singleton.Data.SoundPlayer.close();
            Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
            Singleton.Data.PositionActualSong = -1;
            Singleton.Data.specialList.Sort((x, y) => x.durationSeconds.CompareTo(y.durationSeconds));
            return RedirectToAction("GoPlaylist");
        }

        public ActionResult DescendingDuration()
        {
            Singleton.Data.SoundPlayer.close();
            Singleton.Data.SoundPlayer = new WindowsMediaPlayer();
            Singleton.Data.PositionActualSong = -1;
            Singleton.Data.specialList.Sort((x, y) => y.durationSeconds.CompareTo(x.durationSeconds));
            return RedirectToAction("GoPlaylist");
        }

        public ActionResult PlayPlaylist(string id)
        {
            Singleton.Data.PositionActualSong = Singleton.Data.specialList.FindIndex(x=>x.name==id);
            return Play(id,"GoPlaylist");
        }

        public ActionResult RePlayPlaylist()
        {
            return RePlay("GoPlaylist");
        }


        public ActionResult PausePlaylist()
        {
            return Pause("GoPlaylist");
        }

        public ActionResult Next()
        {
            if (Singleton.Data.specialList.Count == 0 || Singleton.Data.PositionActualSong==-1)
                return RedirectToAction("GoPlaylist");
            
            Singleton.Data.PositionActualSong += 1;
            if (Singleton.Data.PositionActualSong == Singleton.Data.specialList.Count)
            {
                Singleton.Data.PositionActualSong = 0;
            }
            Singleton.Data.SoundPlayer.URL = Singleton.Data.specialList[Singleton.Data.PositionActualSong].SoundPath;
            Singleton.Data.SoundPlayer.controls.play();
            return RedirectToAction("GoPlaylist");
        }

        public ActionResult Previous()
        {
            if (Singleton.Data.specialList.Count == 0 || Singleton.Data.PositionActualSong == -1)
                return RedirectToAction("GoPlaylist");

            Singleton.Data.PositionActualSong -= 1;
            if (Singleton.Data.PositionActualSong == -1)
            {
                Singleton.Data.PositionActualSong = Singleton.Data.specialList.Count-1;
            }
            Singleton.Data.SoundPlayer.URL = Singleton.Data.specialList[Singleton.Data.PositionActualSong].SoundPath;
            Singleton.Data.SoundPlayer.controls.play();
            return RedirectToAction("GoPlaylist");
        }
    }
}
