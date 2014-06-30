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
using Net.Windav.Protocol;

namespace Net.Windav
{

    public class RemoteDirectory : RemoteResource
    {

        public static string NormalizeDirectoryPath(string path)
        {
            if (String.IsNullOrEmpty(path)) return "/";
            if (!path.StartsWith("/")) path = "/" + path;
            if (!path.EndsWith("/")) path = path + "/";
            return path;
        }

        public RemoteDirectory(Server server, string path)
            : base(server, NormalizeDirectoryPath(path))
        {
            // do nothing
        }

        public override string Name
        {
            get
            {
                string[] parts;

                parts = this.Path.Split('/');
                return parts[parts.Length - 2];
            }
        }

        public virtual RemoteDirectoryInformation GetDirectoryInformation()
        {
            foreach (
                PropstatResponse.Response response
                in this.Propfind(1).Responses)
            {
                RemoteResourceInformation information;

                if (response.Resource != this.Path) continue;
                information = this.ParseResourceInformation(response);
                if (!(information is RemoteDirectoryInformation)) continue;
                return (RemoteDirectoryInformation)information;
            }
            return null;
        }

        public IList<RemoteResourceInformation> GetResourceInformations()
        {
            IList<RemoteResourceInformation> list;

            list = new List<RemoteResourceInformation>();
            foreach (
                PropstatResponse.Response response
                in this.Propfind(1).Responses)
            {
                if (response.Resource == this.Path) continue;
                list.Add(this.ParseResourceInformation(response));
            }
            return list;
        }

    }

    public class RemoteDirectoryInformation
        : RemoteDirectory, RemoteResourceInformation
    {

        private DateTime _createdAt;

        private DateTime _modifiedAt;

        public RemoteDirectoryInformation(
            Server server,
            string path,
            DateTime createdAt,
            DateTime modifiedAt)
            : base(server, path)
        {
            this._createdAt = createdAt;
            this._modifiedAt = modifiedAt;
        }

        public DateTime CreatedAt
        {
            get
            {
                return this._createdAt;
            }
        }

        public DateTime ModifiedAt
        {
            get
            {
                return this._modifiedAt;
            }
        }

        public override string ToString()
        {
            StringBuilder builder;

            builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\r\n");
            builder.Append("created_at: ").Append(this.CreatedAt);
            builder.Append("\r\n");
            builder.Append("modified_at: ").Append(this.ModifiedAt);
            builder.Append("\r\n");
            return builder.ToString();
        }

        public override RemoteDirectoryInformation GetDirectoryInformation()
        {
            return this;
        }

    }

}
