using System;
using System.Collections.Generic;
using System.Text;

namespace Deveel.Smpp.Types {
	public sealed class SmppTime : ISmppValue {
		public const int MinShortSize = 10;
		public const int MinRequiredSize = 12;
		public const int MaxRequiredSize = 16;

		private bool _includeUtc = false;

		internal SmppTime() {
		}

		public SmppTime(DateTime dateTime, bool includeUtc) {
			AbsoluteTime = dateTime;
			_includeUtc = includeUtc;
			Type = SmppDateTimeType.Absolute;
		}

		public SmppTime(TimeSpan timeSpan) 
			: this(timeSpan, false) {
		}

		public SmppTime(TimeSpan timeSpan, bool includeUtc) {
			RelativeTime = timeSpan;
			Type = SmppDateTimeType.Relative;
			_includeUtc = includeUtc;
		}

		public SmppTime(SmppTime time) {
			AbsoluteTime = time.AbsoluteTime;
			RelativeTime = time.RelativeTime;
			Type = time.Type;
			_includeUtc = time._includeUtc;
		}

		public SmppDateTimeType Type { get; }

		public DateTime AbsoluteTime { get; }

		public TimeSpan RelativeTime { get; }
		
		/// <inheritdoc />
		Type ISmppValue.RuntimeType => Type == SmppDateTimeType.Relative ? typeof(TimeSpan) : typeof(DateTime);

		/// <inheritdoc />
		object ISmppValue.GetRuntimeValue() {
			switch (Type) {
				case SmppDateTimeType.Relative: return RelativeTime;
				case SmppDateTimeType.Absolute: return AbsoluteTime;
				default: throw new InvalidOperationException();
			}
		}

		/// <inheritdoc />
		public override string ToString() {
			return Type == SmppDateTimeType.Relative ? RelativeTime.ToString() : AbsoluteTime.ToString();
		}

		public string ToSmppString() {
			if (Type == SmppDateTimeType.Relative) {
				if (RelativeTime.TotalSeconds > 0) {
					//TODO: Again, generalizing
					int totalDays = (int) RelativeTime.TotalDays;
					int totalYears = totalDays / 365;
					totalDays -= (totalYears * 365);
					int totalMonths = totalDays / 30;
					totalDays -= (totalMonths * 30);

					var sb = new StringBuilder();
					sb.AppendFormat("{0:d2}", totalDays);
					sb.AppendFormat("{0:d2}", totalMonths);
					sb.AppendFormat("{0:d2}", totalDays);
					sb.AppendFormat("{0:d2}", RelativeTime.Hours);
					sb.AppendFormat("{0:d2}", RelativeTime.Minutes);
					sb.AppendFormat("{0:d2}", RelativeTime.Seconds);
					sb.Append("000R");

					return sb.ToString();
				}
			}
			else {
				// DateTime.MinValue is used to indicate immediate delivery.
				if (AbsoluteTime != DateTime.MinValue) {
					if (_includeUtc) {
						var localTimeZone = TimeZoneInfo.Local;

						int nn = AbsoluteTime.Millisecond / 100;
						int yy = AbsoluteTime.Year;

						if (yy > 100) {
							string syy = yy.ToString();
							yy = Convert.ToInt32(syy.Substring(syy.Length - 2));
						}

						string type = (localTimeZone.GetUtcOffset(AbsoluteTime).Hours > 0) ? "+" : "-";

						var sb = new StringBuilder();
						sb.AppendFormat("{0:d2}", yy);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Month);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Day);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Hour);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Minute);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Second);
						sb.Append(nn.ToString().Substring(0, 1));

						sb.AppendFormat(
							"{0:d2}", (int) Math.Abs(localTimeZone.GetUtcOffset(AbsoluteTime).TotalMinutes) / 15);

						sb.Append(type);

						return sb.ToString();
					}
					else {
						int yy = AbsoluteTime.Year;

						if (yy > 100) {
							string syy = yy.ToString();
							yy = Convert.ToInt32(syy.Substring(syy.Length - 2));
						}

						var sb = new StringBuilder();
						sb.AppendFormat("{0:d2}", yy);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Month);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Day);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Hour);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Minute);
						sb.AppendFormat("{0:d2}", AbsoluteTime.Second);

						return sb.ToString();
					}
				}
			}

			return String.Empty;
		}

		private static bool TryParse(string s, out SmppTime result, out Exception error) {
			try {
				if (s.Length >= SmppTime.MinShortSize) {
					int MM, DD, YY, mm, hh, ss;

					YY = Convert.ToInt32(s.Substring(0, 2));
					MM = Convert.ToInt32(s.Substring(2, 2));
					DD = Convert.ToInt32(s.Substring(4, 2));
					hh = Convert.ToInt32(s.Substring(6, 2));
					mm = Convert.ToInt32(s.Substring(8, 2));
					ss = 0;
					if (s.Length >= SmppTime.MinRequiredSize) {
						ss = Convert.ToInt32(s.Substring(10, 2));
					}

					if (s.Length >= SmppTime.MaxRequiredSize) {
						int ms, nn;
						string type;

						ms = Convert.ToInt32(s.Substring(12, 1));
						nn = Convert.ToInt32(s.Substring(13, 2));
						type = s.Substring(15, 1);

						// If it is a relative time, then use a timespan.
						if (type == "R") {
							//TODO: we are generalizing here; 365 days per year, 30 days per month
							result = new SmppTime(new TimeSpan((YY * 365) + (MM * 30) + DD, hh, mm, ss, ms * 100), true);
							error = null;
							return true;
						}

						// Calculate the date and then convert it to UTC.
						var dateTime = new DateTime(YY + 2000, MM, DD, hh, mm, ss, ms * 100);
						if (type == "+")
							dateTime += new TimeSpan(0, 0, nn * 15, 0, 0);
						else if (type == "-")
							dateTime -= new TimeSpan(0, 0, nn * 15, 0, 0);

						// Now convert it to our local time.
						var localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
						result = new SmppTime(localTime, true);
						error = null;

						return true;
					}

					result = new SmppTime(new DateTime(YY + 2000, MM, DD, hh, mm, ss), false);
					error = null;

					return true;
				}

				error = new FormatException($"The string '{s}' has an invalid format for SMPP");
				result = null;

				return false;
			} catch (Exception ex) {
				error = new FormatException($"The string '{s}' is invalid for a SMPP time", ex);
				result = null;

				return false;
			}
		}

		public static bool TryParse(string s, out SmppTime result) {
			Exception error;

			return TryParse(s, out result, out error);
		}

		public static SmppTime Parse(string s) {
			if (!TryParse(s, out var result, out var error))
				throw error;

			return result;
		}
	}
}
