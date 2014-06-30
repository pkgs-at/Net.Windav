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
using Net.Windav.Protocol;

namespace Net.Windav
{

    public interface RemoteResourceOperation
    {

        Server Server
        {
            get;
        }

        string Path
        {
            get;
        }

        string Name
        {
            get;
        }

    }

    public interface RemoteResourceInformation : RemoteResourceOperation
    {

        DateTime CreatedAt
        {
            get;
        }

        DateTime ModifiedAt
        {
            get;
        }

    }

    public abstract class RemoteResource
    {

        private Server _server;

        private string _path;

        public RemoteResource(Server server, string path)
        {
            this._server = server;
            this._path = path;
        }

        public Server Server
        {
            get
            {
                return this._server;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
        }

        public abstract string Name
        {
            get;
        }

        public override string ToString()
        {
            return new Uri(this.Server.Host, this.Path).ToString();
        }

        protected PropstatResponse Propfind(int depth)
        {
            PropfindRequest request;

            request = new PropfindRequest(this.Path, depth);
            request.Propfind
                .AppendChild(request.Dav.Element("prop"))
                .AppendChild(request.Dav.Element("resourcetype")).ParentNode
                .AppendChild(request.Dav.Element("creationdate")).ParentNode
                .AppendChild(request.Dav.Element("getlastmodified")).ParentNode
                .AppendChild(request.Dav.Element("getcontentlength"));
            return request.Execute(this.Server, new PropstatResponse());
        }

        protected void IsCollection(
            PropstatResponse.Propstat model,
            ref bool value)
        {
            XmlNode node;

            node = model.Prop.SelectSingleNode(
                "./dav:resourcetype/dav:collection",
                model.Namespaces);
            if (node == null) return;
            value = true;
        }

        protected void GetCreationDate(
            PropstatResponse.Propstat model,
            ref DateTime value)
        {
            XmlNode node;

            node = model.Prop.SelectSingleNode(
                "./dav:creationdate",
                model.Namespaces);
            if (node == null) return;
            value = DateTimeVariant.ISO8601.Parse(node.InnerText).Value;
        }

        protected void GetLastModified(
            PropstatResponse.Propstat model,
            ref DateTime value)
        {
            XmlNode node;

            node = model.Prop.SelectSingleNode(
                "./dav:getlastmodified",
                model.Namespaces);
            if (node == null) return;
            value = DateTimeVariant.RFC1123.Parse(node.InnerText).Value;
        }

        protected void GetContentLength(
            PropstatResponse.Propstat model,
            ref long value)
        {
            XmlNode node;

            node = model.Prop.SelectSingleNode(
                "./dav:getcontentlength",
                model.Namespaces);
            if (node == null) return;
            value = long.Parse(node.InnerText);
        }

        protected RemoteResourceInformation ParseResourceInformation(
            PropstatResponse.Response model)
        {
            bool collection;
            DateTime creationDate;
            DateTime lastModified;
            long contentLength;

            collection = false;
            creationDate = default(DateTime);
            lastModified = default(DateTime);
            contentLength = 0L;
            foreach (PropstatResponse.Propstat propstat in model.Propstats)
            {
                if (propstat.Status.StatusCode != 200) continue;
                this.IsCollection(propstat, ref collection);
                this.GetCreationDate(propstat, ref creationDate);
                this.GetLastModified(propstat, ref lastModified);
                this.GetContentLength(propstat, ref contentLength);
            }
            if (collection)
            {
                return new RemoteDirectoryInformation(
                    this.Server,
                    model.Resource,
                    creationDate,
                    lastModified);
            }
            else
            {
                return new RemoteFileInformation(
                    this.Server,
                    model.Resource,
                    creationDate,
                    lastModified,
                    contentLength);
            }

        }

    }

}
