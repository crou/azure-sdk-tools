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
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.CloudService.Cmdlet
{
    using System.Management.Automation;
    using System.Security.Permissions;
    using Model;

    /// <summary>
    /// Adjusts the service configuration.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AzureServiceProject")]
    public class SetAzureServiceProjectCommand : SetSettings
    {
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Location { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Slot { set; get; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Storage { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Subscription { get; set; }

        public SetAzureServiceProjectCommand()
        {
            SkipChannelInit = true;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public ServiceSettings SetAzureServiceProjectProcess(string newLocation, string newSlot, string newStorage, string newSubscription, string settingsPath)
        {
            ServiceSettings settings = ServiceSettings.Load(settingsPath);
            if (newLocation != null)
            {
                settings.Location = newLocation;
            }

            if (newSlot != null)
            {
                settings.Slot = newSlot;
            }

            if (newStorage != null)
            {
                settings.StorageAccountName = newStorage;
            }

            if (newSubscription != null)
            {
                settings.Subscription = newSubscription;
            }

            if (newLocation != null || newSlot != null || newStorage != null || newSubscription != null)
            {
                settings.Save(settingsPath);
            }

            WriteOutputObject(settings);

            return settings;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();
            this.SetAzureServiceProjectProcess(
                Location,
                Slot,
                Storage,
                Subscription,
                base.GetServiceSettingsPath(false));
        }
    }
}