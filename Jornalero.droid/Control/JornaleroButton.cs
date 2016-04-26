using System;
using Android.Widget;
using Android.Runtime;
using Android.Content;
using Android.Util;
using Android.Graphics;

namespace Jornalero.droid
{
	public class JornaleroButton : Button
	{
		protected JornaleroButton (IntPtr javaReference, JniHandleOwnership transfer)
			: base (javaReference, transfer)
		{
		}

		public JornaleroButton (Context context)
			: this (context, null)
		{
		}

		public JornaleroButton (Context context, IAttributeSet attrs)
			: this (context, attrs, 0)
		{
		}

		public JornaleroButton (Context context, IAttributeSet attrs, int defStyle)
			: base (context, attrs, defStyle)
		{
			var a = context.ObtainStyledAttributes (attrs,
				        Resource.Styleable.CustomFonts);
			var customFont = a.GetString (Resource.Styleable.CustomFonts_customFont);
			SetCustomFont (customFont);
			a.Recycle ();
		}

		public void SetCustomFont (string asset)
		{
			Typeface tf;
			try {
				tf = Typeface.CreateFromAsset (Context.Assets, asset);
			} catch (Exception e) {
				Console.WriteLine (e);
				return;
			}

			if (null == tf)
				return;

			var tfStyle = TypefaceStyle.Normal;
			if (null != Typeface) //Takes care of android:textStyle=""
					tfStyle = Typeface.Style;
			SetTypeface (tf, tfStyle);
		}
	}
}