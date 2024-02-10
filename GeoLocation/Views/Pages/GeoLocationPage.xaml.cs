namespace GeoLocation.Views.Pages;

public partial class GeoLocationPage : ContentPage
{
	public GeoLocationPage()
	{
		InitializeComponent();
	}

	protected override bool OnBackButtonPressed()
	{
		return true;
	}
}