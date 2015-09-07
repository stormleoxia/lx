namespace Lx.Tools.Projects.Sync
{
    /// <summary>
    ///     Each platform has a given target
    /// </summary>
    public enum Targets
    {
        /// <summary>
        ///     All is the default when there is not specific targeting
        /// </summary>
        All = 0,

        Basic = 1,
        /// <summary>
        ///     The xammac is for Mac targeting
        /// </summary>
        Xammac = 2,
        XammacNet4Dot5 = 3,
        Mobile = 4,
        MobileStatic = 5,

        /// <summary>
        ///     The monodroid is for Android targeting
        /// </summary>
        Monodroid = 6,

        Monotouch = 7,

        /// <summary>
        ///     The Framework 2.0 targeting
        /// </summary>
        Net2Dot0 = 8,

        /// <summary>
        ///     The Framework 3.5 targeting
        /// </summary>
        Net3Dot5 = 9,

        /// <summary>
        ///     The Framework 4.0 targeting
        /// </summary>
        Net4Dot0 = 10,

        /// <summary>
        ///     The Framework 4.5 targeting
        /// </summary>
        Net4Dot5 = 11
    }
}