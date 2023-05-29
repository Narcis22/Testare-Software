using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordShopTest.Maping
{
    internal class GetSongCartForUserPartitioningCategories
    {
        public class ValidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "narcis@gmail.com", 0 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class InvalidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { GetSongCartForUserEquivalenceClass.generateInvalidEmail(), 0 };

                yield return new object[] { GetSongCartForUserEquivalenceClass.generateRandomNonExistentEmail(), GetSongCartForUserEquivalenceClass.generateInvalidSent() };

                yield return new object[] { "narcis@gmail.com", GetSongCartForUserEquivalenceClass.generateInvalidSent() };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
