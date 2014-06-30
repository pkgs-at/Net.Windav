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
using System.IO;
using System.Xml;

namespace Net.Windav.HttpClient
{

    public class XmlDocumentQuery : StreamQuery
    {

        public class Namespace
        {

            private XmlDocumentQuery _query;

            private string _prefix;

            private string _value;

            public Namespace(
                XmlDocumentQuery query,
                string prefix,
                string value)
            {
                this._query = query;
                this._prefix = prefix;
                this._value = value;
            }

            public XmlElement Element(string name)
            {
                return
                    this._query.Document.CreateElement(
                        this._prefix,
                        name,
                        this._value);
            }

        }

        private string _method;

        private XmlDocument _document;

        public XmlDocumentQuery(string method, string resource)
            : base(resource)
        {
            this._method = method;
            this._document = new XmlDocument();
        }

        public override string Method
        {
            get
            {
                return this._method;
            }
        }

        public XmlDocument Document
        {
            get
            {
                return this._document;
            }
        }

        public Namespace CreateNamespace(string prefix, string value)
        {
            return new Namespace(this, prefix, value);
        }

        public override void WriteTo(Stream stream)
        {
            this.Document.Save(stream);
        }

    }

}
