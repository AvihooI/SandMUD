using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandDataParser
{
    public static class DataParser
    {
        public static IEnumerable<byte> DefaultProcess(IEnumerable<byte> input)
        {
            var result = new List<byte>();

            foreach (var b in input)
                if ((b == 13) || (b > 31 && b < 127))
                    result.Add(b);

            return result;
        }
        public static byte[] Parse(IEnumerable<byte> data, IEnumerable<Func<IEnumerable<byte>, IEnumerable<byte>>> pipeline)
        {
            IEnumerable<byte> result = data;

            foreach (var p in pipeline)
            {
                result = p(result);
            }

            return result.ToArray();
        }

        public static IEnumerable<Func<IEnumerable<byte>, IEnumerable<byte>>> DefaultPipeline
        {
            get
            {
                var result = new List<Func<IEnumerable<byte>, IEnumerable<byte>>>();
                result.Add(DefaultProcess);
                return result;
            }
        }
            

    }
}
