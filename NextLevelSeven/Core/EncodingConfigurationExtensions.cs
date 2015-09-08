﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    static internal class EncodingConfigurationExtensions
    {
        static public string Escape(this EncodingConfiguration config, string s)
        {
            if (s == null)
            {
                return null;
            }

            var componentDelimiter = config.ComponentDelimiter;
            var escapeDelimiter = config.EscapeDelimiter;
            var fieldDelimiter = config.FieldDelimiter;
            var repetitionDelimiter = config.RepetitionDelimiter;
            var subcomponentDelimiter = config.SubcomponentDelimiter;

            var normalTextEscapeCode = WrapCode("N", escapeDelimiter);
            var highlightTextEscapeCode = WrapCode("H", escapeDelimiter);

            var data = s.ToCharArray();
            var length = data.Length;
            var output = new StringBuilder();

            for (var index = 0; index < length; index++)
            {
                var c = data[index];

                if (c == componentDelimiter)
                {
                    output.Append(new[] {escapeDelimiter, 'S', escapeDelimiter});
                    continue;
                }

                if (c == escapeDelimiter)
                {
                    if (length - index >= 3)
                    {
                        if (data[index + 2] == escapeDelimiter)
                        {
                            switch (data[index + 1])
                            {
                                case 'N': // normal text
                                case 'H': // highlight text
                                    output.Append(new[] {escapeDelimiter, data[index + 1], escapeDelimiter});
                                    index += 2;
                                    continue;
                            }
                        }
                        else if (length - index >= 5)
                        {
                            switch (data[index + 1])
                            {
                                case 'C': // single byte character set escape
                                    if (length - index >= 7 && data[index + 6] == escapeDelimiter)
                                    {
                                        output.Append(new string(data, index, 7));
                                        index += 6;
                                        continue;
                                    }
                                    break;
                                case 'M': // multi-byte character set escape
                                    if (length - index >= 7 && data[index + 6] == escapeDelimiter)
                                    {
                                        // without optional third pair
                                        output.Append(new string(data, index, 7));
                                        index += 6;
                                        continue;
                                    }
                                    if (length - index >= 9 && data[index + 8] == escapeDelimiter)
                                    {
                                        // with optional third pair
                                        output.Append(new string(data, index, 9));
                                        index += 8;
                                        continue;
                                    }
                                    break;
                                case 'X': // locally defined hex codes
                                case 'Z': // locally defined escape
                                    var zEscapeIndex = index + 1;
                                    var zEscapeLength = 1;
                                    var zEscapeEndFound = false;

                                    while (zEscapeIndex < length)
                                    {
                                        zEscapeLength++;
                                        if (data[zEscapeIndex] == escapeDelimiter)
                                        {
                                            zEscapeEndFound = true;
                                            break;
                                        }
                                        zEscapeIndex++;
                                    }

                                    if (zEscapeEndFound)
                                    {
                                        output.Append(new string(data, index, zEscapeLength));
                                        index += zEscapeLength - 1;
                                        continue;
                                    }

                                    break;
                            }
                        }
                    }

                    output.Append(new[] { escapeDelimiter, 'E', escapeDelimiter });
                    continue;
                }

                if (c == fieldDelimiter)
                {
                    output.Append(new[] { escapeDelimiter, 'F', escapeDelimiter });
                    continue;                    
                }

                if (c == repetitionDelimiter)
                {
                    output.Append(new[] { escapeDelimiter, 'R', escapeDelimiter });
                    continue;
                }

                if (c == subcomponentDelimiter)
                {
                    output.Append(new[] { escapeDelimiter, 'T', escapeDelimiter });
                    continue;
                }

                output.Append(c);
            }

            return output.ToString();
        }

        static string LookAhead(char[] data, int index, int length)
        {
            if (index >= data.Length)
            {
                return string.Empty;
            }

            if (index < 0)
            {
                length += index;
                index = 0;
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            if (index + length >= data.Length)
            {
                length -= (data.Length - (index + length));
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            return new string(data, index, length);
        }

        static public string UnEscape(this EncodingConfiguration config, string s)
        {
            if (s == null)
            {
                return null;
            }

            var componentDelimiter = config.ComponentDelimiter;
            var escapeDelimiter = config.EscapeDelimiter;
            var fieldDelimiter = config.FieldDelimiter;
            var repetitionDelimiter = config.RepetitionDelimiter;
            var subcomponentDelimiter = config.SubcomponentDelimiter;
            var data = s.ToCharArray();
            var length = data.Length;
            var output = new StringBuilder();

            for (var index = 0; index < length; index++)
            {
                var c = data[index];
            }

            return output.ToString();            
        }

        static string WrapCode(string code, char escapeDelimiter)
        {
            return string.Concat(escapeDelimiter, code, escapeDelimiter);
        }
    }
}