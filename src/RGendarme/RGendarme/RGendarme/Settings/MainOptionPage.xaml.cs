using JetBrains.ReSharper.Features.Environment.Options.Inspections;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using RGendarme.Settings;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  [OptionsPage(PID, "RGendarme", typeof(OptionsPageThemedIcons.UseIntMaxValueOptions), ParentId = CodeInspectionPage.PID)]
  public partial class MainOptionPage : IOptionsPage
  {
    public const string PID = "MainPageId";

    public MainOptionPage()
    {
      InitializeComponent();
    }

    public EitherControl Control
    {
      get { return this; }
    }

    public string Id
    {
      get { return PID; }
    }

    public bool OnOk()
    {
      return true;
    }

    public bool ValidatePage()
    {
      return true;
    }
  }
}
