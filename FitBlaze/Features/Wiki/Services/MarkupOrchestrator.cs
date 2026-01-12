using FitBlaze.Features.Wiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitBlaze.Features.Wiki.Services
{
    public class MarkupOrchestrator : IMarkupOrchestrator
    {
        private readonly IEnumerable<IMarkupRenderer> _renderers;

        public MarkupOrchestrator(IEnumerable<IMarkupRenderer> renderers)
        {
            _renderers = renderers;
        }

        public string Render(string content, MarkupType type)
        {
            var renderer = _renderers.FirstOrDefault(r => r.SupportedType == type);
            if (renderer == null)
            {
                // Fallback or Exception. For now, exception to ensure we don't silently fail.
                throw new NotSupportedException($"No renderer found for markup type: {type}");
            }

            return renderer.Render(content);
        }
    }
}
