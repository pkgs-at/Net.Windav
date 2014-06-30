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
using System.Collections.Generic;
using System.Xml;
using System.Net;
using Net.Windav.HttpClient;

namespace Net.Windav.Protocol
{

    public class PropstatResponse : XmlDocumentHandler
    {

        public class Propstat
        {

            private Response _parent;

            private XmlElement _element;

            private StatusLine _status;

            private XmlElement _prop;

            public Propstat(Response parent, XmlElement element)
            {
                XmlElement status;
                XmlElement prop;

                this._parent = parent;
                this._element = element;
                status =
                    (XmlElement)this.Element.SelectSingleNode(
                        "./dav:status",
                        this.Namespaces);
                if (status == null) throw new ArgumentException();
                this._status = StatusLine.Parse(status.InnerText);
                prop =
                    (XmlElement)this.Element.SelectSingleNode(
                        "./dav:prop",
                        this.Namespaces);
                if (prop == null) throw new ArgumentException();
                this._prop = prop;
            }

            public Response Parent
            {
                get
                {
                    return this._parent;
                }
            }

            public XmlNamespaceManager Namespaces
            {
                get
                {
                    return this.Parent.Namespaces;
                }
            }

            public XmlElement Element
            {
                get
                {
                    return this._element;
                }
            }

            public StatusLine Status
            {
                get
                {
                    return this._status;
                }
            }

            public XmlElement Prop
            {
                get
                {
                    return this._prop;
                }
            }

            public override string ToString()
            {
                StringBuilder builder;
                System.IO.StringWriter buffer;

                builder = new StringBuilder();
                builder.Append(this.Status).Append("\r\n");
                buffer = new System.IO.StringWriter();
                using (XmlTextWriter writer = new XmlTextWriter(buffer))
                {
                    writer.Formatting = Formatting.Indented;
                    this.Prop.WriteTo(writer);
                }
                builder.Append(buffer.ToString());
                return builder.ToString();
            }

        }

        public class Response
        {

            private PropstatResponse _parent;

            private XmlElement _element;

            private string _resource;

            private IList<Propstat> _propstats;

            public Response(PropstatResponse parent, XmlElement element)
            {
                XmlElement href;
                XmlNodeList propstats;

                this._parent = parent;
                this._element = element;
                href =
                    (XmlElement)this.Element.SelectSingleNode(
                        "./dav:href",
                        this.Namespaces);
                if (href == null) throw new ArgumentException();
                this._resource = href.InnerText;
                this._propstats = new List<Propstat>();
                propstats =
                    this.Element.SelectNodes(
                        "./dav:propstat",
                        this.Namespaces);
                foreach (XmlNode node in propstats)
                    this._propstats.Add(new Propstat(this, (XmlElement)node));
            }

            public PropstatResponse Parent
            {
                get
                {
                    return this._parent;
                }
            }

            public XmlNamespaceManager Namespaces
            {
                get
                {
                    return this.Parent.Namespaces;
                }
            }

            public XmlElement Element
            {
                get
                {
                    return this._element;
                }
            }

            public string Resource
            {
                get
                {
                    return this._resource;
                }
            }

            public IList<Propstat> Propstats
            {
                get
                {
                    return this._propstats;
                }
            }

            public override string ToString()
            {
                StringBuilder builder;

                builder = new StringBuilder();
                builder.Append(this.Resource).Append("\r\n");
                foreach (Propstat propstat in this.Propstats)
                    builder.Append(propstat).Append("\r\n");
                return builder.ToString();
            }

        }

        private XmlNamespaceManager _namespaces;

        private IList<Response> _responses;

        public PropstatResponse()
        {
            this._namespaces = new XmlNamespaceManager(new NameTable());
            this._namespaces.AddNamespace("dav", "DAV:");
            this._responses = new List<Response>();
        }

        public XmlNamespaceManager Namespaces
        {
            get
            {
                return this._namespaces;
            }
        }

        public IList<Response> Responses
        {
            get
            {
                return this._responses;
            }
        }

        public override string ToString()
        {
            StringBuilder builder;

            builder = new StringBuilder();
            foreach (Response response in this.Responses)
                builder.Append(response).Append("\r\n");
            return builder.ToString();
        }

        public override void Handle(HttpWebResponse response)
        {
            XmlNodeList responses;

            base.Handle(response);
            responses =
                this.Document.SelectNodes(
                    "/dav:multistatus/dav:response",
                    this.Namespaces);
            foreach (XmlNode node in responses)
                this._responses.Add(new Response(this, (XmlElement)node));
        }

    }

}
