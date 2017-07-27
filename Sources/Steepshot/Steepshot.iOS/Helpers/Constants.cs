﻿using System;
using CoreGraphics;
using UIKit;

namespace Steepshot.iOS
{
    public class Constants
    {
        public const string UserContextKey = "UserContext";
		public static readonly UIColor NavBlue = UIColor.FromRGB(55, 176, 233);
		public static readonly UIColor Blue = UIColor.FromRGB(66, 165, 245);
		public static readonly UIFont Regular12 = UIFont.FromName("Lato-Regular", 12f);
		public static readonly UIFont Regular15 = UIFont.FromName("Lato-Regular", 15f);
		public static readonly UIFont Semibold10 = UIFont.FromName("Lato-Semibold", 10f);
		public static readonly UIFont Bold225 = UIFont.FromName("Lato-Bold", 22.5f);
		public static readonly UIFont Bold175 = UIFont.FromName("Lato-Bold", 17.5f);
		public static readonly UIFont Bold15 = UIFont.FromName("Lato-Bold", 15f);
		public static readonly UIFont Bold13 = UIFont.FromName("Lato-Bold", 13f);
		public static readonly UIFont Bold135 = UIFont.FromName("Lato-Bold", 13.5f);
		public static readonly UIFont Bold125 = UIFont.FromName("Lato-Bold", 12.5f);
		public static readonly UIFont Bold12 = UIFont.FromName("Lato-Bold", 12f);
		public static readonly UIFont Bold9 = UIFont.FromName("Lato-Bold", 9f);
		public static readonly UIFont Bold115 = UIFont.FromName("Lato-Bold", 12f);
		public static readonly UIFont Heavy165 = UIFont.FromName("Lato-Heavy", 16.5f);
		public static readonly UIFont Heavy115 = UIFont.FromName("Lato-Heavy", 11.5f);
		public static readonly UIFont Heavy135 = UIFont.FromName("Lato-Heavy", 13.5f);

        public static readonly float CellSideSize = (float)UIScreen.MainScreen.Bounds.Width / 3 - 1;
		public static readonly CGSize CellSize = new CGSize(CellSideSize, CellSideSize);

		public static readonly TimeSpan ImageCacheDuration = TimeSpan.FromDays(2);
        

		public static readonly UIStringAttributes NicknameAttribute = new UIStringAttributes
			{
				Font = UIFont.BoldSystemFontOfSize(13)
			};
    }

	public enum Networks
	{
		Steem,
		Golos
	}; 
}