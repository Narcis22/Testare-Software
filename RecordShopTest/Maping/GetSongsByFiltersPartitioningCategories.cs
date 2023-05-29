using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordShopTest.Maping
{
    internal class GetSongsByFiltersPartitioningCategories
    {
        public class ValidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1, 36, 50 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class InvalidCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 2, -10, 200 };

                yield return new object[] { 2, 10, -20 };

                yield return new object[] { 2, 20, 20 };

                yield return new object[] { -2, 10, 20 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
