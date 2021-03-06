﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aggregator.cli
{

    [Verb("list.mappings", HelpText = "Lists mappings from existing VSTS Projects to Aggregator Rules.")]
    class ListMappingsCommand : CommandBase
    {
        [Option('i', "instance", Required = true, HelpText = "Aggregator instance name.")]
        public string Instance { get; set; }

        internal override async Task<int> RunAsync()
        {
            var context = await Context
                .WithVstsLogon()
                .Build();
            var instance = new InstanceName(Instance);
            var mappings = new AggregatorMappings(context.Vsts, /*HACK*/null, context.Logger);
            bool any = false;
            foreach (var item in mappings.List(instance))
            {
                context.Logger.WriteOutput(
                    item,
                    (data) => $"Rule {item.rule} in {item.project} for {item.events}");
                any = true;
            }
            if (!any)
            {
                context.Logger.WriteInfo("No rule mappings found.");
            }
            return 0;
        }
    }
}
