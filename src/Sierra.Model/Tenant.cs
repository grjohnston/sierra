﻿namespace Sierra.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using Eshopworld.DevOps;

    [DataContract]
    public class Tenant
    {
        [DataMember]
        [Key, Required, MaxLength(6)]
        public string Code { get; set; }

        [DataMember]
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [DataMember]
        public List<Fork> CustomSourceRepos { get; set; }

        /// <summary>
        /// Forks + Core repos (when supported)
        /// </summary>
        [DataMember]
        public List<VstsBuildDefinition> BuildDefinitions { get; set; }

        [DataMember]
        public List<VstsReleaseDefinition> ReleaseDefinitions { get; set; }

        [DataMember]
        public List<ResourceGroup> ResourceGroups { get; set; }

        [DataMember]
        public List<ManagedIdentity> ManagedIdentities { get; set; }

        private static readonly ToStringEqualityComparer<Fork> ForkEqComparer = new ToStringEqualityComparer<Fork>();

        public Tenant()
        {
            CustomSourceRepos = new List<Fork>();
            BuildDefinitions = new List<VstsBuildDefinition>();
            ReleaseDefinitions = new List<VstsReleaseDefinition>();
            ResourceGroups = new List<ResourceGroup>();
            ManagedIdentities = new List<ManagedIdentity>();
        }

        public Tenant(string code) : this()
        {
            Code = code;
        }

        /// <summary>
        /// project new state onto the current instance
        /// </summary>
        /// <param name="newState">new intended state</param>
        public void Update(Tenant newState)
        {
            if (newState == null)
                return;

            Name = newState.Name;

            var newStateForks = newState.CustomSourceRepos.Select(r => new Fork(r.SourceRepositoryName, Code, r.ProjectType)).ToList();

            //update forks and build definitions (1:1) - additions and removals
            newStateForks
                .Except(CustomSourceRepos, ForkEqComparer)
                .ToList()
                .ForEach(f =>
                {
                    f.TenantCode = Code;
                    CustomSourceRepos.Add(f);
                    var bd = new VstsBuildDefinition(f, Code);
                    BuildDefinitions.Add(bd);
                    ReleaseDefinitions.Add(new VstsReleaseDefinition(bd, Code));
                });

            CustomSourceRepos
                .Except(newStateForks, ForkEqComparer)
                .ToList()
                .ForEach(f =>
                {
                    f.State = EntityStateEnum.ToBeDeleted;
                    var bd = BuildDefinitions.Single(b => Equals(b.SourceCode, f));
                    bd.State = EntityStateEnum.ToBeDeleted;
                    bd.ReleaseDefinition.State = EntityStateEnum.ToBeDeleted;
                });
        }
    }
}
