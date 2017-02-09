using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    Id = "jzeferino.XSAddin",
    Namespace = "jzeferino.XSAddin",
    Version = "0.0.1"
)]

[assembly: AddinName("jzeferino.XSAddin")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("This add-in let you create a Xamarin Cross Platform solution (Xamarin.Native) or (Xamarin.Forms)." +
                            "When creating the project the addin will show you a custom wizard wich the user can select the application name, the application identifier and if he wants, .gitignore, readme and cake." +
                            "It already creates some PCL projects to allow the shared code to be separated in different layers using MVVM pattern." +
                            "It also allows you to create some file templates.")]
[assembly: AddinAuthor("jzeferino")]
[assembly: AddinUrl("https://github.com/jzeferino/jzeferino.XSAddin")]
