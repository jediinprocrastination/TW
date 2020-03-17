using System;
using System.IO;
using System.Linq;
using System.Text;

using Id3;

namespace Tw.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.Unicode;

            var artists = Directory
                .GetDirectories(Environment.CurrentDirectory);

            var command = args != null ? args[0] as string : string.Empty;

            if (string.Equals(command, "check", StringComparison.InvariantCultureIgnoreCase))
            {
                Check(artists, args);
            }
            else if (string.Equals(command, "fix", StringComparison.InvariantCultureIgnoreCase))
            {
                Fix(artists);
            }
            else if (string.Equals(command, "write", StringComparison.InvariantCultureIgnoreCase))
            {
                Write(artists);
            }

            //foreach (var artist in artists)
            //{
            //    var albums = Directory.GetDirectories(artist);
            //    foreach (var album in albums)
            //    {
            //        var tracks = Directory.GetFiles(album, "*.mp3");
            //        foreach (var track in tracks)
            //        {
            //            //using (var mp3file = new Mp3File(track, Mp3Permissions.ReadWrite))
            //            //{
            //            //    var artistName = track.ExtractArtistName();
            //            //    var contributingArtists = track.ExtractContributingArtists();

                //            //    var tag = mp3file.GetTag(Id3TagFamily.Version2x);

                //            //    tag.Band.EncodingType = Id3TextEncoding.Unicode;
                //            //    tag.Band.Value = artistName;

                //            //    tag.Album.EncodingType = Id3TextEncoding.Unicode;
                //            //    tag.Album.Value = track.ExtractAlbumName();

                //            //    tag.Year.EncodingType = Id3TextEncoding.Unicode;
                //            //    tag.Year.Value = track.ExtractAlbumYear();

                //            //    tag.Title.EncodingType = Id3TextEncoding.Unicode;
                //            //    tag.Title.Value = track.ExtractTrackName();

                //            //    tag.Track.EncodingType = Id3TextEncoding.Unicode;
                //            //    tag.Track.Value = track.ExtractTrackNumber();

                //            //    tag.Artists.EncodingType = Id3TextEncoding.Unicode;
                //            //    tag.Artists.Value.Clear();
                //            //    tag.Artists.Value.Add(artistName);

                //            //    if (contributingArtists != null)
                //            //    {
                //            //        contributingArtists.ToList()
                //            //            .ForEach(x => tag.Artists.Value.Add(x));
                //            //    }

                //            //    mp3file.WriteTag(tag);
                //            //}
                //        }
                //    }
                //}
        }

        static void Check(string[] artists, string[] args)
        {
            var defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;

            var notificationMode = "track";

            if (args.Length > 1)
            {
                notificationMode = args[1];
            }

            foreach (var artist in artists)
            {
                var albums = Directory.GetDirectories(artist);
                foreach (var album in albums)
                {
                    var tracks = Directory.GetFiles(album, "*.mp3");
                    foreach (var track in tracks)
                    {
                        if (!track.IsFormatValid())
                        {
                            if (ReferenceEquals(track, tracks.First()))
                            {
                                System.Console.WriteLine("Unexpected format: " + album);
                            }
                            //System.Console.WriteLine("Unexpected format: " + track);
                        }
                    }
                }
            }

            System.Console.ForegroundColor = defaultColor;
        }

        static void Fix(string[] artists)
        {
            System.Console.WriteLine("Fix command started");
            System.Console.ReadLine();

            var defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Green;

            foreach (var artist in artists)
            {
                var albums = Directory.GetDirectories(artist);
                foreach (var album in albums)
                {
                    var tracks = Directory.GetFiles(album, "*.mp3");
                    foreach (var track in tracks)
                    {
                        if (!track.IsFormatValid())
                        {
                            var fileName = Path.GetFileName(track);
                            var directoryPath = Path.GetDirectoryName(track);

                            NameHelper.TrackInfo trackInfo;
                            if (track.TryParsePath(out trackInfo))
                            {
                                var fixedAlbumName = string.Format("[{0}] {1}", trackInfo.Year, trackInfo.Album);
                                var fixedFileName = string.Format("{0}. {1}.mp3", trackInfo.Number, trackInfo.Name);
                                var fixedPath = Path.Combine(trackInfo.Folder, trackInfo.Artist, fixedAlbumName, fixedFileName);
                                
                                try
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(fixedPath));
                                    File.Move(track, fixedPath);

                                    System.Console.WriteLine("File name fixed: {0} --> {1}", track, fixedPath);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }

            System.Console.ForegroundColor = defaultColor;
        }

        static void Write(string[] artists)
        {
            var defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Blue;
            
            foreach (var artist in artists)
            {
                var albums = Directory.GetDirectories(artist);
                foreach (var album in albums)
                {
                    var tracks = Directory.GetFiles(album, "*.mp3");
                    foreach (var track in tracks)
                    {
                        if (track.IsFormatValid())
                        {
                            try
                            {
                                using (var mp3file = new Mp3File(track, Mp3Permissions.ReadWrite))
                                {
                                    var artistName = track.ExtractArtistName();
                                    var tag = mp3file.GetTag(Id3TagFamily.Version2x);

                                    tag.Band.EncodingType = Id3TextEncoding.Unicode;
                                    tag.Band.Value = artistName;

                                    tag.Album.EncodingType = Id3TextEncoding.Unicode;
                                    tag.Album.Value = track.ExtractAlbumName();

                                    tag.Year.EncodingType = Id3TextEncoding.Unicode;
                                    tag.Year.Value = track.ExtractAlbumYear();

                                    tag.Title.EncodingType = Id3TextEncoding.Unicode;
                                    tag.Title.Value = track.ExtractTrackName();

                                    tag.Track.EncodingType = Id3TextEncoding.Unicode;
                                    tag.Track.Value = track.ExtractTrackNumber();

                                    tag.Artists.EncodingType = Id3TextEncoding.Unicode;
                                    tag.Artists.Value.Clear();
                                    tag.Artists.Value.Add(artistName);

                                    mp3file.WriteTag(tag);
                                }

                                System.Console.WriteLine("Tags wited to: " + track);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            System.Console.ForegroundColor = defaultColor;
        }
    }
}
