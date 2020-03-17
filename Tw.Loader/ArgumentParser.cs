using System.Collections.Generic;
using System.Linq;

namespace TagWriter.Loader
{
  internal class ArgumentParser
  {
    private readonly IList<string> _arguments;
    private bool? _silentMode;
    private string _language;

    public ArgumentParser(string[] arguments)
    {
      _arguments = new List<string>(arguments ?? System.Array.Empty<string>());
      _arguments = _arguments.Select(x => x.Trim().ToUpper()).ToList();

      if (_arguments.Contains("/?") ||
          _arguments.Contains("/HELP"))
      {
      }
      else
      {
        Parse();
      }
    }

    public string Path
     => _arguments.Count > 0 ? _arguments[0] : null;

    public bool SilentMode
      => _silentMode ?? false;

    public string Language
      => _language ?? "ENG";

    private void Parse()
    {
      var parameters = _arguments.Skip(1).ToList();
      var enumerator = parameters.GetEnumerator();

      while (enumerator.MoveNext())
      {
        var parameter = enumerator.Current;

        if (string.Equals(parameter, "/LANG") ||
            string.Equals(parameter, "/LANGUAGE"))
        {
          if (enumerator.MoveNext())
            _language = enumerator.Current;
          continue;
        }

        if (string.Equals(parameter, "/S") ||
            string.Equals(parameter, "/SILENT"))
        {
          _silentMode = true;
        }
      }

      enumerator.Dispose();
    }
  }
}
