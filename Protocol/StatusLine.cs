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
using System.Text;
using System.Text.RegularExpressions;

namespace Net.Windav.Protocol
{

    public class StatusLine
    {

        private string _httpVersion;

        private int _statusCode;

        private string _reasonPhrase;

        public StatusLine(
            string httpVersion,
            int statusCode,
            string reasonPhrase)
        {
            this._httpVersion = httpVersion;
            this._statusCode = statusCode;
            this._reasonPhrase = reasonPhrase;
        }

        public string HttpVersion
        {
            get
            {
                return this._httpVersion;
            }
        }

        public int StatusCode
        {
            get
            {
                return this._statusCode;
            }
        }

        public string ReasonPhrase
        {
            get
            {
                return this._reasonPhrase;
            }
        }

        public override string ToString()
        {
            StringBuilder builder;

            builder = new StringBuilder();
            builder.Append(this.HttpVersion);
            builder.Append(' ');
            builder.Append(this.StatusCode);
            builder.Append(' ');
            builder.Append(this.ReasonPhrase);
            return builder.ToString();
        }

        public static readonly Regex Pattern =
            new Regex(@"(HTTP/\d\.\d) (\d{3}) (.*)");

        public static StatusLine Parse(string text)
        {
            Match match;

            match = Pattern.Match(text);
            if (!match.Success) throw new ArgumentException();
            return new StatusLine(
                match.Groups[1].Value,
                int.Parse(match.Groups[2].Value),
                match.Groups[3].Value);
        }

    }

}
