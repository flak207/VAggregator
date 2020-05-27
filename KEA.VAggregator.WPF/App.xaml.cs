using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace KEA.VAggregator.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            try
            {
                Assembly assPresentationCore = typeof(UIElement).Assembly; //PresentationCore.dll
                Assembly assWindowsBase = typeof(System.Windows.Vector).Assembly; //WindowsBase.dll

                Type typeContainer = assPresentationCore.GetType("MS.Internal.AppModel.SiteOfOriginContainer");
                Type typeDS = assWindowsBase.GetType("MS.Internal.SecurityCriticalDataForSet`1").MakeGenericType(typeof(Uri));
                Type typeNullable = typeof(Nullable<int>).GetGenericTypeDefinition().MakeGenericType(typeDS);

                FieldInfo field = typeContainer.GetField("_siteOfOriginForClickOnceApp",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static
                    );

                object objDS = Activator.CreateInstance(typeDS,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                    new object[] { new Uri("http://localhost/") },
                    System.Globalization.CultureInfo.InvariantCulture
                    );
                object objNullable = Activator.CreateInstance(typeNullable, objDS);

                field.SetValue(null, objNullable);

                this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
