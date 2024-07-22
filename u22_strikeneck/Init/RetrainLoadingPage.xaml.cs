using ForwardLeanDetection.DiscriminantModel;

namespace u22_strikeneck.Init;

public partial class RetrainLoadingPage : ContentPage
{
	private API fldAPI = new API();

	public RetrainLoadingPage()
	{
		InitializeComponent();

		MainThread.InvokeOnMainThreadAsync(async () => {
			await Retrain();

			while (fldAPI.IsRetraining) { }
			await Shell.Current.GoToAsync("//Init3");
		});
	}

	private async Task Retrain()
	{
        var directoryAccessor = new InitDirectoryAccessor();
        var correctDirectoryInfo = directoryAccessor.CorrectDirectoryInfo;
        var forwardDirectoryInfo = directoryAccessor.ForwardDirectoryInfo;
        await fldAPI.Retrain(correctDirectoryInfo, forwardDirectoryInfo);
    }
}