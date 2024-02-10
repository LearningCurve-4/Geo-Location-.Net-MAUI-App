namespace GeoLocation.Services;

public class GeoLocationService
{
	static CancellationTokenSource? cancelTokenSource;
	static bool isCheckingLocation;

	public static async Task<GeoLocationModel> GetCurrentGeoLocation()
	{
		GeoLocationModel locationDetail = new();
		try
		{
			isCheckingLocation = true;
			GeolocationRequest request = new(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
			cancelTokenSource = new CancellationTokenSource();
			Location? location = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);
			if (location != null)
			{
				locationDetail = await GetGeocodeReverseData(location.Latitude, location.Longitude);
			}
			else
			{
				locationDetail.ExceptionMessage = "Error: Unable to get location detail.";
			}
		}
		// Catch one of the following exceptions:
		catch (FeatureNotSupportedException fnsEx)
		{
			locationDetail.ExceptionMessage = fnsEx.Message;  //Handle not supported on device exception
		}
		catch (FeatureNotEnabledException fneEx)
		{
			locationDetail.ExceptionMessage = fneEx.Message;  //Handle not enabled on device exception
		}
		catch (PermissionException pEx)
		{
			locationDetail.ExceptionMessage = pEx.Message;  //Handle permission exception
		}
		catch (Exception ex)
		{
			locationDetail.ExceptionMessage = ex.Message;  //Unable to get location
		}
		finally
		{
			isCheckingLocation = false;
		}
		return locationDetail;
	}

	public static async Task<GeoLocationModel> GetCachedGeoLocation()
	{
		GeoLocationModel locationDetail = new();
		try
		{
			Location? location = await Geolocation.Default.GetLastKnownLocationAsync();
			if (location != null)
			{
				locationDetail = await GetGeocodeReverseData(location.Latitude, location.Longitude);
			}
			else
			{
				locationDetail.ExceptionMessage = "Error: Unable to get cached location detail.";
			}
		}
		catch (FeatureNotSupportedException fnsEx)
		{
			locationDetail.ExceptionMessage = fnsEx.Message;  //Handle not supported on device exception
		}
		catch (FeatureNotEnabledException fneEx)
		{
			locationDetail.ExceptionMessage = fneEx.Message;  //Handle not enabled on device exception
		}
		catch (PermissionException pEx)
		{
			locationDetail.ExceptionMessage = pEx.Message;  //Handle permission exception
		}
		catch (Exception ex)
		{
			locationDetail.ExceptionMessage = ex.Message;  //Unable to get location
		}
		return locationDetail;
	}

	public static async Task<GeoLocationModel> GetGeocodeReverseData(double latitude, double longitude)
	{
		//Reverse geocoding is the process of getting placemarks for an existing set of coordinates. Please refer to .NET MAUI Geocoding page for detail.

		//The following example demonstrates getting placemarks:
		IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
		Placemark? placemark = placemarks?.FirstOrDefault();
		GeoLocationModel locationDetail = new();

		if (placemark != null)
		{
			locationDetail.AdminArea = placemark.AdminArea;
			locationDetail.CountryCode = placemark.CountryCode;
			locationDetail.CountryName = placemark.CountryName;
			locationDetail.FeatureName = placemark.FeatureName;
			locationDetail.Locality = placemark.Locality;
			locationDetail.SubAdminArea = placemark.SubAdminArea;
			locationDetail.PostalCode = placemark.PostalCode;
			locationDetail.SubLocality = placemark.SubLocality;
			locationDetail.SubThoroughfare = placemark.SubThoroughfare;
			locationDetail.Thoroughfare = placemark.Thoroughfare;
			locationDetail.Latitude = latitude;
			locationDetail.Longitude = longitude;

		}
		return locationDetail;
	}

	public static void CancelRequest()
	{
		if (isCheckingLocation && cancelTokenSource != null && cancelTokenSource.IsCancellationRequested == false) { cancelTokenSource.Cancel(); }
	}
}
