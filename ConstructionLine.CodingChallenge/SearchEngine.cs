using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts ?? throw new ArgumentNullException(nameof(shirts));
        }

        public SearchResults Search(SearchOptions options)
        {
            if (options?.Colors == null || options.Sizes == null) throw new ArgumentNullException(nameof(options));

            var searchResults = new SearchResults
            {
                Shirts = new List<Shirt>(),
                ColorCounts = Color.All.Select(x => new ColorCount { Color = x, Count = 0 }).Distinct().ToList(),
                SizeCounts = Size.All.Select(x => new SizeCount { Size = x, Count = 0 }).Distinct().ToList()
            };

            // Parallel could be used if performance test failed
            var matchedShirts = _shirts.Where(shirt =>
                (!options.Colors.Any() || options.Colors.Any(x => x == shirt.Color)) &&
                (!options.Sizes.Any() || options.Sizes.Any(x => x == shirt.Size))).ToList();

            foreach (var shirt in matchedShirts)
            {
                UpdateMatchedShirtColorCount(searchResults, shirt);
                UpdateMatchedShirtSizeCount(searchResults, shirt);
            }

            return searchResults;
        }

        private void UpdateMatchedShirtColorCount(SearchResults results, Shirt shirt)
        {
            results.ColorCounts.Single(x => x.Color == shirt.Color).Count += 1;
        }

        private void UpdateMatchedShirtSizeCount(SearchResults results, Shirt shirt)
        {
            results.SizeCounts.Single(x => x.Size == shirt.Size).Count += 1;
        }
    }
}