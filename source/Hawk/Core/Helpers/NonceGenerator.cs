using System;
using System.Linq;
using System.Threading;

namespace Thinktecture.IdentityModel.Hawk.Core.Helpers
{
    /// <summary>
    /// Generates a nonce to be used by a .NET client.
    /// </summary>
    public class NonceGenerator
    {
        private static int seed = Environment.TickCount;

        private static ThreadLocal<Random> generator = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref seed))
        );

        private static Random Generator
        {
            get
            {
                return generator.Value;
            }
        }

        /// <summary>
        /// Generates a nonce matching the specified length and returns the same. Default length is 6.
        /// </summary>
        public static string Generate(int length = 6)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            int min = 0;
            int max = alphabet.Length - 1;

            char[] randomCharacters = Enumerable.Range(0, length)
                                            .Select(i => alphabet[Generator.Next(min, max)])
                                                .ToArray();
            return new String(randomCharacters);
        }
    }
}
