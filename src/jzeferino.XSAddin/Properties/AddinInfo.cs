using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "jzeferino.XSAddin",
    Namespace = "jzeferino.XSAddin",
    Version = "1.0.0"
)]

[assembly: AddinName("jzeferino.XSAddin")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("This add-in let you create a Xamarin Cross Platform solution (Xamarin.Native) or (Xamarin.Forms)." +
                            "When creating the project the user can select the application name, the application identifier and if he wants .gitignore, readme and cake." +
                            "It already creates some PCL projects to allow the shared code to be separated in different layers using MVVM pattern." +
                            "It also allows you to create some file templates.")]
[assembly: AddinAuthor("jzeferino")]