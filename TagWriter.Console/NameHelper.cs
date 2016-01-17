using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TagWriter.Console
{
    public static class NameHelper
    {
        private const string PATH_PATTERN = @"^.*\\(?<artist>.+)\\\[(?<year>\d{4})\] (?<album>.+)\\(?<no>\d{2})\. (((?<track>.+) \(feat\. (?<artists>.+)\))|(?<track>.+))\.mp3$";

        private const string INVALID_PATH_PATTERN_01 = @"^(?<folder>.*)\\(?<artist>.+)\\\[(?<year>\d{4})\] (?<album>.+)\\(?<no>\d{2}) (?<track>.+)\.mp3$";
        private const string INVALID_PATH_PATTERN_02 = @"^(?<folder>.*)\\(?<artist>.+)\\\[(?<year>\d{4})\] (?<album>.+)\\(?<no>\d{2})\.(?<track>.+)\.mp3$";
        private const string INVALID_PATH_PATTERN_03 = @"^(?<folder>.*)\\(?<artist>.+)\\\[(?<year>\d{4})\] (?<album>.+)\\(?<no>\d{2})-(?<track>.+)\.mp3$";
        private const string INVALID_PATH_PATTERN_04 = @"^(?<folder>.*)\\(?<artist>.+)\\\[(?<year>\d{4})\] (?<album>.+)\\(?<no>\d{2}) - (?<track>.+)\.mp3$";
        private const string INVALID_PATH_PATTERN_05 = @"^(?<folder>.*)\\(?<artist>.+)\\\[(?<year>\d{4})\] (?<album>.+)\\(?<no>\d{2})-(?<track>.+)\.Mp3$";

        private const string INVALID_PATH_PATTERN_06 = @"^(?<folder>.*)\\(?<artist>.+)\\\((?<year>\d{4})\) (?<album>.+)\\(?<no>\d{2}) - (?<track>.+)\.mp3$";
        private const string INVALID_PATH_PATTERN_07 = @"^(?<folder>.*)\\(?<artist>.+)\\\d{2}-(?<album>.+) \((?<year>\d{4})\)\\(?<no>\d{2})\. (?<track>.+)\.mp3$";
        private const string INVALID_PATH_PATTERN_08 = @"^(?<folder>.*)\\(?<artist>.+)\\(?<album>.+) \((?<year>\d{4})\)\\(?<no>\d{2})\. (?<track>.+)\.mp3$";

        private static readonly Regex _pathRegex;
        private static readonly List<Regex> _invalidPathRegexes;

        static NameHelper()
        {
            _pathRegex = new Regex(PATH_PATTERN);

            _invalidPathRegexes = new List<Regex>
            {
                new Regex(INVALID_PATH_PATTERN_01),
                new Regex(INVALID_PATH_PATTERN_02),
                new Regex(INVALID_PATH_PATTERN_03),
                new Regex(INVALID_PATH_PATTERN_04),
                new Regex(INVALID_PATH_PATTERN_05),
                new Regex(INVALID_PATH_PATTERN_06),
                new Regex(INVALID_PATH_PATTERN_07),
                new Regex(INVALID_PATH_PATTERN_07)
            };
        }

        public static bool IsFormatValid(this string path)
        {
            return _pathRegex.IsMatch(path);
        }

        public static bool TryParsePath(this string path, out TrackInfo trackInfo)
        {
            try
            {
                foreach (var regex in _invalidPathRegexes)
                {
                    var match = regex.Match(path);
                    if (match.Success)
                    {
                        trackInfo = new TrackInfo
                        {
                            Folder = match.Groups["folder"].Value,
                            Artist = match.Groups["artist"].Value,
                            Year = match.Groups["year"].Value,
                            Album = match.Groups["album"].Value,
                            Number = match.Groups["no"].Value,
                            Name = match.Groups["track"].Value
                        };

                        return true;
                    }
                }

                trackInfo = null;
                return false;
            }
            catch
            {
                trackInfo = null;
                return false;
            }
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

        public class TrackInfo
        {
            public string Folder { get; set; }
            public string Artist { get; set; }
            public string Year { get; set; }
            public string Album { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }
    }
}
