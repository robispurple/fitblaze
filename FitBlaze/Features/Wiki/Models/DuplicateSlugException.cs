using System;

namespace FitBlaze.Features.Wiki.Models
{
    public class DuplicateSlugException : Exception
    {
        public string Slug { get; }

        public DuplicateSlugException(string slug) : base($"A page with slug '{slug}' already exists.")
        {
            Slug = slug;
        }

        public DuplicateSlugException(string slug, Exception innerException) : base($"A page with slug '{slug}' already exists.", innerException)
        {
            Slug = slug;
        }
    }
}
