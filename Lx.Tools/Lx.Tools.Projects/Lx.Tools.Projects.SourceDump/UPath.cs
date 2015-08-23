using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lx.Tools.Projects.SourceDump
{
    public class UPath
    {
        private readonly UPathComponents _absolute;
        private readonly UPathComponents _relative;

        public UPath(string path)
        {
            if (IsPathRooted(path))
            {
                _absolute = new UPathComponents(path, true);
            }
            else
            {
                _relative = new UPathComponents(path, false);
            }
            if (_absolute == null)
            {
                if (File.Exists(path))
                {
                    var fileInfo = new FileInfo(path);
                    _absolute = new UPathComponents(fileInfo.FullName, true);
                }
                else if (Directory.Exists(path))
                {
                    var directoryInfo = new DirectoryInfo(path);
                    _absolute = new UPathComponents(directoryInfo.FullName, true);
                }
            }
        }

        private UPath(UPathComponents path)
        {
            if (path.IsAbsolute)
            {
                _absolute = path;
            }
            else
            {
                _relative = path;
            }
        }

        public bool HasAbsolute
        {
            get { return _absolute != null; }
        }

        public bool HasRelative
        {
            get { return _relative != null; }
        }

        public UPathComponents Absolute
        {
            get { return _absolute; }
        }

        private bool IsPathRooted(string path)
        {
            return (path.Contains(':') || path[0] == '/');
        }

        public UPath MakeRelativeUPath(UPath file)
        {
            if (file.HasAbsolute && HasAbsolute)
            {
                return MakeRelativeUPath(_absolute, file._absolute);
            }
            if (file.HasRelative && HasRelative)
            {
                return MakeRelativeUPath(_relative, file._relative);
            }
            throw new InvalidOperationException(
                "We cannot make relative path between two not found absolute and relative paths:" + this + " and " +
                file);
        }

        private UPath MakeRelativeUPath(UPathComponents referal, UPathComponents referee)
        {
            var fileComponents = new Queue<string>(referee.Components.Take(referee.Components.Length - 1));
            var components = new Queue<string>(referal.Components);
            var newComponents = new List<string>();
            var differencefound = false;
            while (components.Count > 0 && fileComponents.Count > 0)
            {
                var fileComponent = fileComponents.Peek();
                var component = components.Peek();
                if (component != fileComponent || differencefound)
                {
                    differencefound = true;
                    newComponents.Add("..");
                    components.Dequeue();
                }
                else
                {
                    fileComponents.Dequeue();
                    components.Dequeue();
                }
            }
            while (components.Count > 0)
            {
                newComponents.Add("..");
                components.Dequeue();
            }
            while (fileComponents.Count > 0)
            {
                newComponents.Add(fileComponents.Dequeue());
            }
            newComponents.Add(referee.Components.Last());
            return new UPath(new UPathComponents(referal.Drive, newComponents.ToArray(), false));
        }

        public override string ToString()
        {
            var first = string.Empty;
            string[] components;
            if (_relative != null)
            {
                components = _relative.Components;
            }
            else
            {
                components = _absolute.Components;
                first = _absolute.Drive;
                if (!string.IsNullOrEmpty(first))
                {
                    first += ":";
                }
            }
            return components.Aggregate(first, (x, y) =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    return y;
                }
                return x + Path.DirectorySeparatorChar + y;
            });
        }
    }
}