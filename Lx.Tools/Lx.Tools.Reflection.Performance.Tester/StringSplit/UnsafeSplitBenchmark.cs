using System.Collections.Generic;
using System.Security;
using Lx.Tools.Common;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class UnsafeSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "Unsafe Split"; } }
        public void Init()
        {
        }

        public void Call()
        {
            foreach (var input in Inputs)
            {
                var result = SplitKeepDelimiters(input.Value, input.Key.ToArray());
            }
        }

        private unsafe string[] SplitKeepDelimiters(string input, string[] delimiters)
        {
            unchecked
            {
                List<string> result = new List<string>(10);
                int length = input.Length;
                fixed (char* inputFixed = input)
                {
                    char* inputPtr = inputFixed;
                    char* endPtr = inputPtr + length;
                    char* lastMatchPtr = inputFixed;
                    var delLen = delimiters.Length;
                    for (; inputPtr < endPtr; inputPtr++)
                    {
                        for (int j = 0; j < delLen; j++)
                        {
                            string str = delimiters[j];
                            fixed (char* sepFixed = str)
                            {
                                char* sep = sepFixed;
                                int sepLen = str.Length;
                                if (((*inputPtr == *sep) && (inputPtr + sepLen <= endPtr)) &&
                                    ((sepLen == 1) || (CompareOrdinal(inputPtr, sep, sepLen))))
                                {
                                    result.Add(Substring(lastMatchPtr, inputPtr));
                                    result.Add(str);
                                    inputPtr += sepLen;
                                    lastMatchPtr = inputPtr;
                                    break;
                                }
                            }
                        }
                    }

                    if (lastMatchPtr < endPtr)
                        result.Add(Substring(lastMatchPtr, endPtr));
                    return result.ToArray();
                }
            }
        }

        /// <summary>
        /// Create a substrings start at the specified start and ending just before the end pointer.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        private unsafe static string Substring(char* start, char* end)
        {
            unchecked
            {
                var res = new string('c', (int)(end - start));
                fixed (char* result = res)
                {
                    var moving = result;
                    while (start < end)
                    {
                        *moving = *start;
                        ++start;
                        ++moving;
                    }
                    *moving = '\0';
                }
                return res;
            }
        }

        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        internal unsafe static bool CompareOrdinal(char* first, char* second, int length)
        {
            unchecked
            {
                int i = 0;
                while (*first == *second)
                {
                    ++first;
                    ++second;
                    ++i;
                    if (i == length)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void Cleanup()
        {
        }
    }
}
