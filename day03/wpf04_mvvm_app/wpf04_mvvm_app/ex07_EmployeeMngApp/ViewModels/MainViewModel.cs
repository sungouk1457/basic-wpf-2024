using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ex07_EmployeeMngApp.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        public MainViewModel()
        {
            DisplayName = "직원관리 시스템";
        }
        /// <summary>
        /// Caliburn.Micro가 Xaml의 버튼 x:Name과 동일한 이름의 메서드로 매핑
        /// </summary>
        public void SaveEmployee()
        {
            // MessageBox.Show("저장버튼 동작");

        }
    }
}
