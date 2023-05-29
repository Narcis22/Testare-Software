using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecordShopTest.Maping
{
    public class GetSongsByFiltersEquivalenceClass
    {
        public class ValidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1, 36, 50};

                yield return new object[] { 2, 10, 200 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class InvalidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // Invalid minPrice
                yield return new object[] { 2, -10, 200 };

                yield return new object[] { 2, 0, 200 };

                // Invalid maxPrice
                yield return new object[] { 2, 10, -20 };

                yield return new object[] { 2, 10, 0 };

                // Invalid minPrice < maxPrice
                yield return new object[] { 2, 50, 20 };
                yield return new object[] { 2, 20, 20 };

                // Invalid idArtist
                yield return new object[] { -2, 10, 20 };
                
                yield return new object[] { 0, 10, 20 };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
