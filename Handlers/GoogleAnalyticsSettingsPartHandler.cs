using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using SH.GoogleAnalytics.Models;

namespace SH.GoogleAnalytics.Handlers {
	[UsedImplicitly]
	[OrchardFeature("SH.GoogleAnalytics")]
	public class GoogleAnalyticsSettingsPartHandler : ContentHandler {
		public GoogleAnalyticsSettingsPartHandler(IRepository<GoogleAnalyticsSettingsPartRecord> repository) {
			Filters.Add(new ActivatingFilter<GoogleAnalyticsSettingsPart>("Site"));
			Filters.Add(StorageFilter.For(repository));
		}
	}
}