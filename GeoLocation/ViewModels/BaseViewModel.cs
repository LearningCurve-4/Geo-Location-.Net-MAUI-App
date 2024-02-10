namespace GeoLocation.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	bool isBusy = false;
	public bool IsBusy
	{
		get => isBusy;
		set
		{
			if (isBusy != value)
			{
				isBusy = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsNotBusy));
			}
		}
	}
	public bool IsNotBusy => !IsBusy;

	public static string CurrentPage { get; set; } = string.Empty;
	public Command GoToPageCommand => new Command<string>(async (page) =>
	{
		try
		{
			IsBusy = true;
			if (CurrentPage != page)
			{
				var pageType = Type.GetType(GlobalVariables.pageFolder + page);
				if (pageType != null)
				{
					await Shell.Current.GoToAsync(page, true);
					CurrentPage = page;
				}
				else
				{
					await Shell.Current.DisplayAlert("Error:", $"{page} - Not available", "OK");
				}
			}
		}
		catch (Exception ex) { await Shell.Current.DisplayAlert("Error:", ex.Message, "OK"); }
		finally
		{
			IsBusy = false;
		}
	}, (page) => IsNotBusy);

	public Command GoBackToPageCommand => new Command<string>(async (page) =>
	{
		try
		{
			IsBusy = true;
			await Shell.Current.GoToAsync(page, true);
			CurrentPage = string.Empty;
		}
		catch (Exception ex) { await Shell.Current.DisplayAlert("Error:", ex.Message, "OK"); }
		finally
		{
			IsBusy = false;
		}
	}, (page) => IsNotBusy);
}
