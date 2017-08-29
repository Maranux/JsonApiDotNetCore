using System;
using System.Collections.Generic;
using JsonApiDotNetCore.Builders;
using JsonApiDotNetCore.Internal;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace JsonApiDotNetCore.Configuration
{
    public class JsonApiOptions
    {
        public string Namespace { get; set; }
        public int DefaultPageSize { get; set; }
        public bool IncludeTotalRecordCount { get; set; }
        public bool AllowClientGeneratedIds { get; set; }
        public IContextGraph ContextGraph { get; set; }
        public bool RelativeLinks { get; set; }
        public IContractResolver JsonContractResolver { get; set; } = new DasherizedResolver();
        internal IContextGraphBuilder ContextGraphBuilder { get; } = new ContextGraphBuilder();
        internal List<JsonApiExtension> EnabledExtensions { get; set; } = new List<JsonApiExtension>();

        public void BuildContextGraph<TContext>(Action<IContextGraphBuilder> builder)
        where TContext : DbContext
        {
            BuildContextGraph(builder);

            ContextGraphBuilder.AddDbContext<TContext>();

            ContextGraph = ContextGraphBuilder.Build();
        }

        public void BuildContextGraph(Action<IContextGraphBuilder> builder)
        {
            if (builder == null) return;

            builder(ContextGraphBuilder);

            ContextGraph = ContextGraphBuilder.Build();
        }

        public void EnableExtension(JsonApiExtension extension)
        {
            EnabledExtensions.Add(extension);
        }
    }
}
