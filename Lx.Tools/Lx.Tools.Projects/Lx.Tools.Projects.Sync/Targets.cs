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
        Monotouch = 3,

        /// <summary>
        ///     The monodroid is for Android targeting
        /// </summary>
        Monodroid = 4,

        /// <summary>
        ///     The Framework 2.0 targeting
        /// </summary>
        Net2Dot0 = 5,

        /// <summary>
        ///     The Framework 3.5 targeting
        /// </summary>
        Net3Dot5 = 6,

        /// <summary>
        ///     The Framework 4.0 targeting
        /// </summary>
        Net4Dot0 = 7,

        /// <summary>
        ///     The Framework 4.5 targeting
        /// </summary>
        Net4Dot5 = 8
    }
}