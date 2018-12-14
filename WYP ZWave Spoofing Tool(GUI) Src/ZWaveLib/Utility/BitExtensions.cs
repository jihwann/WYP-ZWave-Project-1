/*
  This file is part of ZWaveLib (https://github.com/genielabs/zwave-lib-dotnet)

  Copyright (2012-2018) G-Labs (https://github.com/genielabs)

  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
*/

using System;

namespace ZWaveLib
{
    internal static class BitExtensions
    {
        internal static byte[] ToBigEndianBytes(this ushort value)
        {
            var valueBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                valueBytes = new[] { valueBytes[1], valueBytes[0] };
            return valueBytes;
        }

        internal static ushort FromBigEndianBytes(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                bytes = new[] {bytes[1], bytes[0]};

            return BitConverter.ToUInt16(bytes, 0);
        }
    }
}
