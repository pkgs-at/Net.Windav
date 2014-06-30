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

namespace Net.Windav.HttpClient
{

    public abstract class Query
    {

        private string _resource;

        private WebHeaderCollection _headers;

        public Query(string resource)
        {
            this._resource = resource;
            this._headers = new WebHeaderCollection();
        }

        public abstract string Method
        {
            get;
        }

        public virtual string Resource
        {
            get
            {
                return this._resource;
            }
        }

        public WebHeaderCollection Headers
        {
            get
            {
                return this._headers;
            }
        }

        public virtual void Prepare(HttpWebRequest request)
        {
            request.Headers.Add(this.Headers);
        }

        public virtual HttpWebResponse Execute(HttpWebRequest request)
        {
            return (HttpWebResponse)request.GetResponse();
        }

        public virtual HandlerType Execute<HandlerType>(
            Server server,
            HandlerType handler)
            where HandlerType : Handler
        {
            using (HttpWebResponse response = server.Invoke(this))
            {
                handler.Handle(response);
                return handler;
            }
        }

    }

}
