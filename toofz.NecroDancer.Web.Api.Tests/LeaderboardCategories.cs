using System.Collections.Generic;
using toofz.NecroDancer.Leaderboards;

namespace toofz.NecroDancer.Web.Api.Tests
{
    static class LeaderboardCategories
    {
        public static IEnumerable<Product> Products
        {
            get
            {
                return new[]
                {
                    new Product(0, "classic", "Classic"),
                    new Product(1, "amplified", "Amplified"),
                };
            }
        }

        public static IEnumerable<Mode> Modes
        {
            get
            {
                return new[]
                {
                    new Mode(0, "standard", "Standard"),
                    new Mode(1, "no-return", "No Return"),
                    new Mode(2, "hard", "Hard"),
                    new Mode(3, "phasing", "Phasing"),
                    new Mode(4, "randomizer", "Randomizer"),
                    new Mode(5, "mystery", "Mystery"),
                };
            }
        }

        public static IEnumerable<Run> Runs
        {
            get
            {
                return new[]
                {
                    new Run(0, "score", "Score"),
                    new Run(1, "speed", "Speed"),
                    new Run(2, "seeded-score", "Seeded Score"),
                    new Run(3, "seeded-speed", "Seeded Speed"),
                    new Run(4, "deathless", "Deathless"),
                };
            }
        }

        public static IEnumerable<Character> Characters
        {
            get
            {
                return new[]
                {
                    new Character(-4, "all-characters-amplified", "All Characters (Amplified)"),
                    new Character(-3, "all-characters", "All Characters"),
                    new Character(-2, "story-mode", "Story Mode"),
                    new Character(0, "cadence", "Cadence"),
                    new Character(1, "melody", "Melody"),
                    new Character(2, "aria", "Aria"),
                    new Character(3, "dorian", "Dorian"),
                    new Character(4, "eli", "Eli"),
                    new Character(5, "monk", "Monk"),
                    new Character(6, "dove", "Dove"),
                    new Character(7, "coda", "Coda"),
                    new Character(8, "bolt", "Bolt"),
                    new Character(9, "bard", "Bard"),
                    new Character(10, "nocturna", "Nocturna"),
                    new Character(11, "diamond", "Diamond"),
                    new Character(12, "mary", "Mary"),
                    new Character(13, "tempo", "Tempo"),
                };
            }
        }
    }
}
