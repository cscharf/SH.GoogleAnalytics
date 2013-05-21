using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;

namespace SH.GoogleAnalytics.Models {
	public class GoogleAnalyticsSettingsPart : ContentPart<GoogleAnalyticsSettingsPartRecord> {
		/// <summary>
		/// Gets or sets the Google Analytics tracking key used to perform analytics tracking.
		/// </summary>
		[Required]
		[RegularExpression(@"^UA\-\d{1,}\-\d{1,}$")]
		public string GoogleAnalyticsKey {
			get { return Record.GoogleAnalyticsKey; }
			set { Record.GoogleAnalyticsKey = value; }
		}

		/// <summary>
		/// Gets or sets the override domain name that may optionally be used for performing things like multiple domain/sub-domain tracking.
		/// </summary>
		public string DomainName {
			get { return Record.DomainName; }
			set { Record.DomainName = value; }
		}

		/// <summary>
		/// Gets or sets whether the new Google Analytics asynchronous tracking method should be used.
		/// </summary>
		public bool UseAsyncTracking {
			get { return Record.UseAsyncTracking; }
			set { Record.UseAsyncTracking = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether Google Analytics tracking will be included on /Admin pages.
		/// </summary>
		public bool TrackOnAdmin {
			get { return Record.TrackOnAdmin; }
			set { Record.TrackOnAdmin = value; }
		}
	}
}