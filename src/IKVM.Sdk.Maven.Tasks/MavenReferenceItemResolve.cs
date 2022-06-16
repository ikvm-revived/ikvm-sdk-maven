﻿
using System;
using System.Collections.Generic;

using IKVM.Sdk.Maven.Tasks.Resources;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using org.apache.maven.model;
using org.apache.maven.project;

namespace IKVM.Sdk.Maven.Tasks
{

    /// <summary>
    /// For each <see cref="MavenReferenceItem"/>, resolves the full set of IkvmReferenceItem's that should be generated.
    /// </summary>
    public class MavenReferenceItemResolve : Task
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MavenReferenceItemResolve() :
            base(SR.ResourceManager, "MAVEN:")
        {

        }

        /// <summary>
        /// Set of MavenReferenceItem
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Set of IkvmReferenceItem to emit.
        /// </summary>
        [Output]
        public ITaskItem[] ResolvedItems { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            try
            {
                var items = MavenReferenceItemUtil.Import(Items);
                var resolvedItems = new List<ITaskItem>();

                foreach (var item in items)
                    resolvedItems.Add(GetResolvedItem(item));

                ResolvedItems = resolvedItems.ToArray();
                return true;
            }
            catch (MavenTaskMessageException e)
            {
                Log.LogErrorWithCodeFromResources(e.MessageResourceName, e.MessageArgs);
                return false;
            }
        }

        TaskItem GetResolvedItem(MavenReferenceItem item)
        {
            var i = new TaskItem(item.GroupId);
            i.SetMetadata("Compile", item.ItemSpec);
            return i;
        }

    }

}