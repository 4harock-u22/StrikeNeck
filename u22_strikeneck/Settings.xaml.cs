namespace u22_strikeneck;
using u22_strikeneck.AppSettingIO;
using u22_strikeneck.Camera;

public partial class Settings : ContentPage
{
    private bool isSwitchOn;
    private string selectedValue;
    private double sense;
    public Settings()
    {
        InitializeComponent();
        
    }
    private void ToggleSwitch_Toggled(Object sender, ToggledEventArgs e)
    {
        
        isSwitchOn = e.Value;
    }
    private void NotificationIntervalPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Picker で選択されたアイテムを取得
        
        selectedValue = notificationIntervalPicker.SelectedItem as string;


    }
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        
        sense = e.NewValue; // スライダーの新しい値を取得

    }

    private async void ToStats(Object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Stats");
    }
    private async void CompleteButton_Clicked(Object sender, EventArgs e)
    {
        
        AppSettingWriter appSettinger = new AppSettingWriter();

        appSettinger.UpdateNotificationStatus(isSwitchOn);

        var interval = ConvertStringToNotoficationInterval(selectedValue);
        appSettinger.UpdateNotificationInterval(interval);

        appSettinger.UpdateDetectionSensitivity(new DetectionSensitivity(sense));

        appSettinger.UpdateUsedCameraName(usedCameraPicker.SelectedItem as string);

        await Shell.Current.GoToAsync("//Stats");

    }

    private void ToInit1(Object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Init1");
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var cameraNames = cameraComponent.GetCameraSelector().getCameraNames();
        usedCameraPicker.ItemsSource = cameraNames;
        usedCameraPicker.SelectedItem = new AppSettingReader().GetUsedCameraName();
        cameraComponent.StartPeriodicTask();
        AppSettingReader appSettingReader = new AppSettingReader();
        toggleSwitch.IsToggled = appSettingReader.GetNotificationStatus();
        if (appSettingReader.GetNotificationInterval() == NotificationInterval.OneMinute)
        {
            notificationIntervalPicker.SelectedItem = "1";
        }
        else if (appSettingReader.GetNotificationInterval() == NotificationInterval.FiveMinutes)
        {
            notificationIntervalPicker.SelectedItem = "5";
        }
        else if (appSettingReader.GetNotificationInterval() == NotificationInterval.FifteenMinutes)
        {
            notificationIntervalPicker.SelectedItem = "15";
        }
        else if (appSettingReader.GetNotificationInterval() == NotificationInterval.ThirtyMinutes)
        {
            notificationIntervalPicker.SelectedItem = "30";
        }
        else
        {
            notificationIntervalPicker.SelectedItem = "60";
        }

        slider.Value = appSettingReader.GetDetectionSensitivity().value;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraComponent.StopPeriodicTask();
    }

    private NotificationInterval ConvertStringToNotoficationInterval(string interval)
    {
        if (interval == "1") return NotificationInterval.OneMinute;
        
        else if (interval == "5") return NotificationInterval.FiveMinutes;
        
        else if (interval == "15") return NotificationInterval.FifteenMinutes;
        
        else if (interval == "30") return NotificationInterval.ThirtyMinutes;
        
        else if (interval == "60")return NotificationInterval.OneHour;

        else throw new ArgumentException();
        
    }
}