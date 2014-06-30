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
using System.Xml;
using Net.Windav.HttpClient;

namespace Net.Windav.Protocol
{

    public class PropfindRequest : XmlDocumentQuery
    {

        private int? _depth;

        private Namespace _dav;

        private XmlElement _propfind;

        public PropfindRequest(string resource, int? depth)
            : base("PROPFIND", resource)
        {
            this._depth = depth;
            if (depth != null)
                this.Headers.Add(
                    "depth",
                    depth.Value.ToString("D"));
            this._dav = this.CreateNamespace("dav", "DAV:");
            this._propfind =
                (XmlElement)this.Document.AppendChild(
                    this.Dav.Element("propfind"));
        }

        public int? Depth
        {
            get
            {
                return this._depth;
            }
        }

        public Namespace Dav
        {
            get
            {
                return this._dav;
            }
        }

        public XmlElement Propfind
        {
            get
            {
                return this._propfind;
            }
        }

    }

}
