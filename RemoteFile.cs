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
using Net.Windav.Protocol;

namespace Net.Windav
{

    public class RemoteFile : RemoteResource
    {

        public RemoteFile(Server server, string path)
            : base(server, path)
        {
            // do nothing
        }

        public override string Name
        {
            get
            {
                string[] parts;

                parts = this.Path.Split('/');
                return parts[parts.Length - 1];
            }
        }

        public virtual RemoteFileInformation GetFileInformation()
        {
            foreach (
                PropstatResponse.Response response
                in this.Propfind(1).Responses)
            {
                RemoteResourceInformation information;

                if (response.Resource != this.Path) continue;
                information = this.ParseResourceInformation(response);
                if (!(information is RemoteFileInformation)) continue;
                return (RemoteFileInformation)information;
            }
            return null;
        }

    }

    public class RemoteFileInformation
        : RemoteFile, RemoteResourceInformation
    {

        private DateTime _createdAt;

        private DateTime _modifiedAt;

        private long _size;

        public RemoteFileInformation(
            Server server,
            string path,
            DateTime createdAt,
            DateTime modifiedAt,
            long size)
            : base(server, path)
        {
            this._createdAt = createdAt;
            this._modifiedAt = modifiedAt;
            this._size = size;
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

        public long Size
        {
            get
            {
                return this._size;
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
            builder.Append("size: ").Append(this.Size);
            builder.Append("\r\n");
            return builder.ToString();
        }

        public override RemoteFileInformation GetFileInformation()
        {
            return this;
        }

    }

}
