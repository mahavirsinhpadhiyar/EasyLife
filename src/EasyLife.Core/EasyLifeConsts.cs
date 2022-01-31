using EasyLife.Debugging;

namespace EasyLife
{
    public class EasyLifeConsts
    {
        public const string LocalizationSourceName = "EasyLife";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "d4109e238b444afab4ac15f98eb5d9a9";
    }
}
