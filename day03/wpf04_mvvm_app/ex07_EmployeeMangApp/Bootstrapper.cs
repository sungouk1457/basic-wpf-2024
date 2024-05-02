using Caliburn.Micro;
using ex07_EmployeeMangApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ex07_EmployeeMangApp
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        { 
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(sender, e);
            DisplayRootViewForAsync<MainViewModel>();
        }
    }
}
