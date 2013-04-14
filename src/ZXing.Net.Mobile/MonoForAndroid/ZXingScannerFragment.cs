
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace ZXing.Mobile
{
	public class ZXingScannerFragment : Fragment
	{
		public ZXingScannerFragment(Action<ZXing.Result> scanResultCallback) : base()
		{
			this.callback = scanResultCallback;
		}

		Action<ZXing.Result> callback;

		public override View OnCreateView (LayoutInflater layoutInflater, ViewGroup viewGroup, Bundle bundle)
		{
			var frame = new FrameLayout(this.Activity);
			var layoutParams = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent);

			scanner = new ZXingSurfaceView (this.Activity, ScanningOptions, callback);
			frame.AddView(scanner, layoutParams);


			if (!UseCustomView)
			{
				zxingOverlay = new ZxingOverlayView (this.Activity);
				zxingOverlay.TopText = TopText ?? "";
				zxingOverlay.BottomText = BottomText ?? "";

				frame.AddView (zxingOverlay, layoutParams);
			}
			else if (CustomOverlayView != null)
			{
				frame.AddView(CustomOverlayView, layoutParams);
			}

			return frame;
		}

		public override void OnPause ()
		{
			scanner.ShutdownCamera();
		}

		public event Action<ZXing.Result> OnScanCompleted;

		public View CustomOverlayView { get;set; }
		public bool UseCustomView { get; set; }
		public MobileBarcodeScanningOptions ScanningOptions { get;set; }
		public string TopText { get;set; }
		public string BottomText { get;set; }
		
		ZXingSurfaceView scanner;
		ZxingOverlayView zxingOverlay;

		public void SetTorch(bool on)
		{
			this.scanner.Torch(on);
		}
		
		public void AutoFocus()
		{
			this.scanner.AutoFocus();
		}

	}
}
