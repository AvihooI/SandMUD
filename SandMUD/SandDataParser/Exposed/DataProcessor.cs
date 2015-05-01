using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandDataProcessor
{
    public static class DataProcessor
    {
        public static IEnumerable<byte> InputClearingProcess(IEnumerable<byte> input)
        {
            var result = new List<byte>();

            foreach (var b in input)
                if ((b == 13) || (b > 31 && b < 127))
                    result.Add(b);

            return result;
        }
        public static byte[] Process(IEnumerable<byte> data, IEnumerable<Func<IEnumerable<byte>, IEnumerable<byte>>> pipeline)
        {
            IEnumerable<byte> result = data;

            foreach (var process in pipeline)
            {
                result = process(result);
            }
            
            return result.ToArray();
        }

        public static IEnumerable<Func<IEnumerable<byte>, IEnumerable<byte>>> DefaultPipeline
        {
            get
            {
                var result = new List<Func<IEnumerable<byte>, IEnumerable<byte>>>();
                result.Add(InputClearingProcess);
                return result;
            }
        }
            

    }
}
