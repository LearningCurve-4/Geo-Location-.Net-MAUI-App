namespace GeoLocation.ViewModels;

public class HomeViewModel : BaseViewModel
{
	public Command GetGeoLocationInformationCommand => new Command<string>(async (str) =>
	{
		GeoLocationModel geoLocationInfo = new();

		try
		{
			IsBusy = true;
			geoLocationInfo = await GeoLocationService.GetCurrentGeoLocation();
			if (string.IsNullOrEmpty(geoLocationInfo.ExceptionMessage))
			{
				_ = new GeoLocationViewModel { GeoLocationInfo = geoLocationInfo };
				GoToPageCommand.Execute(str);
			}
			else
			{
				await Shell.Current.DisplayAlert("Error", geoLocationInfo.ExceptionMessage, "OK");
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
	}, (str) => IsNotBusy);
}
