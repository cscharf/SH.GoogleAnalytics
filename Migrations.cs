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
        
        public int UpdateFrom1() {
            SchemaBuilder.AlterTable("GoogleAnalyticsSettingsPartRecord", table => table
                .AddColumn<bool>("UseDoubleClick")
           );
            return 2;
        }

        public int UpdateFrom2()
        {
            SchemaBuilder.AlterTable("GoogleAnalyticsSettingsPartRecord", table => {
                table.AddColumn<bool>("UseUniversalAnalytics");
                table.AddColumn<bool>("UseEnhancedLinkAttribution");
            });
            return 3;
        }

        public int UpdateFrom3()
        {
            SchemaBuilder.AlterTable("GoogleAnalyticsSettingsPartRecord", table => table
                .AddColumn<string>("AdditionalScripts", c => c.Unlimited())
           );
            return 4;
        }
    }
}