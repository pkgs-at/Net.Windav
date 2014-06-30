/*
 * Copyright (c) 2009-2013, Architector Inc., Japan
 * All rights reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Globalization;

namespace Net.Windav.Protocol
{

    public class DateTimeVariant
    {

        private DateTime _value;

        public DateTimeVariant(DateTime value)
        {
            this._value = value;
        }

        public DateTime Value
        {
            get
            {
                return this._value;
            }
        }

        public class RFC1123 : DateTimeVariant
        {

            public RFC1123(DateTime value)
                : base(value)
            {
                // do nothing
            }

            public override string ToString()
            {
                return this.Value.ToUniversalTime().ToString("r");
            }

            public static RFC1123 Parse(string text)
            {
                DateTime value;

                value =
                    DateTime.ParseExact(
                        text,
                        CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern,
                        DateTimeFormatInfo.InvariantInfo,
                        DateTimeStyles.None);
                return new RFC1123(value.ToLocalTime());
            }

        }

        public class ISO8601 : DateTimeVariant
        {

            public ISO8601(DateTime value)
                : base(value)
            {
                // do nothing
            }

            public override string ToString()
            {
                return this.Value.ToUniversalTime().ToString("o");
            }

            public static ISO8601 Parse(string text)
            {
                DateTime value;

                value =
                    DateTime.Parse(
                        text,
                        null,
                        DateTimeStyles.RoundtripKind);
                return new ISO8601(value.ToLocalTime());
            }

        }

    }

}
