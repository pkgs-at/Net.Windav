/*
 * Copyright (c) 2009-2014, Architector Inc., Japan
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
using System.Net;
using Net.Windav.HttpClient;

namespace Net.Windav
{

    public class Server
    {

        private Uri _host;

        private int _timeout;

        private WebHeaderCollection _headers;

        public Server(Uri host, int timeout, WebHeaderCollection headers)
        {
            this._host = host;
            this._timeout = timeout;
            this._headers = headers;
        }

        public Server(string host, int timeout, WebHeaderCollection headers)
            : this(new Uri(host), timeout, headers)
        {
            // do nothing
        }

        public Server(Uri host, int timeout)
            : this(host, timeout, new WebHeaderCollection())
        {
            // do nothing
        }

        public Server(string host, int timeout)
            : this(host, timeout, new WebHeaderCollection())
        {
            // do nothing
        }

        public Server(Uri host, WebHeaderCollection headers)
            : this(host, 10000, headers)
        {
            // do nothing
        }

        public Server(string host, WebHeaderCollection headers)
            : this(host, 10000, headers)
        {
            // do nothing
        }

        public Server(Uri host)
            : this(host, 10000, new WebHeaderCollection())
        {
            // do nothing
        }

        public Server(string host)
            : this(host, 10000, new WebHeaderCollection())
        {
            // do nothing
        }

        public Uri Host
        {
            get
            {
                return this._host;
            }
        }

        public int Timeout
        {
            get
            {
                return this._timeout;
            }
            set
            {
                this._timeout = value;
            }
        }

        public WebHeaderCollection Headers
        {
            get
            {
                return this._headers;
            }
        }

        protected virtual void Build(HttpWebRequest request)
        {
            request.Timeout = this.Timeout;
            request.Headers.Add(this.Headers);
        }

        public virtual HttpWebResponse Invoke(Query query)
        {
            Uri uri;
            HttpWebRequest request;

            uri = new Uri(this.Host, query.Resource);
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = query.Method;
            this.Build(request);
            query.Prepare(request);
            return query.Execute(request);
        }

        public RemoteDirectory Directory(string path)
        {
            return new RemoteDirectory(this, path);
        }

        public RemoteFile File(string path)
        {
            return new RemoteFile(this, path);
        }

    }

}
