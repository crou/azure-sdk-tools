﻿// ----------------------------------------------------------------------------------
//
// Copyright 2011 Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ---------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.Websites.Services.Github.Entities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GithubAuthorizationRequest
    {
        [DataMember(Name = "scopes", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> Scopes { get; set; }

        [DataMember(Name = "note", IsRequired = false, EmitDefaultValue = false)]
        public string Note { get; set; }

        [DataMember(Name = "note_url", IsRequired = false, EmitDefaultValue = false)]
        public string NoteUrl { get; set; }
    }
}
