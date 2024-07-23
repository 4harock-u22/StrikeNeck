namespace u22_strikeneck;
using u22_strikeneck.AppSettingIO;

public partial class Settings : ContentPage
{
    private bool isSwitchOn;
    private string selectedValue;
    private double sense;
    public Settings()
    {
        InitializeComponent();
        AppSettingReader appSettingReader = new AppSettingReader();
        toggleSwitch.IsToggled = appSettingReader.GetNotificationStatus();
        if (appSettingReader.GetNotificationInterval() == NotificationInterval.OneMinute)
        {
            selectedValue = "1";
        }
        else if (appSettingReader.GetNotificationInterval() == NotificationInterval.FiveMinutes)
        {
            selectedValue = "5";
        }
        else if (appSettingReader.GetNotificationInterval() == NotificationInterval.FifteenMinutes)
        {
            selectedValue = "15";
        }
        else if (appSettingReader.GetNotificationInterval() == NotificationInterval.ThirtyMinutes)
        {
            selectedValue = "30";
        }
        else
        {
            selectedValue = "60";
        }
        

        slider.Value = appSettingReader.GetDetectionSensitivity().value;
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
        await Shell.Current.GoToAsync("//Stats");
        
        AppSettingWriter appSettinger = new AppSettingWriter();
        appSettinger.UpdateNotificationStatus(isSwitchOn);
        if (selectedValue == "1")
        {
            appSettinger.UpdateNotificationInterval(NotificationInterval.OneMinute);
        }
        else if (selectedValue == "5")
        {
            appSettinger.UpdateNotificationInterval(NotificationInterval.FiveMinutes);
        }
        else if (selectedValue == "15")
        {
            appSettinger.UpdateNotificationInterval(NotificationInterval.FifteenMinutes);
        }
        else if (selectedValue == "30")
        {
            appSettinger.UpdateNotificationInterval(NotificationInterval.ThirtyMinutes);
        }
        else
        {
            appSettinger.UpdateNotificationInterval(NotificationInterval.OneHour);
        }
        appSettinger.UpdateDetectionSensitivity(new DetectionSensitivity(sense));


    }

    private async void ToInit1(Object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Init1");
    }
}