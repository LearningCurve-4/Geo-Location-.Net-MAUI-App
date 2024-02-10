namespace GeoLocation.ViewModels;

public class GeoLocationViewModel : BaseViewModel
{
	static GeoLocationModel? geoLocationInfo = new();
	public GeoLocationModel? GeoLocationInfo
	{
		get => geoLocationInfo;
		set
		{
			if (geoLocationInfo != value)
			{
				geoLocationInfo = value;
				OnPropertyChanged();
			}
		}
	}

	public Command RefreshGeoLocationInformationCommand => new(async () =>
	{
		try
		{
			IsBusy = true;
			GeoLocationInfo = await GeoLocationService.GetCurrentGeoLocation();
			if (!string.IsNullOrEmpty(GeoLocationInfo.ExceptionMessage))
			{
				await Shell.Current.DisplayAlert("Error", GeoLocationInfo.ExceptionMessage, "OK");
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}, () => IsNotBusy);
}
