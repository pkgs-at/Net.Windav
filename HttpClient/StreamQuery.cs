﻿/*
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
using System.IO;
using System.Net;

namespace Net.Windav.HttpClient
{

    public abstract class StreamQuery : Query
    {

        public StreamQuery(string resource)
            : base(resource)
        {
            // do nothing
        }

        public abstract void WriteTo(Stream stream);

        public override HttpWebResponse Execute(HttpWebRequest request)
        {
            using (Stream stream = request.GetRequestStream())
            {
                this.WriteTo(stream);
            }
            return base.Execute(request);
        }

    }

}
