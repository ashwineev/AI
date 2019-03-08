// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Solutions.Skills;
using PhoneSkill.Common;
using PhoneSkill.Models;
using PhoneSkill.ServiceClients.GoogleAPI;
using PhoneSkill.ServiceClients.MSGraphAPI;

namespace PhoneSkill.ServiceClients
{
    public class ServiceManager : IServiceManager
    {
        private SkillConfigurationBase _skillConfig;

        public ServiceManager(SkillConfigurationBase config)
        {
            _skillConfig = config;
        }

        public IContactProvider GetContactProvider(string token, ContactSource source)
        {
            switch (source)
            {
                case ContactSource.Microsoft:
                    var serviceClient = GraphClient.GetAuthenticatedClient(token);
                    return new GraphContactProvider(serviceClient);
                case ContactSource.Google:
                    var googleClient = new GoogleClient(_skillConfig, token);
                    return new GoogleContactProvider(googleClient);
                default:
                    throw new Exception($"ContactSource not covered in switch statement: {source.ToString()}");
            }
        }
    }
}
