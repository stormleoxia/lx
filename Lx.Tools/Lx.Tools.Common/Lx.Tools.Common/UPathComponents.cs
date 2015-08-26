using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lx.Tools.Common
{
    public class UPathComponents
    {
        private readonly string _path;

        public UPathComponents(string path, bool isAbsolute)
        {
            _path = path;
            if (_path.Contains(':'))
            {
                var splits = _path.Split(':');
                Drive = splits[0];
                _path = splits[1];
            }
            Components = _path.Split('/', '\\');
            Components = ResolveComponents(Components);
            IsAbsolute = isAbsolute;
        }

        public UPathComponents(string drive, string[] newComponents, bool isAbsolute)
        {
            Drive = drive;
            Components = newComponents;
            IsAbsolute = isAbsolute;
        }

        public string[] Components { get; private set; }
        public bool IsAbsolute { get; private set; }
        public string Drive { get; set; }

        private string[] ResolveComponents(string[] components)
        {
            var result = new List<string>();
            foreach (var component in components)
            {
                if (component == ".")
                {
                    continue;
                }
                if (component == ".." && result.Count > 0 && result.Last() != "..")
                {
                    result.RemoveAt(result.Count - 1);
                }
                else
                {
                    result.Add(component);
                }
            }
            if (result.Count > 0)
            {
                if (result.Last() == string.Empty)
                {
                    result.RemoveAt(result.Count - 1);
                }
            }
            return result.ToArray();
        }

        public override string ToString()
        {
            var first = string.Empty;
            if (IsAbsolute)
            {
                first = Drive;
                if (!string.IsNullOrEmpty(first))
                {
                    first += ":";
                }
                else
                {
                    first = Path.DirectorySeparatorChar.ToString();
                }
            }
            return Components.Aggregate(first, (x, y) =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    return y;
                }
                if (string.IsNullOrEmpty(y))
                {
                    return x;
                }
                if (x == Path.DirectorySeparatorChar.ToString())
                {
                    return x + y;
                }
                return x + Path.DirectorySeparatorChar + y;
            });
        }
    }
}