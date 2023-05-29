using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecordShopTest.Maping
{
    public class GetSongCartForUserEquivalenceClass
    {
        private static Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        private static List<string> listEmailValid = new List<string> { "narcis@gmail.com", 
                                                                        "narcis@user.com" };

        public class ValidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "narcis@gmail.com", 0 };

                yield return new object[] { "narcis@user.com", 1 };

                yield return new object[] { "narcis@gmail.com", 1 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class InvalidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // Invalid email format
                yield return new object[] { generateInvalidEmail(), 0 };

                yield return new object[] { generateInvalidEmail(), 1 };

                yield return new object[] { generateInvalidEmail(), generateInvalidSent() };

                // Non existent email

                yield return new object[] { generateRandomNonExistentEmail(), 0 };

                yield return new object[] { generateRandomNonExistentEmail(), 1 };

                yield return new object[] { generateRandomNonExistentEmail(), generateInvalidSent() };

                // Valid mail - invalid Sent

                yield return new object[] { "narcis@gmail.com", generateInvalidSent() };

                yield return new object[] { "narcis@user.com", generateInvalidSent() };


            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public static string generateInvalidEmail()
        {
            string nonEmail = string.Join("", Enumerable.Repeat(0, 20).Select(n => (char)new Random().Next(33, 122)));

            Match match = regex.Match(nonEmail);

            while (match.Success)
            {
                nonEmail = string.Join("", Enumerable.Repeat(0, 20).Select(n => (char)new Random().Next(33, 122)));

                match = regex.Match(nonEmail);
            }

            return nonEmail;
        }

        public static string generateRandomNonExistentEmail()
        {
            string part1 = string.Join("", Enumerable.Repeat(0, 5).Select(n => (char)new Random().Next(97, 122)));
            string part2 = string.Join("", Enumerable.Repeat(0, 5).Select(n => (char)new Random().Next(97, 122)));
            string part3 = string.Join("", Enumerable.Repeat(0, 3).Select(n => (char)new Random().Next(97, 122)));

            string nonEmail = part1 + "@" + part2 + "." + part3;

            Match match = regex.Match(nonEmail);

            while (!match.Success || listEmailValid.Contains(nonEmail))
            {
                part1 = string.Join("", Enumerable.Repeat(0, 5).Select(n => (char)new Random().Next(97, 122)));
                part2 = string.Join("", Enumerable.Repeat(0, 5).Select(n => (char)new Random().Next(97, 122)));
                part3 = string.Join("", Enumerable.Repeat(0, 3).Select(n => (char)new Random().Next(97, 122)));

                nonEmail = part1 + "@" + part2 + "." + part3;
                match = regex.Match(nonEmail);
            }

            return nonEmail;
        }

        public static int generateInvalidSent()
        {
            int newSent = new Random().Next(-100, 100);

            while (newSent == 0 || newSent == 1)
            {
                newSent = new Random().Next(-100, 100);
            }

            return newSent;
        }
    }
}
