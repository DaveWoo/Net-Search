using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net.Layout.Pattern;
using log4net.Util;

namespace Net.Utils.Logging
{
	public class CallContextPatternConverter : PatternLayoutConverter
	{
		/// <inheritdoc/>
		protected override void Convert(TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
		{
			var properties = loggingEvent.Properties[CallContextPredefinedProperties.CallContextProperties] as IEnumerable<Tuple<string, object>>;

			if (properties == null)
			{
				return;
			}

			var outputValues =
				properties.Where(prop => !prop.Item1.StartsWith(CallContextPredefinedProperties.NoLogPrefix))
					.Select(prop => string.Format("{0}=\"{1}\"", prop.Item1, prop.Item2 ?? SystemInfo.NullText))
					.ToArray();

			if (outputValues.Any())
			{
				writer.Write(", {0}", String.Join(", ", outputValues));
			}
		}
	}
}