using System;

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
            {
                throw new ArgumentNullException("Input is missing");
            }

            var score = 0;

            switch (input) {
                case GraphicsProcUnit gpu:
                    score = Score(gpu, type);
                    break;
                case CentralProcUnit cpu:
                    score = Score(cpu, type);
                    break;
                case PowerSupplyUnit psu:
                    score = Score(psu, type);
                    break;
                case RAM ram:
                    score = Score(ram, type);
                    break;
                case ComputerCase cCase:
                    score = Score(cCase, type);
                    break;
                case ICooler cooler:
                    score = Score(cooler, type);
                    break;
                //case Monitor monitor:
                //    score = OverloadedScore(monitor, type);
                //    break;
                case Motherboard mobo:
                    score = Score(mobo, type);
                    break;
                case HardDrive hardDrive:
                    score = Score(hardDrive, type);
                    break;
                default:
                    break;
            }

            return score;
        }

        #region Private Scoring Helper Methods
        /// <summary>
        /// Overloaded methods that take in a product and build type
        /// to determine a score based off of stat weights. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private int Score(GraphicsProcUnit input, BuildType type)
        {
            var price = input.Price;
            var mem = 0;
            var coreClock = 0;
            var boostClock = 0;
            var effectiveMem = 0;
            var powerDraw = input.PowerDraw;
            var frameSync = input.FrameSync;
            var frameSyncScore = 0;
            
            if (string.IsNullOrEmpty(input.Memory))
            {
                mem = ParseInt(input.Memory);
            }
            if (!string.IsNullOrEmpty(input.CoreClock))
            {
                coreClock = ParseInt(input.CoreClock);
            }
            if (input.BoostClock != null)
            {
                boostClock = ParseInt(input.BoostClock);
            }
            if (!string.IsNullOrEmpty(input.EffectiveMemClock))
            {
                effectiveMem = ParseInt(input.EffectiveMemClock);
            } 

            // Unresolved magic values.
            if (frameSync != null || frameSync.ToLower().Contains("g-sync")
                || frameSync.ToLower().Contains("freesync")
                || frameSync.ToLower().Contains("gsync"))
            {
                frameSyncScore += RecWeightGlobals.BONUS_VALUE;
            }
                
            // Bonus score to gigabyte memory value.
            if (input.Memory.ToLower().Contains("gb"))
            {
                mem *= RecWeightGlobals.BONUS_VALUE;
            }

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_GPU_WEIGHTS[0] +
                    mem * RecWeightGlobals.GAMING_GPU_WEIGHTS[1] +
                    coreClock * RecWeightGlobals.GAMING_GPU_WEIGHTS[2] +
                    boostClock * RecWeightGlobals.GAMING_GPU_WEIGHTS[3] +
                    effectiveMem * RecWeightGlobals.GAMING_GPU_WEIGHTS[4] +
                    powerDraw * RecWeightGlobals.GAMING_GPU_WEIGHTS[5];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_GPU_WEIGHTS[0] +
                    mem * RecWeightGlobals.WORK_GPU_WEIGHTS[1] +
                    coreClock * RecWeightGlobals.WORK_GPU_WEIGHTS[2] +
                    boostClock * RecWeightGlobals.WORK_GPU_WEIGHTS[3] +
                    effectiveMem * RecWeightGlobals.WORK_GPU_WEIGHTS[4] +
                    powerDraw * RecWeightGlobals.WORK_GPU_WEIGHTS[5];
                    break;
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_GPU_WEIGHTS[0] +
                    mem * RecWeightGlobals.ARTIST_GPU_WEIGHTS[1] +
                    coreClock * RecWeightGlobals.ARTIST_GPU_WEIGHTS[2] +
                    boostClock * RecWeightGlobals.ARTIST_GPU_WEIGHTS[3] +
                    effectiveMem * RecWeightGlobals.ARTIST_GPU_WEIGHTS[4] +
                    powerDraw * RecWeightGlobals.ARTIST_GPU_WEIGHTS[5];
                    break;
                default:
                    break;
            }

            scoreTotal += frameSyncScore;

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int Score(CentralProcUnit input, BuildType type)
        {
            var price = input.Price;
            var coreCount = input.CoreCount;
            var coreClock = 0;
            var boostClock = 0;
            var powerDraw = input.PowerDraw;
            var errCorrection = 0;

            if (!string.IsNullOrEmpty(input.CoreClock))
            {
                coreClock = ParseInt(input.CoreClock);
            }
            if (!string.IsNullOrEmpty(input.BoostClock))
            {
                boostClock = ParseInt(input.BoostClock);
            }


            if (input.ErrCorrectionCodeSupport)
                errCorrection += RecWeightGlobals.BONUS_VALUE;

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_CPU_WEIGHTS[0] +
                        coreCount * RecWeightGlobals.ARTIST_CPU_WEIGHTS[1] *
                        (coreClock + boostClock) / 2 +
                        powerDraw * RecWeightGlobals.ARTIST_CPU_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_CPU_WEIGHTS[0] +
                        coreCount * RecWeightGlobals.WORK_CPU_WEIGHTS[1] *
                        (coreClock + boostClock) / 2 +
                        powerDraw * RecWeightGlobals.WORK_CPU_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_CPU_WEIGHTS[0] +
                        coreCount * RecWeightGlobals.GAMING_CPU_WEIGHTS[1] *
                        (coreClock + boostClock) / 2 +
                        powerDraw * RecWeightGlobals.GAMING_CPU_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            scoreTotal += errCorrection;
            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int Score(PowerSupplyUnit input, BuildType type)
        {
            var price = input.Price;
            var wattage = input.Wattage;
            var efficancyBonus = 0;
            var efRating = "";

            if (!string.IsNullOrEmpty(input.EfficiencyRating))
            {
                efRating = input.EfficiencyRating.ToLower().Trim();
            }

            if (efRating == "80+ Bronze")
                efficancyBonus = RecWeightGlobals.BONUS_VALUE;
            else if (efRating == "80+ Silver")
                efficancyBonus = RecWeightGlobals.BONUS_VALUE * 2;
            else if (efRating == "80+ Gold")
                efficancyBonus = RecWeightGlobals.BONUS_VALUE * 3;
            else if (efRating == "80+ Platinum")
                efficancyBonus = RecWeightGlobals.BONUS_VALUE * 4;
            else if (efRating == "80+ Titanium")
                efficancyBonus = RecWeightGlobals.BONUS_VALUE * 5;

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_PSU_WEIGHTS[0] +
                        wattage * RecWeightGlobals.ARTIST_PSU_WEIGHTS[1];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_PSU_WEIGHTS[0] +
                        wattage * RecWeightGlobals.WORK_PSU_WEIGHTS[1];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_PSU_WEIGHTS[0] +
                        wattage * RecWeightGlobals.GAMING_PSU_WEIGHTS[1];
                    break;
                default:
                    break;
            }

            scoreTotal += efficancyBonus;
            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int Score(RAM input, BuildType type)
        {
            var price = input.Price;
            var firstWord = 0.0;
            var numOfModules = input.NumOfModules;
            var moduleSize = input.ModuleCapacity;
            var casLat = 0;

            if (!string.IsNullOrEmpty(input.FirstWordLat))
            {
                firstWord = ParseDouble(input.FirstWordLat);
            }

            if (string.IsNullOrEmpty(input.CASLat)){
                casLat = ParseInt(input.CASLat);
            }

            double scoreTotal = 0;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_RAM_WEIGHTS[0] +
                        numOfModules * moduleSize / firstWord * RecWeightGlobals.ARTIST_RAM_WEIGHTS[1] +
                        casLat * RecWeightGlobals.ARTIST_RAM_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_RAM_WEIGHTS[0] +
                        numOfModules * moduleSize / firstWord * RecWeightGlobals.WORK_RAM_WEIGHTS[1] +
                        casLat * RecWeightGlobals.WORK_RAM_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_RAM_WEIGHTS[0] +
                        numOfModules * moduleSize / firstWord * RecWeightGlobals.GAMING_RAM_WEIGHTS[1] +
                        casLat * RecWeightGlobals.ARTIST_RAM_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int Score(ComputerCase input, BuildType type)
        {
            var price = input.Price;
            var expansion = input.ExpansionSlots;
            var psuShroud = input.PsuShroud;
            var frontPanel = 0;

            if (input.FrontPanel != null)
            {
                frontPanel = input.FrontPanel.Count;
            }
                

            double scoreTotal = 0;

            if (psuShroud)
                scoreTotal += RecWeightGlobals.BONUS_VALUE;

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_CASE_WEIGHTS[0] +
                        expansion * RecWeightGlobals.ARTIST_CASE_WEIGHTS[1] +
                        frontPanel * RecWeightGlobals.ARTIST_CASE_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_CASE_WEIGHTS[0] +
                        expansion * RecWeightGlobals.WORK_CASE_WEIGHTS[1] +
                        frontPanel * RecWeightGlobals.WORK_CASE_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_CASE_WEIGHTS[0] +
                        expansion * RecWeightGlobals.GAMING_CASE_WEIGHTS[1] +
                        frontPanel * RecWeightGlobals.GAMING_CASE_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int Score(ICooler input, BuildType type)
        {
            var price = input.Price;
            var speed = 0;
            var noise = 0;
            double scoreTotal = 0;

            if (!string.IsNullOrEmpty(input.FanRPM))
            {
                speed = ParseInt(input.FanRPM);
            }
            if (!string.IsNullOrEmpty(input.NoiseVolume))
            {
                noise = ParseInt(input.NoiseVolume);
            }


            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_COOLER_WEIGHTS[0] +
                        speed * RecWeightGlobals.ARTIST_COOLER_WEIGHTS[1] +
                        noise * RecWeightGlobals.ARTIST_COOLER_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_COOLER_WEIGHTS[0] +
                        speed * RecWeightGlobals.WORK_COOLER_WEIGHTS[1] +
                        noise * RecWeightGlobals.WORK_COOLER_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_COOLER_WEIGHTS[0] +
                        speed * RecWeightGlobals.GAMING_COOLER_WEIGHTS[1] +
                        noise * RecWeightGlobals.GAMING_COOLER_WEIGHTS[2];
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

        private int Score(HardDrive input, BuildType type)
        {
            var price = input.Price;
            var capacity = 0;
            var cache = 0;
            double scoreTotal = 0;

            if (!string.IsNullOrEmpty(input.Cache))
            {
                cache = ParseInt(input.Cache);
            }

            if (!string.IsNullOrEmpty(input.Capacity))
            {
                capacity = ParseInt(input.Capacity);
            }

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_HARDDRIVE_WEIGHTS[0] +
                        capacity * RecWeightGlobals.ARTIST_HARDDRIVE_WEIGHTS[1] +
                        cache * RecWeightGlobals.ARTIST_HARDDRIVE_WEIGHTS[2];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_HARDDRIVE_WEIGHTS[0] +
                        capacity * RecWeightGlobals.WORK_HARDDRIVE_WEIGHTS[1] +
                        cache * RecWeightGlobals.WORK_HARDDRIVE_WEIGHTS[2];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_HARDDRIVE_WEIGHTS[0] +
                        capacity * RecWeightGlobals.GAMING_HARDDRIVE_WEIGHTS[1] +
                        cache * RecWeightGlobals.GAMING_HARDDRIVE_WEIGHTS[2];
                    break;
                default:
                    break;
            }

            return (int)Math.Round(scoreTotal, MidpointRounding.AwayFromZero);
        }

        private int Score(Motherboard input, BuildType type)
        {
            var price = input.Price;
            var maxMemSupport = input.MaxMemoryType;
            var maxMem = 0;
            double scoreTotal = 0;

            if (!string.IsNullOrEmpty(input.MaxMemory))
            {
                maxMem = ParseInt(input.MaxMemory);
            }

            switch (type)
            {
                case BuildType.GraphicArtist:
                    scoreTotal = price * RecWeightGlobals.ARTIST_MOTHERBOARD_WEIGHTS[0] +
                        maxMem * RecWeightGlobals.ARTIST_MOTHERBOARD_WEIGHTS[1];
                    break;
                case BuildType.WordProcessing:
                    scoreTotal = price * RecWeightGlobals.WORK_MOTHERBOARD_WEIGHTS[0] +
                        maxMem * RecWeightGlobals.WORK_MOTHERBOARD_WEIGHTS[1];
                    break;
                case BuildType.Gaming:
                    scoreTotal = price * RecWeightGlobals.GAMING_MOTHERBOARD_WEIGHTS[0] +
                        maxMem * RecWeightGlobals.GAMING_MOTHERBOARD_WEIGHTS[1];
                    break;
                default:
                    break;
            }

            if (maxMemSupport == MemoryType.DDR4)
                scoreTotal += RecWeightGlobals.BONUS_VALUE;

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
