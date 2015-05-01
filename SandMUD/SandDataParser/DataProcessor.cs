using System;
using System.Collections.Generic;
using System.Linq;

namespace SandDataProcessor
{
    public static class DataProcessor
    {
        public static IEnumerable<Func<IEnumerable<byte>, IEnumerable<byte>>> DefaultPipeline
        {
            get
            {
                var result = new List<Func<IEnumerable<byte>, IEnumerable<byte>>>();
                result.Add(InputClearingProcess);
                return result;
            }
        }

        public static IEnumerable<byte> InputClearingProcess(IEnumerable<byte> input)
        {
            return input.Where(b => (b == 13) || (b > 31 && b < 127));
        }

        public static byte[] Process(IEnumerable<byte> data,
            IEnumerable<Func<IEnumerable<byte>, IEnumerable<byte>>> pipeline)
        {
            var result = pipeline.Aggregate(data, (current, process) => process(current));

            return result.ToArray();
        }
    }
}