using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Id3;

namespace TagWriter.Console
{
    class Program
    {
        private const string ROOT_DIRECTORY = @"D:\Music";

        static void Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.Unicode;
            //Debug.Assert(false);
            

            var serialNumbers = "01016359482619120376802384023984023840283049238048";

            var bytes = new byte[serialNumbers.Length * sizeof(char)];
            Buffer.BlockCopy(serialNumbers.ToCharArray(), 0, bytes, 0, bytes.Length);

            var sha = new SHA1CryptoServiceProvider();
            var result = sha.ComputeHash(bytes);

            var chars = new char[result.Length / sizeof(char)];
            Buffer.BlockCopy(result, 0, chars, 0, result.Length);

            var output = new string(chars);
            System.Console.WriteLine(serialNumbers.GetHashCode());

            //System.Console.OutputEncoding = Encoding.Unicode;

            //var artists = Directory.GetDirectories(ROOT_DIRECTORY);
            //foreach (var artist in artists)
            //{
            //    var albums = Directory.GetDirectories(artist);
            //    foreach (var album in albums)
            //    {
            //        var tracks = Directory.GetFiles(album, "*.mp3");
            //        foreach (var track in tracks)
            //        {
            //            using (var mp3file = new Mp3File(track, Mp3Permissions.ReadWrite))
            //            {
            //                var artistName = track.ExtractArtistName();
            //                var contributingArtists = track.ExtractContributingArtists();

            //                var tag = mp3file.GetTag(Id3TagFamily.Version2x);

            //                tag.Band.EncodingType = Id3TextEncoding.Unicode;
            //                tag.Band.Value = artistName;

            //                tag.Album.EncodingType = Id3TextEncoding.Unicode;
            //                tag.Album.Value = track.ExtractAlbumName();

            //                tag.Year.EncodingType = Id3TextEncoding.Unicode;
            //                tag.Year.Value = track.ExtractAlbumYear();

            //                tag.Title.EncodingType = Id3TextEncoding.Unicode;
            //                tag.Title.Value = track.ExtractTrackName();

            //                tag.Track.EncodingType = Id3TextEncoding.Unicode;
            //                tag.Track.Value = track.ExtractTrackNumber();

            //                tag.Artists.EncodingType = Id3TextEncoding.Unicode;
            //                tag.Artists.Value.Clear();
            //                tag.Artists.Value.Add(artistName);

            //                if (contributingArtists != null)
            //                {
            //                    contributingArtists.ToList()
            //                        .ForEach(x => tag.Artists.Value.Add(x));
            //                }

            //                mp3file.WriteTag(tag);
            //            }
            //        }
            //    }
            //}
        }
    }
}
