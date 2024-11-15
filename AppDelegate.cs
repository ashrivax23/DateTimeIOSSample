namespace DateTimeFormatIOSTest;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
        // Create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        // Create your view controller instance
        var viewController = new ViewController();

        // Make the view controller the root view controller
        Window.RootViewController = viewController;

        // Make the window visible
        Window.MakeKeyAndVisible();

        return true;
	}
}
