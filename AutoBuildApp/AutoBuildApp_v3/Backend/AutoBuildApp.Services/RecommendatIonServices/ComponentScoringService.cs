using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;

/**
 * This service will score the components according to a passed key value
 * that will determine the value of each specification with regards to computer
 * components.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.RecommendationServices
{
    public class ComponentScoringService
    {
        public readonly int BONUS_VALUE = 100;
        public readonly double[] GAMING_GPU_WEIGHTS = { -1.75, 1.1, 1.3, 1.1, 1.15, -1.4 };
        public readonly double[] GAMING_CPU_WEIGHTS = { -1.75, 1.5, -1.3 };
        public readonly double[] GAMING_PSU_WEIGHTS = { -1.75, 1.4 };
        public readonly double[] GAMING_RAM_WEIGHTS = { -1.75, 1.3, -2 };
        public readonly double[] GAMING_CASE_WEIGHTS = { -1.75, 1, 1 };
        public readonly double[] GAMING_COOLER_WEIGHTS = { -1.75, 1.3, 1 };
        //public readonly double[] GAMING_MONITOR_WEIGHTS = { -1.75 };
        public readonly double[] GAMING_MOTHERBOARD_WEIGHTS = { -1.75, 2 };
        public readonly double[] GAMING_HARDDRIVE_WEIGHTS = { -1.75, 2, 1.1 };

        public readonly double[] ARTIST_GPU_WEIGHTS = { -1.75, 1.3, 1.1, 1.1, 1.15, -1.4 };
        public readonly double[] ARTIST_CPU_WEIGHTS = { -1.75, 1.6 , -1.3 };
        public readonly double[] ARTIST_PSU_WEIGHTS = { -1.75, 1.4 };
        public readonly double[] ARTIST_RAM_WEIGHTS = { -1.75, 1.3, -1.2 };
        public readonly double[] ARTIST_CASE_WEIGHTS = { -1.75, 1, 1.2 };
        public readonly double[] ARTIST_COOLER_WEIGHTS = { -1.75, 1.2, -1.1 };
        //public readonly double[] ARTIST_MONITOR_WEIGHTS = { -1.75 };
        public readonly double[] ARTIST_MOTHERBOARD_WEIGHTS = { -1.75, 2 };
        public readonly double[] ARTIST_HARDDRIVE_WEIGHTS = { -1.75, 2, 2 };

        public readonly double[] WORK_GPU_WEIGHTS = { -1.75, 1.1, 1, .8, 1, -1.5};
        public readonly double[] WORK_CPU_WEIGHTS = { -1.75, 1.5, -1.5};
        public readonly double[] WORK_PSU_WEIGHTS = { -1.75, 1 };
        public readonly double[] WORK_RAM_WEIGHTS = { -1.75, 1.4, -1 };
        public readonly double[] WORK_CASE_WEIGHTS = { -1.75, .8, 1.2 };
        public readonly double[] WORK_COOLER_WEIGHTS = { -1.75, .8, -1.2 };
        //public readonly double[] WORK_MONITOR_WEIGHTS = {-1.75 };
        public readonly double[] WORK_MOTHERBOARD_WEIGHTS = { -1.75, 2 };
        public readonly double[] WORK_HARDDRIVE_WEIGHTS = { -1.75, 2, 1 };

        public ComponentScoringService()
        {
        }

        /// <summary>
        /// Scores an individual component based off the build type.
        /// Ignores quantity.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int ScoreComponent(IComponent input, BuildType type)
        {
            if (input == null)
                return 0;

            var score = 0;

            switch (input) {
                case GPU gpu:
                    score = OverloadedScore(gpu, type);
                    break;
                case CPU cpu:
                    score = OverloadedScore(cpu, type);
                    break;
                case PowerSupplyUnit psu:
                    score = OverloadedScore(psu, type);
                    break;
                case RAM ram:
                    score = OverloadedScore(ram, type);
                    break;
                case ComputerCase cCase:
                    score = OverloadedScore(cCase, type);
                    break;
                case ICooler cooler:
                    score = OverloadedScore(cooler, type);
                    break;
                //case Monitor monitor:
                //    score = OverloadedScore(monitor, type);
                //    break;
                case Motherboard mobo:
                    score = OverloadedScore(mobo, type);
                    break;
                case IHardDrive hardDrive:
                    score = OverloadedScore(hardDrive, type);
                    break;
                default:
                    break;
            }

            return score;
        }

        #region "Private Overload Methods"
        /// <summary>
        /// Overloaded methods that take in a product and build type
        /// to determine a score based off of stat weights. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private int OverloadedScore(GPU input, BuildType type)
        {
            if (input.Price == 0 || input.Memory == null || input.CoreClock == null ||
                input.BoostClock == null || input.EffectiveMemClock == null ||
                    input.PowerDraw == 0)
                return -1;
            var price = input.Price;
            var mem = ParseInt(input.Memory);
            var coreClock = ParseInt(input.CoreClock);
            var boostClock = ParseInt(input.BoostClock);
            var effectiveMem = ParseInt(input.EffectiveMemClock);
            var powerDraw = input.PowerDraw;
            var frameSync = input.FrameSync;
            var frameSyncScore = 0;

            // Unresolved magic values.
            if (frameSync != null || frameSync.ToLower().Contains("g-sync")
                || frameSync.ToLower().Contains("freesync")
                || frameSync.ToLower().Contains("gsync"))
                frameSyncScore += BONUS_VALUE;

            // Bonus score to gigabyte memory value.
            if (input.Memory.ToLower().Contains("gb"))
                mem *= BONUS_VALUE;

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_GPU_WEIGHTS[0] +
                    mem * GAMING_GPU_WEIGHTS[1] +
                    coreClock * GAMING_GPU_WEIGHTS[2] +
                    boostClock * GAMING_GPU_WEIGHTS[3] +
                    effectiveMem * GAMING_GPU_WEIGHTS[4] +
                    powerDraw * GAMING_GPU_WEIGHTS[5];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_GPU_WEIGHTS[0] +
                    mem * WORK_GPU_WEIGHTS[1] +
                    coreClock * WORK_GPU_WEIGHTS[2] +
                    boostClock * WORK_GPU_WEIGHTS[3] +
                    effectiveMem * WORK_GPU_WEIGHTS[4] +
                    powerDraw * WORK_GPU_WEIGHTS[5];
                    break;
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_GPU_WEIGHTS[0] +
                    mem * ARTIST_GPU_WEIGHTS[1] +
                    coreClock * ARTIST_GPU_WEIGHTS[2] +
                    boostClock * ARTIST_GPU_WEIGHTS[3] +
                    effectiveMem * ARTIST_GPU_WEIGHTS[4] +
                    powerDraw * ARTIST_GPU_WEIGHTS[5];
                    break;
                default:
                    break;
            }

            scoreTotal += frameSyncScore;

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int OverloadedScore(CPU input, BuildType type)
        {
            if (input.Price == 0 || input.CoreCount == 0 || input.CoreClock == null ||
                input.BoostClock == null || input.PowerDraw == 0)
                return -1;

            var price = input.Price;
            var coreCount = input.CoreCount;
            var coreClock = ParseInt(input.CoreClock);
            var boostClock = ParseInt(input.BoostClock);
            var powerDraw = input.PowerDraw;
            var errCorrection = 0;

            if (input.ErrCorrectionCodeSupport)
                errCorrection += BONUS_VALUE;

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_CPU_WEIGHTS[0] +
                        coreCount * ARTIST_CPU_WEIGHTS[1] *
                        (coreClock + boostClock) / 2 +
                        powerDraw * ARTIST_CPU_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_CPU_WEIGHTS[0] +
                        coreCount * WORK_CPU_WEIGHTS[1] *
                        (coreClock + boostClock) / 2 +
                        powerDraw * WORK_CPU_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_CPU_WEIGHTS[0] +
                        coreCount * GAMING_CPU_WEIGHTS[1] *
                        (coreClock + boostClock) / 2 +
                        powerDraw * GAMING_CPU_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            scoreTotal += errCorrection;
            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int OverloadedScore(PowerSupplyUnit input, BuildType type)
        {
            if (input.Price == 0 || input.Wattage == 0 || input.EfficiencyRating == null)
                return -1;
            var price = input.Price;
            var wattage = input.Wattage;
            var efficancyBonus = 0;
            var efRating = input.EfficiencyRating.ToLower().Trim();

            if (efRating == "80+ Bronze")
                efficancyBonus = BONUS_VALUE;
            else if (efRating == "80+ Silver")
                efficancyBonus = BONUS_VALUE * 2;
            else if (efRating == "80+ Gold")
                efficancyBonus = BONUS_VALUE * 3;
            else if (efRating == "80+ Platinum")
                efficancyBonus = BONUS_VALUE * 4;
            else if (efRating == "80+ Titanium")
                efficancyBonus = BONUS_VALUE * 5;

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_PSU_WEIGHTS[0] +
                        wattage * ARTIST_PSU_WEIGHTS[1];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_PSU_WEIGHTS[0] +
                        wattage * WORK_PSU_WEIGHTS[1];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_PSU_WEIGHTS[0] +
                        wattage * GAMING_PSU_WEIGHTS[1];
                    break;
                default:
                    break;
            }

            scoreTotal += efficancyBonus;
            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int OverloadedScore(RAM input, BuildType type)
        {
            if (input.Price == 0 || input.NumOfModules == 0 || input.ModuleSize == 0
                || input.FirstWordLat == null || input.CASLat == null)
                return -1;

            var price = input.Price;
            var firstWord = ParseDouble(input.FirstWordLat);
            var numOfModules = input.NumOfModules;
            var moduleSize = input.ModuleSize;
            var casLat = ParseInt(input.CASLat);

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_RAM_WEIGHTS[0] +
                        numOfModules * moduleSize / firstWord * ARTIST_RAM_WEIGHTS[1] +
                        casLat * ARTIST_RAM_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_RAM_WEIGHTS[0] +
                        numOfModules * moduleSize / firstWord * WORK_RAM_WEIGHTS[1] +
                        casLat * WORK_RAM_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_RAM_WEIGHTS[0] +
                        numOfModules * moduleSize / firstWord * GAMING_RAM_WEIGHTS[1] +
                        casLat * ARTIST_RAM_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int OverloadedScore(ComputerCase input, BuildType type)
        {
            if (input.Price == 0)
                return -1;
            var price = input.Price;
            var expansion = input.ExpansionSlots;
            var psuShroud = input.PsuShroud;
            var frontPanel = 0;
            if (input.FrontPanel != null)
                frontPanel = input.FrontPanel.Count;

            double scoreTotal = 0;

            if (psuShroud)
                scoreTotal += BONUS_VALUE;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_CASE_WEIGHTS[0] +
                        expansion * ARTIST_CASE_WEIGHTS[1] +
                        frontPanel * ARTIST_CASE_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_CASE_WEIGHTS[0] +
                        expansion * WORK_CASE_WEIGHTS[1] +
                        frontPanel * WORK_CASE_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_CASE_WEIGHTS[0] +
                        expansion * GAMING_CASE_WEIGHTS[1] +
                        frontPanel * GAMING_CASE_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int OverloadedScore(ICooler input, BuildType type)
        {
            if (input.Price == 0 || input.FanRPM == null || input.NoiseVolume == null)
                return -1;

            var price = input.Price;
            var speed = ParseInt(input.FanRPM);
            var noise = ParseInt(input.NoiseVolume);

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_COOLER_WEIGHTS[0] +
                        speed * ARTIST_COOLER_WEIGHTS[1] +
                        noise * ARTIST_COOLER_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_COOLER_WEIGHTS[0] +
                        speed * WORK_COOLER_WEIGHTS[1] +
                        noise * WORK_COOLER_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_COOLER_WEIGHTS[0] +
                        speed * GAMING_COOLER_WEIGHTS[1] +
                        noise * GAMING_COOLER_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        // Out of scope at this time.
        //private int OverloadedScore(Monitor input, BuildType type)
        //{
        //    if (input.Price == 0 )
        //        return -1;

        //    double scoreTotal = 0;

        //    switch (type)
        //    {
        //        case BuildType.GraphicArtist:
        //            break;
        //        case BuildType.WordProcessing:
        //            break;
        //        case BuildType.Gaming:
        //            break;
        //        default:
        //            break;
        //    }
        //    scoreTotal += frameSyncScore;

        //    return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        //}

        private int OverloadedScore(IHardDrive input, BuildType type)
        {
            if (input.Price == 0 || input.Capacity == null)
                return -1;

            var price = input.Price;
            var capacity = ParseInt(input.Capacity);
            var cache = ParseInt(input.Cache);
            var driveType = input.DriveType;

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_HARDDRIVE_WEIGHTS[0] +
                        capacity * ARTIST_HARDDRIVE_WEIGHTS[1] +
                        cache * ARTIST_HARDDRIVE_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_HARDDRIVE_WEIGHTS[0] +
                        capacity * WORK_HARDDRIVE_WEIGHTS[1] +
                        cache * WORK_HARDDRIVE_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_HARDDRIVE_WEIGHTS[0] +
                        capacity * GAMING_HARDDRIVE_WEIGHTS[1] +
                        cache * GAMING_HARDDRIVE_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            if (driveType != HardDriveType.NVMe)
                scoreTotal += BONUS_VALUE;

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int OverloadedScore(Motherboard input, BuildType type)
        {
            if(input.Price == 0 || input.Socket == null || input.MaxMemory == null ||
                input.SupportedMemory == null)
                return -1;

            var price = input.Price;
            var maxMemSupport = input.MaxMemoryType;
            var maxMem = ParseInt(input.MaxMemory);

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * ARTIST_MOTHERBOARD_WEIGHTS[0] +
                        maxMem * ARTIST_MOTHERBOARD_WEIGHTS[1];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * WORK_MOTHERBOARD_WEIGHTS[0] +
                        maxMem * WORK_MOTHERBOARD_WEIGHTS[1];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * GAMING_MOTHERBOARD_WEIGHTS[0] +
                        maxMem * GAMING_MOTHERBOARD_WEIGHTS[1];
                    break;
                default:
                    break;
            }

            if (maxMemSupport == MemoryType.DDR4)
                scoreTotal += BONUS_VALUE;

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private double ParseDouble(string input)
        {
            if (input == null || input == string.Empty)
                return 0;

            var tokens = input.Split(' ');
            var output = 0.0;

            if (!string.IsNullOrEmpty(tokens[0]))
                output = double.Parse(tokens[0]);

            return output;
        }

        private int ParseInt(string input)
        {
            if (input == null || input == string.Empty)
                return 0;

            string toParse = string.Empty;

            foreach (char c in input)
                if (char.IsDigit(c))
                    toParse += c;
            

            var output = 0;
            if(toParse != string.Empty)
                output = int.Parse(toParse);

            return output;
        }
        #endregion
    }
}
