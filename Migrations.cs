using Orchard.Data.Migration;
using Orchard;
using Orchard.ContentManagement;
using SH.GoogleAnalytics.Models;

namespace SH.GoogleAnalytics {
	public class Migrations : DataMigrationImpl {
		public int Create() {
			SchemaBuilder.CreateTable("GoogleAnalyticsSettingsPartRecord", 
				table => table
					.ContentPartRecord()
					.Column<string>("GoogleAnalyticsKey")
					.Column<string>("DomainName")
					.Column<bool>("UseAsyncTracking")
					.Column<bool>("TrackOnAdmin")
				);
			return 1;
		}
	}
}