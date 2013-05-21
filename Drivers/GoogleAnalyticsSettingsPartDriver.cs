using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using SH.GoogleAnalytics.Models;

namespace GoogleAnalytics.Drivers {
	public class GoogleAnalyticsSettingsPartDriver : ContentPartDriver<GoogleAnalyticsSettingsPart> {
		private const string TemplateName = "Parts/GoogleAnalyticsSettings";

		public GoogleAnalyticsSettingsPartDriver() {
			T = NullLocalizer.Instance;
		}

		public Localizer T { get; set; }

		protected override string Prefix { get { return "GoogleAnalyticsSettings"; } }
		
		//GET
		protected override DriverResult Editor(GoogleAnalyticsSettingsPart part, dynamic shapeHelper) {
			return ContentShape("Parts_GoogleAnalyticsSettings_Edit",
					() => shapeHelper.EditorTemplate(
						TemplateName: TemplateName,
						Model: part,
						Prefix: Prefix));
		}

		//POST
		protected override DriverResult Editor(GoogleAnalyticsSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
			updater.TryUpdateModel(part, Prefix, null, null);
			return Editor(part, shapeHelper);
		}
	}
}