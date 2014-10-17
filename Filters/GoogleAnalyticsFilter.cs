using System.Text;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Mvc.Filters;
using Orchard.UI.Resources;
using SH.GoogleAnalytics.Models;
using System;

namespace SH.GoogleAnalytics.Filters {
    [OrchardFeature("SH.GoogleAnalytics")]
    public class GoogleAnalyticsFilter : FilterProvider, IResultFilter {
        private readonly IResourceManager _resourceManager;
        private readonly IOrchardServices _orchardServices;

        public GoogleAnalyticsFilter(IResourceManager resourceManager, IOrchardServices orchardServices) {
            _resourceManager = resourceManager;
            _orchardServices = orchardServices;
        }

        #region IResultFilter Members

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
                return;	
                
            //Determine if we're on an admin page
            bool isAdmin = Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext);

            //Get our part data/record if available for rendering scripts
            var part = _orchardServices.WorkContext.CurrentSite.As<GoogleAnalyticsSettingsPart>();
            if (part == null || string.IsNullOrWhiteSpace(part.GoogleAnalyticsKey) || (!part.TrackOnAdmin && isAdmin))
                return; // Not a valid configuration, ignore filter

            if (part.UseUniversalAnalytics)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine("<script>");
                script.AppendLine("(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)})(window,document,'script','//www.google-analytics.com/analytics.js','ga');");
                script.AppendFormat("ga('create','{0}','{1}');", part.GoogleAnalyticsKey, string.IsNullOrWhiteSpace(part.DomainName) ? "auto" : part.DomainName);
                // Display Advertising
                if (part.UseDoubleClick)
                    script.Append("ga('require','displayfeatures');");
                // Link Attribution
                if (part.UseEnhancedLinkAttribution)
                    script.Append("ga('require','linkid','linkid.js');");
                script.AppendLine("ga('send','pageview');");
                script.AppendLine("</script>");
                _resourceManager.RegisterHeadScript(script.ToString());
            }
            else if (part.UseAsyncTracking) {
                StringBuilder script = new StringBuilder(800);
                script.AppendLine("<script type=\"text/javascript\">");
                script.AppendLine("var _gaq=_gaq||[];");
                script.AppendLine("_gaq.push([\"_setAccount\",\"" + part.GoogleAnalyticsKey + "\"]);");
                if (!string.IsNullOrEmpty(part.DomainName)) {
                    script.AppendLine("_gaq.push([\"_setDomainName\",\"" + part.DomainName + "\"]);");
                    script.AppendLine("_gaq.push([\"_setAllowHash\",false]);");
                }
                if (part.UseEnhancedLinkAttribution)
                    script.AppendLine("_gaq.push(['_require','inpage_linkid','//www.google-analytics.com/plugins/ga/inpage_linkid.js']);");
                script.AppendLine("_gaq.push([\"_trackPageview\"]);");
                script.AppendLine("(function() {");
                script.AppendLine("\tvar ga=document.createElement(\"script\");ga.type=\"text/javascript\";ga.async=true;");
                if (part.UseDoubleClick)
                    script.AppendLine("\tga.src=((\"https:\" == document.location.protocol)?\"https://\":\"http://\")+\"stats.g.doubleclick.net/dc.js\";");
                else
                    script.AppendLine("\tga.src=((\"https:\" == document.location.protocol)?\"https://ssl\":\"http://www\")+\".google-analytics.com/ga.js\";");
                script.AppendLine("\tvar s=document.getElementsByTagName(\"script\")[0];s.parentNode.insertBefore(ga, s);");
                script.AppendLine("})();");
                script.AppendLine("</script>");
                // Register Google's new, recommended asynchronous analytics script to the header
                _resourceManager.RegisterHeadScript(script.ToString());
            }
            else {
                StringBuilder script = new StringBuilder(700);
                script.AppendLine("<script type=\"text/javascript\">");
                
                if (part.UseDoubleClick) {
                    script.AppendLine("var gaJsHost=((\"https:\"==document.location.protocol)?\"https://\":\"http://\");");
                    script.AppendLine("document.write(unescape(\"%3Cscript src='\"+gaJsHost+\"stats.g.doubleclick.net/dc.js' type='text/javascript'%3E%3C/script%3E\"));");
                }
                else {
                    script.AppendLine("var gaJsHost=((\"https:\"==document.location.protocol)?\"https://ssl.\":\"http://www.\");");
                    script.AppendLine("document.write(unescape(\"%3Cscript src='\"+gaJsHost+\"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\"));");
                }
                script.AppendLine("</script>");
                script.AppendLine("<script type=\"text/javascript\">");
                script.AppendLine("try {");
                script.AppendLine("\tvar pageTracker=_gat._getTracker(\"" + part.GoogleAnalyticsKey + "\");");
                if (!string.IsNullOrWhiteSpace(part.DomainName))
                    script.AppendLine("\tpageTracker._setDomainName(\"" + part.DomainName + "\");");
                script.AppendLine("\tpageTracker._trackPageview();");
                script.AppendLine("}");
                script.AppendLine("catch(err){}");
                script.AppendLine("</script>");
                // Register Google's old synchronous analytics script to the footer
                _resourceManager.RegisterFootScript(script.ToString());
            }

            if (!string.IsNullOrWhiteSpace(part.AdditionalScripts))
            {
                string addScript = part.AdditionalScripts.Trim();
                if (!addScript.StartsWith("<"))
                    addScript = string.Concat("<script>", Environment.NewLine, addScript, Environment.NewLine, "</script>");
                _resourceManager.RegisterHeadScript(addScript);
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        #endregion
    }
}