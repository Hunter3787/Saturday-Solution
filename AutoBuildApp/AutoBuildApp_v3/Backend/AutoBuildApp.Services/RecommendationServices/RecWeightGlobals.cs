using System;
namespace AutoBuildApp.Services.RecommendationServices
{
    public static class RecWeightGlobals
    {
        public const int BONUS_VALUE = 100;
        public static readonly double[] GAMING_GPU_WEIGHTS = { -1.75, 1.1, 1.3, 1.1, 1.15, -1.4 };
        public static readonly double[] GAMING_CPU_WEIGHTS = { -1.75, 1.5, -1.3 };
        public static readonly double[] GAMING_PSU_WEIGHTS = { -1.75, 1.4 };
        public static readonly double[] GAMING_RAM_WEIGHTS = { -1.75, 1.3, -2 };
        public static readonly double[] GAMING_CASE_WEIGHTS = { -1.75, 1, 1 };
        public static readonly double[] GAMING_COOLER_WEIGHTS = { -1.75, 1.3, 1 };
        //public static readonly double[] GAMING_MONITOR_WEIGHTS = { -1.75 };
        public static readonly double[] GAMING_MOTHERBOARD_WEIGHTS = { -1.75, 2 };
        public static readonly double[] GAMING_HARDDRIVE_WEIGHTS = { -1.75, 2, 1.1 };

        public static readonly double[] ARTIST_GPU_WEIGHTS = { -1.75, 1.3, 1.1, 1.1, 1.15, -1.4 };
        public static readonly double[] ARTIST_CPU_WEIGHTS = { -1.75, 1.6, -1.3 };
        public static readonly double[] ARTIST_PSU_WEIGHTS = { -1.75, 1.4 };
        public static readonly double[] ARTIST_RAM_WEIGHTS = { -1.75, 1.3, -1.2 };
        public static readonly double[] ARTIST_CASE_WEIGHTS = { -1.75, 1, 1.2 };
        public static readonly double[] ARTIST_COOLER_WEIGHTS = { -1.75, 1.2, -1.1 };
        //public static readonly double[] ARTIST_MONITOR_WEIGHTS = { -1.75 };
        public static readonly double[] ARTIST_MOTHERBOARD_WEIGHTS = { -1.75, 2 };
        public static readonly double[] ARTIST_HARDDRIVE_WEIGHTS = { -1.75, 2, 2 };

        public static readonly double[] WORK_GPU_WEIGHTS = { -1.75, 1.1, 1, .8, 1, -1.5 };
        public static readonly double[] WORK_CPU_WEIGHTS = { -1.75, 1.5, -1.5 };
        public static readonly double[] WORK_PSU_WEIGHTS = { -1.75, 1 };
        public static readonly double[] WORK_RAM_WEIGHTS = { -1.75, 1.4, -1 };
        public static readonly double[] WORK_CASE_WEIGHTS = { -1.75, .8, 1.2 };
        public static readonly double[] WORK_COOLER_WEIGHTS = { -1.75, .8, -1.2 };
        //public static readonly double[] WORK_MONITOR_WEIGHTS = {-1.75 };
        public static readonly double[] WORK_MOTHERBOARD_WEIGHTS = { -1.75, 2 };
        public static readonly double[] WORK_HARDDRIVE_WEIGHTS = { -1.75, 2, 1 };
    }
}
