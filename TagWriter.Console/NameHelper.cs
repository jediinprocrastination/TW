using System.Text.RegularExpressions;

namespace TagWriter.Console
{
    public static class NameHelper
    {
        private const string PATH_PATTERN = @"^.*\\(?<artist>.+)\\\[(?<year>\d{4})\] (?<album>.+)\\(?<no>\d{2})\. (((?<track>.+) \(feat\. (?<artists>.+)\))|(?<track>.+))\.mp3$";
        private static readonly Regex _pathRegex;

        static NameHelper()
        {
            _pathRegex = new Regex(PATH_PATTERN);
        }

        public static string ExtractArtistName(this string path)
        {
            return _pathRegex.Match(path).Groups["artist"].Value;
        }

        public static string ExtractAlbumName(this string path)
        {
            return _pathRegex.Match(path).Groups["album"].Value;
        }

        public static int? ExtractAlbumYear(this string path)
        {
            int year;
            var yearString = _pathRegex.Match(path).Groups["year"].Value;
            
            if (int.TryParse(yearString, out year))
            {
                return year;
            }
            return null;
        }

        public static string ExtractTrackName(this string path)
        {
            return _pathRegex.Match(path).Groups["track"].Value;
        }

        public static int ExtractTrackNumber(this string path)
        {
            int number;
            var numberString = _pathRegex.Match(path).Groups["no"].Value;

            if (int.TryParse(numberString, out number))
            {
                return number;
            }
            return 0;
        }

        public static string[] ExtractContributingArtists(this string path)
        {
            var artistsString = _pathRegex.Match(path).Groups["artists"].Value;
            if (!string.IsNullOrEmpty(artistsString))
            {
                return artistsString.Split(',');
            }
            return null;
        }
    }
}
