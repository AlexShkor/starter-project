﻿using System.Collections.Generic;

namespace DQF.Platform.Extensions
{
    public static class EnumerableExt
    {
        /// <summary>
        /// Enumerates a sequence in chunks, yielding batches of a certain size to the enumerator.
        /// </summary>
        /// <typeparam name="T">The type of item in the batch.</typeparam>
        /// <param name="sequence">The sequence of items to be enumerated.</param>
        /// <param name="batchSize">The maximum number of items to include in a batch.</param>
        /// <returns>A sequence of arrays, with each array containing at most
        /// <paramref name="batchSize"/> elements.</returns>
        public static IEnumerable<T[]> Batch<T>(this IEnumerable<T> sequence, int batchSize)
        {

            var batch = new List<T>(batchSize);

            foreach (var item in sequence)
            {

                batch.Add(item);

                // when we've accumulated enough in the
                // batch, send it out
                if (batch.Count >= batchSize)
                {
                    yield return batch.ToArray();
                    batch.Clear();
                }   // if

            }   // foreach

            // send out any leftovers
            if (batch.Count > 0)
            {
                yield return batch.ToArray();
                batch.Clear();
            }   // if
        }
    }
}