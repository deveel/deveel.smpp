using System;

using Xunit;

namespace Deveel.Smpp.Types {
	public static class SmppTimeTests {
		[Theory]
		[InlineData("2012120114", 2020, 12, 12, 01, 14)]
		[InlineData("2103110923", 2021, 03, 11, 09, 23)]
		public static void ParseAbsoluteShort(string s, int year, int month, int day, int hour, int minute) {
			var result = SmppTime.TryParse(s, out SmppTime time);

			Assert.True(result);
			Assert.Equal(SmppDateTimeType.Absolute, time.Type);
			Assert.Equal(year, time.AbsoluteTime.Year);
			Assert.Equal(month, time.AbsoluteTime.Month);
			Assert.Equal(day, time.AbsoluteTime.Day);
			Assert.Equal(hour, time.AbsoluteTime.Hour);
			Assert.Equal(minute, time.AbsoluteTime.Minute);
		}

		[Theory]
		[InlineData("201212011432", 2020, 12, 12, 01, 14, 32)]
		[InlineData("210311092311", 2021, 03, 11, 09, 23, 11)]
		public static void ParseAbsoluteLong(string s, int year, int month, int day, int hour, int minute, int second) {
			var result = SmppTime.TryParse(s, out SmppTime time);

			Assert.True(result);
			Assert.Equal(SmppDateTimeType.Absolute, time.Type);
			Assert.Equal(year, time.AbsoluteTime.Year);
			Assert.Equal(month, time.AbsoluteTime.Month);
			Assert.Equal(day, time.AbsoluteTime.Day);
			Assert.Equal(hour, time.AbsoluteTime.Hour);
			Assert.Equal(minute, time.AbsoluteTime.Minute);
			Assert.Equal(second, time.AbsoluteTime.Second);
		}

		[Theory]
		[InlineData("201212011432101+", 2020, 12, 12, 01, 29, 32, 100)]
		[InlineData("210311092311200-", 2021, 03, 11, 09, 23, 11, 200)]
		public static void ParseAbsoluteFull(string s, int year, int month, int day, int hour, int minute, int second, int millis) {
			var result = SmppTime.TryParse(s, out SmppTime time);

			Assert.True(result);
			Assert.Equal(SmppDateTimeType.Absolute, time.Type);

			var utc = time.AbsoluteTime.ToUniversalTime();

			Assert.Equal(year, utc.Year);
			Assert.Equal(month, utc.Month);
			Assert.Equal(day, utc.Day);
			Assert.Equal(hour, utc.Hour);
			Assert.Equal(minute, utc.Minute);
			Assert.Equal(second, utc.Second);
			Assert.Equal(millis, utc.Millisecond);
		}

		[Theory]
		[InlineData("010003200000000R", 368, 20, 00)]
		public static void ParseRelative(string s, int days, int hours, int minutes) {
			var result = SmppTime.TryParse(s, out var time);

			Assert.True(result);
			Assert.Equal(SmppDateTimeType.Relative, time.Type);

			Assert.Equal(days, time.RelativeTime.Days);
			Assert.Equal(hours, time.RelativeTime.Hours);
			Assert.Equal(minutes, time.RelativeTime.Minutes);
		}

		[Theory]
		[InlineData(2020, 11, 12, 20, 09, 00, "201112200900")]
		public static void FormatShortDate(int year, int month, int day, int hour, int minute, int second, string expected) {
			var date = new SmppTime(new DateTime(year, month, day, hour, minute, second), false);

			Assert.Equal(expected, date.ToSmppString());
		}

		[Theory]
		[InlineData(2020, 11, 12, 20, 09, 00, 100, "201112200900100+")]
		public static void FormatFullDate(int year, int month, int day, int hour, int minute, int second, int millis, string expected) {
			var date = new SmppTime(new DateTime(year, month, day, hour, minute, second, millis), true);

			Assert.Equal(expected, date.ToSmppString());
		}

	}
}
