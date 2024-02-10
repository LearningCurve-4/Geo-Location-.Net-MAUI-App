using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace GeoLocation
{
	[Activity(
		//Theme = "@style/Maui.SplashTheme",
		Theme = "@style/SplashTheme",
		MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : MauiAppCompatActivity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue(GlobalVariables.headerBarFillColor, out object hdrBgColor); Color statusBarColor = (Color)hdrBgColor;
			Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue(GlobalVariables.footerBarFillColor, out object ftrBgColor); Color navigationBarColor = (Color)ftrBgColor;
			double brightnessStatusBarColor = statusBarColor.Red * .3 + statusBarColor.Green * .59 + statusBarColor.Blue * .11;
			double brightnessNavigationBarColor = navigationBarColor.Red * .3 + navigationBarColor.Green * .59 + navigationBarColor.Blue * .11;
			_ = SetStatusNavigationBarsColorAsync(statusBarColor, brightnessStatusBarColor > 0.5, navigationBarColor, brightnessNavigationBarColor > 0.5);
			InitializeCustomHandler();
		}

		public static async Task SetStatusNavigationBarsColorAsync(Color setStatusBarColor, bool appearanceLightStatusBar, Color setNavigationBarColor, bool appearanceLightNavigationBar)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop) { return; }

			var activity = await Platform.WaitForActivityAsync();
			var window = activity.Window!;

			//this may not be necessary (but maybe for older than M)
			window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			window.ClearFlags(WindowManagerFlags.TranslucentStatus);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
			{
				AndroidX.Core.View.WindowCompat.GetInsetsController(window, window.DecorView).AppearanceLightStatusBars = appearanceLightStatusBar;
				AndroidX.Core.View.WindowCompat.GetInsetsController(window, window.DecorView).AppearanceLightNavigationBars = appearanceLightNavigationBar;
			}

			window.SetStatusBarColor(new Android.Graphics.Color(setStatusBarColor.ToInt()));
			window.SetNavigationBarColor(new Android.Graphics.Color(setNavigationBarColor.ToInt()));
		}

		public static void InitializeCustomHandler()
		{
			Color transparentColor = Colors.Transparent;

			Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
			{
				handler.PlatformView.SetPadding(20, 0, 0, 0);
				handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf((new Android.Graphics.Color(transparentColor.ToInt())));
			});

			Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
			{
				handler.PlatformView.SetPadding(20, 0, 0, 0);
				handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf((new Android.Graphics.Color(transparentColor.ToInt())));
			});

			Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
			{
				handler.PlatformView.SetPadding(0, 0, 0, 0);
				handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf((new Android.Graphics.Color(transparentColor.ToInt())));
			});

			Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping(nameof(Switch), (handler, view) =>
			{
				handler.PlatformView.SetPadding(0, 0, 0, 0);
			});

			Microsoft.Maui.Handlers.ToolbarHandler.Mapper.AppendToMapping(nameof(Toolbar), (handler, view) =>
			{
				handler.PlatformView.SetContentInsetsRelative(0, 0);
			});
		}
	}
}
