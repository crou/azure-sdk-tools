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

namespace Microsoft.WindowsAzure.Management.ServiceBus.Test.UnitTests.Cmdlet
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Samples.WindowsAzure.ServiceManagement;
    using Microsoft.WindowsAzure.Management.CloudService.Test;
    using Microsoft.WindowsAzure.Management.CloudService.Test.Utilities;
    using Microsoft.WindowsAzure.Management.ServiceBus.Cmdlet;
    using Microsoft.WindowsAzure.Management.ServiceBus.Properties;
    using Microsoft.WindowsAzure.Management.Test.Stubs;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NewAzureSBNamespaceTests : TestBase
    {
        [TestInitialize]
        public void SetupTest()
        {
            Management.Extensions.CmdletSubscriptionExtensions.SessionManager = new InMemorySessionManager();
        }

        [TestMethod]
        public void NewAzureSBNamespaceSuccessfull()
        {
            // Setup
            SimpleServiceManagement channel = new SimpleServiceManagement();
            FakeWriter writer = new FakeWriter();
            string name = "test";
            string location = "West US";
            NewAzureSBNamespaceCommand cmdlet = new NewAzureSBNamespaceCommand(channel) { Name = name, Location = location, Writer = writer };
            ServiceBusNamespace expected = new ServiceBusNamespace { Name = name, Region = location };
            channel.CreateServiceBusNamespaceThunk = csbn => { return expected; };
            channel.ListServiceBusRegionsThunk = lsbr => 
            {
                List<ServiceBusRegion> list = new List<ServiceBusRegion>();
                list.Add(new ServiceBusRegion { Code = location });
                return list;
            };

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            ServiceBusNamespace actual = writer.OutputChannel[0] as ServiceBusNamespace;
            Assert.AreEqual<ServiceBusNamespace>(expected, actual);
        }

        [TestMethod]
        public void NewAzureSBNamespaceWithInvalidNamesFail()
        {
            // Setup
            string[] invalidNames = { "1test", "test#", "test invaid", "-test", "_test" };

            foreach (string invalidName in invalidNames)
            {
                FakeWriter writer = new FakeWriter();
                NewAzureSBNamespaceCommand cmdlet = new NewAzureSBNamespaceCommand() { Name = invalidName, Location = "West US", Writer = writer };
                string expected = string.Format("{0}\r\nParameter name: Name", string.Format(Resources.InvalidNamespaceName, invalidName));

                Testing.AssertThrows<ArgumentException>(() => cmdlet.ExecuteCmdlet(), expected);
            }
        }

        [TestMethod]
        public void NewAzureSBNamespaceWithInternalServerError()
        {
            // Setup
            SimpleServiceManagement channel = new SimpleServiceManagement();
            FakeWriter writer = new FakeWriter();
            string name = "test";
            string location = "West US";
            NewAzureSBNamespaceCommand cmdlet = new NewAzureSBNamespaceCommand(channel) { Name = name, Location = location, Writer = writer };
            channel.CreateServiceBusNamespaceThunk = csbns => { throw new Exception(Resources.InternalServerErrorMessage); };
            channel.ListServiceBusRegionsThunk = lsbr =>
            {
                List<ServiceBusRegion> list = new List<ServiceBusRegion>();
                list.Add(new ServiceBusRegion { Code = location });
                return list;
            };
            string expected = Resources.NewNamespaceErrorMessage;

            Testing.AssertThrows<Exception>(() => cmdlet.ExecuteCmdlet(), expected);
        }

        [TestMethod]
        public void NewAzureSBNamespaceWithInvalidLocation()
        {
            // Setup
            SimpleServiceManagement channel = new SimpleServiceManagement();
            FakeWriter writer = new FakeWriter();
            string name = "test";
            string location = "Invalid location";
            NewAzureSBNamespaceCommand cmdlet = new NewAzureSBNamespaceCommand(channel) { Name = name, Location = location, Writer = writer };
            channel.ListServiceBusRegionsThunk = lsbr =>
            {
                List<ServiceBusRegion> list = new List<ServiceBusRegion>();
                list.Add(new ServiceBusRegion { Code = "West US" });
                return list;
            };
            string expected = string.Format("{0}\r\nParameter name: Location", string.Format(Resources.InvalidServiceBusLocation, location));

            Testing.AssertThrows<ArgumentException>(() => cmdlet.ExecuteCmdlet(), expected);
        }
    }
}