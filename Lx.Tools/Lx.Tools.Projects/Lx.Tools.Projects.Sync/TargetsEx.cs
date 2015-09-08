using System;
using System.Linq;

namespace Lx.Tools.Projects.Sync
{
    internal static class TargetsEx
    {
        private static readonly Targets[] _targetsValue;
        private static readonly Targets[] _targetsValueButAll;

        static TargetsEx()
        {
            _targetsValue = (Targets[]) Enum.GetValues(typeof (Targets));
            _targetsValueButAll = _targetsValue.Where(x => x != Targets.All).ToArray();
        }

        public static string Convert(this Targets target)
        {
            switch (target)
            {
                case Targets.All:
                    return string.Empty;
                case Targets.Basic:
                    return "basic";
                case Targets.Xammac:
                    return "xammac";
                case Targets.XammacNet4Dot5:
                    return "xammac_net_4_5";
                case Targets.Monotouch:
                    return "monotouch";
                case Targets.Mobile:
                    return "mobile";
                case Targets.MobileStatic:
                    return "mobile_static";
                case Targets.Monodroid:
                    return "monodroid";
                case Targets.Net2Dot0:
                    return "net_2_0";
                case Targets.Net3Dot5:
                    return "net_3_5";
                case Targets.Net4Dot0:
                    return "net_4_0";
                case Targets.Net4Dot5:
                    return "net_4_5";
                default:
                    throw new ArgumentOutOfRangeException("target", target, null);
            }
        }

        public static Targets[] GetValues()
        {
            return _targetsValue;
        }

        public static Targets[] GetValuesButAll()
        {
            return _targetsValueButAll;
        }

        /// <summary>
        ///     Extracts target depending on the csproj name.
        ///     Try to find the target substring in the name for doing so.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Targets ExtractTarget(this string fileName)
        {
            if (fileName.Contains(Targets.XammacNet4Dot5.Convert()))
            {
                return Targets.XammacNet4Dot5;
            }
            foreach (var target in GetValuesButAll())
            {
                if (fileName.Contains(target.Convert()))
                {
                    return target;
                }
            }
            return Targets.All;
        }
    }
}