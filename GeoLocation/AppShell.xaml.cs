namespace GeoLocation
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
			InitializeRouting();
		}

		public void InitializeRouting()
		{
			Routing.RegisterRoute(nameof(GeoLocationPage), typeof(GeoLocationPage));
		}
	}
}
